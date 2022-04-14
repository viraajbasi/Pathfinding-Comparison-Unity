using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Maze
{
    public static class SaveScript
    {
        public static void SaveStatsToFile(List<string> dijkstraList, List<string> aStarList, List<string> bellmanFordList, List<string> generalStats)
        {
            // Finds current date and time for the statistics value.
            // CultureInfo.CurrentCulture ensures that its in the format of the region of the computer.
            var currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            
            // Finds filename from PlayerPrefs.
            var fileName = PlayerPrefs.GetString("FileName");
            
            // Environment.SpecialFolder.DesktopDirectory is an OS-agnostic way of finding the desktop directory.
            var destination = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/Statistics-{fileName}.txt";
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
