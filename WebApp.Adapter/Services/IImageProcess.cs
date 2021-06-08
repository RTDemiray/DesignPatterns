using System.IO;

namespace BaseProject.Services
{
    public interface IImageProcess
    {
        void AddWaterMark(string text, string filename, Stream imageStream);
    }
}