using Microsoft.AspNet.Identity;
using QuizbeePlus.Commons;
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
    public class QuestionController : BaseController
    {
        public ActionResult Index(int quizID, string search, int? page, int? items)
        {
            QuestionListViewModel model = new QuestionListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Questions",
                PageDescription = "List of Questions for Selected Quiz."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;
            
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(quizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), quizID);
            
            if (quiz != null)
            {
                model.QuizID = quizID;

                var result = QuestionsService.Instance.GetQuizQuestions(quiz.ID, model.searchTerm, model.pageNo, model.pageSize);

                model.Questions = result.Questions;
                model.TotalCount = result.TotalCount;

                model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

                return View(model);
            }
            else return HttpNotFound();
        }
        [CustomAuthorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult NewQuestion(int quizID)
        {
            NewQuestionViewModel model = new NewQuestionViewModel();
            
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(quizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), quizID);

            if (quiz == null) return HttpNotFound();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Add New Question",
                PageDescription = "Add questions to selected quiz."
            };

            model.QuizID = quiz.ID;
            model.QuizType = quiz.QuizType;

            return View(model);
        }
        [CustomAuthorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> NewQuestion(FormCollection collection)
        {
            NewQuestionViewModel model = new NewQuestionViewModel();

            model.QuizID = GetQuizIDFromCollection(collection);

            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(model.QuizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), model.QuizID);
            
            if (quiz == null) return HttpNotFound();

            if (quiz.QuizType == QuizType.Image)
            {
                model = GetNewImageQuestionViewModelFromFormCollection(model, collection);
            }
            else
            {
                model = GetNewTextQuestionViewModelFromFormCollection(model, collection);
            }

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Add New Question",
                PageDescription = "Add questions to selected quiz."
            };

            if (string.IsNullOrEmpty(model.Title) || model.CorrectOptions.Count == 0 || model.Options.Count == 0)
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    ModelState.AddModelError("Title", "Please enter question title.");
                }

                if (model.CorrectOptions.Count == 0)
                {
                    ModelState.AddModelError("CorrectOptions", "Please enter some correct options.");
                }

                if (model.Options.Count == 0)
                {
                    ModelState.AddModelError("Options", "Please enter some other options.");
                }

                model.QuizType = quiz.QuizType;

                return View(model);
            }

            var question = new Question();

            question.QuizID = quiz.ID;
            question.Title = model.Title;

            question.Options = new List<Option>();
            question.Options.AddRange(model.CorrectOptions);
            question.Options.AddRange(model.Options);

            question.ModifiedOn = DateTime.Now;

            if (await QuestionsService.Instance.SaveNewQuestion(question))
            {
                return RedirectToAction("NewQuestion", new { quizID = question.QuizID });
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [CustomAuthorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult EditQuestion(int quizID, int ID)
        {
            EditQuestionViewModel model = new EditQuestionViewModel();
            
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(quizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), quizID);

            if (quiz == null) return HttpNotFound();

            var question = QuestionsService.Instance.GetQuizQuestion(quizID, ID);

            if (question == null) return HttpNotFound();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Modify Question",
                PageDescription = "Modify selected question."
            };

            model.ID = question.ID;
            model.QuizID = question.QuizID;
            model.QuizType = quiz.QuizType;
            model.Title = question.Title;
            model.CorrectOptions = question.Options.Where(q => q.IsCorrect).ToList();
            model.Options = question.Options.Where(q => !q.IsCorrect).ToList();

            return View(model);
        }
        [CustomAuthorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> EditQuestion(FormCollection collection)
        {
            EditQuestionViewModel model = new EditQuestionViewModel();

            model.QuizID = GetQuizIDFromCollection(collection);
            
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(model.QuizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), model.QuizID);

            if (quiz == null) return HttpNotFound();

            if (quiz.QuizType == QuizType.Image)
            {
                model = GetEditImageQuestionViewModelFromFormCollection(model, collection);
            }
            else
            {
                model = GetEditTextQuestionViewModelFromFormCollection(model, collection);
            }

            var question = QuestionsService.Instance.GetQuizQuestion(model.QuizID, model.ID);

            if (question == null) return HttpNotFound();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Modify Question",
                PageDescription = "Modify selected question."
            };

            if (model == null || string.IsNullOrEmpty(model.Title) || model.CorrectOptions.Count == 0 || model.Options.Count == 0)
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    ModelState.AddModelError("Title", "Please enter question title.");
                }

                if (model.CorrectOptions.Count == 0)
                {
                    ModelState.AddModelError("CorrectOptions", "Please enter some correct options.");
                }

                if (model.Options.Count == 0)
                {
                    ModelState.AddModelError("Options", "Please enter some other options.");
                }

                model.QuizType = quiz.QuizType;

                return View(model);
            }

            question.QuizID = model.QuizID;
            question.Title = model.Title;

            question.Options = new List<Option>();
            question.Options.AddRange(model.CorrectOptions);
            question.Options.AddRange(model.Options);

            question.ModifiedOn = DateTime.Now;

            if (await QuestionsService.Instance.UpdateQuestion(question))
            {
                return RedirectToAction("Index", new { quizID = question.QuizID });
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }
        [CustomAuthorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> DeleteQuestion(FormCollection collection)
        {
            EditQuestionViewModel model = new EditQuestionViewModel();

            model.QuizID = GetQuizIDFromCollection(collection);
            
            var quiz = UserHasRights() ? QuizzesService.Instance.GetQuizForAdmin(model.QuizID) : QuizzesService.Instance.GetUserQuiz(User.Identity.GetUserId(), model.QuizID);

            if (quiz == null) return HttpNotFound();

            if (quiz.QuizType == QuizType.Image)
            {
                model = GetEditImageQuestionViewModelFromFormCollection(model, collection);
            }
            else
            {
                model = GetEditTextQuestionViewModelFromFormCollection(model, collection);
            }

            var question = QuestionsService.Instance.GetQuizQuestion(model.QuizID, model.ID);

            if (question == null) return HttpNotFound();

            if (await QuestionsService.Instance.DeleteQuestion(question))
            {
                return RedirectToAction("Index", new { quizID = question.QuizID });
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        private NewQuestionViewModel GetNewImageQuestionViewModelFromFormCollection(NewQuestionViewModel model, FormCollection collection)
        {
            model.Options = new List<Option>();
            model.CorrectOptions = new List<Option>();

            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "Title")
                    {
                        model.Title = collection[key];
                    }
                    else if (key.Contains("correctOption")) //this must be Correct Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            try
                            {
                                var correctOption = new Option();
                                correctOption.Image = ImagesService.Instance.GetImage(int.Parse(collection[key]));
                                correctOption.IsCorrect = true;
                                correctOption.ModifiedOn = DateTime.Now;

                                model.CorrectOptions.Add(correctOption);
                            }
                            catch (Exception ex)
                            {
                                //ignore this option
                            }
                        }
                    }
                    else if (key.Contains("option")) //this must be Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        { 
                            try
                            {
                                var option = new Option();
                                option.Image = ImagesService.Instance.GetImage(int.Parse(collection[key]));
                                option.IsCorrect = false;
                                option.ModifiedOn = DateTime.Now;

                                model.Options.Add(option);
                            }
                            catch (Exception ex)
                            {
                                //ignore this option
                            }
                        }
                    }
                }
            }

            return model;
        }
        
        private NewQuestionViewModel GetNewTextQuestionViewModelFromFormCollection(NewQuestionViewModel model, FormCollection collection)
        {
            model.Options = new List<Option>();
            model.CorrectOptions = new List<Option>();

            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "Title")
                    {
                        model.Title = collection[key];
                    }
                    else if (key.Contains("correctOption")) //this must be Correct Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            var correctOption = new Option();                            
                            correctOption.Answer = collection[key];
                            correctOption.IsCorrect = true;
                            correctOption.ModifiedOn = DateTime.Now;

                            model.CorrectOptions.Add(correctOption);
                        }
                    }
                    else if (key.Contains("option")) //this must be Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            var option = new Option();
                            option.Answer = collection[key];
                            option.IsCorrect = false;
                            option.ModifiedOn = DateTime.Now;

                            model.Options.Add(option);
                        }
                    }
                }
            }

            return model;
        }

        private EditQuestionViewModel GetEditImageQuestionViewModelFromFormCollection(EditQuestionViewModel model, FormCollection collection)
        {
            model.Options = new List<Option>();
            model.CorrectOptions = new List<Option>();

            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "ID")
                    {
                        model.ID = int.Parse(collection[key]);
                    }
                    else if (key == "Title")
                    {
                        model.Title = collection[key];
                    }
                    else if (key.Contains("correctOption")) //this must be Correct Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            try
                            {
                                var correctOption = new Option();
                                correctOption.Image = ImagesService.Instance.GetImage(int.Parse(collection[key]));
                                correctOption.IsCorrect = true;
                                correctOption.ModifiedOn = DateTime.Now;

                                model.CorrectOptions.Add(correctOption);
                            }
                            catch
                            {
                                //ignore this option
                            }
                        }
                    }
                    else if (key.Contains("option")) //this must be Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            try
                            {
                                var option = new Option();
                                option.Image = ImagesService.Instance.GetImage(int.Parse(collection[key]));
                                option.IsCorrect = false;
                                option.ModifiedOn = DateTime.Now;

                                model.Options.Add(option);
                            }
                            catch
                            {
                                //ignore this option
                            }
                        }
                    }
                }
            }

            return model;
        }

        private EditQuestionViewModel GetEditTextQuestionViewModelFromFormCollection(EditQuestionViewModel model, FormCollection collection)
        {
            model.Options = new List<Option>();
            model.CorrectOptions = new List<Option>();

            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "ID")
                    {
                        model.ID = int.Parse(collection[key]);
                    }
                    else if (key == "Title")
                    {
                        model.Title = collection[key];
                    }
                    else if (key.Contains("correctOption")) //this must be Correct Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            var correctOption = new Option();
                            correctOption.Answer = collection[key];
                            correctOption.IsCorrect = true;
                            correctOption.ModifiedOn = DateTime.Now;

                            model.CorrectOptions.Add(correctOption);
                        }
                    }
                    else if (key.Contains("option")) //this must be Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            var option = new Option();
                            option.Answer = collection[key];
                            option.IsCorrect = false;
                            option.ModifiedOn = DateTime.Now;

                            model.Options.Add(option);
                        }
                    }
                }
            }

            return model;
        }
        
        private int GetQuizIDFromCollection(FormCollection collection)
        {
            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "QuizID")
                    {
                        return int.Parse(collection[key]);
                    }
                }
            }

            return 0;
        }
    }
}