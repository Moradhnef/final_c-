using EasyTestMaker.Services;
using System.IO;
using System.Windows.Media.Imaging;
using System;

public class ImageService : IImageService
{
    // Set the base directory path for storing images
    private string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Temp", "img");

    // Add an image to the specified directory
    public string AddImage(string imagePath, Guid newPath)
    {
        // Combine the base path with the new path to create a directory path for the image
        var directoryPath = Path.Combine(_basePath, newPath.ToString());

        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Copy the image to the directory
        var newImagePath = Path.Combine(directoryPath, Path.GetFileName(imagePath));
        File.Copy(imagePath, newImagePath);

        // Return the full path to the copied image
        return newImagePath;
    }

    // Retrieve an image from the specified path and return it as a BitmapImage
    public BitmapImage GetMedia(string path)
    {
        BitmapImage bitmap = new BitmapImage();

        // Initialize the BitmapImage and set the URI source to the path
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(path, UriKind.Absolute);
        bitmap.EndInit();

        // Return the BitmapImage
        return bitmap;
    }
}
