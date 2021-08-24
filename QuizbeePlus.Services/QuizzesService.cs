using QuizbeePlus.Data;
using QuizbeePlus.Entities;
using QuizbeePlus.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Services
{
    public class QuizzesService
    {
        #region Define as Singleton
        private static QuizzesService _Instance;

        public static QuizzesService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new QuizzesService();
                }

                return (_Instance);
            }
        }

        private QuizzesService()
        {
        }
        #endregion

        public QuizzesSearch GetQuizzes(string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Quizzes = context.Quizzes
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Count();
                }
                else
                {
                    search.Quizzes = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public QuizzesSearch GetQuizzesForHomePage(string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Quizzes = context.Quizzes
                                        .Include(q => q.Questions)
                                        .Where(x => x.Questions.Count > 0)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Where(x => x.Questions.Count > 0).Count();
                }
                else
                {
                    search.Quizzes = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(q => q.Questions)
                                        .Where(x => x.Questions.Count > 0)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Where(x => x.Questions.Count > 0)
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public QuizzesSearchStudent GetQuizzesForHomePageWithStudent(string userID,string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuizzesSearchStudent();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Quizzes = context.Quizzes
                                        .Include(q => q.Questions)
                                        .Where(x => x.Questions.Count > 0)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.StudentQuizzesResult = (from quiz in context.Quizzes
                                 join quizattempt in context.StudentQuizzes
                               on quiz.ID equals quizattempt.QuizID
                                where quizattempt.StudentID == userID
                                select new StudentQuizResult { QuizID= quiz.ID, StartedAt = quizattempt.StartedAt,CompletedAt= quizattempt.CompletedAt }).ToList<StudentQuizResult>();                     

                    search.TotalCount = context.Quizzes.Where(x => x.Questions.Count > 0).Count();
                }
                else
                {
                    search.Quizzes = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(q => q.Questions)
                                        .Where(x => x.Questions.Count > 0)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    var Query = (from quiz in context.Quizzes
                                 join quizattempt in context.StudentQuizzes
                               on quiz.ID equals quizattempt.QuizID
                                 where quizattempt.StudentID == userID
                                 select new { quiz.ID, quizattempt.StartedAt, quizattempt.CompletedAt }).ToList();






                    search.TotalCount = context.Quizzes.Where(x => x.Questions.Count > 0)
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public async Task<bool> SaveNewQuiz(Quiz quiz)
        {
            using (var context = new QuizbeeContext())
            {
                context.Quizzes.Add(quiz);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateQuiz(Quiz quiz)
        {
            using (var context = new QuizbeeContext())
            {
                context.Entry(quiz).State = System.Data.Entity.EntityState.Modified;

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> DeleteQuiz(Quiz quiz)
        {
            using (var context = new QuizbeeContext())
            {
                context.Quizzes.Attach(quiz);

                quiz.Questions.ForEach(qu => context.Options.RemoveRange(qu.Options));
                context.Questions.RemoveRange(quiz.Questions);
                context.Quizzes.Remove(quiz);

                return await context.SaveChangesAsync() > 0;
            }
        }

        public QuizzesSearch GetUserQuizzes(string userID, string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Quizzes = context.Quizzes
                                        .Where(q => q.OwnerID == userID)
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Where(q => q.OwnerID == userID).Count();
                }
                else
                {
                    search.Quizzes = context.Quizzes
                                        .Where(q => q.OwnerID == userID)
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Where(q => q.OwnerID == userID)
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public QuizzesSearch GetQuizzesForAdmin(string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuizzesSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Quizzes = context.Quizzes
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes.Count();
                }
                else
                {
                    search.Quizzes = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(q => q.Questions)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Quizzes
                                        .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public Quiz GetUserQuiz(string userID, int ID)
        {
            using (var context = new QuizbeeContext())
            {
                return context.Quizzes
                                    .Where(q => q.OwnerID == userID && q.ID == ID)
                                    .Include(q => q.Questions)
                                    .FirstOrDefault();
            }
        }
        
        public Quiz GetQuizForAdmin(int ID)
        {
            using (var context = new QuizbeeContext())
            {
                return context.Quizzes
                                    .Where(q => q.ID == ID)
                                    .Include(q => q.Questions)
                                    .FirstOrDefault();
            }
        }

        public Quiz GetQuiz(int ID)
        {
            using (var context = new QuizbeeContext())
            {
                return context.Quizzes
                                    .Where(q => q.ID == ID)
                                    .Include(q => q.Questions)
                                    .Include("Questions.Image")
                                    .Include("Questions.Options")
                                    .Include("Questions.Options.Image")
                                    .Where(q=>q.Questions.Count > 0) //get quiz with Questions greater than 0
                                    .FirstOrDefault();
            }
        }

        public Quiz GetQuizWithUnAttemptedQuestions(int ID,int StudentQuizId)
        {
            using (var context = new QuizbeeContext())
            {
                var Quizze= context.Quizzes
                                    .Where(q => q.ID == ID)
                                    .Include(q => q.Questions)
                                    .Include("Questions.Image")
                                    .Include("Questions.Options")
                                    .Include("Questions.Options.Image")
                                    .Where(q => q.Questions.Count > 0) //get quiz with Questions greater than 0
                                    .FirstOrDefault();

                var QuestionsAttempted = context.AttemptedQuestions.Where(x => x.StudentQuizID == StudentQuizId)
                    .Include(q => q.QuestionID);

                return Quizze;

            }
        }
    }
}
