using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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
        public List<float> Stress = new List<float>();

        // Reaction Lists
        List<byte> ReactNode = new List<byte>();
        List<float> ReactX = new List<float>();
        List<float> ReactY = new List<float>();
        List<float> ReactZero = new List<float>();
        List<float> ReactVal = new List<float>();
        List<float> ReactStraight = new List<float>();

        // Influence List
        public List<List<string>> inf = new List<List<string>>();
        public List<List<float>> inf2 = new List<List<float>>();

        // CSV writer Lists
        List<Elem> Elems = new List<Elem>();
        List<Node> Nodes = new List<Node>();

        // Summary Data
        float TotalMass = new float();
        float TotalElementMass = new float();
        float TotalConcMass = new float();


        private string[] CleanLine(string line)
        {
            string trimmed = line.Trim();
            string[] newline = trimmed.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

            return newline;
        }

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
                            float val = float.Parse(newline[2]);
                            if (val > 0)
                            {
                                ConcMassNode.Add(byte.Parse(newline[0]));
                                ConcMassVal.Add(val);

                                TotalConcMass += val;
                            }

                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse Reaction Node
                    if (newline[0] == "SPRING")
                    {
                        i = i + 5;
                        newline = CleanLine(lines[i]);
                        while (newline[0] != null)
                        {
                            ReactNode.Add(byte.Parse(newline[0]));

                            i++;
                            newline = CleanLine(lines[i]);
                        }
                    }

                    // Parse reaction value
                    if (newline[0] == "Bearing")
                    {
                        i = i + 4;                
                        newline = CleanLine(lines[i]);
                        
                        if (newline[0] == "0")
                        {
                            i++;
                            newline = CleanLine(lines[i]);
                            foreach (string val in newline)
                            {
                                ReactVal.Add(float.Parse(val) / 1000);
                            }                            
                            
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

                        // straight reactions
                        foreach (string val in newline)
                        {
                            ReactStraight.Add(float.Parse(val) / 1000);
                        }


                        // influence table values
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
                Stress.Add(record.Stress);
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

            // Set ReactX and ReactY for bearing locations
            foreach (byte i in ReactNode)
            {
                ReactX.Add(NodeX[i - 1]);
                ReactY.Add(Disp[i - 1]);
                ReactZero.Add(0);
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
                csv.WriteComment(" Concentrated Masses");
                csv.NextRecord();
                csv.WriteComment(" Node, Mass (kg)");
                csv.NextRecord();
                for (int i = 0; i < ConcMassNode.Count; i++)
                {
                    csv.WriteField(ConcMassNode[i]);
                    csv.WriteField(ConcMassVal[i]);
                    csv.NextRecord();
                }
                csv.NextRecord();

                // write reactions table
                csv.WriteComment(" Bearing Reactions");
                csv.NextRecord();
                csv.WriteComment(" Node, x (m), Straight (kN), Offset (mm), Reaction (kN), Name, L/D");
                csv.NextRecord();

                for (int i = 0; i < ReactNode.Count; i++)
                {
                    csv.WriteField(ReactNode[i]);
                    csv.WriteField(NodeX[ReactNode[i] - 1]);
                    csv.WriteField(ReactStraight[i]);
                    csv.WriteField(Disp[ReactNode[i] - 1]);
                    csv.WriteField(ReactVal[i]);
                    csv.NextRecord();
                }
                csv.NextRecord();



                // write elements table
                csv.WriteComment("Elements");
                csv.NextRecord();
                csv.WriteComment(" , OD (m), ID (m), Length (m), E (GPa), G (GPa), Density (kg/m^3), Mass (kg), Inertia (m^4), Sec. Modulus (m^3)");
                csv.NextRecord();
                csv.WriteRecords(Elems);
                csv.NextRecord();

                // write nodes table
                csv.WriteComment("Nodes");
                csv.NextRecord();
                csv.WriteComment(" , x (m), Disp. (mm), Slope (mrad), Moment (kNm), Shear (kN), Stress (MPa)");
                csv.NextRecord();
                csv.WriteRecords(Nodes);
                csv.NextRecord();

            }
        }

        public void CreatePlots(string outputPath)
        {
            // try https://stackoverflow.com/questions/37791187/c-sharp-creating-custom-chart-class
            string filename = outputPath + "\\disp.jpg";
            clsCustomChart chartDisp = new clsCustomChart(filename, "Position (m)", "Displacement (mm)",
                                                          NodeX, Disp, ReactX, ReactY);

            filename = outputPath + "\\slope.jpg";
            clsCustomChart chartSlope = new clsCustomChart(filename, "Position (m)", "Slope (mrad)",
                                                          NodeX, Slope, ReactX, ReactZero);

            filename = outputPath + "\\shear.jpg";
            clsCustomChart chartShear = new clsCustomChart(filename, "Position (m)", "Shear (kN)",
                                                          NodeX, Shear, ReactX, ReactZero);

            filename = outputPath + "\\moment.jpg";
            clsCustomChart chartMoment = new clsCustomChart(filename, "Position (m)", "Bending Moment (kNm)",
                                                          NodeX, Moment, ReactX, ReactZero);

            filename = outputPath + "\\stress.jpg";
            clsCustomChart chartStress = new clsCustomChart(filename, "Position (m)", "Bending Stress (MPa)",
                                                          NodeX, Stress, ReactX, ReactZero);

            filename = outputPath + "\\model.jpg";
            PlotModel(filename);

        }


        private void PlotModel(string filename)
        {
            List<float> ReactYOD = new List<float>();

            Chart chartModel = new Chart();

            // setup chart options    
            Font textFont = new Font("Arial", 24);

            //  Create the chart
            // Chart this = new Chart();
            chartModel.BackColor = Color.FromArgb(50, Color.White);
            chartModel.BorderlineDashStyle = ChartDashStyle.Solid;
            chartModel.BorderlineColor = Color.Black;
            chartModel.Width = 1500;
            chartModel.Height = 700;
            chartModel.AntiAliasing = AntiAliasingStyles.All;
            chartModel.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

            //  Create the chart area
            ChartArea a = new ChartArea("ChartArea1");
            a.Area3DStyle.Enable3D = false;
            a.Area3DStyle.WallWidth = 0;
            a.BackColor = Color.FromArgb(100, Color.White);
            chartModel.ChartAreas.Add(a);

            //  Create the axis
            a.AxisX.LineColor = Color.Black;
            a.AxisX.MajorGrid.Enabled = true;
            a.AxisX.MinorGrid.Enabled = false;
            a.AxisX.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisX.LabelStyle.Font = textFont;
            a.AxisX.LabelStyle.Format = "0";
            a.AxisX.Title = "Position (m)";
            a.AxisX.TitleFont = textFont;
            a.AxisX.Maximum = NodeX[NodeX.Count - 1] * 1.05;
            a.AxisX.Minimum = -1 * (a.AxisX.Maximum - NodeX[NodeX.Count - 1]);

            a.AxisY.LineColor = Color.Black;
            a.AxisY.MajorGrid.Enabled = true;
            a.AxisY.MinorGrid.Enabled = false;
            a.AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisY.LabelStyle.Font = textFont;
            a.AxisY.LabelStyle.Format = "0.0";
            a.AxisY.Title = "Diameter (m)";
            a.AxisY.TitleFont = textFont;
            a.AxisY.Maximum = ElemOD.Max() * 1.75;
            a.AxisY.Minimum = -a.AxisY.Maximum;


            // Plot OD series
            Series sOD = new Series("OD");
            sOD.ChartType = SeriesChartType.Area;

            sOD.Points.AddXY(0, 0);
            for (int i = 0; i < NodeX.Count - 1; i++)
            {
                sOD.Points.AddXY(NodeX[i], ElemOD[i]);
                sOD.Points.AddXY(NodeX[i + 1], ElemOD[i]);
                sOD.Points.AddXY(NodeX[i + 1], 0);

                if (ReactX.Contains(NodeX[i]))
                {
                    ReactYOD.Add(ElemOD[i]);
                }
            }

            for (int i = NodeX.Count - 2; i >= 0; i--)
            {
                sOD.Points.AddXY(NodeX[i + 1], -ElemOD[i]);
                sOD.Points.AddXY(NodeX[i], -ElemOD[i]);
                sOD.Points.AddXY(NodeX[i], 0);
            }
            sOD.Color = Color.FromArgb(200, Color.LightBlue);
            sOD.BorderWidth = 1;
            chartModel.Series.Add(sOD);


            // OD series to plot black border
            Series sOD2 = new Series("OD Border");
            sOD2.ChartType = SeriesChartType.Line;
            sOD2.Color = Color.FromArgb(200, Color.Black);
            sOD2.BorderWidth = 2;
            chartModel.Series.Add(sOD2);

            chartModel.DataManipulator.CopySeriesValues(sOD.Name, sOD2.Name);


            // Plot ID series
            Series sID = new Series("ID");
            sID.ChartType = SeriesChartType.Area;

            sID.Points.AddXY(0, 0);
            for (int i = 0; i < NodeX.Count - 1; i++)
            {
                sID.Points.AddXY(NodeX[i], ElemID[i]);
                sID.Points.AddXY(NodeX[i + 1], ElemID[i]);
                sID.Points.AddXY(NodeX[i + 1], 0);

            }

            for (int i = NodeX.Count - 2; i >= 0; i--)
            {
                sID.Points.AddXY(NodeX[i + 1], -ElemID[i]);
                sID.Points.AddXY(NodeX[i], -ElemID[i]);
                sID.Points.AddXY(NodeX[i], 0);
            }
            sID.Color = Color.FromArgb(200, Color.White);
            sID.BorderWidth = 1;
            chartModel.Series.Add(sID);


            // OD series to plot black border
            Series sID2 = new Series("ID Border");
            sID2.ChartType = SeriesChartType.Line;
            sID2.Color = Color.FromArgb(200, Color.Black);
            sID2.BorderWidth = 2;
            chartModel.Series.Add(sID2);

            chartModel.DataManipulator.CopySeriesValues(sID.Name, sID2.Name);

            
            //  Create the bearing series
            Series s3 = new Series("Bearings");
            s3.ChartType = SeriesChartType.Line;

            for (int i = 0; i < ReactX.Count; i++)
            {
                s3.Points.AddXY(ReactX[i], -ReactYOD[i]);
            }

            s3.Color = Color.FromArgb(0, Color.Black);
            s3.MarkerStyle = MarkerStyle.Triangle;
            s3.MarkerColor = Color.FromArgb(200, Color.Red);
            s3.MarkerSize = 20;
            chartModel.Series.Add(s3);


            // Initiate drawing shapes
            // chartModel.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.PostPaint);

            // save to file
            chartModel.SaveImage(filename, ChartImageFormat.Jpeg);
        }

        //private void PostPaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        //{
        //    Rectangle r1 = new Rectangle();
        //    r1.X = 10;
        //    r1.Y = 10;
        //    r1.Width = 200;
        //    r1.Height = 200;

        //    Rectangle r2 = new Rectangle();
        //    r2.X = 100;
        //    r2.Y = 100;
        //    r2.Width = 200;
        //    r2.Height = 300;

        //    e.ChartGraphics.Graphics.FillRectangle(new SolidBrush(Color.Red), r1);
        //    e.ChartGraphics.Graphics.DrawRectangle(new Pen(Color.Black, 50), r2);
        //}

    }

   
    public class clsCustomChart : System.Windows.Forms.DataVisualization.Charting.Chart
    {
        public clsCustomChart(string filename,
                              string xLabel,
                              string yLabel,
                              List<float> X,
                              List<float> Y,
                              List<float> ReactX,
                              List<float> ReactY)
        {

            // setup chart options    
            Font textFont = new Font("Arial", 24);

            //  Create the chart
            // Chart this = new Chart();
            this.BackColor = Color.FromArgb(50, Color.White);
            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = Color.Black;
            this.Width = 1500;
            this.Height = 700;
            this.AntiAliasing = AntiAliasingStyles.All;
            this.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

            //  Create the chart area
            ChartArea a = new ChartArea("ChartArea1");
            a.Area3DStyle.Enable3D = false;
            a.Area3DStyle.WallWidth = 0;
            a.BackColor = Color.FromArgb(100, Color.White);
            this.ChartAreas.Add(a);

            //  Create the axis
            a.AxisX.LineColor = Color.Black;
            a.AxisX.MajorGrid.Enabled = true;
            a.AxisX.MinorGrid.Enabled = false;
            a.AxisX.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisX.LabelStyle.Font = textFont;
            a.AxisX.LabelStyle.Format = "0";
            a.AxisX.Title = xLabel;
            a.AxisX.TitleFont = textFont;

            a.AxisY.LineColor = Color.Black;
            a.AxisY.MajorGrid.Enabled = true;
            a.AxisY.MinorGrid.Enabled = false;
            a.AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            a.AxisY.LabelStyle.Font = textFont;
            a.AxisY.LabelStyle.Format = "0.0";
            a.AxisY.Title = yLabel;
            a.AxisY.TitleFont = textFont;

            //  Chart title
            //this.Titles.Add(new Title(strChartTitle));

            //  Add the data
            //  Create the data series
            Series s = new Series("Series1");
            s.ChartType = SeriesChartType.Line;

            for (int i = 0; i < X.Count; i++)
            {
                s.Points.AddXY(X[i], Y[i]);
            }

            s.Color = Color.FromArgb(200, Color.DarkBlue);
            s.BorderWidth = 3;
            this.Series.Add(s);

            //  Create the bearing series
            Series s2 = new Series("Series2");
            s2.ChartType = SeriesChartType.Line;

            for (int i = 0; i < ReactX.Count; i++)
            {
                s2.Points.AddXY(ReactX[i], ReactY[i]);
            }

            s2.Color = Color.FromArgb(0, Color.Black);
            s2.MarkerStyle = MarkerStyle.Triangle;
            s2.MarkerColor = Color.FromArgb(200, Color.Red);
            s2.MarkerSize = 20;
            this.Series.Add(s2);

            this.SaveImage(filename, ChartImageFormat.Jpeg);

        }

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

}
