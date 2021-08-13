using QuizbeePlus.Services;
using QuizbeePlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizbeePlus.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index(string search, int? page, int? items)
        {
            HomeViewModel model = new HomeViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Quizbee",
                PageDescription = "Quizbee helps you to create scalable and dynamic quizzes with any number of questions and related options. Creating and attempting Quizzes have never been this easy. Try it now!"
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 9;

            var quizzesSearch = QuizzesService.Instance.GetQuizzesForHomePage(model.searchTerm, model.pageNo, model.pageSize);

            model.Quizzes = quizzesSearch.Quizzes;
            model.TotalCount = quizzesSearch.TotalCount;

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            if(User.IsInRole("MockUser") && quizzesSearch.Quizzes.Count>0)
            {
                return RedirectToAction(quizzesSearch.Quizzes[0].ID.ToString(), "attempt-quiz");
            }

            return View(model);
        }
    }
}