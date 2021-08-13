using Microsoft.AspNet.Identity;
using QuizbeePlus.Entities;
using QuizbeePlus.Services;
using QuizbeePlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuizbeePlus.Controllers
{
    public class QuizController : BaseController
    {
        // GET: Quiz
        public ActionResult Index(string search, int? page, int? items)
        {
            QuizListViewModel model = new QuizListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Quizzes",
                PageDescription = "List of Quizzes."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var quizzesSearch = UserHasRights() ? QuizzesService.Instance.GetQuizzesForAdmin(model.searchTerm, model.pageNo, model.pageSize) : QuizzesService.Instance.GetUserQuizzes(User.Identity.GetUserId(), model.searchTerm, model.pageNo, model.pageSize);

            model.Quizzes = quizzesSearch.Quizzes;
            model.TotalCount = quizzesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult NewQuiz()
        {
            NewQuizViewModel model = new NewQuizViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Create New Quiz",
                PageDescription = "Create new quiz."
            };

            model.EnableQuizTimer = true; //set Default Quiz Timer to true
            
            model.QuizTypes = new List<QuizType>() { QuizType.Text, QuizType.Image };
            model.SelectedQuizType = QuizType.Text;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> NewQuiz(NewQuizViewModel model)
        {
            //check if Model is valid
            if (!ModelState.IsValid)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Create New Quiz",
                    PageDescription = "Create new quiz."
                };

                model.QuizTypes = new List<QuizType>() { QuizType.Text, QuizType.Image };

                return View(model);
            }

            //populate new Quiz from Model
            var quiz = new Quiz();
            quiz.OwnerID = User.Identity.GetUserId();
            quiz.Name = model.Name;
            quiz.Description = model.Description;

            quiz.QuizType = model.SelectedQuizType;

            quiz.EnableQuizTimer = model.EnableQuizTimer;

            if (quiz.EnableQuizTimer)
            {
                quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
                quiz.EnableQuestionTimer = model.EnableQuestionTimer;
            }
            else
            {
                quiz.TimeDuration = new TimeSpan(0, 0, 0);
                quiz.EnableQuestionTimer = false;
            }

            quiz.ModifiedOn = DateTime.Now;

            if (await QuizzesService.Instance.SaveNewQuiz(quiz))
            {
                //now redirect to New Question screen for this new Quiz
                return RedirectToAction("NewQuestion", "Question", new { quizID = quiz.ID });
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpGet]
        public ActionResult EditQuiz(int ID)
        {
            //get quiz by supplied ID and check if this User is its owner
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(ID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), ID);

            if (quiz == null)
                return HttpNotFound();

            EditQuizViewModel model = new EditQuizViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Modify Quiz",
                PageDescription = "Modify this quiz."
            };

            model.ID = quiz.ID;
            model.Name = quiz.Name;
            model.Description = quiz.Description;

            model.QuizTypes = new List<QuizType>() { QuizType.Text, QuizType.Image };
            model.SelectedQuizType = quiz.QuizType;

            model.EnableQuizTimer = quiz.EnableQuizTimer;

            if (model.EnableQuizTimer)
            {
                model.Hours = quiz.TimeDuration.Hours;
                model.Minutes = quiz.TimeDuration.Minutes;

                model.EnableQuestionTimer = quiz.EnableQuestionTimer;
            }
            else
            {
                model.Hours = 0;
                model.Minutes = 0;

                model.EnableQuestionTimer = false;
            }

            model.NoOfQuestions = quiz.Questions.Count;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditQuiz(EditQuizViewModel model)
        {
            //get quiz by supplied ID and check if this User is its owner
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(model.ID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), model.ID);

            if (quiz == null)
                return HttpNotFound();

            //check if Model is valid
            if (!ModelState.IsValid)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Quiz",
                    PageDescription = "Modify this quiz."
                };

                return View(model);
            }

            //populate quiz from Model
            //quiz.OwnerID = User.Identity.GetUserId(); //<- Do not change owner of Quiz on update. Reason: if the Super Admin updated the quiz, it will no longer show in original owner
            quiz.Name = model.Name;
            quiz.Description = model.Description;

            //quiz type should not be changed because this will affect the display of questions 
            //quiz.QuizType = model.SelectedQuizType;

            quiz.EnableQuizTimer = model.EnableQuizTimer;

            if (quiz.EnableQuizTimer)
            {
                quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
                quiz.EnableQuestionTimer = model.EnableQuestionTimer;
            }
            else
            {
                quiz.TimeDuration = new TimeSpan(0, 0, 0);
                quiz.EnableQuestionTimer = false;
            }
            quiz.ModifiedOn = DateTime.Now;

            if (await QuizzesService.Instance.UpdateQuiz(quiz))
            {
                //now redirect to Quiz listing Page
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteQuiz(EditQuizViewModel model)
        {
            //get quiz by supplied ID and check if this User is its owner
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(model.ID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), model.ID);
            
            if (quiz == null)
                return HttpNotFound();

            quiz.ModifiedOn = DateTime.Now;

            if (await QuizzesService.Instance.DeleteQuiz(quiz))
            {
                //now redirect to Quiz listing Page
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }
    }
}