using System.IO;
using System.Linq;
using UnityEngine;

namespace AudioPlayer
{
    public static class Extensions
    {
        //Checks if a file exists
        public static bool AudioFileExist()
        {
            var files = Directory.GetFiles(Plugin.plugin.AudioPath);
            return files?.Length > 0 && files.FirstOrDefault(a => a.EndsWith(".ogg")) != null;
        }
        //Gets the path to the file
        public static string GetAudioFilePath()
        {
            var files = Directory.GetFiles(Plugin.plugin.AudioPath);
            var audios = files.Where(a => a.EndsWith(".ogg"));

            return audios.Any() ? audios.ElementAtOrDefault(Random.Range(0, audios.Count())) : null;
        }
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