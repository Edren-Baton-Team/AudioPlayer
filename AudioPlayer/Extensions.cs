using System.IO;

namespace AudioPlayer
{
    public static class Extensions
    {
        //Creates a directory if it does not exist.
        public static void CreateDirectory()
        {
            if (!Directory.Exists(Plugin.plugin.AudioPath))
            {
                Directory.CreateDirectory(Plugin.plugin.AudioPath);
            }
        }
    }
}