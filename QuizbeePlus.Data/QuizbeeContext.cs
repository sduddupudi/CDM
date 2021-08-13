using Microsoft.AspNet.Identity.EntityFramework;
using QuizbeePlus.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Data
{
    public class QuizbeeContext : IdentityDbContext<QuizbeeUser>, IDisposable
    {
        public QuizbeeContext()
            : base("QuizbeePlusConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<QuizbeeContext>(new CreateDatabaseIfNotExists<QuizbeeContext>());

            Database.SetInitializer<QuizbeeContext>(new QuizbeeDatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<QuizbeeUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
        }

        public static QuizbeeContext Create()
        {
            return new QuizbeeContext();
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        public DbSet<AttemptedQuestion> AttemptedQuestions { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
