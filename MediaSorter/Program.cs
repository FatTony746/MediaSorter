using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;

namespace MediaSorter
{
    class Program
    {
        static HashSet<string> directorySet = new HashSet<string>();
        static string directoryName = @"C:\Users\Tonyt\Desktop\Tony's PC\Music";

        static void Main(string[] args)
        {
            string[] directories = Directory.GetDirectories(directoryName);

            ConvertDirectories(directories);

            string[] files = Directory.GetFiles(directoryName);

            foreach (string f in files)
            {
                TagLib.File file = TagLib.File.Create(f);
                if (file.Tag.Performers.Length > 0)
                {
                    if (directorySet.Contains(directoryName + @"\" + file.Tag.Performers[0]))
                    {
                        AddSongToDirectory(file);
                    }
                    else
                    {
                        Menu(file);
                    }
                }
                else
                {
                    Menu(file);
                }
            }
        }

        static void ConvertDirectories(string[] directories)
        {
            foreach (string d in directories)
            {
                directorySet.Add(d);
            }
        }

        static void Menu(TagLib.File file)
        {
            Console.Clear();
            Console.WriteLine("Undecided file.\n");

            Console.WriteLine("Title: " + file.Name + "\n");
            if (file.Tag.Performers.Length > 0)
            {
                Console.WriteLine("Artist: " + file.Tag.Performers[0] + "\n");
            }
            else
            {
                Console.WriteLine("Artist: No Artist" + "\n");
            }

            Console.WriteLine("What would you like to do with this file?\n");
            Console.WriteLine("1. Add/Modify artist\n");
            Console.WriteLine("2. Create a directory for this artist and add to it\n");
            Console.WriteLine("3. Continue for now\n");

            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("Please specify the artist name: ");

                string artist = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(artist))
                {
                    file.Tag.Performers = new string[] { artist };
                    if (directorySet.Contains(file.Tag.Performers[0]))
                    {
                        AddSongToDirectory(file);
                    }
                    else
                    {
                        CreateDirectory(file);
                        AddSongToDirectory(file);
                    }
                }
            }
            else if (input == "2")
            {
                if (!directorySet.Contains(file.Tag.Performers[0]))
                {
                    CreateDirectory(file);
                }
                AddSongToDirectory(file);
            }

        }

        static void AddSongToDirectory(TagLib.File file)
        {
            file.Save();
            string fileName = file.Name.Split('\\').Last();
            string currentPosition = file.Name;
            string destination = directoryName + @"\" + file.Tag.Performers[0] + @"\" + file.Tag.Performers[0] + " - " + fileName;
            System.IO.File.Move(currentPosition, destination);
        }

        static void CreateDirectory(TagLib.File file)
        {
            string newDirectoryPath = directoryName + @"\" + file.Tag.Performers[0];
            Directory.CreateDirectory(newDirectoryPath);
            directorySet.Add(newDirectoryPath);
        }
    }
}
