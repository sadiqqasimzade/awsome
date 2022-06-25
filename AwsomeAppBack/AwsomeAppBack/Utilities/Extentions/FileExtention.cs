using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AwsomeAppBack.Utilities.Extentions
{
    public static class FileExtention
    {
        public async static Task<string> SaveFile(this IFormFile File, string folderpath, int maxnamelength)
        {
            string filename = Guid.NewGuid().ToString() + File.FileName;
            if (filename.Length > maxnamelength)
                filename = filename.Substring(filename.Length - maxnamelength, maxnamelength);

            using (FileStream fs = new FileStream(Path.Combine(folderpath, filename), FileMode.Create))
                await File.CopyToAsync(fs);

            return filename;
        }

        public static void DeleteFile(this string path, string root)
        {
            if (File.Exists(Path.Combine(root, path)))
                File.Delete(Path.Combine(root, path));
        }
    }
}
