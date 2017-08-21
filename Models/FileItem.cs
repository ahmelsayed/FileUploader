using System;

namespace FileUploader.Models
{
    public class FileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}