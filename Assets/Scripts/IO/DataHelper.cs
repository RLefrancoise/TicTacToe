using System.IO;
using System.Text;
using UnityEngine;

namespace TicTacToe.IO
{
    /// <summary>
    /// Data helper class
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Get JSON content from data folder. If file doesn't exist, return empty data.
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <typeparam name="T">Type of data to get</typeparam>
        /// <returns>data inside the file</returns>
        public static T GetJsonContent<T>(string fileName) where T : new()
        {
            if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            if(!File.Exists(filePath)) return new T();
            
            var content = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(content);
        }

        /// <summary>
        /// Write data to JSON file in data folder
        /// </summary>
        /// <param name="data">data to write</param>
        /// <param name="fileName">name of the file</param>
        /// <typeparam name="T">Type of data to write</typeparam>
        public static void WriteToJson<T>(T data, string fileName)
        {
            if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            var content = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        /// <summary>
        /// Is file existing in data folder ?
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns>true if file exists</returns>
        public static bool FileExists(string fileName)
        {
            if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            return File.Exists(filePath);
        }

        /// <summary>
        /// Delete a file in data folder
        /// </summary>
        /// <param name="fileName">name of the file to delete</param>
        public static void DeleteFile(string fileName)
        {
            if (!FileExists(fileName)) return;
            
            if (!fileName.EndsWith(".json")) fileName = $"{fileName}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            File.Delete(filePath);
            File.Delete($"{filePath}.meta");
        }
    }
}