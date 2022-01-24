using Microsoft.Win32;
using System.IO;

namespace DraftDesktopApp.Services
{
    public class SimpleFileSelector : IFileSelector<byte[]>
    {
        public bool TryToSelect(out byte[] value, string pattern = "")
        {
            value = null;
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = pattern
            };
            if ((bool)dialog.ShowDialog())
            {
                value = File.ReadAllBytes(dialog.FileName);
                return true;
            }
            return false;
        }
    }
}
