using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace UBALDI_EmailValidation.Models
{
    public static class Logger
    {
        // Calculate the log file's name.
        private static string LogFile = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Log.txt";

        // Write the current date and time plus
        // a line of text into the log file.
        public static void WriteLine(string txt)
        {
            File.AppendAllText(LogFile, DateTime.Now.ToString() + ": " + txt + "\n");
        }

        // Delete the log file.
        public static void DeleteLog()
        {
            File.Delete(LogFile);
        }
    }
}