namespace QuizbeePlus.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using QuizbeePlus.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuizbeePlus.Data.QuizbeeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QuizbeePlus.Data.QuizbeeContext";
        }

        protected override void Seed(QuizbeePlus.Data.QuizbeeContext context)
        {
            //  This method will be called after migrating to the latest version.
        }

    }
}
