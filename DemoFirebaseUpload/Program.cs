using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DemoFirebaseUpload
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stream = File.Open(@"C:\Users\thang\Documents\GitHub\CoStudy.WebAPI\DemoFirebaseUpload\Image\level1.png", FileMode.Open);

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage("costudy-c5390.appspot.com").Child("demo").Child(Guid.NewGuid().ToString())
                .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = (await task).ToString();
            Console.WriteLine(downloadUrl);
            Console.Read();
        }
    }
}
