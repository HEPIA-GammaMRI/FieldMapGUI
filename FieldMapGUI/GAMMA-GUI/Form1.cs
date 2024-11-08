using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.GenericMotorCLI.Settings;
using Thorlabs.MotionControl.KCube.DCServoCLI;

using Plot3D;

// enums
using eRaster = Plot3D.Editor3D.eRaster;
using ePolygonMode = Plot3D.Editor3D.ePolygonMode;
using eSelEvent = Plot3D.Editor3D.eSelEvent;
using eSelType = Plot3D.Editor3D.eSelType;
using eObjType = Plot3D.Editor3D.eObjType;
using eTooltip = Plot3D.Editor3D.eTooltip;
using eNormalize = Plot3D.Editor3D.eNormalize;
using eMouseCtrl = Plot3D.Editor3D.eMouseCtrl;
using eScatterShape = Plot3D.Editor3D.eScatterShape;
using eColorScheme = Plot3D.Editor3D.eColorScheme;
using eInvalidate = Plot3D.Editor3D.eInvalidate;
using eLegendPos = Plot3D.Editor3D.eLegendPos;
// classes
using cObject3D = Plot3D.Editor3D.cObject3D;
using cPoint3D = Plot3D.Editor3D.cPoint3D;
using cShape3D = Plot3D.Editor3D.cShape3D;
using cLine3D = Plot3D.Editor3D.cLine3D;
using cPolygon3D = Plot3D.Editor3D.cPolygon3D;
using cColorScheme = Plot3D.Editor3D.cColorScheme;
using cMessgData = Plot3D.Editor3D.cMessgData;
using cSurfaceData = Plot3D.Editor3D.cSurfaceData;
using cScatterData = Plot3D.Editor3D.cScatterData;
using cLineData = Plot3D.Editor3D.cLineData;
using cPolygonData = Plot3D.Editor3D.cPolygonData;
using cUserInput = Plot3D.Editor3D.cUserInput;
// callback function
using delRendererFunction = Plot3D.Editor3D.delRendererFunction;
using Thorlabs.MotionControl.Tools.Common;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Data.Common;
using ScottPlot.WinForms;
using ScottPlot;
using Microsoft.Win32;






namespace FieldMapGUI
{
    public partial class Form1 : Form
    {

        readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        double Bm;
        double Bx;
        double By;
        double Bz;

        double BmSD;
        double BxSD;
        double BySD;
        double BzSD;


        double Px;
        double Py;
        double Pz;
        double Sx;
        double Sy;
        double Sz;
        double Temp;
        int ArrayIndexX;
        int ArrayIndexY;
        int ArrayIndexZ;
        DateTime measTime;


        cScatterData[] i_ShapeData;
        cMessgData[] i_Mesg ;
        cColorScheme i_Scheme ;

        DateTime startTime;

        bool bStartTimeMeasure, bStartCubeMeasure;

        string timeRemainString;
        int measurePointsX, measurePointsY, measurePointsZ;
        int pointsMeasured, pointsMeasuredX, pointsMeasuredY;
        double stepSizeX, stepSizeY, stepSizeZ;
        int currentZindex,lastZindex;
       

        bool stopMeasure;
        bool measurementRunning;
        bool updateChart;
        bool newData;

        double nextXpos,nextYpos,nextZpos;
        bool nextPos;

        double percentageComplete;

        int lastPointPlotted;
        bool fieldPlotChanged = false;
        int plotIndex=0;
        string saveFilePath = "";
        string csv;
        TextWriter tsw;
        #region enums

        enum eDemo
        {
            Math_Callback,
            Math_Formula,
            Surface_Fill,
            Surface_Grid,
            Surface_Fill_Missing,
            Surface_Grid_Missing,
            Nested_Graphs,
            Scatter_Plot,
            Connected_Lines,
            Scatter_Shapes,
            Pyramid,
            Sphere_Fill_Closed,
            Sphere_Fill_Open,
            Sphere_Grid,
            Valentine,
            Animation,
        }

        #endregion

        eDemo me_Demo;
        eColorScheme me_ColorScheme;
      
        cMessgData mi_MesgTop = new cMessgData("", -7, 7, System.Drawing.Color.Blue); // For special hint
        cMessgData mi_MesgBottom = new cMessgData("", -7, -7, System.Drawing.Color.Gray); // For selection mode
        System.Windows.Forms.Timer mi_StatusTimer = new System.Windows.Forms.Timer();


        TeslameterF71Comms TeslameterF71;
        KCubeDCServo kCubeX, kCubeY, kCubeZ;


        // We create the serial number string of your connected controller. This will
        // be used as an argument for LoadMotorConfiguration(). You can replace this
        // serial number with the number printed on your device.
        string serialNoX = "27268143";
        string serialNoY = "27268080";
        string serialNoZ = "27268120";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

           

            cMessgData i_Mesg = new cMessgData("Waiting for data points", 10, 10, System.Drawing.Color.Blue);

            editor3D.Clear();
            editor3D.AddMessageData(i_Mesg);
            editor3D.Invalidate();

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            
            statusLabel.AutoSize = false;
            statusLabel.Width = ClientSize.Width - 30;

            
        }
        void OnClearStatusTimer(object sender, EventArgs e)
        {
            mi_StatusTimer.Stop();
            statusLabel.Text = "";
        }



        public Form1()
        {

            InitializeComponent();

            this.FormClosing += Form1_FormClosing;

            mi_StatusTimer.Interval = 8000;
            mi_StatusTimer.Tick += new EventHandler(OnClearStatusTimer);

            // Add the FormsPlot to the panel
            panelScott.Controls.Add(FormsPlot1);

            


        }
        private void DrawDemo()
        {
            
            editor3D.SetUserInputs(eMouseCtrl.L_Theta_L_Phi);

            mi_StatusTimer.Stop();

            me_ColorScheme = eColorScheme.Rainbow_Bright;
            editor3D.TooltipMode = eTooltip.All;


            //DemoSurface(ePolygonMode.Fill, false);
            DemoScatterPlot(false);
            editor3D.AxisZ.IncludeZero = false;

            // All demos call editor3D.Clear() --> messages must be added always anew.
            editor3D.AddMessageData(mi_MesgTop, mi_MesgBottom);

            // Show total count of Lines, Shapes, Polygons
            //lblInfo.Text = editor3D.ObjectStatistics;

            statusLabel.Text = (editor3D.Selection.Callback == null) ? "Callback: OFF" : "";

            //SetSelectionMessages();
        }

       

       


        private void DemoScatterPlot(bool b_Lines)
        {
            // 3 pixels for line width and for circle radius
            const int SIZE = 5;

            
            

            


           

            double d_X =0; // X must be related to Colum
            double d_Y = 0; // Y must be related to Row
            double d_Z = 0;
            double d_B = 0;
            double d_T = 0;

            int tempLow = lastPointPlotted;


            if (fieldPlotChanged)
            {
                i_ShapeData = new cScatterData[measurePointsZ + 1];

                for (int i = 0; i < measurePointsZ + 1; i++)
                {
                    i_ShapeData[i] = new cScatterData(i_Scheme);
                    Console.WriteLine("shapeData " + i);
                }

                tempLow = 0;
                fieldPlotChanged = false;
            }

            for (int C = tempLow; C < pointsMeasured; C++)
            {


                //dataGridView1.Rows.Add(new object[] { pointsMeasured, dateString, timeString, Sx, Sy, Sz, Px, Py, Pz, Bm, BmSD, Bx, BxSD, By, BySD, Bz, BzSD, Temp, ArrayIndexX, ArrayIndexY, ArrayIndexZ });


                d_X = (double)dataGridView1.Rows[C].Cells[3].Value; 
                d_Y = (double)dataGridView1.Rows[C].Cells[4].Value;
                d_Z = (double)dataGridView1.Rows[C].Cells[5].Value;
                if (plotIndex == 0)
                {
                    d_B =  (double)dataGridView1.Rows[C].Cells[9].Value;

                }
                else if (plotIndex == 1)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[11].Value; 

                }
                else if (plotIndex == 2)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[13].Value;

                }
                else if (plotIndex == 3)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[15].Value;

                }
                else if (plotIndex == 4)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[10].Value;

                }
                else if (plotIndex == 5)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[12].Value;

                }
                else if (plotIndex == 6)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[14].Value;

                }
                else if (plotIndex == 7)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[16].Value;

                }
                else if (plotIndex == 8)
                {
                    d_B = (double)dataGridView1.Rows[C].Cells[17].Value;

                }


                d_T = (double)dataGridView1.Rows[C].Cells[17].Value;

                String s_Tooltip = String.Format("X = {0} \nY = {1}\nZ {2} \nB {3} mT\nT {4} C\n",
                                                 d_X, d_Y, d_Z, d_B, d_T);

                cPoint3D i_Point = new cPoint3D(d_X, d_Y, d_B, s_Tooltip);

                int zDist = int.Parse(textBoxZdist.Text);

                if (checkBoxPlotAll.Checked)
                {
                    i_Point = new cPoint3D(d_X, d_Y , d_B + (int)dataGridView1.Rows[C].Cells[20].Value * zDist , s_Tooltip);

                }
               
                i_ShapeData[(int)dataGridView1.Rows[C].Cells[20].Value].AddShape(i_Point, eScatterShape.Circle, SIZE, null);

                lastPointPlotted = C + 1;
                
                
            }

            

            // Depending on your use case you can also specify MaintainXY or MaintainXYZ here
            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;

            if (checkBoxPlotAll.Checked)
            {
                for (int i = 0; i < currentZindex + 1; i++)
                {
                    
                        editor3D.AddRenderData(i_ShapeData[i]);
                        i_Mesg[i] = new cMessgData("Z = " + d_Z.ToString(), 7, -7, System.Drawing.Color.Orange);
                        editor3D.AddMessageData(i_Mesg[i]);
                    

                }
            }
            else
            {
                if(pointsMeasured>0)
                {
                    int chosenZ = comboBoxPlotSelect.SelectedIndex;
                        cScatterData i_ShapeDataSingle = new cScatterData(i_Scheme);
                        i_ShapeDataSingle = i_ShapeData[chosenZ];
                        editor3D.AddRenderData(i_ShapeDataSingle);
                        i_Mesg[chosenZ] = new cMessgData("Z = " + chosenZ.ToString(), 7, -7, System.Drawing.Color.Orange);
                        editor3D.AddMessageData(i_Mesg[chosenZ]);
                }

            }
            
            

            editor3D.Selection.HighlightColor = System.Drawing.Color.FromArgb(90, 90, 90);
            
            
            
            editor3D.Selection.MultiSelect = false;

            editor3D.TooltipMode = eTooltip.UserText;
            editor3D.AxisY.Mirror = true;
            editor3D.AxisX.Mirror = true;
            editor3D.AxisX.LegendText = "X";
            editor3D.AxisY.LegendText = "Y";
            editor3D.AxisZ.LegendText = "Bm";
            editor3D.LegendPos = eLegendPos.AxisEnd;


            editor3D.Selection.Enabled = false;
            editor3D.UndoBuffer.Enabled = false;
            editor3D.Raster = (eRaster.Labels);
            editor3D.Invalidate();

           

            updateChart = false;

         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateChart = false;
            lastZindex = -1;
            stopMeasure = true;
            measurementRunning = false;

            nextXpos = 0;
            nextYpos = 0;
            nextZpos = 0;
            nextPos = false;

            timeRemainString = "";
            newData = false;

            i_Scheme = new cColorScheme(me_ColorScheme);

            bStartTimeMeasure = false;
            bStartCubeMeasure = false;





            DeviceManagerCLI.BuildDeviceList();

            Parallel.Invoke(
                () =>
                {
                    TeslameterF71 = new TeslameterF71Comms();
                    TeslameterF71.Init();

                },  // close first Action

                () =>
                {

                    kCubeX = KCubeDCServo.CreateKCubeDCServo(serialNoX);
                    Console.WriteLine("Opening kCubeX {0}", serialNoX);
                    kCubeX.Connect(serialNoX);
                    kCubeX.WaitForSettingsInitialized(5000);
                    MotorConfiguration motorSettingsX = kCubeX.LoadMotorConfiguration(serialNoX, DeviceConfiguration.DeviceSettingsUseOptionType.UseFileSettings);
                    kCubeX.StartPolling(250);
                    kCubeX.EnableDevice();
                    Console.WriteLine("kCubeX Enabled");

                    kCubeY = KCubeDCServo.CreateKCubeDCServo(serialNoY);
                    Console.WriteLine("Opening kCubeY {0}", serialNoY);
                    kCubeY.Connect(serialNoY);
                    kCubeY.WaitForSettingsInitialized(5000);
                    MotorConfiguration motorSettingsY = kCubeY.LoadMotorConfiguration(serialNoY, DeviceConfiguration.DeviceSettingsUseOptionType.UseFileSettings);
                    kCubeY.StartPolling(250);
                    kCubeY.EnableDevice();
                    Console.WriteLine("kCubeY Enabled");

                    kCubeZ = KCubeDCServo.CreateKCubeDCServo(serialNoZ);
                    Console.WriteLine("Opening kCubeZ {0}", serialNoZ);
                    kCubeZ.Connect(serialNoZ);
                    kCubeZ.WaitForSettingsInitialized(5000);
                    MotorConfiguration motorSettingsZ = kCubeZ.LoadMotorConfiguration(serialNoZ, DeviceConfiguration.DeviceSettingsUseOptionType.UseFileSettings);
                    kCubeZ.StartPolling(250);
                    kCubeZ.EnableDevice();
                    Console.WriteLine("kCubeZ Enabled");

                } //close second Action

                
            ); //close parallel.invoke



            dataGridView1.Columns.Add("Num", "Num");
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Xs","Xset");
            dataGridView1.Columns.Add("Ys", "Yset");
            dataGridView1.Columns.Add("Zs", "Zset");
            dataGridView1.Columns.Add("Xp", "Xpos");
            dataGridView1.Columns.Add("Yp", "Ypos");
            dataGridView1.Columns.Add("Zp", "Zpos");
            dataGridView1.Columns.Add("Bm", "Bm");
            dataGridView1.Columns.Add("BmSD", "BmSD");
            dataGridView1.Columns.Add("Bx", "Bx");
            dataGridView1.Columns.Add("BxSD", "BxSD");
            dataGridView1.Columns.Add("By", "By");
            dataGridView1.Columns.Add("BySD", "BySD");
            dataGridView1.Columns.Add("Bz", "Bz");
            dataGridView1.Columns.Add("BzSD", "BzSD");
            dataGridView1.Columns.Add("T", "Temp C");
            dataGridView1.Columns.Add("IndexX", "IndexX");
            dataGridView1.Columns.Add("IndexY", "IndexY");
            dataGridView1.Columns.Add("IndexZ", "IndexZ");

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.AllowUserToAddRows = false;

            //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);

            comboBoxTeslaAveraging.Items.Add("1");
            comboBoxTeslaAveraging.Items.Add("10");
            comboBoxTeslaAveraging.Items.Add("20");
            comboBoxTeslaAveraging.Items.Add("100");
            comboBoxTeslaAveraging.Items.Add("500");
            comboBoxTeslaAveraging.Items.Add("1000");

            comboBoxTeslaAveraging.SelectedIndex = 2;
            comboBoxFieldSelect.SelectedIndex = 0;

            labelProgress.Text = "";
            progressBarMeasure.Minimum = 0;
            progressBarMeasure.Maximum = 100;

            setupMeasureButtons();

            buttonStopMeasure.Enabled = false;

            timerReadbacks.Interval = 1000;
            timerReadbacks.Enabled = true;
            
            timerCubeMeasure.Interval = 10;
            timerCubeMeasure.Enabled = true;

            



        }


        private void setupMeasureButtons() 
        {


            stepSizeX = (Double.Parse(textBoxMeasMaxX.Text) - Double.Parse(textBoxMeasMinX.Text)) / (double.Parse(textBoxMeasStepX.Text) - 1);
            labelStepSizeX.Text = stepSizeX.ToString("0.00");



            stepSizeY = (Double.Parse(textBoxMeasMaxY.Text) - Double.Parse(textBoxMeasMinY.Text)) / (double.Parse(textBoxMeasStepY.Text) - 1);
            labelStepSizeY.Text = stepSizeY.ToString("0.00");


            stepSizeZ = (Double.Parse(textBoxMeasMaxZ.Text) - Double.Parse(textBoxMeasMinZ.Text)) / (double.Parse(textBoxMeasStepZ.Text) - 1);
            labelStepSizeZ.Text = stepSizeZ.ToString("0.00");
            
            
        }
       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopMeasure = true;
            if(measurementRunning)
            {
                timerShutdown.Enabled = true;
                e.Cancel = true;
                return;
            }
            timerReadbacks.Enabled = false;
            timerCubeMeasure.Enabled = false;



            // Perform three tasks in parallel on the source array
            Parallel.Invoke(
                () =>
                {
                    // Stop polling the device.
                    kCubeX.StopPolling();
                    kCubeX.ShutDown();
                    kCubeX.Disconnect(true);
                },  // close first Action

                () =>
                {

                    // Stop polling the device.
                    kCubeY.StopPolling();
                    kCubeY.ShutDown();
                    kCubeY.Disconnect(true);


                }, //close second Action

                () =>
                {


                    // Stop polling the device.
                    kCubeZ.StopPolling();
                    kCubeZ.ShutDown();
                    kCubeZ.Disconnect(true);
                }, //close third Action //close second Action

                () =>
                {
                    TeslameterF71.Closedown();
                    


                } //close third Action
            ); //close parallel.invoke
           
            
            
          


            
        }

        private async void buttonkCubeHomeX_Click(object sender, EventArgs e)
        {
            await Task.Run(() => homeXYZ());
        }

        

        private string checkInput(string inputText, double limitLower,double limitUpper)
        {
            string tempString = inputText;
            try {
                
                if (double.Parse(inputText) > limitLower && double.Parse(inputText) < limitUpper)
                {
                    tempString = inputText;
                }
                else if (double.Parse(inputText) < limitLower)
                {
                    tempString = limitLower.ToString();
                }
                else if (double.Parse(inputText) > limitUpper)
                {
                    tempString = limitUpper.ToString();
                }
            }
            catch { tempString = ""; }
            

            return tempString;

        }

        private void textBoxMeasMinX_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMinX.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMinX.Text = result;
                setupMeasureButtons();
            }


        }
        private void textBoxMeasMinY_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMinY.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMinY.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasMinZ_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMinZ.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMinZ.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasMaxX_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMaxX.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMaxX.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasMaxY_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMaxY.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMaxY.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasMaxZ_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasMaxZ.Text, 0, 50);
            if (result != "")
            {
                textBoxMeasMaxZ.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasStepX_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasStepX.Text, 1, 1000);
            if (result != "")
            {
                textBoxMeasStepX.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasStepY_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasStepY.Text, 1, 1000);
            if (result != "")
            {
                textBoxMeasStepY.Text = result;
                setupMeasureButtons();
            }
        }

        private void textBoxMeasStepZ_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(textBoxMeasStepZ.Text, 1, 1000);
            if (result != "")
            {
                textBoxMeasStepZ.Text = result;
                setupMeasureButtons();
            }
        }

        private void txtBoxSetPosX_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(txtBoxSetPosX.Text, 0, 50);
            if (result != "")
            {
                txtBoxSetPosX.Text = result;

            }
            else
            {
                txtBoxSetPosX.Text = "0";
            }
        }

        private void comboBoxPlotSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateChart = true;


            
                newData = true;

            



        }

        private void checkBoxPlotAll_CheckedChanged(object sender, EventArgs e)
        {
            updateChart = true;
            comboBoxPlotSelect.Enabled = !checkBoxPlotAll.Checked;
        
                newData = true;

          

        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            //Build the CSV file data as a Comma separated string.
            csv = string.Empty;

            

            csv += "ProbeModel:" + TeslameterF71.UnitProbeModel + ',';
            csv += "ProbeSerial:" + TeslameterF71.UnitProbeSerial + ',';
            csv += "ProbeCalDate:" + TeslameterF71.UnitProbeCalDate + ',';
            csv += "ProbeOrientation:" + TeslameterF71.UnitProbeOrientation + ',';
            csv += "ProbeType:" + TeslameterF71.UnitProbeType + ',';
            csv += "AveragingOn:" + checkBoxAveraging.Checked + ',';
            csv += "AveragingNum:" + textBoxAverageMeasure.Text + ',';
            csv += "MeasureTime:" + comboBoxTeslaAveraging.Text + ',';
            csv += "DwellOn:" + checkBoxDwell.Checked + ',';
            csv += "DwellTime:" +textBoxDwell.Text + ',';
            csv += "StopForMeas:" + checkBoxStopToMeasure.Checked + ',';
            csv += "PointsX:" + textBoxMeasStepX.Text + ',';
            csv += "PointsY:" + textBoxMeasStepY.Text + ',';
            csv += "PointsZ:" + textBoxMeasStepZ.Text + ',';
            csv += "PointsMeasured:" + pointsMeasured + ','; 

            csv += "\r\n";

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //Add the Data rows.
                    if(cell.Value != null)
                    {
                        csv += cell.Value.ToString().Replace(",", ";") + ',';
                    }
                    Console.WriteLine(cell.RowIndex);
                    
                }

                //Add new line.
                csv += "\r\n";
            }

            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult result = dialog.ShowDialog();
            string selectedPath = "";
            if (result == DialogResult.OK)
            {
                selectedPath = dialog.FileName;
            }

            //Exporting to CSV.

            File.WriteAllText(selectedPath , csv);
        }

        private void comboBoxTeslaAveraging_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            TeslameterF71.QueueCommand(":SENS:AVER:COUN " + (int.Parse(comboBoxTeslaAveraging.GetItemText(comboBoxTeslaAveraging.SelectedItem)) /10).ToString());
           
        }

        private void checkBoxStopToMeasure_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStopToMeasure.Checked)
            {
                textBoxAverageMeasure.Enabled = true;
                checkBoxAveraging.Enabled = true;
                textBoxDwell.Enabled = true;
                checkBoxDwell.Enabled = true;

            }
            else
            {
                textBoxAverageMeasure.Enabled = false;
                checkBoxAveraging.Enabled = false;
                textBoxDwell.Enabled = false;
                checkBoxDwell.Enabled = false;
            }
        }

        private void timerShutdown_Tick(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            this.Close();
        }

        private void buttonLoadData_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string line1 = File.ReadLines(dlg.FileName).First();
            String[] readbacks = line1.Split(',');
         
           
            if (readbacks[7].Split(':')[1] == "10") { comboBoxTeslaAveraging.SelectedIndex = 0; }
            else if (readbacks[7].Split(':')[1] == "100") { comboBoxTeslaAveraging.SelectedIndex = 1; }
            else if (readbacks[7].Split(':')[1] == "200") { comboBoxTeslaAveraging.SelectedIndex = 2; }
            else if (readbacks[7].Split(':')[1] == "1000") { comboBoxTeslaAveraging.SelectedIndex = 3; }
            else if (readbacks[7].Split(':')[1] == "5000") { comboBoxTeslaAveraging.SelectedIndex = 4; }
            else if (readbacks[7].Split(':')[1] == "10000") { comboBoxTeslaAveraging.SelectedIndex = 5; }

            checkBoxAveraging.Checked = bool.Parse(readbacks[5].Split(':')[1]);
            textBoxAverageMeasure.Text = readbacks[6].Split(':')[1];
            checkBoxDwell.Checked = bool.Parse(readbacks[8].Split(':')[1]);
            textBoxDwell.Text = readbacks[9].Split(':')[1];
            checkBoxStopToMeasure.Checked = bool.Parse(readbacks[10].Split(':')[1]);
            measurePointsX= int.Parse(readbacks[11].Split(':')[1]);
           
            measurePointsY = int.Parse(readbacks[12].Split(':')[1]);
            measurePointsZ = int.Parse(readbacks[13].Split(':')[1]);
            pointsMeasured = int.Parse(readbacks[14].Split(':')[1]);
            // Adding the rows
            foreach (var line in File.ReadLines(dlg.FileName).Skip(2))
                {
                   
                    dataGridView1.Rows.Add(line.Split(','));
                }

            updateChart = true;
            newData = true;
             
            
        }

        private void buttonConnectTeslameter_Click(object sender, EventArgs e)
        {
            TeslameterF71.Init();
        }

     


        private void comboBoxFieldSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            plotIndex = comboBoxFieldSelect.SelectedIndex;
            fieldPlotChanged = true;
            updateChart = true;
         
                newData = true;

            

        }


        private async void buttonStartTimeMeasure_Click(object sender, EventArgs e)
        {


            pointsMeasured = 0;
            panelScott.Visible = true;

            bStartTimeMeasure = true;

            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult result = dialog.ShowDialog();
            saveFilePath = "";
            if (result == DialogResult.OK)
            {
                saveFilePath = dialog.FileName;
            }

            csv = string.Empty;

            tsw = new StreamWriter(saveFilePath, true);

            csv += "ProbeModel:" + TeslameterF71.UnitProbeModel + ',';
            csv += "ProbeSerial:" + TeslameterF71.UnitProbeSerial + ',';
            csv += "ProbeCalDate:" + TeslameterF71.UnitProbeCalDate + ',';
            csv += "ProbeOrientation:" + TeslameterF71.UnitProbeOrientation + ',';
            csv += "ProbeType:" + TeslameterF71.UnitProbeType + ',';
            csv += "AveragingOn:" + checkBoxAveraging.Checked + ',';
            csv += "AveragingNum:" + textBoxAverageMeasure.Text + ',';
            csv += "MeasureTime:" + comboBoxTeslaAveraging.Text + ',';
        
           
           

            tsw.WriteLine(csv);
            csv = "";

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            tsw.WriteLine(csv);
            csv = "";

            measurementRunning = true;
            textBoxMeasMaxX.Enabled = false;
            textBoxMeasMaxY.Enabled = false;
            textBoxMeasMaxZ.Enabled = false;
            textBoxMeasMinX.Enabled = false;
            textBoxMeasMinY.Enabled = false;
            textBoxMeasMinZ.Enabled = false;
            textBoxMeasStepX.Enabled = false;
            textBoxMeasStepY.Enabled = false;
            textBoxMeasStepZ.Enabled = false;

            textBoxAverageMeasure.Enabled = false;
            checkBoxAveraging.Enabled = false;
            textBoxDwell.Enabled = false;
            checkBoxDwell.Enabled = false;
            comboBoxTeslaAveraging.Enabled = false;
            checkBoxStopToMeasure.Enabled = false;

            buttonMeasure.Enabled = false;
            buttonStopMeasure.Enabled = true;
            buttonLoadData.Enabled = false;

            txtBoxSetPosX.Enabled = false;
            txtBoxSetPosY.Enabled = false;
            txtBoxSetPosZ.Enabled = false;

            buttonGoTo.Enabled = false;
            buttonkCubeHomeX.Enabled = false;

            stopMeasure = false;

            dataGridView1.Rows.Clear();

            await Task.Run(() => StartTimeMeasure());

            measurementRunning = false;
            textBoxMeasMaxX.Enabled = true;
            textBoxMeasMaxY.Enabled = true;
            textBoxMeasMaxZ.Enabled = true;
            textBoxMeasMinX.Enabled = true;
            textBoxMeasMinY.Enabled = true;
            textBoxMeasMinZ.Enabled = true;
            textBoxMeasStepX.Enabled = true;
            textBoxMeasStepY.Enabled = true;
            textBoxMeasStepZ.Enabled = true;

            buttonMeasure.Enabled = true;
            buttonStopMeasure.Enabled = false;
            buttonLoadData.Enabled = true;

            txtBoxSetPosX.Enabled = true;
            txtBoxSetPosY.Enabled = true;
            txtBoxSetPosZ.Enabled = true;

            buttonGoTo.Enabled = true;
            buttonkCubeHomeX.Enabled = true;

            textBoxAverageMeasure.Enabled = true;
            checkBoxAveraging.Enabled = true;
            textBoxDwell.Enabled = true;
            checkBoxDwell.Enabled = true;
            comboBoxTeslaAveraging.Enabled = true;
            checkBoxStopToMeasure.Enabled = true;
            bStartTimeMeasure = false;

        }

        private void txtBoxSetPosY_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(txtBoxSetPosY.Text, 0, 50);
            if (result != "")
            {
                txtBoxSetPosY.Text = result;

            }
            else
            {
                txtBoxSetPosY.Text = "0";
            }
        }

        private void txtBoxSetPosZ_TextChanged(object sender, EventArgs e)
        {
            string result = checkInput(txtBoxSetPosZ.Text, 0, 50);
            if (result != "")
            {
                txtBoxSetPosZ.Text = result;

            }
            else
            {
                txtBoxSetPosZ.Text = "0";
            }
        }

        private void buttonStopMeasure_Click(object sender, EventArgs e)
        {
            stopMeasure = true;
        }



        private void homeXYZ() {
            // Home the stage/actuator.




            #region ParallelTasks
            // Perform three tasks in parallel on the source array
            Parallel.Invoke(
                () =>
                {
                    Console.WriteLine("kCubeX is Homing");
                    kCubeX.Home(90000);

                },  // close first Action

                () =>
                {

                    Console.WriteLine("kCubeY is Homing");
                    kCubeY.Home(90000);

                }, //close second Action

                () =>
                {


                    Console.WriteLine("kCubeZ is Homing");
                    kCubeZ.Home(90000);
                } //close third Action
            ); //close parallel.invoke


            #endregion
        }

            private async void buttonGoTo_Click(object sender, EventArgs e)
        {

            await Task.Run(() => goToXYZ(double.Parse(txtBoxSetPosX.Text), double.Parse(txtBoxSetPosY.Text), double.Parse(txtBoxSetPosZ.Text)));



        }


      
        private async void buttonMeasure_Click(object sender, EventArgs e)
        {
            bStartCubeMeasure = true;
            panelScott.Visible = false;


            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult result = dialog.ShowDialog();
            saveFilePath = "";
            if (result == DialogResult.OK)
            {
                saveFilePath = dialog.FileName;
            }

            csv = string.Empty;

            tsw = new StreamWriter(saveFilePath, true);

            csv += "ProbeModel:" + TeslameterF71.UnitProbeModel + ',';
            csv += "ProbeSerial:" + TeslameterF71.UnitProbeSerial + ',';
            csv += "ProbeCalDate:" + TeslameterF71.UnitProbeCalDate + ',';
            csv += "ProbeOrientation:" + TeslameterF71.UnitProbeOrientation + ',';
            csv += "ProbeType:" + TeslameterF71.UnitProbeType + ',';
            csv += "AveragingOn:" + checkBoxAveraging.Checked + ',';
            csv += "AveragingNum:" + textBoxAverageMeasure.Text + ',';
            csv += "MeasureTime:" + comboBoxTeslaAveraging.Text + ',';
            csv += "DwellOn:" + checkBoxDwell.Checked + ',';
            csv += "DwellTime:" + textBoxDwell.Text + ',';
            csv += "StopForMeas:" + checkBoxStopToMeasure.Checked + ',';
            csv += "PointsX:" + textBoxMeasStepX.Text + ',';
            csv += "PointsY:" + textBoxMeasStepY.Text + ',';
            csv += "PointsZ:" + textBoxMeasStepZ.Text + ',';
            csv += "PointsMeasured:" + pointsMeasured + ',';

            tsw.WriteLine(csv);
            csv = "";

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            tsw.WriteLine(csv);
            csv = "";

            measurementRunning = true;
            textBoxMeasMaxX.Enabled = false;
            textBoxMeasMaxY.Enabled = false;
            textBoxMeasMaxZ.Enabled = false;
            textBoxMeasMinX.Enabled = false;
            textBoxMeasMinY.Enabled = false;
            textBoxMeasMinZ.Enabled = false;
            textBoxMeasStepX.Enabled = false;
            textBoxMeasStepY.Enabled = false;
            textBoxMeasStepZ.Enabled = false;

            textBoxAverageMeasure.Enabled = false;
            checkBoxAveraging.Enabled = false;
            textBoxDwell.Enabled = false;
            checkBoxDwell.Enabled = false;
            comboBoxTeslaAveraging.Enabled = false;
            checkBoxStopToMeasure.Enabled = false;

            buttonMeasure.Enabled = false;
            buttonStopMeasure.Enabled = true;
            buttonLoadData.Enabled = false;

            txtBoxSetPosX.Enabled = false;
            txtBoxSetPosY.Enabled = false;
            txtBoxSetPosZ.Enabled = false;

            buttonGoTo.Enabled = false;
            buttonkCubeHomeX.Enabled = false;

            stopMeasure = false;

            dataGridView1.Rows.Clear();

            await Task.Run(() => StartCubeMeasure());

            measurementRunning = false;
            textBoxMeasMaxX.Enabled = true;
            textBoxMeasMaxY.Enabled = true;
            textBoxMeasMaxZ.Enabled = true;
            textBoxMeasMinX.Enabled = true;
            textBoxMeasMinY.Enabled = true;
            textBoxMeasMinZ.Enabled = true;
            textBoxMeasStepX.Enabled = true;
            textBoxMeasStepY.Enabled = true;
            textBoxMeasStepZ.Enabled = true;

            buttonMeasure.Enabled = true;
            buttonStopMeasure.Enabled = false;
            buttonLoadData.Enabled = true;

            txtBoxSetPosX.Enabled = true;
            txtBoxSetPosY.Enabled = true;
            txtBoxSetPosZ.Enabled = true;

            buttonGoTo.Enabled = true;
            buttonkCubeHomeX.Enabled = true;

            textBoxAverageMeasure.Enabled = true;
            checkBoxAveraging.Enabled = true;
            textBoxDwell.Enabled = true;
            checkBoxDwell.Enabled = true;
            comboBoxTeslaAveraging.Enabled = true;
            checkBoxStopToMeasure.Enabled = true;

            bStartCubeMeasure = false;

        }

        private void StartCubeMeasure()
        {


            double measMinX = Double.Parse(textBoxMeasMinX.Text);
            double measMinY = Double.Parse(textBoxMeasMinY.Text);
            double measMinZ = Double.Parse(textBoxMeasMinZ.Text);

            double measMaxX = Double.Parse(textBoxMeasMaxX.Text);
            double measMaxY = Double.Parse(textBoxMeasMaxY.Text);
            double measMaxZ = Double.Parse(textBoxMeasMaxZ.Text);


            double LengthX = measMaxX - measMinX;
            measurePointsX = (int)(LengthX / stepSizeX);

            double LengthY = measMaxY - measMinY;
            measurePointsY = (int)(LengthY / stepSizeY);

            double LengthZ = measMaxZ - measMinZ;
            measurePointsZ = (int)(LengthZ / stepSizeZ);

            lastZindex = -1;

            i_Scheme = new cColorScheme(me_ColorScheme);
            i_Mesg = new cMessgData[measurePointsZ + 1];
            lastPointPlotted = 0;
            i_ShapeData = new cScatterData[measurePointsZ + 1];



            for (int i = 0; i < measurePointsZ + 1; i++)
            {
                i_ShapeData[i] = new cScatterData(i_Scheme);

            }



            pointsMeasured = 0;
            pointsMeasuredX = 0;
            pointsMeasuredY = 0;


            goToXYZ(measMinX, measMinY, measMinZ);
            startTime = DateTime.Now;

            int averageLoops = int.Parse(textBoxAverageMeasure.Text);
            int dwellTime = int.Parse(textBoxDwell.Text);
            int averageTime = TeslameterF71.UnitAveragingWindow * 10;


            for (int k = 0; k < measurePointsZ + 1; k++)
            {
                for (int j = 0; j < measurePointsY + 1; j++)
                {

                    for (int i = 0; i < measurePointsX + 1; i++)
                    {
                        currentZindex = k;

                        double tempX, tempY, tempZ;
                        tempX = 0;
                        tempY = 0;
                        tempZ = 0;


                        if (k % 2 == 0) //is even
                        {
                            tempY = measMinY + j * stepSizeY;

                            if (j % 2 == 0) //is even
                            {
                                tempX = measMinX + i * stepSizeX;
                            }
                            else
                            {
                                tempX = measMinX + (measurePointsX - i) * stepSizeX;
                            }

                        }
                        else
                        {
                            tempY = measMinY + (measurePointsY - j) * stepSizeY;


                            if (j % 2 == 0) //is even
                            {
                                tempX = measMinX + (measurePointsX - i) * stepSizeX;
                            }
                            else
                            {
                                tempX = measMinX + i * stepSizeX;
                            }

                        }



                        if (Double.IsNaN(tempX))
                        {
                            tempX = measMinX;
                        }
                        if (Double.IsNaN(tempY))
                        {
                            tempY = measMinY;
                        }


                        tempZ = measMinZ + k * stepSizeZ;
                        if (Double.IsNaN(tempZ))
                        {
                            tempZ = measMinZ;
                        }

                        tempX = Math.Round(tempX, 2);
                        tempY = Math.Round(tempY, 2);
                        tempZ = Math.Round(tempZ, 2);

                        nextXpos = tempX;
                        nextYpos = tempY;
                        nextZpos = tempZ;
                        nextPos = true;




                        goToXYZ(Math.Round(tempX, 2), Math.Round(tempY, 2), Math.Round(tempZ, 2));






                        if (checkBoxStopToMeasure.Checked)
                        {
                            if (checkBoxDwell.Checked)
                            {

                                Thread.Sleep(dwellTime);
                            }




                            Thread.Sleep(averageTime);

                        }
                        double posX = (double)kCubeX.Position;
                        double posY = (double)kCubeY.Position;
                        double posZ = -((double)kCubeZ.Position - 50);

                        Sx = tempX;
                        Sy = tempY;
                        Sz = tempZ;

                        Px = posX;
                        Py = posY;
                        Pz = posZ;

                        ArrayIndexX = i;
                        ArrayIndexY = j;
                        ArrayIndexZ = k;

                        while (!TeslameterF71.ReadbacksAvailable)
                        {
                            Thread.Sleep(10);

                        }


                        double UnitFieldMagnitude = TeslameterF71.UnitFieldMagnitude * 1000;
                        double UnitFieldX = TeslameterF71.UnitFieldX * 1000;
                        double UnitFieldY = TeslameterF71.UnitFieldY * 1000;
                        double UnitFieldZ = TeslameterF71.UnitFieldZ * 1000;
                        double UnitFieldTemp = TeslameterF71.UnitFieldTemp;

                        if (UnitFieldMagnitude > 200)
                        {
                            Console.WriteLine("fuck up");
                        }

                        double[] statsBMag = new double[averageLoops];
                        double[] statsBx = new double[averageLoops];
                        double[] statsBy = new double[averageLoops];
                        double[] statsBz = new double[averageLoops];



                        double SDx, SDy, SDz, SDm;

                        SDx = 0;
                        SDy = 0;
                        SDz = 0;
                        SDm = 0;


                        if (checkBoxAveraging.Checked)
                        {
                            statsBMag[0] = UnitFieldMagnitude;
                            statsBx[0] = UnitFieldX;
                            statsBy[0] = UnitFieldY;
                            statsBz[0] = UnitFieldZ;
                            TeslameterF71.ReadbacksAvailable = false;

                            for (int aveLoop = 1; aveLoop < averageLoops; aveLoop++)
                            {
                                Thread.Sleep(averageTime);
                                while (!TeslameterF71.ReadbacksAvailable)
                                {
                                    Thread.Sleep(10);
                                }

                                statsBMag[aveLoop] = TeslameterF71.UnitFieldMagnitude * 1000;
                                statsBx[aveLoop] = TeslameterF71.UnitFieldX * 1000;
                                statsBy[aveLoop] = TeslameterF71.UnitFieldY * 1000;
                                statsBz[aveLoop] = TeslameterF71.UnitFieldZ * 1000;
                                TeslameterF71.ReadbacksAvailable = false;



                            }

                            UnitFieldMagnitude = statsBMag.Average();
                            UnitFieldX = statsBx.Average();
                            UnitFieldY = statsBy.Average();
                            UnitFieldZ = statsBz.Average();

                            SDx = getStandardDeviation(statsBMag);
                            SDy = getStandardDeviation(statsBx);
                            SDz = getStandardDeviation(statsBy);
                            SDm = getStandardDeviation(statsBz);

                        }
                        TeslameterF71.ReadbacksAvailable = false;


                        Bm = UnitFieldMagnitude;
                        Bx = UnitFieldX;
                        By = UnitFieldY;
                        Bz = UnitFieldZ;

                        BmSD = SDm;
                        BxSD = SDx;
                        BySD = SDy;
                        BzSD = SDz;


                        Temp = UnitFieldTemp;







                        DateTime nowTime = DateTime.Now;
                        measTime = nowTime;


                        TimeSpan timeBetweenMeasurements = nowTime.Subtract(startTime);
                        int pointsRemaining = ((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1)) - (pointsMeasured + 1);


                        if (pointsMeasured > 0)
                        {
                            double timeRemaining = pointsRemaining * (timeBetweenMeasurements.TotalSeconds / (pointsMeasured));
                            TimeSpan timeSpanRemaining = TimeSpan.FromSeconds(timeRemaining);
                            timeRemainString = timeSpanRemaining.ToString(@"dd\:HH\:mm\:ss");
                        }
                        pointsMeasured++;
                        percentageComplete = 100 * pointsMeasured / ((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1));






                        if (pointsMeasuredX < i + 1) { pointsMeasuredX = i + 1; }
                        if (pointsMeasuredY < j + 1) { pointsMeasuredY = j + 1; }

                        updateChart = true;
                        newData = true;





                        if (stopMeasure)
                        {
                            measurementRunning = false;
                            return;
                        }





                    }

                }


            }
            tsw.Close();

        }

        private void StartTimeMeasure()
        {


            double setX = Double.Parse(textBoxTimeMeasX.Text);
            double setY = Double.Parse(textBoxTimeMeasY.Text);
            double setZ = Double.Parse(textBoxTimeMeasZ.Text);

           

           
            


           


            goToXYZ(setX, setY, setZ);
            startTime = DateTime.Now;
            DateTime endTime;

            endTime = startTime.AddHours(double.Parse(textBoxTimeMeasHour.Text));
            endTime = endTime.AddMinutes(double.Parse(textBoxTimeMeasMin.Text));
            endTime = endTime.AddSeconds(double.Parse(textBoxTimeMeasSec.Text));


            int averageLoops = int.Parse(textBoxAverageMeasure.Text);
            
            int averageTime = TeslameterF71.UnitAveragingWindow * 10;

            //chyange to while loop - while time elapsed less than timeend - timestart

            while(DateTime.Now< endTime)
            {
   
                        

                         Thread.Sleep(averageTime);

                        
                        double posX = (double)kCubeX.Position;
                        double posY = (double)kCubeY.Position;
                        double posZ = -((double)kCubeZ.Position-50);

                        Sx = setX ;
                        Sy = setY;
                        Sz = setZ ;

                        Px = posX ;
                        Py = posY ;
                        Pz = posZ ;

                       

                        while(!TeslameterF71.ReadbacksAvailable)
                        {
                            Thread.Sleep(10);
                            
                        }
                        

                        double UnitFieldMagnitude =  TeslameterF71.UnitFieldMagnitude * 1000;
                        double UnitFieldX = TeslameterF71.UnitFieldX * 1000;
                        double UnitFieldY = TeslameterF71.UnitFieldY * 1000;
                        double UnitFieldZ = TeslameterF71.UnitFieldZ * 1000;
                        double UnitFieldTemp = TeslameterF71.UnitFieldTemp;

                        if(UnitFieldMagnitude>200) { 
                            Console.WriteLine("fuck up");
                        }

                        double[] statsBMag = new double[averageLoops];
                        double[] statsBx = new double[averageLoops];
                        double[] statsBy = new double[averageLoops];
                        double[] statsBz = new double[averageLoops];
                        


                        double SDx, SDy, SDz, SDm;

                        SDx = 0;
                        SDy = 0;
                        SDz = 0;
                        SDm = 0;


                        if (checkBoxAveraging.Checked)
                        {
                            statsBMag[0] = UnitFieldMagnitude;
                            statsBx[0] = UnitFieldX;
                            statsBy[0] = UnitFieldY;
                            statsBz[0] = UnitFieldZ;
                            TeslameterF71.ReadbacksAvailable = false;

                            for (int aveLoop = 1; aveLoop < averageLoops; aveLoop++)
                            {
                                Thread.Sleep(averageTime);
                                while (!TeslameterF71.ReadbacksAvailable)
                                {
                                    Thread.Sleep(10);
                                }
                                
                                statsBMag[aveLoop] = TeslameterF71.UnitFieldMagnitude * 1000;
                                statsBx[aveLoop] = TeslameterF71.UnitFieldX * 1000;
                                statsBy[aveLoop] = TeslameterF71.UnitFieldY * 1000;
                                statsBz[aveLoop] = TeslameterF71.UnitFieldZ * 1000;
                                TeslameterF71.ReadbacksAvailable = false;



                            }
                            
                            UnitFieldMagnitude = statsBMag.Average();
                            UnitFieldX = statsBx.Average();
                            UnitFieldY = statsBy.Average();
                            UnitFieldZ = statsBz.Average();

                            SDx = getStandardDeviation(statsBMag);
                            SDy = getStandardDeviation(statsBx);
                            SDz = getStandardDeviation(statsBy);
                            SDm = getStandardDeviation(statsBz);

                        }
                        TeslameterF71.ReadbacksAvailable = false;


                        Bm = UnitFieldMagnitude ;
                        Bx = UnitFieldX  ;
                        By = UnitFieldY ;
                        Bz = UnitFieldZ ;

                        BmSD = SDm ;
                        BxSD = SDx ;
                        BySD = SDy ;
                        BzSD = SDz ;


                        Temp = UnitFieldTemp ;







                DateTime nowTime = DateTime.Now;
                measTime = nowTime;



                TimeSpan timeRemaining;
                TimeSpan timeTotal ;
                TimeSpan timeElapsed ;


                if (pointsMeasured > 0)
                        {
                            timeRemaining = endTime- nowTime;
                            timeTotal = endTime- startTime;
                            timeElapsed = nowTime - startTime;
                            timeRemainString = timeRemaining.ToString();
                            percentageComplete = Math.Round(timeElapsed.TotalSeconds / timeTotal.TotalSeconds,3);
                }
                        pointsMeasured++;
                

                       
                        
                                                       


                      

                        updateChart = true;
                        newData = true;

                        
                      
                        

                        if (stopMeasure) {
                            measurementRunning = false;
                            return; }





                    
                   
                
               
               
            }
            tsw.Close();
            
        }

        private double getStandardDeviation(double[] someDoubles)
        {
            double sd = 0;
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);

            return sd;
        }
        private void goToXYZ(double PosX, double PosY, double PosZ)
        {
            #region ParallelTasks

            PosZ = 50 - PosZ;

            // Perform three tasks in parallel on the source array
            Parallel.Invoke(
                () =>
                {
                    try
                    {
                        kCubeX.MoveTo((decimal)PosX, 60000);
                       
                       

                    }
                    catch {
                    
                    }
                },  // close first Action

                () =>
                {
                    try
                    {

                        kCubeY.MoveTo((decimal)PosY, 60000);


                    }
                    catch { 
                    
                    }
                }, //close second Action

                () =>
                {
                    try
                    {

                        kCubeZ.MoveTo((decimal)PosZ, 60000);

                    }
                    catch { }
                } //close third Action
            ); //close parallel.invoke


            #endregion
        }
        private void timerUI_Tick(object sender, EventArgs e)
        {
            

            try
            {
                labelFieldX.Text = "x: " + (TeslameterF71.UnitFieldX * 1000).ToString("00.000") + " m" + TeslameterF71.UnitUnits;
                labelFieldY.Text = "y: " + (TeslameterF71.UnitFieldY * 1000).ToString("00.000") + " m" + TeslameterF71.UnitUnits;
                labelFieldZ.Text = "z: " + (TeslameterF71.UnitFieldZ * 1000).ToString("00.000") + " m" + TeslameterF71.UnitUnits;

                labelFieldMag.Text = (TeslameterF71.UnitFieldMagnitude * 1000).ToString("00.000") + " m" + TeslameterF71.UnitUnits;

                labelFieldTemp.Text = TeslameterF71.UnitFieldTemp.ToString("00.00") + " C";

                labelAveWindow.Text = "Averaging: " + Math.Round((double)((double)TeslameterF71.UnitAveragingWindow / 100),4) + " s" ;

                labelXpos.Text =  kCubeX.DevicePosition.ToString("0.00") ;
                labelYpos.Text =  kCubeY.Position.ToString("0.00") ;
                labelZpos.Text =  (50-kCubeZ.Position).ToString("0.00");
                
                
                if (updateChart&&newData)
                    {
                    string dateString = measTime .ToString(@"yyyy\-MM\-dd");
                    string timeString = measTime .ToString(@"HH\:mm\:ss\.fff");

                    if(pointsMeasured> dataGridView1.Rows.Count)
                    {
                        csv += pointsMeasured.ToString() + "," + dateString + "," + timeString + "," + Sx.ToString() + "," + Sy.ToString() + "," + Sz.ToString() + "," + Px.ToString() + "," + Py.ToString() + "," + Pz.ToString() + "," + Bm.ToString() + "," + BmSD.ToString() + "," + Bx.ToString() + "," + BxSD.ToString() + "," + By.ToString() + "," + BySD.ToString() + "," + Bz.ToString() + "," + BzSD.ToString() + "," + Temp.ToString() + "," + ArrayIndexX.ToString() + "," + ArrayIndexY.ToString() + "," + ArrayIndexZ.ToString();
                        tsw.WriteLine(csv);
                        csv = "";
                        dataGridView1.Rows.Add(new object[] { pointsMeasured, dateString, timeString, Sx , Sy , Sz , Px , Py , Pz , Bm , BmSD , Bx , BxSD , By , BySD , Bz , BzSD , Temp , ArrayIndexX , ArrayIndexY , ArrayIndexZ  }); 
                    //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;


                    progressBarMeasure.Value = (int)percentageComplete;

                        if (bStartCubeMeasure)
                        {
                            labelProgress.Text = "Point " + pointsMeasured.ToString() + " of " + ((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1)).ToString() + " " + Math.Round(percentageComplete, 1).ToString() + " % " + timeRemainString;

                        }
                        else
                        {
                            labelProgress.Text = "Point " + pointsMeasured.ToString() + " " + Math.Round(percentageComplete, 1).ToString() + " % " + timeRemainString;

                        }
                    }
                    newData = false;


                    //reintialise plot ishapes



                    if (bStartCubeMeasure)
                    {
                        DrawDemo();
                    }

                    if (bStartTimeMeasure)
                    {

                        DateTime[] xS = new DateTime[dataGridView1.Rows.Count];
                        double[] xY = new double[dataGridView1.Rows.Count];

                        int i = 0;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            xS[i] = DateTime.Parse(row.Cells[1].Value.ToString()+" "+row.Cells[2].Value.ToString()) ;
                            xY[i] = double.Parse(row.Cells[9].Value.ToString()) ;
                            i++;
                        }

                        FormsPlot1.Plot.Clear();

                        FormsPlot1.Plot.Add.SignalXY(xS, xY);
                        FormsPlot1.Plot.Axes.DateTimeTicksBottom();

                        FormsPlot1.Refresh();
                    }

                }

                if (bStartCubeMeasure)
                {
                    if (currentZindex > lastZindex)
                    {
                        comboBoxPlotSelect.Items.Clear();
                        for (int l = 0; l < currentZindex + 1; l++)
                        {
                            comboBoxPlotSelect.Items.Add(l);
                            comboBoxPlotSelect.SelectedIndex = l;
                        }
                        lastZindex = currentZindex;

                    }

                    if (nextPos)
                    {
                        txtBoxSetPosX.Text = Math.Round(nextXpos, 2).ToString();
                        txtBoxSetPosY.Text = Math.Round(nextYpos, 2).ToString();
                        txtBoxSetPosZ.Text = Math.Round(nextZpos, 2).ToString();
                        nextPos = false;
                    }
                }
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

       

     


        

    }
}
