using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace FileUploader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapMvcAttributeRoutes();
            MakeSureUploadsFolderExists();
        }

        private void MakeSureUploadsFolderExists()
        {
            try
            {
                if (!Directory.Exists(Server.MapPath("~/uploads/files")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/uploads/files"));
                }
            }
            catch
            {
                // Log error somewhere
            }
        }
    }
}
