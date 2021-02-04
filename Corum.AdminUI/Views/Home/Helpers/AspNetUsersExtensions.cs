using System.Web;
using System.Web.Mvc;
using Corum.Models;


namespace Corum.Helpers
{
    public static class ExtensionMethods
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }
    }

    public static class AspNetUsersExtensions
    {
        public static ICorumDataProvider context { get; set; }
        
        static AspNetUsersExtensions()
        {
            context = DependencyResolver.Current.GetService<ICorumDataProvider>();
        }

        public static string GetDisplayName(string userId)
        {
            var user = context.getUser(userId);
            return (user == null) ? string.Empty : user.displayName ?? user.userEmail;
        }

        public static bool IsUserAdmin(string userId)
        {            
            return context.UserHasRole(userId,   "1000");
        }
    }

    
    
}