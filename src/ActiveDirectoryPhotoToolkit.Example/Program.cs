using System;
using System.Collections.Generic;
using System.IO;
using Console = System.Console;

namespace ActiveDirectoryPhotoToolkit.Example
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Active Directory Photo Uploaded");

            // Directories
            const string photoDirectory = @"C:\Photos";
            const string uploadedDirectory = @"C:\Photos\Uploaded\";

            // Setup
            Console.WriteLine("Setting up ActiveDirectoryPhoto...");
            var activeDirectoryPhoto = new ActiveDirectoryPhoto();

            // Get photos to be uploaded
            var photos = Directory.GetFiles(photoDirectory);

            foreach (var photo in photos)
            {
                Console.WriteLine("\n####################\n");

                Console.WriteLine("Extracting file information....");
                var fileName = Path.GetFileName(photo);
                var username = Path.GetFileNameWithoutExtension(photo);
                var uploadedFile = Path.Combine(uploadedDirectory, fileName);

                Console.WriteLine("Username: " + username);
                Console.WriteLine("Photo Location: " + photo);
                Console.WriteLine("File name: " + fileName);
                Console.WriteLine("Upload Directory: " + uploadedDirectory);

                Console.WriteLine("Setting user photo...");
                activeDirectoryPhoto.SetThumbnailPhoto(username, photo);

                Console.WriteLine($"Moving {fileName}....");
                File.Move(photo, uploadedFile);
            }

            Console.WriteLine("Photo upload completed successfully");
        }
    }
}