using System.Drawing;
using System.IO;

namespace BaseProject.Services
{
    public interface IAdvanceImageProcess
    {
        void AddWaterMarkImage(Stream stream, string text, string filePath, Color color, Color outlineColor);
    }
}