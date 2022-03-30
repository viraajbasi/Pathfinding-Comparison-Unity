using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Maze
{
    public static class SaveScript
    {
        public static void SaveToFile(List<string> dijkstraList, List<string> aStarList, List<string> bellmanFordList, List<string> generalStats)
        {
            var currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var fileName = DateTime.Now.ToString("yyyMMdd");
            var destination = $"{Application.persistentDataPath}/{fileName}.txt";
            var headerString = $"Statistics calculated on {currentDate}.";
            var streamWriter = new StreamWriter(destination);
            
            streamWriter.WriteLine(headerString);
            
            streamWriter.WriteLine("Dijkstra:");
            foreach (var str in dijkstraList)
            {
                streamWriter.WriteLine(str);
            }
            
            streamWriter.WriteLine("A*:");
            foreach (var str in aStarList)
            {
                streamWriter.WriteLine(str);
            }
            
            streamWriter.WriteLine("Bellman-Ford:");
            foreach (var str in bellmanFordList)
            {
                streamWriter.WriteLine(str);
            }
            
            streamWriter.WriteLine("General Statistics:");
            foreach (var str in generalStats)
            {
                streamWriter.WriteLine(str);
            }
            
            streamWriter.Close();
            
            PlayerPrefs.SetString("FileLocation", destination);
        }
    }
}
