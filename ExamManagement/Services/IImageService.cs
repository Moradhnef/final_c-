using System;
using System.Windows.Media.Imaging;

namespace EasyTestMaker.Services
{
    public interface IImageService
    {
        string AddImage(string imagePath, Guid newPath);
        BitmapImage GetMedia(string path);
    }
}