using System;
using System.Collections;


namespace ShaftkitMSA2_Parser
{
    public class FileHelper
    {
        static char[] delimiterChars = { ' ', ',', ':', '\t' };

        // Node lists
        public static List<byte> NodeNum = new List<byte>();
        public static List<float> NodeX = new List<float>();

        // Element lists
        public static List<byte> ElemNum = new List<byte>();
        public static List<float> ElemOD = new List<float>();
        public static List<float> ElemID = new List<float>();
        public static List<float> ElemE = new List<float>();
        public static List<float> ElemG = new List<float>();
        public static List<float> ElemRho = new List<float>();


        // Conc Mass Lists
        public static List<float> ConcMassNode = new List<float>();
        public static List<float> ConcMassVal = new List<float>();

        // Nodal Results Lists
        public static List<float> Disp = new List<float>();
        public static List<float> Slope = new List<float>();
        public static List<float> Moment = new List<float>();
        public static List<float> Shear = new List<float>();
        public static List<float> Stress = new List<float>();

        // Reaction Lists
        public static List<byte> ReactNode = new List<byte>();
        public static List<float> ReactVal = new List<float>();
        public static string?[] ReactStraight;

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
                            NodeNum.Add(byte.Parse(newline[0]));
                            NodeX.Add(float.Parse(newline[1]));
                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Elements
                    if (newline[0] == "BEAM" && newline[1] == "TYPES")
                    {
                        i++;
                        byte j = 1;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != "CONC")
                        {
                            ElemNum.Add(j);
                            ElemOD.Add(float.Parse(newline[0]));
                            ElemID.Add(float.Parse(newline[1]));
                            ElemE.Add(float.Parse(newline[2]));
                            ElemG.Add(float.Parse(newline[3]));
                            ElemRho.Add(float.Parse(newline[4]));

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
                            ConcMassNode.Add(short.Parse(newline[1]));
                            ConcMassVal.Add(float.Parse(newline[2]));

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
                            ReactNode.Add(byte.Parse(newline[0]));
                            ReactVal.Add(float.Parse(newline[2]));

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
                            Disp.Add(float.Parse(newline[1]));
                            Slope.Add(float.Parse(newline[2]));

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
                                Shear.Add(float.Parse(newline[2]));
                                Moment.Add(float.Parse(newline[3]));

                                i += 2;
                                newline = CleanLine(lines[i]);
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                // how to fix exception called on line 539???
                                // MessageBox.Show(ex.Message + " " + i.ToString());
                                newline = CleanLine(lines[i - 1]);
                                Shear.Add(float.Parse(newline[2]));
                                Moment.Add(float.Parse(newline[3]));

                                break;
                            }
                                                        
                        }
                        
                    }

                    //// Parse Influence
                    if (newline[0] == "Influence")
                    {
                        i = i + 3;
                        newline = CleanLine(lines[i]);
                        ReactStraight = newline;

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
