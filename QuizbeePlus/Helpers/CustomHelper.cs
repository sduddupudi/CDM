using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace QuizbeePlus.Helpers
{
    public class CustomHelper
    {
        

        public static bool isMockUser(IPrincipal CurrentUser)
        {
            if(CurrentUser.IsInRole("MockUser"))
            {
                return true;
            }
            return false;
        }

        public static bool isGeneralUser(IPrincipal CurrentUser)
        {
            if (CurrentUser.Identity.Name!="")
            {
                if(!CurrentUser.IsInRole("MockUser"))
                {
                    return true;
                }
            }
            return false;
        }
        // 1*100/15 = 50, 2*100/2=100
         public static string GetPercentageProgress(int TotalProgress,int CurrentProgress)
        {
            string PercentageString = string.Format("{0}%", CurrentProgress * 100 / TotalProgress);
            return PercentageString;
        }       

    }
}