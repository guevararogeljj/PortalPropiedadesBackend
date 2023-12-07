using System.Text;

namespace Utilities
{
    public static class ReadFile
    {
        public static string ReadAll(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }
    }
}
