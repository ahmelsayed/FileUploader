using FileUploader.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploader.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route()]
        [Route("Home")]
        [Route("Home/Index")]
        public ActionResult Index()
        {
            var files = Directory
                // Server.MapPath takes a relative path to the site root (~) and maps it to the file system.
                // Meaning if you're running your site from say C:\MyFiles\FileUploader, then the uploads folder
                // should be under 'C:\MyFiles\FileUploader\uploads\files', therefore you can guess that
                // Server.MapPath("~") becomes "C:\MyFiles\FileUploader" and
                // Server.MapPath("~/uploads/files") becomes "C:\MyFiles\FileUploader\uploads\files"
                // GetFiles itself will list all the files in that folder under all folders in there
                .GetFiles(Server.MapPath("~/uploads/files"), "*", SearchOption.AllDirectories)
                // Select maps files. Basically the call above to GetFiles returns a list of files like
                // [
                //     "C:\MyFiles\FileUploader\uploads\files\file1.zip",
                //     "C:\MyFiles\FileUploader\uploads\files\file2.zip",
                //     "C:\MyFiles\FileUploader\uploads\files\file3.zip",
                //     "C:\MyFiles\FileUploader\uploads\files\file4.zip",
                // ]
                // for example
                // Select (called map in other programming languages) maps every item to something else
                // for example calling [1, 2, 3].Select(i => i * 2) will give you [2, 4, 6]
                // In here I'm mapping a file from something that looks like this "C:\MyFiles\FileUploader\uploads\files\file1.zip"
                // To an object that looks like this 
                // {
                //     Id = 0,
                //     Name = "file1.zip",
                //     Uri = "http://whatEverName/uploads/files/file1.zip"
                // }
                .Select((file, i) => new FileItem
                {
                    Id = i,
                    Name = Path.GetFileName(file),
                    Uri = new Uri($"{Request.Url.Scheme}://{Request.Url.Authority}/uploads/files/{Path.GetFileName(file)}")
                });

            // We then pass that list to the view to display it. Look into Home\Index.cshtml for details about this
            return View(files);
        }

        [HttpPost]
        [Route("UploadFile")]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            // if the user just clicks upload without selecting a file, this will be null
            // We can block the click itself, but who cares.
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/uploads/files"), fileName);
                file.SaveAs(path);
            }

            // After saving the file, redirect to the Index page again.
            return RedirectToAction("Index");
        }

        [HttpDelete]
        [Route("Delete/{fileName}")]
        public ActionResult DeleteFile(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/uploads/files"), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return RedirectToAction("Index");
        }
    }
}