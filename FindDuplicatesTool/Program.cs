using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FindDuplicatesTool
{
    class Program
    {
        public static class Globals
        {
            public static Logger report = new Logger(String.Format("C:\\Users\\lwpea\\FindDuplicatesTool\\{0}DuplicateReport.txt", DateTime.Now.ToString("yyyyMMddmmss")));
        }
        public static SHA256 Sha256 = SHA256.Create();

        static public void writeHeaderToReport(string dir1)
        {
            Globals.report.Print("DuplicatesReport {0}", DateTime.Now.ToString("MM/dd/yyyy"));
            Globals.report.Print("Finding Duplicates in: " + dir1);
        }
        static public void WriteTextToReport(string text)
        {
            Globals.report.Print(text);
        }


        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        public static byte[] ComputeFileHash(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }
        public class fileAndLength
        {
            public string fileName { get; set; }
            public long length { get; set; }
        }

        static void FindDuplicates(List<string> listOfFiles)
        {
           
            List<fileAndLength> files = new List<fileAndLength>();
            //List<string> filesInDirectory = Directory.GetFiles(folder).ToList();
            foreach (string file in listOfFiles)
            {
                files.Add(new fileAndLength
                {
                    fileName = new System.IO.FileInfo(file).FullName.ToString(),
                    length = new System.IO.FileInfo(file).Length
                });
            }

            var duplicateFiles = files.GroupBy(x => x.length)
                .Where(group => group.Count() > 1)
                 .SelectMany(group => group).ToList();

            if (duplicateFiles.Any())
            {
                foreach (var dup in duplicateFiles)
                {
                    WriteTextToReport("   " + dup.fileName + "   " + dup.length);
                }
                WriteTextToReport("----------------------------------------");
            }

        }
        static void Main(string[] args)
        {
            //List<fileAndLength> files = new List<fileAndLength>();
            Console.WriteLine("Enter Drive: ");
            string drive = Console.ReadLine().TrimEnd();
            writeHeaderToReport(drive);
            List<string> yearPhotoDirectories = Directory.GetDirectories(drive + @":\Photos").Where(y => y.Contains(" Photos")).ToList();
            List<string> yearVideoDirectories = Directory.GetDirectories(drive + @":\Home Videos").Where(y => y.Contains(" Home Videos")).ToList();
            WriteTextToReport("These files are the same length: ");
            foreach (var year in yearPhotoDirectories)
            {
                List<string> folders = Directory.GetDirectories(year).ToList();
                List<string> files = Directory.GetFiles(year).ToList();
                // if there's no months, look for duplicates in this year directory
                if (folders.Any()) { 
                    Console.WriteLine(year);
                    foreach (var month in folders)
                    {
                        Console.WriteLine(month);
                        //WriteTextToReport(month);
                        FindDuplicates(Directory.GetFiles(month).ToList());
                    }                   
                }
                if (files.Any())
                {
                    Console.WriteLine(year);
                    //WriteTextToReport(year);
                    FindDuplicates(files);
                }
            }

            foreach (var year in yearVideoDirectories)
            {
                List<string> folders = Directory.GetDirectories(year).ToList();
                List<string> files = Directory.GetFiles(year).ToList();
                // if there's no months, look for duplicates in this year directory
                if (folders.Any())
                {
                    Console.WriteLine(year);
                    foreach (var month in folders)
                    {
                        //WriteTextToReport(month);
                        //Console.WriteLine(month);
                        FindDuplicates(Directory.GetFiles(month).ToList());
                    }
                }
                if (files.Any())
                {
                    WriteTextToReport(year);
                    Console.WriteLine(year);
                    FindDuplicates(files);
                }
            }                   
        }
    }
}
