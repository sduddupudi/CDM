using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizbeePlus.Commons
{
    public static class Variables
    {
        public static string ImageFolderPath { get; set; } = "/Content/images/";

        public static string Administrator { get; set; } = "Administrator";
        public static string Manager { get; set; } = "Manager";
        public static string UserRole { get; set; } = "User";
        public static string MockUserRole { get; set; } = "MockUser";
        public static string RandomString { get {
                return "";
            }
        }

        public static string NewMockUser { 
            get {               
                string MockUsername = string.Format("MockUser_{0}", Guid.NewGuid().ToString().Replace("-", "_")); ;
                return MockUsername; }}

        public static string NewMockUserEMail
        {
            get
            {
                string MockUsername = string.Format("{0}@email.com", Guid.NewGuid().ToString().Replace("-", "_")); ;
                return MockUsername;
            }
        }

    }
}