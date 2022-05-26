using System;


namespace ShaftkitMSA2_Parser
{
    public class FileHelper
    {
        /*public List<string> nodes = null;*/
        static char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

        private static string[] CleanLine(string line)
        {
            string trimmed = line.Trim();
            string[] newline = trimmed.Split(delimiterChars);
            
            return newline;
        }

        public static void ReadFromFile(string filename)
        {

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(filename);
            string[] newline = { };

            for (int i = 0; i < lines.Length; i++)
            {
                newline = CleanLine(lines[i]);
                if (newline[0] == "NODES")
                {
                    i += 2;
                    newline = CleanLine(lines[i]);
                    while (newline[0] !=  "ELEMEN")
                    {
                        Console.WriteLine("\t" + lines[i]);
                        i++;
                        newline = CleanLine(lines[i]);
                    }
                }

                if (newline[0] == "BEAM")
                {
                    Console.WriteLine("\t" + lines[i]);
                }




            }

        }
    }
}
