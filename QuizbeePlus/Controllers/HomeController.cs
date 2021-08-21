using QuizbeePlus.Services;
using QuizbeePlus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using QuizbeePlus.Helpers;
using QuizbeePlus.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

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

            if((User.IsInRole("MockUser") || Request.QueryString["user"]=="mockuser") && quizzesSearch.Quizzes.Count>0)
            {               
               
                if (Request.UrlReferrer!=null && Request.UrlReferrer.ToString().Contains(ConfigurationManager.AppSettings["referrerdomain"]))
                {
                    return RedirectToAction(quizzesSearch.Quizzes[0].ID.ToString(), "attempt-quiz", new { user = "mockuser" });
                }
            }

            return View(model);

        }

        public ActionResult CreateUser(string createUser , int ID)
        {
            if(createUser=="newuser")
            {              
               
                return RedirectToAction(createUser, "loginUser", new { user = "mockuser",quizId= ID });
                // Redirect To Quiz
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }


            public ActionResult AvatarPage()
        {
            return View();
        }
    }
}