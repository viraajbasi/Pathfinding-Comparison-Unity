using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Maze
{
    public static class SaveScript
    {
        public static void SaveToFile(string fileName, string algName, string totNodes, string totVisNodes, string totPathNodes, string algTimeTaken, string totTimeTaken, string avgTimeTaken)
        {
            var destination = $"{Application.persistentDataPath}/{fileName}";
            var currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var headerString = $"Statistics calculated on {currentDate}.";

            var data = new List<string>()
            {
                headerString,
                algName,
                totNodes,
                totVisNodes,
                totPathNodes,
                algTimeTaken,
                totTimeTaken,
                avgTimeTaken
            };
            
            var streamWriter = new StreamWriter(destination);
            streamWriter.Write(data);
        }
    }
}
