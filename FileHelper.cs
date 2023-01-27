using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using CsvHelper;



namespace ShaftkitMSA2_Parser
{
    public class FileHelper
    {
        char[] delimiterChars = { ' ', ',', ':', '\t' };

        // Node lists
        List<byte> NodeNum = new List<byte>();
        public List<float> NodeX = new List<float>();

        // Element lists
        List<byte> ElemNum = new List<byte>();
        List<float> ElemOD = new List<float>();
        List<float> ElemID = new List<float>();
        List<float> ElemE = new List<float>();
        List<float> ElemG = new List<float>();
        List<float> ElemRho = new List<float>();


        // Conc Mass Lists
        List<float> ConcMassNode = new List<float>();
        List<float> ConcMassVal = new List<float>();

        // Nodal Results Lists
        public List<float> Disp = new List<float>();
        public List<float> Slope = new List<float>();
        public List<float> Moment = new List<float>();
        public List<float> Shear = new List<float>();

        // Reaction Lists
        List<byte> ReactNode = new List<byte>();
        List<float> ReactVal = new List<float>();
        string?[] ReactStraight;

        // Influence List
        public List<List<string>> inf = new List<List<string>>();
        public List<List<float>> inf2 = new List<List<float>>();

        // CSV writer Lists
        List<Elem> Elems = new List<Elem>();
        List<Node> Nodes = new List<Node>();
        List<ConcMass> ConcMasses = new List<ConcMass>();

        // Summary Data
        float TotalMass = new float();
        float TotalElementMass = new float();
        float TotalConcMass = new float();


        private string[] CleanLine(string line)
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

        public void ReadFromFile(string filename)
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
                            var record = new ConcMass();
                            record.Node = byte.Parse(newline[1]);
                            record.Mass = float.Parse(newline[2]);
                            ConcMasses.Add(record);

                            TotalConcMass += record.Mass;

                            i++;
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
                            Disp.Add(float.Parse(newline[1]) * 1000);
                            Slope.Add(float.Parse(newline[2]) * 1000);

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
                                Shear.Add(float.Parse(newline[2]) / 1000);
                                Moment.Add(float.Parse(newline[3]) / 1000 * -1);

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

            AssembleClasses();

        }

        private void AssembleClasses()
        {
            // assemble model data
            for (byte i = 0; i < ElemNum.Count; i++)
            {
                var record = new Elem();
                record.Num = ElemNum[i];
                record.OD = ElemOD[i];
                record.ID = ElemID[i];
                record.Length = NodeX[i + 1] - NodeX[i];
                record.E = ElemE[i];
                record.G = ElemG[i];
                record.Rho = ElemRho[i];
                float power = Convert.ToSingle(Math.Pow(Convert.ToDouble(record.OD), 2) - Math.Pow(Convert.ToDouble(record.ID), 2));
                record.Mass = Convert.ToSingle(Math.PI) / 4 * record.Rho * record.Length * power;
                power = Convert.ToSingle(Math.Pow(Convert.ToDouble(record.OD), 4) - Math.Pow(Convert.ToDouble(record.ID), 4));
                record.PolInertia = Convert.ToSingle(Math.PI) / 64 * power;
                record.SecMod = record.PolInertia / (record.OD / 2);
                Elems.Add(record);

                TotalElementMass += record.Mass;


            }

            // assemble nodal results data
            for (byte i = 0; i < NodeNum.Count; i++)
            {
                var record = new Node();
                record.Num = NodeNum[i];
                record.X = NodeX[i];
                record.Disp = Disp[i];
                record.Slope = Slope[i];
                record.Moment = Moment[i];
                record.Shear = Shear[i];
                if (i < ElemNum.Count)
                {
                    record.Stress = Moment[i] * (ElemOD[i] / 2) / Elems[i].PolInertia / 1000;
                }
                else
                {
                    record.Stress = Moment[i] * (ElemOD[i - 1] / 2) / Elems[i - 1].PolInertia / 1000;
                }
                Nodes.Add(record);
            }

            TotalMass = TotalElementMass + TotalConcMass;

            // Fix influence matrix
            for (byte i = 0; i < ReactStraight.Count(); i++)
            {
                for (byte j = 0; j < inf[i].Count(); j++)
                {
                    if (inf[i][j].Length > 10)
                    {
                        if (inf[i][j].Substring(9, 1) == "-")
                        {
                            string temp = inf[i][j];
                            string one = temp.Substring(0, 9);
                            string two = temp.Substring(9);
                            inf[i][j] = one;
                            inf[i].Insert(j + 1, two);
                        }
                    }
                }
            }

            // Convert influence strings to float
            for (byte i = 0; i < ReactStraight.Count(); i++)
            {
                List<float> temp = new List<float>();
                for (byte j = 0; j < inf[i].Count(); j++)
                {
                    temp.Add(float.Parse(inf[i][j], NumberStyles.Float) / 1000);
                }
                inf2.Add(temp);
            }
        }


        public void WriteCSV(string filename)
        {
            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // write model totals
                csv.WriteComment("Model Summary");
                csv.NextRecord();
                var Totals = new List<object>
                {
                    new { Item = "Number of Elements", Value = ElemNum.Count},
                    new { Item = "Overall Length (m)", Value = NodeX[NodeX.Count-1]},
                    new { Item = "Total Element Mass (kg)", Value = TotalElementMass},
                    new { Item = "Total Concentrated Mass (kg)", Value = TotalConcMass},
                    new { Item = "Total Mass (kg)", Value = TotalMass},
                    new { Item = "Total Weight (kN)" , Value = TotalMass * 9.81 / 1000},
                };
                csv.WriteRecords(Totals);
                csv.NextRecord();

                // write influence
                csv.WriteComment(" Influence (kN/mm)");
                csv.NextRecord();
                for (int i = 0; i < inf2.Count; i++)
                {
                    for (int j = 0; j < inf2[i].Count; j++)
                    {
                        csv.WriteField(inf2[i][j]);
                    }
                    csv.NextRecord();
                }
                csv.NextRecord();

                // write conc_mass summary
                // write reactions table

                csv.WriteComment("Elements");
                csv.NextRecord();
                csv.WriteComment(" , OD (m), ID (m), Length (m), E (GPa), G (GPa), Density (kg/m^3), Mass (kg), Inertia (m^4), Sec. Modulus (m^3)");
                csv.NextRecord();
                csv.WriteRecords(Elems);
                csv.NextRecord();

                csv.WriteComment("Nodes");
                csv.NextRecord();
                csv.WriteComment(" , x (m), Disp. (mm), Slope (mrad), Moment (kNm), Shear (kN), Stress (MPa)");
                csv.NextRecord();
                csv.WriteRecords(Nodes);
                csv.NextRecord();





            }
        }

        public List<xy> PrepareSeries(List<float> x, List<float> y)
        {
            List<xy> data = new List<xy>();

            for (byte i = 0; i < x.Count; i++)
            {
                xy rec = new xy();

                rec.x = x[i];
                rec.y = y[i];
                data.Add(rec);
            }

            return data;
        }

        public void PlotDisp(string outputPath)
        {
            // try https://stackoverflow.com/questions/37791187/c-sharp-creating-custom-chart-class
            // create series of x and y

            //List<xy> srs = new List<xy>();
            //srs = PrepareSeries(NodeX, Disp);

            //Chart chartDisp = new Chart();3
            //chartDisp.DataSource = srs;
            //chartDisp.Series.Add("Displacement");
            //chartDisp.Series[0].XValueMember = "x";
            //chartDisp.Series[0].YValueMembers = "y";
            //chartDisp.Series[0].AxisLabel = "Displacement (mm)";
            //chartDisp.Series[0].ChartType = SeriesChartType.Line;
            //chartDisp.DataBind();
            //chartDisp.Update();
            //chartDisp.SaveImage(outputPath + "\\disp.jpg", ChartImageFormat.Jpeg);

            clsCustomChart chartDisp = new clsCustomChart("Displacement", NodeX, Disp);

        }

    }

    public class xy
    {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class Elem
    {
        public byte Num { get; set; }
        public float OD { get; set; }
        public float ID { get; set; }
        public float Length { get; set; }
        public float E { get; set; }
        public float G { get; set; }
        public float Rho { get; set; }
        public float Mass { get; set; }
        public float PolInertia { get; set; }
        public float SecMod { get; set; }

    }

    public class Node
    {
        public byte Num { get; set; }
        public float X { get; set; }
        public float Disp { get; set; }
        public float Slope { get; set; }
        public float Moment { get; set; }
        public float Shear { get; set; }
        public float Stress { get; set; }
    }

    public class ConcMass
    {
        public byte Node { get; set; }
        public float Mass { get; set; }
    }


    public class clsCustomChart : System.Windows.Forms.DataVisualization.Charting.Chart
    {
        public clsCustomChart(string strChartTitle, List<float> X, List<float> Y)
        {
            //  Create the chart

            //  Create the chart
            // Chart this = new Chart();
            this.BackColor = Color.FromArgb(50, Color.DarkGray);
            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = Color.Black;
            this.Width = 300;
            this.Height = 300;

            //  Create the legend
            Legend l = new Legend("Legend");
            l.Docking = Docking.Bottom;
            l.BackColor = Color.Transparent;
            this.Legends.Add(l);

            //  Create the chart area
            ChartArea a = new ChartArea("ChartArea1");
            a.Area3DStyle.Enable3D = false;
            a.Area3DStyle.WallWidth = 0;
            a.BackColor = Color.FromArgb(100, Color.Black);

            this.ChartAreas.Add(a);

            //  Create the axis
            a.AxisX.LineColor = Color.Silver;
            a.AxisX.MajorGrid.Enabled = true;
            a.AxisX.MinorGrid.Enabled = false;
            a.AxisX.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);

            a.AxisY.LineColor = Color.Silver;
            a.AxisY.MajorGrid.Enabled = true;
            a.AxisY.MinorGrid.Enabled = false;
            a.AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);

            //  Chart title
            this.Titles.Add(new Title(strChartTitle));

            //  Add the data
            //  Create the data series
            Series s = new Series("IN");
            s.ChartType = SeriesChartType.Line;

            X.ForEach(x => { s.Points.Add(x); });
            s.Color = Color.FromArgb(200, Color.Red);
            s.BorderWidth = 3;

            Series s2 = new Series("OUT");
            s2.ChartType = SeriesChartType.Line;

            Y.ForEach(x => { s2.Points.Add(x); });
            s2.Color = Color.FromArgb(200, Color.Green);
            s2.BorderWidth = 3;

            this.Series.Add(s);
            this.Series.Add(s2);

            this.SaveImage("c:\\temp\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".jpeg", ChartImageFormat.Jpeg);

        }

    }

}
