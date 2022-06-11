using System;
using System.Collections;


namespace ShaftkitMSA2_Parser
{
    public class FileHelper
    {
        static char[] delimiterChars = { ' ', ',', ':', '\t' };
        public static ArrayList nodes = new ArrayList();
        public static ArrayList elems = new ArrayList();

        // Node lists
        public static ArrayList NodeNum = new ArrayList();
        public static ArrayList NodeX = new ArrayList();

        // Element lists
        public static ArrayList ElemNum = new ArrayList();
        public static ArrayList ElemOD = new ArrayList();
        public static ArrayList ElemID = new ArrayList();
        public static ArrayList ElemE = new ArrayList();
        public static ArrayList ElemG = new ArrayList();
        public static ArrayList ElemRho = new ArrayList();

        // Conc Mass Lists
        public static ArrayList ConcMassNode = new ArrayList();
        public static ArrayList ConcMassVal = new ArrayList();

        // Nodal Results Lists
        public static ArrayList disp = new ArrayList();
        public static ArrayList slope = new ArrayList();
        public static ArrayList moment = new ArrayList();
        public static ArrayList shear = new ArrayList();
        public static ArrayList stress = new ArrayList();

        // Reaction Lists
        public static ArrayList reactNode = new ArrayList();
        public static ArrayList reactVal = new ArrayList();
        public static ArrayList reactStraight = new ArrayList();

        // Influence List
        public static List<List<string>> inf = new List<List<string>>();


        private static string[] CleanLine(string line)
        {
            string trimmed = line.Trim();
            string[] newline = trimmed.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            //string[] newline = nextline.Except(new List<string> { string.Empty }).ToArray();

            // need to return something if string{0}


            return newline;
        }

        //private static IEnumerable<String> SplitInParts(String s, Int32 partLength)
        //{
        //    if (s == null)
        //        throw new ArgumentNullException(nameof(s));
        //    if (partLength <= 0)
        //        throw new ArgumentException("Part length has to be positive.", nameof(partLength));

        //    for (var i = 0; i < s.Length; i += partLength)
        //        yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        //}

        public static void ReadFromFile(string filename)
        {

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(filename);
            string[] newline = { };

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    // Parse Nodes
                    newline = CleanLine(lines[i]);
                    if (newline[0] == "NODES")
                    {
                        i += 1;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != "ELEMEN")
                        {
                            NodeNum.Add(newline[0]);
                            NodeX.Add(newline[1]);
                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Elements
                    if (newline[0] == "BEAM" && newline[1] == "TYPES")
                    {
                        i++;
                        int j = 1;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != "CONC")
                        {
                            ElemNum.Add(j.ToString());
                            ElemOD.Add(newline[0]);
                            ElemID.Add(newline[1]);
                            ElemE.Add(newline[2]);
                            ElemG.Add(newline[3]);
                            ElemRho.Add(newline[4]);

                            i++;
                            j++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse ConcMass
                    if (newline[0] == "CONC" && newline[1] == "MASS")
                    {
                        i++;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != "CONC")
                        {
                            ConcMassNode.Add(newline[1]);
                            ConcMassVal.Add(newline[2]);

                            i += 2;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Reactions
                    if (newline[0] == "SPRING")
                    {
                        i = i + 5;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != null)
                        {
                            reactNode.Add(newline[0]);
                            reactVal.Add(newline[2]);

                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Displacements & Slope
                    if (newline[0] == "DISPLACEMENTS")
                    {
                        i = i + 6;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != null)
                        {
                            disp.Add(newline[1]);
                            slope.Add(newline[2]);

                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Bending, Moment
                    if (newline[0] == "BEAM" && newline[1] == "FORCES")
                    {
                        i += 4;
                        newline = CleanLine(lines[i]);
                        while (newline != null)
                        {
                            try
                            {
                                shear.Add(newline[2]);
                                moment.Add(newline[3]);

                                i += 2;
                                newline = CleanLine(lines[i]);
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                // how to fix exception called on line 539???
                                // MessageBox.Show(ex.Message + " " + i.ToString());
                                newline = CleanLine(lines[i - 1]);
                                shear.Add(newline[2]);
                                moment.Add(newline[3]);

                                break;
                            }
                                                        
                        }
                        
                    }

                    //// Parse Influence
                    if (newline[0] == "Influence")
                    {
                        i = i + 3;
                        newline = CleanLine(lines[i]);
                        reactStraight.Add(newline);

                        i++;

                        newline = CleanLine(lines[i]);
                        while (newline[0] != null)
                        {

                            inf.Add(newline.ToList());

                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                }

                catch (IndexOutOfRangeException ex)
                {
                    // MessageBox.Show(ex.Message + i.ToString());
                    continue;
                }



            }

        }
    }
}
