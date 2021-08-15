using Microsoft.AspNet.Identity;
using QuizbeePlus.Commons;
using QuizbeePlus.Entities;
using QuizbeePlus.Services;
using QuizbeePlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuizbeePlus.Controllers
{
    public class AttemptQuizController : BaseController
    {
        [HttpGet]
        public ActionResult Attempt(int QuizID)
        {
            var quiz = QuizzesService.Instance.GetQuiz(QuizID);

            if (quiz == null) return HttpNotFound();

            QuizDetailViewModel model = new QuizDetailViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = string.Format("Quiz : {0}", quiz.Name),
                PageDescription = quiz.Description
            };

            model.ID = quiz.ID;
            model.Name = quiz.Name;
            model.Description = quiz.Description;
            model.TimeDuration = quiz.TimeDuration;
            model.EnableQuizTimer = quiz.EnableQuizTimer;

            StudentQuiz studentQuiz = StudentQuizzesService.Instance.GetStudentQuiz(QuizID, User.Identity.GetUserId());
            if (studentQuiz != null )
            {
                ViewBag.QuizCompleted = "inprogress";

                if (studentQuiz.CompletedAt != null)
                {
                    ViewBag.QuizCompleted = "completed";
                }
            }
            else
            {
                ViewBag.QuizCompleted = "start";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Attempt(AttemptQuizViewModel model)
        {
            var quiz = QuizzesService.Instance.GetQuiz(model.QuizID);

            if (quiz == null) return HttpNotFound();

            StudentQuiz studentQuiz = new StudentQuiz();

            studentQuiz.StudentID = User.Identity.GetUserId();
            studentQuiz.QuizID = quiz.ID;
            studentQuiz.StartedAt = DateTime.Now;
            studentQuiz.ModifiedOn = DateTime.Now;

            StudentQuiz studentQuizAttempted = StudentQuizzesService.Instance.GetStudentQuiz(quiz.ID, User.Identity.GetUserId());
            if (studentQuizAttempted != null)
            {
                foreach (AttemptedQuestion aq in studentQuizAttempted.AttemptedQuestions)
                {
                    quiz.Questions.RemoveAll(item => item.ID == aq.Question.ID);
                }

                if (studentQuizAttempted.CompletedAt != null)
                {
                    return RedirectToAction("AttemptDetails", new { studentQuizID = studentQuizAttempted.ID, isPartial = true, timerExpired = model.TimerExpired });
                }
            }
            
           
            if (quiz.Questions.Count != 0)
            {
                if (await StudentQuizzesService.Instance.NewStudentQuiz(studentQuiz))
                {
                    model.QuizType = quiz.QuizType;
                    model.StudentQuizID = studentQuiz.ID;
                    model.TotalQuestions = quiz.Questions.Count;
                    model.Question = quiz.Questions.FirstOrDefault();
                    model.QuestionIndex = 0;

                    model.Options = new List<Option>();
                    model.Options.AddRange(model.Question.Options);
                    model.Options.Shuffle();

                    model.EnableQuestionTimer = quiz.EnableQuestionTimer;
                    model.Seconds = Calculator.CalculateAllowedQuestionTime(quiz);

                    return PartialView("_QuizQuestion", model);
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
            else
            {
                if (studentQuizAttempted.CompletedAt != null)
                {
                    return RedirectToAction("AttemptDetails", new { studentQuizID = studentQuiz.ID, isPartial = true, timerExpired = model.TimerExpired });
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> AnswerQuestion(AttemptQuizViewModel model)
        {
            //Thread.Sleep(2000);

            var studentQuiz = StudentQuizzesService.Instance.GetStudentQuiz(model.StudentQuizID);

            if (studentQuiz == null) return new HttpStatusCodeResult(500);

            if (model.TimerExpired)
            {
                studentQuiz.CompletedAt = DateTime.Now;
                studentQuiz.ModifiedOn = DateTime.Now;

                if (await StudentQuizzesService.Instance.UpdateStudentQuiz(studentQuiz))
                {
                    StudentQuizViewModel studentQuizModel = new StudentQuizViewModel();

                    studentQuizModel.StudentQuiz = studentQuiz;
                    studentQuizModel.TimerExpired = model.TimerExpired;

                    return PartialView("_AttemptDetails", studentQuizModel);
                }
                else return new HttpStatusCodeResult(500);
            }
            else
            {
                var quiz = QuizzesService.Instance.GetQuiz(model.QuizID);

                if (quiz == null) return HttpNotFound();

                var question = QuestionsService.Instance.GetQuizQuestion(quiz.ID, model.QuestionID);

                if (question == null) return HttpNotFound();

                var selectedOptions = QuestionsService.Instance.GetOptionsByIDs(model.SelectedOptionIDs.CSVToListInt());

                if (selectedOptions == null) return HttpNotFound();

                AttemptedQuestion attemptedQuestion = new AttemptedQuestion();

                attemptedQuestion.StudentQuizID = studentQuiz.ID;
                attemptedQuestion.QuestionID = question.ID;
                attemptedQuestion.SelectedOptions = selectedOptions.Select(x => new AttemptedOption() { AttemptedQuestionID = attemptedQuestion.ID, Option = x, OptionID = x.ID }).ToList();
                attemptedQuestion.Score = Calculator.CalculateAttemptedQuestionScore(question.Options, attemptedQuestion.SelectedOptions);
                attemptedQuestion.AnsweredAt = DateTime.Now;
                attemptedQuestion.ModifiedOn = DateTime.Now;

                if (await StudentQuizzesService.Instance.NewAttemptedQuestion(attemptedQuestion))
                {
                    if (model.QuestionIndex != quiz.Questions.Count() - 1)
                    {
                        model.QuizType = quiz.QuizType;

                        model.TotalQuestions = quiz.Questions.Count;
                        model.Question = quiz.Questions.ElementAtOrDefault(model.QuestionIndex + 1);
                        model.QuestionIndex = model.QuestionIndex + 1;

                        model.Options = new List<Option>();
                        model.Options.AddRange(model.Question.Options);
                        model.Options.Shuffle();

                        model.EnableQuestionTimer = quiz.EnableQuestionTimer;
                        model.Seconds = Calculator.CalculateAllowedQuestionTime(quiz);

                        return PartialView("_QuizQuestion", model);
                    }
                    else //this was the Last question so display the result
                    {
                        studentQuiz.CompletedAt = DateTime.Now;

                        if (!await StudentQuizzesService.Instance.UpdateStudentQuiz(studentQuiz))
                            return new HttpStatusCodeResult(500);
                        
                        return RedirectToAction("AttemptDetails", new { studentQuizID = studentQuiz.ID, isPartial = true, timerExpired = model.TimerExpired });
                    }
                }
                else return new HttpStatusCodeResult(500);
            }
        }

		[HttpGet]
		public ActionResult AttemptDetails(int studentQuizID, bool? timerExpired, bool? isPartial = false)
		{
			var studentQuiz = StudentQuizzesService.Instance.GetStudentQuiz(studentQuizID);

            if (studentQuiz == null) return HttpNotFound();

			StudentQuizViewModel model = new StudentQuizViewModel();

            model.TimerExpired = timerExpired.HasValue ? timerExpired.Value : false;

			model.PageInfo = new PageInfo()
			{
				PageTitle = "Quiz Attempt Details",
				PageDescription = "Details of Attempted Quiz"
			};

			model.StudentQuiz = studentQuiz;

            if(isPartial.HasValue && isPartial.Value)
            {
                return PartialView("_AttemptDetails", model);
            }
            else
            {
                return View(model);
            }
		}

        [HttpGet]
        public ActionResult MyResults(string search, int? page, int? items, bool? isPartial)
        {
            StudentQuizListViewModel model = new StudentQuizListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "My Quiz Results",
                PageDescription = "Results of My Quiz Attempts"
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;
            
            var resultsSearch = StudentQuizzesService.Instance.GetUserQuizAttempts(User.Identity.GetUserId(), model.searchTerm, model.pageNo, model.pageSize);

            model.StudentQuizzes = resultsSearch.StudentQuizzes;
            model.TotalCount = resultsSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            if (isPartial.HasValue && isPartial.Value)
            {
                model.isPartialView = true;
                return PartialView(model);
            }
            else return View(model);
        }

        [HttpGet]
        public ActionResult PrintResult(int studentQuizID)
        {
            var studentQuiz = StudentQuizzesService.Instance.GetStudentQuiz(studentQuizID);

            if (studentQuiz == null) return HttpNotFound();

            StudentQuizViewModel model = new StudentQuizViewModel();
            
            model.PageInfo = new PageInfo()
            {
                PageTitle = "Quiz Attempt Details",
                PageDescription = "Details of Attempted Quiz"
            };

            model.StudentQuiz = studentQuiz;

            return PartialView("_PrintResult", model);
        }
    }
}