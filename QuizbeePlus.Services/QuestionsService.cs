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
    public class QuestionsService
    {
        #region Define as Singleton
        private static QuestionsService _Instance;

        public static QuestionsService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new QuestionsService();
                }

                return (_Instance);
            }
        }

        private QuestionsService()
        {
        }
        #endregion
        

        public QuestionsSearch GetQuizQuestions(int quizID, string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new QuestionsSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Questions = context.Questions
                                        .Where(q => q.QuizID == quizID)
                                        .Include(x => x.Options)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Questions.Where(q => q.QuizID == quizID).Count();
                }
                else
                {
                    search.Questions = context.Questions
                                        .Where(q => q.QuizID == quizID)
                                        .Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(x => x.Options)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                    search.TotalCount = context.Questions.Where(q => q.QuizID == quizID)
                                        .Where(x => x.Title.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }

        public async Task<bool> SaveNewQuestion(Question question)
        {
            using (var context = new QuizbeeContext())
            {
                context.Questions.Add(question);

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            using (var context = new QuizbeeContext())
            {
                //get the question by ID
                var oldQuestion = context.Questions.Find(question.ID);

                //delete this old question with all options
                context.Options.RemoveRange(oldQuestion.Options);
                context.Questions.Remove(oldQuestion);

                //add a new question with new options
                context.Questions.Add(question);

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> DeleteQuestion(Question question)
        {
            using (var context = new QuizbeeContext())
            {
                context.Questions.Attach(question);
                
                context.Options.RemoveRange(question.Options);
                context.Questions.Remove(question);
                
                return await context.SaveChangesAsync() > 0;                
            }
        }
                
        public Question GetQuizQuestion(int quizID, int ID)
        {
            using (var context = new QuizbeeContext())
            {
                return context.Questions
                               .Where(q => q.QuizID == quizID && q.ID == ID)
                               .Include(x => x.Options)
                               .Include("Options.Image")
                               .FirstOrDefault();
            }            
        }

        public List<Option> GetQuestionOptions(int questionID, List<int> optionIDs)
        {
            using (var context = new QuizbeeContext())
            {
                var question = context.Questions.Where(x => x.ID == questionID).Include(x => x.Options).FirstOrDefault();

                if (question == null) return null;

                List<Option> Options = new List<Option>();

                foreach (var optionID in optionIDs)
                {
                    var option = question.Options.Where(x=>x.ID == optionID).FirstOrDefault();

                    if (option != null) Options.Add(option);
                }

                return Options;
            }
        }

        public List<Option> GetOptionsByIDs(List<int> optionIDs)
        {
            using (var context = new QuizbeeContext())
            {
                var options = new List<Option>();

                if (optionIDs != null)
                {
                    foreach (var ID in optionIDs)
                    {
                        var option = context.Options.Find(ID);

                        if (option != null) options.Add(option);
                    }
                }

                return options;
            }
        }
    }
}
