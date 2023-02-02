using System.IO;
using System.Linq;
using System.Reflection;
using MapGeneration;
using Mirror;
using PluginAPI.Core;
using PluginAPI.Helpers;
using UnityEngine;

namespace AudioPlayer
{
    public static class Extensions
    {
        /// <summary>
        /// Checks for .ogg files in the sounds folder
        /// </summary>
        /// <returns></returns>
        public static bool AudioFileExist()
        {
            var files = Directory.GetFiles(Plugin.plugin.AudioPath);
            return files?.Length > 0 && files.FirstOrDefault(a => a.EndsWith(".ogg")) != null;
        }


        /// <summary>
        /// Gets the audio files from the folder, if there is more than one it will take one at random.
        /// </summary>
        /// <returns></returns>
        public static string GetAudioFilePath()
        {
            var files = Directory.GetFiles(Plugin.plugin.AudioPath);
            var audios = files.Where(a => a.EndsWith(".ogg"));

            return audios.Any() ? audios.ElementAtOrDefault(Random.Range(0, audios.Count())) : null;
        }

        /// <summary>
        /// Create the Sound Directory if it does not exist
        /// </summary>
        public static void CreateDirectory()
        {
            if (!Directory.Exists(Plugin.plugin.AudioPath))
            {
                Directory.CreateDirectory(Plugin.plugin.AudioPath);
            }
        }
    }
}