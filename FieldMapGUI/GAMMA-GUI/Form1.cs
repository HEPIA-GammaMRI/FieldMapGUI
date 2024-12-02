// .NET Libraries for general functionality
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;

// 3rd Party Libraries for Plotting and Data Visualization
using ScottPlot;
using ScottPlot.WinForms;
using Plot3D;

// Libraries for Motion Control
using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.GenericMotorCLI.Settings;
using Thorlabs.MotionControl.KCube.DCServoCLI;
using Thorlabs.MotionControl.Tools.Common;

// Enums from Plot3D
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

// Classes from Plot3D for handling 3D data and objects
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

// Callback function for rendering
using delRendererFunction = Plot3D.Editor3D.delRendererFunction;

// XML and Cryptography Libraries
using System.Xml.Linq;
using System.Security.Cryptography;

// Database and Data Management
using System.Data.Common;



namespace FieldMapGUI
{
    public partial class Form1 : Form
    {

        // Initialize FormsPlot to be added to the form
        readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        // Magnetic field data variables (Bm = total magnetic field, Bx, By, Bz = components of magnetic field)
        double Bm;
        double Bx;
        double By;
        double Bz;

        // Standard deviation values for the magnetic field components
        double BmSD;
        double BxSD;
        double BySD;
        double BzSD;

        // Position data (Px, Py, Pz = coordinates, Sx, Sy, Sz = additional position-related measurements)
        double Px;
        double Py;
        double Pz;
        double Sx;
        double Sy;
        double Sz;
        double Temp; // Temperature value
        int ArrayIndexX; // Index for X dimension
        int ArrayIndexY; // Index for Y dimension
        int ArrayIndexZ; // Index for Z dimension

        // Time of measurement
        DateTime measTime;

        // Data structures for scatter plot
        cScatterData[] i_ShapeData;
        cMessgData[] i_Mesg;
        cColorScheme i_Scheme; // Color scheme for plotting

        // Timing variables
        DateTime startTime; // Start time of measurement
        bool bStartTimeMeasure, bStartCubeMeasure; // Flags for measurement state

        // Remaining time as string for UI display
        string timeRemainString;

        // Measurement parameters
        int measurePointsX, measurePointsY, measurePointsZ; // Number of measurement points in each dimension
        int pointsMeasured, pointsMeasuredX, pointsMeasuredY; // Counter for measured points
        double stepSizeX, stepSizeY, stepSizeZ; // Step sizes for measurement grid
        int currentZindex, lastZindex; // Z-axis measurement indices

        // Measurement status flags
        bool stopMeasure; // Stop measurement flag
        bool measurementRunning; // Is the measurement currently running?
        bool updateChart; // Flag to update chart display
        bool newData; // Flag to check for new data

        // Positioning variables for the next measurement point
        double nextXpos, nextYpos, nextZpos; // Next position coordinates
        bool nextPos; // Flag indicating whether the next position is ready

        // Progress tracking for measurement completion
        double percentageComplete;

        // Tracking the last point that was plotted
        int lastPointPlotted;

        // Flags for plot data change
        bool fieldPlotChanged = false;
        int plotIndex = 0; // Index for plotting different datasets

        // File path for saving data and CSV export
        string saveFilePath = "";
        string csv; // CSV data in string format
        TextWriter tsw; // TextWriter for saving data to file

        #region enums

        // Enum for different demo modes for visualizations
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
            Animation
        }

        #endregion

        // Demo mode and color scheme variables
        eDemo me_Demo;
        eColorScheme me_ColorScheme;

        // Message data for special hints and selection mode
        cMessgData mi_MesgTop = new cMessgData("", -7, 7, System.Drawing.Color.Blue); // For special hint
        cMessgData mi_MesgBottom = new cMessgData("", -7, -7, System.Drawing.Color.Gray); // For selection mode

        // Timer for status updates
        System.Windows.Forms.Timer mi_StatusTimer = new System.Windows.Forms.Timer();

        // External device communication classes for Teslameter and Motor control
        TeslameterF71Comms TeslameterF71;
        KCubeDCServo kCubeX, kCubeY, kCubeZ;

        // Serial numbers for connected controllers (e.g., motors)
        string serialNoX = "27268143";
        string serialNoY = "27268080";
        string serialNoZ = "27268120";



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Create a message to indicate the system is waiting for data points
            cMessgData waitingMessage = new cMessgData("Waiting for data points", 10, 10, System.Drawing.Color.Blue);

            // Clear previous data and add the waiting message
            editor3D.Clear();
            editor3D.AddMessageData(waitingMessage);

            // Refresh the 3D editor to reflect changes
            editor3D.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // Adjust the status label width to fit the form's client size
            statusLabel.AutoSize = false;
            statusLabel.Width = ClientSize.Width - 30; // Leave a small margin
        }

        void OnClearStatusTimer(object sender, EventArgs e)
        {
            // Stop the timer and clear the status label text
            mi_StatusTimer.Stop();
            statusLabel.Text = "";
        }

        public Form1()
        {
            // Initialize the components of the form
            InitializeComponent();

            // Attach the FormClosing event handler to handle form close actions
            this.FormClosing += Form1_FormClosing;

            // Set the status timer interval to 8000 milliseconds (8 seconds)
            mi_StatusTimer.Interval = 8000;

            // Attach the event handler to the timer tick event
            mi_StatusTimer.Tick += new EventHandler(OnClearStatusTimer);

            // Add the FormsPlot control to the panel for rendering
            panelScott.Controls.Add(FormsPlot1);
        }

        private void DrawDemo()
        {
            // Set user inputs for 3D editor
            editor3D.SetUserInputs(eMouseCtrl.L_Theta_L_Phi);

            // Stop the status timer
            mi_StatusTimer.Stop();

            // Set the color scheme and tooltip mode
            me_ColorScheme = eColorScheme.Rainbow_Bright;
            editor3D.TooltipMode = eTooltip.All;

            // Call the scatter plot demo (line removed for clarity)
            DemoScatterPlot(false);

            // Hide the zero on the Z-axis
            editor3D.AxisZ.IncludeZero = false;

            // Clear previous messages and add new ones
            editor3D.Clear();
            editor3D.AddMessageData(mi_MesgTop, mi_MesgBottom);

            // Update the status label with selection callback status
            statusLabel.Text = (editor3D.Selection.Callback == null) ? "Callback: OFF" : "";

            // Optionally, set selection-related messages (currently commented out)
            // SetSelectionMessages();
        }

        private void DemoScatterPlot(bool plotLines)
        {
            // Define constant for the size of the plot points
            const int SIZE = 5;

            // Initialize variables for storing plot data
            double d_X = 0, d_Y = 0, d_Z = 0, d_B = 0, d_T = 0;
            int tempLow = lastPointPlotted;

            // Handle changes in the field plot
            if (fieldPlotChanged)
            {
                i_ShapeData = new cScatterData[measurePointsZ + 1];

                // Initialize shape data array
                for (int i = 0; i <= measurePointsZ; i++)
                {
                    i_ShapeData[i] = new cScatterData(i_Scheme);
                    Console.WriteLine("ShapeData " + i);
                }

                tempLow = 0;
                fieldPlotChanged = false;
            }

            // Iterate through the measured points
            for (int C = tempLow; C < pointsMeasured; C++)
            {
                // Retrieve data from the DataGridView
                d_X = (double)dataGridView1.Rows[C].Cells[3].Value;
                d_Y = (double)dataGridView1.Rows[C].Cells[4].Value;
                d_Z = (double)dataGridView1.Rows[C].Cells[5].Value;

                // Set B based on the plot index
                d_B = GetBValueFromPlotIndex(C);

                // Retrieve Temperature
                d_T = (double)dataGridView1.Rows[C].Cells[17].Value;

                // Format the tooltip
                string tooltip = string.Format("X = {0}\nY = {1}\nZ = {2}\nB = {3} mT\nT = {4} C\n", d_X, d_Y, d_Z, d_B, d_T);

                // Create a 3D point
                cPoint3D point = new cPoint3D(d_X, d_Y, d_B, tooltip);

                int zDist = int.Parse(textBoxZdist.Text);

                // If "Plot All" is checked, modify the Z value
                if (checkBoxPlotAll.Checked)
                {
                    point = new cPoint3D(d_X, d_Y, d_B + (int)dataGridView1.Rows[C].Cells[20].Value * zDist, tooltip);
                }

                // Add point to the appropriate Z index
                i_ShapeData[(int)dataGridView1.Rows[C].Cells[20].Value].AddShape(point, eScatterShape.Circle, SIZE, null);

                // Update the last point plotted
                lastPointPlotted = C + 1;
            }

            // Clear previous data and prepare for rendering
            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;

            // Render data based on whether "Plot All" is checked
            if (checkBoxPlotAll.Checked)
            {
                for (int i = 0; i <= currentZindex; i++)
                {
                    editor3D.AddRenderData(i_ShapeData[i]);
                    i_Mesg[i] = new cMessgData("Z = " + d_Z.ToString(), 7, -7, System.Drawing.Color.Orange);
                    editor3D.AddMessageData(i_Mesg[i]);
                }
            }
            else
            {
                if (pointsMeasured > 0)
                {
                    int chosenZ = comboBoxPlotSelect.SelectedIndex;
                    cScatterData selectedShapeData = i_ShapeData[chosenZ];

                    editor3D.AddRenderData(selectedShapeData);
                    i_Mesg[chosenZ] = new cMessgData("Z = " + chosenZ.ToString(), 7, -7, System.Drawing.Color.Orange);
                    editor3D.AddMessageData(i_Mesg[chosenZ]);
                }
            }

            // Set 3D editor options
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
            editor3D.Raster = eRaster.Labels;
            editor3D.Invalidate();

            // Set the flag to false indicating the chart update is complete
            updateChart = false;
        }

        // Helper method to determine the B value based on the plot index
        private double GetBValueFromPlotIndex(int rowIndex)
        {
            switch (plotIndex)
            {
                case 0: return (double)dataGridView1.Rows[rowIndex].Cells[9].Value;
                case 1: return (double)dataGridView1.Rows[rowIndex].Cells[11].Value;
                case 2: return (double)dataGridView1.Rows[rowIndex].Cells[13].Value;
                case 3: return (double)dataGridView1.Rows[rowIndex].Cells[15].Value;
                case 4: return (double)dataGridView1.Rows[rowIndex].Cells[10].Value;
                case 5: return (double)dataGridView1.Rows[rowIndex].Cells[12].Value;
                case 6: return (double)dataGridView1.Rows[rowIndex].Cells[14].Value;
                case 7: return (double)dataGridView1.Rows[rowIndex].Cells[16].Value;
                case 8: return (double)dataGridView1.Rows[rowIndex].Cells[17].Value;
                default: return 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Reset Flags
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

            try
            {
                // Build device list for the Teslameter and Motors
                DeviceManagerCLI.BuildDeviceList();

                // Parallel initialization of devices
                Parallel.Invoke(
                    () =>
                    {
                        // Initialize the Teslameter
                        TeslameterF71 = new TeslameterF71Comms();
                        TeslameterF71.Init();
                    },

                    () =>
                    {
                        // Initialize each motor (X, Y, Z) in parallel
                        InitializeMotor(ref kCubeX, serialNoX);
                        InitializeMotor(ref kCubeY, serialNoY);
                        InitializeMotor(ref kCubeZ, serialNoZ);
                    }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing devices: " + ex.Message);
            }

            // Setup DataGridView for displaying measurements
            InitializeDataGridView();

            // Setup ComboBoxes for Tesla Averaging and Field Selection
            InitializeComboBoxes();

            // Setup Progress Bar for measurements
            SetupProgressBar();

            // Setup measurement timers
            SetupTimers();

            // Setup measurement buttons
            setupMeasureButtons();

            // Disable stop measurement button initially
            buttonStopMeasure.Enabled = false;
        }

        private void InitializeMotor(ref KCubeDCServo motor, string serialNo)
        {
            try
            {
                motor = KCubeDCServo.CreateKCubeDCServo(serialNo);
                Console.WriteLine($"Opening motor with serial {serialNo}");
                motor.Connect(serialNo);
                motor.WaitForSettingsInitialized(5000);
                motor.LoadMotorConfiguration(serialNo, DeviceConfiguration.DeviceSettingsUseOptionType.UseFileSettings);
                motor.StartPolling(250);
                motor.EnableDevice();
                Console.WriteLine($"Motor {serialNo} enabled");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing motor {serialNo}: " + ex.Message);
            }
        }

        private void InitializeDataGridView()
        {
            // Add columns to dataGridView1
            dataGridView1.Columns.Add("Num", "Num");
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Xs", "Xset");
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

            // Disable sorting on columns
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Prevent the user from adding rows directly
            dataGridView1.AllowUserToAddRows = false;
        }

        private void InitializeComboBoxes()
        {
            comboBoxTeslaAveraging.Items.Add("1");
            comboBoxTeslaAveraging.Items.Add("10");
            comboBoxTeslaAveraging.Items.Add("20");
            comboBoxTeslaAveraging.Items.Add("100");
            comboBoxTeslaAveraging.Items.Add("500");
            comboBoxTeslaAveraging.Items.Add("1000");

            comboBoxTeslaAveraging.SelectedIndex = 2;  // Default selection
            comboBoxFieldSelect.SelectedIndex = 0;  // Default field selection
        }

        private void SetupProgressBar()
        {
            labelProgress.Text = "";
            progressBarMeasure.Minimum = 0;
            progressBarMeasure.Maximum = 100;
        }

        private void SetupTimers()
        {
            timerReadbacks.Interval = 1000;
            timerReadbacks.Enabled = true;

            timerCubeMeasure.Interval = 10;
            timerCubeMeasure.Enabled = true;
        }



        private void setupMeasureButtons()
        {
            // Validate and calculate step size for X-axis
            if (ValidateAndCalculateStepSize(textBoxMeasMaxX.Text, textBoxMeasMinX.Text, textBoxMeasStepX.Text, out stepSizeX, labelStepSizeX))
            {
                labelStepSizeX.Text = stepSizeX.ToString("0.00");
            }
            else
            {
                labelStepSizeX.Text = "Error";
            }

            // Validate and calculate step size for Y-axis
            if (ValidateAndCalculateStepSize(textBoxMeasMaxY.Text, textBoxMeasMinY.Text, textBoxMeasStepY.Text, out stepSizeY, labelStepSizeY))
            {
                labelStepSizeY.Text = stepSizeY.ToString("0.00");
            }
            else
            {
                labelStepSizeY.Text = "Error";
            }

            // Validate and calculate step size for Z-axis
            if (ValidateAndCalculateStepSize(textBoxMeasMaxZ.Text, textBoxMeasMinZ.Text, textBoxMeasStepZ.Text, out stepSizeZ, labelStepSizeZ))
            {
                labelStepSizeZ.Text = stepSizeZ.ToString("0.00");
            }
            else
            {
                labelStepSizeZ.Text = "Error";
            }
        }

        private bool ValidateAndCalculateStepSize(string maxText, string minText, string stepsText, out double stepSize, System.Windows.Forms.Label stepLabel)
        {
            stepSize = 0;

            // Validate if the inputs are numeric and the number of steps is greater than 1
            if (double.TryParse(maxText, out double max) &&
                double.TryParse(minText, out double min) &&
                int.TryParse(stepsText, out int steps) &&
                steps > 1)
            {
                // Calculate step size
                stepSize = (max - min) / (steps - 1);
                return true;
            }
            else
            {
                // Set error message if input is invalid
                stepLabel.Text = "Invalid Input";
                return false;
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopMeasure = true;

            if (measurementRunning)
            {
                timerShutdown.Enabled = true;
                e.Cancel = true;  // Prevent form from closing immediately.
                return;
            }

            // Disable the timers to stop further measurements or data collection
            timerReadbacks.Enabled = false;
            timerCubeMeasure.Enabled = false;

            // Parallel execution of shutdown tasks with logging and error handling
            try
            {
                Parallel.Invoke(
                    () =>
                    {
                        try
                        {
                            kCubeX.StopPolling();
                            kCubeX.ShutDown();
                            kCubeX.Disconnect(true);
                            Console.WriteLine("kCubeX shutdown successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error shutting down kCubeX: " + ex.Message);
                        }
                    },
                    () =>
                    {
                        try
                        {
                            kCubeY.StopPolling();
                            kCubeY.ShutDown();
                            kCubeY.Disconnect(true);
                            Console.WriteLine("kCubeY shutdown successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error shutting down kCubeY: " + ex.Message);
                        }
                    },
                    () =>
                    {
                        try
                        {
                            kCubeZ.StopPolling();
                            kCubeZ.ShutDown();
                            kCubeZ.Disconnect(true);
                            Console.WriteLine("kCubeZ shutdown successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error shutting down kCubeZ: " + ex.Message);
                        }
                    },
                    () =>
                    {
                        try
                        {
                            TeslameterF71.Closedown();
                            Console.WriteLine("TeslameterF71 shutdown successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error shutting down TeslameterF71: " + ex.Message);
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during shutdown process: " + ex.Message);
            }
        }


        // Asynchronous event handler for the "Home" button click
        private async void buttonkCubeHomeX_Click(object sender, EventArgs e)
        {
            // Run the homeXYZ method on a separate background thread using Task.Run
            // This prevents the UI from freezing while the motors are being homed
            await Task.Run(() => homeXYZ());
        }




        // This method checks if the input string can be parsed into a number and ensures it falls within the specified limits.
        private string checkInput(string inputText, double limitLower, double limitUpper)
        {
            // Initialize the result with the original inputText.
            string result = inputText;

            try
            {
                // Try parsing the inputText into a double.
                double parsedInput = double.Parse(inputText);

                // Check if the parsed input is within the specified limits
                if (parsedInput > limitLower && parsedInput < limitUpper)
                {
                    // If within the limits, keep the original input
                    result = inputText;
                }
                else if (parsedInput < limitLower)
                {
                    // If input is below the lower limit, set to the lower limit
                    result = limitLower.ToString();
                }
                else if (parsedInput > limitUpper)
                {
                    // If input exceeds the upper limit, set to the upper limit
                    result = limitUpper.ToString();
                }
            }
            catch
            {
                // If input cannot be parsed to a double, return an empty string
                result = "";
            }

            // Return the validated or adjusted result
            return result;
        }


        // This method handles the validation and updating of text input values for various measurement parameters.
        private void HandleMeasurementInput(TextBox textBox, double lowerLimit, double upperLimit)
        {
            // Validate and adjust the input value within the specified range
            string result = checkInput(textBox.Text, lowerLimit, upperLimit);

            // If the input is valid, update the TextBox and refresh the measurement buttons
            if (!string.IsNullOrEmpty(result))
            {
                textBox.Text = result;
                setupMeasureButtons(); // Update the measurement buttons when input changes
            }
        }

        // Handles the TextChanged event for textBoxMeasMinX
        private void textBoxMeasMinX_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMinX, 0, 50); // MinX: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasMinY
        private void textBoxMeasMinY_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMinY, 0, 50); // MinY: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasMinZ
        private void textBoxMeasMinZ_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMinZ, 0, 50); // MinZ: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasMaxX
        private void textBoxMeasMaxX_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMaxX, 0, 50); // MaxX: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasMaxY
        private void textBoxMeasMaxY_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMaxY, 0, 50); // MaxY: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasMaxZ
        private void textBoxMeasMaxZ_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasMaxZ, 0, 50); // MaxZ: Range 0 to 50
        }

        // Handles the TextChanged event for textBoxMeasStepX
        private void textBoxMeasStepX_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasStepX, 1, 1000); // StepX: Range 1 to 1000
        }

        // Handles the TextChanged event for textBoxMeasStepY
        private void textBoxMeasStepY_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasStepY, 1, 1000); // StepY: Range 1 to 1000
        }

        // Handles the TextChanged event for textBoxMeasStepZ
        private void textBoxMeasStepZ_TextChanged(object sender, EventArgs e)
        {
            HandleMeasurementInput(textBoxMeasStepZ, 1, 1000); // StepZ: Range 1 to 1000
        }

        // Handles the TextChanged event for txtBoxSetPosX
        private void txtBoxSetPosX_TextChanged(object sender, EventArgs e)
        {
            // Validate and adjust input for position, defaulting to 0 if invalid
            string result = checkInput(txtBoxSetPosX.Text, 0, 50);
            txtBoxSetPosX.Text = !string.IsNullOrEmpty(result) ? result : "0"; // Default to "0" if invalid
        }


        // This method is called when the selected index of comboBoxPlotSelect changes
        private void comboBoxPlotSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set updateChart to true, indicating that chart needs to be updated
            updateChart = true;

            // Mark that new data is available for plotting
            newData = true;
        }

        // This method is called when the state of checkBoxPlotAll changes
        private void checkBoxPlotAll_CheckedChanged(object sender, EventArgs e)
        {
            // Set updateChart to true, indicating that chart needs to be updated
            updateChart = true;

            // Enable or disable comboBoxPlotSelect based on the checkBoxPlotAll state
            comboBoxPlotSelect.Enabled = !checkBoxPlotAll.Checked;

            // Mark that new data is available for plotting
            newData = true;
        }


        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            // Initialize a string variable to hold the CSV data
            csv = string.Empty;

            // Collect probe and measurement settings data to include in the CSV
            csv += $"ProbeModel:{TeslameterF71.UnitProbeModel},";
            csv += $"ProbeSerial:{TeslameterF71.UnitProbeSerial},";
            csv += $"ProbeCalDate:{TeslameterF71.UnitProbeCalDate},";
            csv += $"ProbeOrientation:{TeslameterF71.UnitProbeOrientation},";
            csv += $"ProbeType:{TeslameterF71.UnitProbeType},";
            csv += $"AveragingOn:{checkBoxAveraging.Checked},";
            csv += $"AveragingNum:{textBoxAverageMeasure.Text},";
            csv += $"MeasureTime:{comboBoxTeslaAveraging.Text},";
            csv += $"DwellOn:{checkBoxDwell.Checked},";
            csv += $"DwellTime:{textBoxDwell.Text},";
            csv += $"StopForMeas:{checkBoxStopToMeasure.Checked},";
            csv += $"PointsX:{textBoxMeasStepX.Text},";
            csv += $"PointsY:{textBoxMeasStepY.Text},";
            csv += $"PointsZ:{textBoxMeasStepZ.Text},";
            csv += $"PointsMeasured:{pointsMeasured},\r\n";  // New line after the settings data

            // Add the header row for CSV file (column names from DataGridView)
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                csv += column.HeaderText + ',';
            }
            csv += "\r\n";  // New line after the header

            // Add data rows from DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Add cell data, replace commas in values with semicolons (to avoid CSV formatting issues)
                    if (cell.Value != null)
                    {
                        csv += cell.Value.ToString().Replace(",", ";") + ',';
                    }
                }
                csv += "\r\n";  // New line after each row of data
            }

            // Open the SaveFileDialog for the user to select the file location and name
            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult result = dialog.ShowDialog();
            string selectedPath = string.Empty;

            // If the user selects a file, get the file path
            if (result == DialogResult.OK)
            {
                selectedPath = dialog.FileName;
            }

            // Export the collected CSV data to the selected file
            if (!string.IsNullOrEmpty(selectedPath))
            {
                File.WriteAllText(selectedPath, csv);
            }
        }


        // Event handler for changes in the Tesla Averaging combo box selection
        private void comboBoxTeslaAveraging_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Parse the selected averaging value, divide by 10, and send the command to the Teslameter
            int averagingValue = int.Parse(comboBoxTeslaAveraging.GetItemText(comboBoxTeslaAveraging.SelectedItem)) / 10;
            TeslameterF71.QueueCommand(":SENS:AVER:COUN " + averagingValue.ToString());
        }

        // Event handler for the checkbox that enables/disables measurement settings based on selection
        private void checkBoxStopToMeasure_CheckedChanged(object sender, EventArgs e)
        {
            // Enable or disable input fields and checkboxes depending on whether 'Stop to Measure' is checked
            bool isChecked = checkBoxStopToMeasure.Checked;

            textBoxAverageMeasure.Enabled = isChecked;
            checkBoxAveraging.Enabled = isChecked;
            textBoxDwell.Enabled = isChecked;
            checkBoxDwell.Enabled = isChecked;
        }

        // Event handler for the timer that shuts down the application
        private void timerShutdown_Tick(object sender, EventArgs e)
        {
            // Delay the shutdown slightly (500ms) before closing the form
            Thread.Sleep(500);
            this.Close();
        }


        // Event handler to load data from a CSV file into the DataGridView
        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            // Clear existing rows in the DataGridView
            dataGridView1.Rows.Clear();

            // Open a file dialog to select the CSV file
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // Read the first line (header) from the selected CSV file
            string line1 = File.ReadLines(dlg.FileName).First();
            String[] readbacks = line1.Split(',');

            // Update Tesla Averaging combo box based on the value in the file
            switch (readbacks[7].Split(':')[1])
            {
                case "10": comboBoxTeslaAveraging.SelectedIndex = 0; break;
                case "100": comboBoxTeslaAveraging.SelectedIndex = 1; break;
                case "200": comboBoxTeslaAveraging.SelectedIndex = 2; break;
                case "1000": comboBoxTeslaAveraging.SelectedIndex = 3; break;
                case "5000": comboBoxTeslaAveraging.SelectedIndex = 4; break;
                case "10000": comboBoxTeslaAveraging.SelectedIndex = 5; break;
            }

            // Update various controls based on the values from the CSV file
            checkBoxAveraging.Checked = bool.Parse(readbacks[5].Split(':')[1]);
            textBoxAverageMeasure.Text = readbacks[6].Split(':')[1];
            checkBoxDwell.Checked = bool.Parse(readbacks[8].Split(':')[1]);
            textBoxDwell.Text = readbacks[9].Split(':')[1];
            checkBoxStopToMeasure.Checked = bool.Parse(readbacks[10].Split(':')[1]);
            measurePointsX = int.Parse(readbacks[11].Split(':')[1]);
            measurePointsY = int.Parse(readbacks[12].Split(':')[1]);
            measurePointsZ = int.Parse(readbacks[13].Split(':')[1]);
            pointsMeasured = int.Parse(readbacks[14].Split(':')[1]);

            // Add the data rows to the DataGridView (skip header row)
            foreach (var line in File.ReadLines(dlg.FileName).Skip(2))
            {
                dataGridView1.Rows.Add(line.Split(','));
            }

            // Mark that the chart should be updated with the new data
            updateChart = true;
            newData = true;
        }

        // Event handler to initialize the Teslameter connection
        private void buttonConnectTeslameter_Click(object sender, EventArgs e)
        {
            TeslameterF71.Init(); // Initialize the Teslameter communication
        }

        // Event handler when the selected field in the ComboBox changes
        private void comboBoxFieldSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the plot index and flag indicating the field plot has changed
            plotIndex = comboBoxFieldSelect.SelectedIndex;
            fieldPlotChanged = true;
            updateChart = true;
            newData = true;
        }


        // Event handler to start the time-based measurement process
        private async void buttonStartTimeMeasure_Click(object sender, EventArgs e)
        {
            // Initialize measurement variables
            pointsMeasured = 0;
            panelScott.Visible = true; // Show the progress panel
            bStartTimeMeasure = true; // Flag to indicate measurement has started

            // Prompt the user to select a save file for storing measurement data
            SaveFileDialog dialog = new SaveFileDialog();
            DialogResult result = dialog.ShowDialog();
            saveFilePath = ""; // Default empty file path
            if (result == DialogResult.OK)
            {
                saveFilePath = dialog.FileName; // Set selected file path
            }

            // Prepare CSV header and initial data for file output
            csv = string.Empty;
            tsw = new StreamWriter(saveFilePath, true); // Open the file for writing

            // Add the probe and measurement settings to the CSV file
            csv += "ProbeModel:" + TeslameterF71.UnitProbeModel + ',';
            csv += "ProbeSerial:" + TeslameterF71.UnitProbeSerial + ',';
            csv += "ProbeCalDate:" + TeslameterF71.UnitProbeCalDate + ',';
            csv += "ProbeOrientation:" + TeslameterF71.UnitProbeOrientation + ',';
            csv += "ProbeType:" + TeslameterF71.UnitProbeType + ',';
            csv += "AveragingOn:" + checkBoxAveraging.Checked + ',';
            csv += "AveragingNum:" + textBoxAverageMeasure.Text + ',';
            csv += "MeasureTime:" + comboBoxTeslaAveraging.Text + ',';

            // Write the CSV header to the file
            tsw.WriteLine(csv);
            csv = ""; // Reset csv string after writing

            // Add the DataGridView column headers to the CSV file
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                csv += column.HeaderText + ','; // Append each column header
            }
            tsw.WriteLine(csv); // Write column headers to file
            csv = ""; // Reset csv string after writing header

            // Set flags and disable controls to prevent changes during measurement
            measurementRunning = true;
            SetControlsEnabled(false); // Disable various controls during measurement

            // Disable buttons and other interactive elements
            buttonMeasure.Enabled = false;
            buttonStopMeasure.Enabled = true;
            buttonLoadData.Enabled = false;

            // Start the measurement process asynchronously
            stopMeasure = false;
            dataGridView1.Rows.Clear(); // Clear the DataGridView before starting new data

            // Start the time measurement in a separate task
            await Task.Run(() => StartTimeMeasure());

            // Re-enable controls once the measurement is finished
            measurementRunning = false;
            SetControlsEnabled(true); // Re-enable controls after measurement

            // Reset button states
            buttonMeasure.Enabled = true;
            buttonStopMeasure.Enabled = false;
            buttonLoadData.Enabled = true;

            // Reset position control states
            txtBoxSetPosX.Enabled = true;
            txtBoxSetPosY.Enabled = true;
            txtBoxSetPosZ.Enabled = true;

            // Reset other related controls
            buttonGoTo.Enabled = true;
            buttonkCubeHomeX.Enabled = true;

            // Reset measurement-related controls
            textBoxAverageMeasure.Enabled = true;
            checkBoxAveraging.Enabled = true;
            textBoxDwell.Enabled = true;
            checkBoxDwell.Enabled = true;
            comboBoxTeslaAveraging.Enabled = true;
            checkBoxStopToMeasure.Enabled = true;

            // Reset the flag indicating measurement started
            bStartTimeMeasure = false;
        }

        // Helper method to enable or disable controls based on the measurement state
        private void SetControlsEnabled(bool enabled)
        {
            // Enable or disable controls based on the argument 'enabled'
            textBoxMeasMaxX.Enabled = enabled;
            textBoxMeasMaxY.Enabled = enabled;
            textBoxMeasMaxZ.Enabled = enabled;
            textBoxMeasMinX.Enabled = enabled;
            textBoxMeasMinY.Enabled = enabled;
            textBoxMeasMinZ.Enabled = enabled;
            textBoxMeasStepX.Enabled = enabled;
            textBoxMeasStepY.Enabled = enabled;
            textBoxMeasStepZ.Enabled = enabled;

            textBoxAverageMeasure.Enabled = enabled;
            checkBoxAveraging.Enabled = enabled;
            textBoxDwell.Enabled = enabled;
            checkBoxDwell.Enabled = enabled;
            comboBoxTeslaAveraging.Enabled = enabled;
            checkBoxStopToMeasure.Enabled = enabled;
        }


        // Event handler for changes in the Y position input
        private void txtBoxSetPosY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Validate the input for Y position within the allowed range (0 to 50)
                string result = checkInput(txtBoxSetPosY.Text, 0, 50);

                // If the input is valid, update the text box with the result
                if (!string.IsNullOrEmpty(result))
                {
                    txtBoxSetPosY.Text = result;
                }
                else
                {
                    // If input is invalid, reset to 0
                    txtBoxSetPosY.Text = "0";
                }
            }
            catch (FormatException ex)
            {
                // Handle specific input format errors (e.g., invalid number)
                MessageBox.Show($"Invalid input for Y position: {ex.Message}", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxSetPosY.Text = "0"; // Reset to default value
            }
            catch (Exception ex)
            {
                // Handle any other unforeseen errors
                MessageBox.Show($"An error occurred while processing the Y position: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxSetPosY.Text = "0"; // Reset to default value
            }
        }

        // Event handler for changes in the Z position input
        private void txtBoxSetPosZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Validate the input for Z position within the allowed range (0 to 50)
                string result = checkInput(txtBoxSetPosZ.Text, 0, 50);

                // If the input is valid, update the text box with the result
                if (!string.IsNullOrEmpty(result))
                {
                    txtBoxSetPosZ.Text = result;
                }
                else
                {
                    // If input is invalid, reset to 0
                    txtBoxSetPosZ.Text = "0";
                }
            }
            catch (FormatException ex)
            {
                // Handle specific input format errors (e.g., invalid number)
                MessageBox.Show($"Invalid input for Z position: {ex.Message}", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxSetPosZ.Text = "0"; // Reset to default value
            }
            catch (Exception ex)
            {
                // Handle any other unforeseen errors
                MessageBox.Show($"An error occurred while processing the Z position: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxSetPosZ.Text = "0"; // Reset to default value
            }
        }

        // Event handler to stop the measurement process
        private void buttonStopMeasure_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the stopMeasure flag to true to indicate the measurement should stop
                stopMeasure = true;
                // Optionally, you can add more code here to perform cleanup or additional actions after stopping
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur when stopping the measurement process
                MessageBox.Show($"An error occurred while stopping the measurement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void homeXYZ()
        {
            // Home the stage/actuator for all three axes (X, Y, Z)
            // This will home each axis in parallel for better performance

            #region ParallelTasks

            // Execute the homing process for each axis (X, Y, Z) in parallel
            Parallel.Invoke(
                () =>
                {
                    try
                    {
                        // Homing process for the X axis
                        Console.WriteLine("kCubeX is Homing");
                        kCubeX.Home(90000);  // Home the X axis with a given timeout or value (90000)
                    }
                    catch (Exception ex)
                    {
                        // Catch any exceptions that occur during the homing process for X axis
                        Console.WriteLine($"Error while homing kCubeX: {ex.Message}");
                        // Optionally log the error to a file or display it in a UI message box.
                    }
                },

                () =>
                {
                    try
                    {
                        // Homing process for the Y axis
                        Console.WriteLine("kCubeY is Homing");
                        kCubeY.Home(90000);  // Home the Y axis with a given timeout or value (90000)
                    }
                    catch (Exception ex)
                    {
                        // Catch any exceptions that occur during the homing process for Y axis
                        Console.WriteLine($"Error while homing kCubeY: {ex.Message}");
                        // Optionally log the error to a file or display it in a UI message box.
                    }
                },

                () =>
                {
                    try
                    {
                        // Homing process for the Z axis
                        Console.WriteLine("kCubeZ is Homing");
                        kCubeZ.Home(90000);  // Home the Z axis with a given timeout or value (90000)
                    }
                    catch (Exception ex)
                    {
                        // Catch any exceptions that occur during the homing process for Z axis
                        Console.WriteLine($"Error while homing kCubeZ: {ex.Message}");
                        // Optionally log the error to a file or display it in a UI message box.
                    }
                }
            );  // End of Parallel.Invoke block

            #endregion
        }

        // Event handler for the "Go To" button click event
        private async void buttonGoTo_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the X, Y, and Z position values from the text boxes
                double xPos = double.Parse(txtBoxSetPosX.Text); // Parse X position from the input
                double yPos = double.Parse(txtBoxSetPosY.Text); // Parse Y position from the input
                double zPos = double.Parse(txtBoxSetPosZ.Text); // Parse Z position from the input

                // Call the goToXYZ method asynchronously in a separate task to avoid blocking the UI thread
                await Task.Run(() => GoToXYZ(xPos, yPos, zPos));
            }
            catch (FormatException ex)
            {
                // Handle errors related to invalid number format in the text boxes
                MessageBox.Show("Invalid position values entered. Please ensure all positions are valid numbers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle any other unforeseen errors
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Event handler for the "Measure" button click event
        private async void buttonMeasure_Click(object sender, EventArgs e)
        {
            try
            {
                // Indicate that the measurement process has started
                bStartCubeMeasure = true;
                panelScott.Visible = false; // Hide the "Scott" panel (possibly indicating measurement progress)

                // Prompt user to select a save file path for CSV output
                SaveFileDialog dialog = new SaveFileDialog();
                DialogResult result = dialog.ShowDialog();
                saveFilePath = string.Empty;

                // If the user selects a file, store the file path
                if (result == DialogResult.OK)
                {
                    saveFilePath = dialog.FileName;
                }
                else
                {
                    // If no file is selected, exit the function
                    return;
                }

                // Initialize the CSV string and prepare the StreamWriter to write to the selected file
                csv = string.Empty;
                tsw = new StreamWriter(saveFilePath, true);

                // Add probe and measurement configuration details to the CSV file
                csv += $"ProbeModel:{TeslameterF71.UnitProbeModel},";
                csv += $"ProbeSerial:{TeslameterF71.UnitProbeSerial},";
                csv += $"ProbeCalDate:{TeslameterF71.UnitProbeCalDate},";
                csv += $"ProbeOrientation:{TeslameterF71.UnitProbeOrientation},";
                csv += $"ProbeType:{TeslameterF71.UnitProbeType},";
                csv += $"AveragingOn:{checkBoxAveraging.Checked},";
                csv += $"AveragingNum:{textBoxAverageMeasure.Text},";
                csv += $"MeasureTime:{comboBoxTeslaAveraging.Text},";
                csv += $"DwellOn:{checkBoxDwell.Checked},";
                csv += $"DwellTime:{textBoxDwell.Text},";
                csv += $"StopForMeas:{checkBoxStopToMeasure.Checked},";
                csv += $"PointsX:{textBoxMeasStepX.Text},";
                csv += $"PointsY:{textBoxMeasStepY.Text},";
                csv += $"PointsZ:{textBoxMeasStepZ.Text},";
                csv += $"PointsMeasured:{pointsMeasured},";

                // Write the header information to the CSV
                tsw.WriteLine(csv);
                csv = string.Empty;

                // Add the header row for the CSV file (from DataGridView columns)
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csv += column.HeaderText + ",";
                }

                tsw.WriteLine(csv); // Write header row
                csv = string.Empty;

                // Disable UI elements during the measurement process to prevent user interaction
                ToggleMeasurementControls(false);

                // Clear existing rows in the data grid
                dataGridView1.Rows.Clear();

                // Start the measurement process asynchronously
                await Task.Run(() => StartCubeMeasure());

                // After measurement, re-enable UI controls
                ToggleMeasurementControls(true);

                // Measurement has finished
                bStartCubeMeasure = false;
            }
            catch (Exception ex)
            {
                // Handle any errors during the process
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to enable or disable controls based on the measurement state
        private void ToggleMeasurementControls(bool enable)
        {
            // Enable/disable controls based on the provided flag
            SetControlsEnabled(enable);

            buttonMeasure.Enabled = enable;
            buttonStopMeasure.Enabled = !enable;
            buttonLoadData.Enabled = enable;

            txtBoxSetPosX.Enabled = enable;
            txtBoxSetPosY.Enabled = enable;
            txtBoxSetPosZ.Enabled = enable;

            buttonGoTo.Enabled = enable;
            buttonkCubeHomeX.Enabled = enable;
        }


        private void StartCubeMeasure()
        {
            try
            {
                // Parse the measurement boundaries from textboxes
                double measMinX = Double.Parse(textBoxMeasMinX.Text);
                double measMinY = Double.Parse(textBoxMeasMinY.Text);
                double measMinZ = Double.Parse(textBoxMeasMinZ.Text);

                double measMaxX = Double.Parse(textBoxMeasMaxX.Text);
                double measMaxY = Double.Parse(textBoxMeasMaxY.Text);
                double measMaxZ = Double.Parse(textBoxMeasMaxZ.Text);

                // Calculate the total length in each axis and the number of measurement points
                double LengthX = measMaxX - measMinX;
                measurePointsX = (int)(LengthX / stepSizeX);

                double LengthY = measMaxY - measMinY;
                measurePointsY = (int)(LengthY / stepSizeY);

                double LengthZ = measMaxZ - measMinZ;
                measurePointsZ = (int)(LengthZ / stepSizeZ);

                lastZindex = -1;

                // Initialize measurement data structures
                i_Scheme = new cColorScheme(me_ColorScheme);
                i_Mesg = new cMessgData[measurePointsZ + 1];
                lastPointPlotted = 0;
                i_ShapeData = new cScatterData[measurePointsZ + 1];

                // Initialize scatter data for plotting
                for (int i = 0; i < measurePointsZ + 1; i++)
                {
                    i_ShapeData[i] = new cScatterData(i_Scheme);
                }

                pointsMeasured = 0;
                pointsMeasuredX = 0;
                pointsMeasuredY = 0;

                // Move to the initial measurement position (starting coordinates)
                GoToXYZ(measMinX, measMinY, measMinZ);
                startTime = DateTime.Now;

                // Parse the averaging settings from textboxes
                int averageLoops = int.Parse(textBoxAverageMeasure.Text);
                int dwellTime = int.Parse(textBoxDwell.Text);
                int averageTime = TeslameterF71.UnitAveragingWindow * 10; // Averaging time in milliseconds

                // Measurement loop for all points (Z, Y, X)
                for (int k = 0; k < measurePointsZ + 1; k++)
                {
                    for (int j = 0; j < measurePointsY + 1; j++)
                    {
                        for (int i = 0; i < measurePointsX + 1; i++)
                        {
                            currentZindex = k;

                            // Initialize position variables
                            double tempX = 0, tempY = 0, tempZ = 0;

                            // Determine the X and Y positions based on the current index and scanning pattern
                            if (k % 2 == 0) // Even Z index
                            {
                                tempY = measMinY + j * stepSizeY;
                                tempX = (j % 2 == 0) ? measMinX + i * stepSizeX : measMinX + (measurePointsX - i) * stepSizeX;
                            }
                            else // Odd Z index
                            {
                                tempY = measMinY + (measurePointsY - j) * stepSizeY;
                                tempX = (j % 2 == 0) ? measMinX + (measurePointsX - i) * stepSizeX : measMinX + i * stepSizeX;
                            }

                            // Ensure no NaN values
                            tempX = Double.IsNaN(tempX) ? measMinX : tempX;
                            tempY = Double.IsNaN(tempY) ? measMinY : tempY;

                            // Calculate the Z position
                            tempZ = measMinZ + k * stepSizeZ;
                            tempZ = Double.IsNaN(tempZ) ? measMinZ : tempZ;

                            // Round the positions for precision
                            tempX = Math.Round(tempX, 2);
                            tempY = Math.Round(tempY, 2);
                            tempZ = Math.Round(tempZ, 2);

                            // Move to the new position
                            nextXpos = tempX;
                            nextYpos = tempY;
                            nextZpos = tempZ;
                            nextPos = true;
                            GoToXYZ(tempX, tempY, tempZ);

                            // Wait if necessary (based on dwell and averaging settings)
                            if (checkBoxStopToMeasure.Checked)
                            {
                                if (checkBoxDwell.Checked)
                                {
                                    Thread.Sleep(dwellTime); // Dwell time
                                }
                                Thread.Sleep(averageTime); // Averaging time
                            }

                            // Get the actual position from the kCube actuators
                            double posX = (double)kCubeX.Position;
                            double posY = (double)kCubeY.Position;
                            double posZ = -((double)kCubeZ.Position - 50);

                            // Prepare variables for measurement and statistics
                            Sx = tempX;
                            Sy = tempY;
                            Sz = tempZ;
                            Px = posX;
                            Py = posY;
                            Pz = posZ;

                            // Wait until the Tesla meter readbacks are available
                            while (!TeslameterF71.ReadbacksAvailable)
                            {
                                Thread.Sleep(10);
                            }

                            // Get the magnetic field measurements
                            double UnitFieldMagnitude = TeslameterF71.UnitFieldMagnitude * 1000;
                            double UnitFieldX = TeslameterF71.UnitFieldX * 1000;
                            double UnitFieldY = TeslameterF71.UnitFieldY * 1000;
                            double UnitFieldZ = TeslameterF71.UnitFieldZ * 1000;
                            double UnitFieldTemp = TeslameterF71.UnitFieldTemp;

                            // Prepare arrays for averaging measurements
                            double[] statsBMag = new double[averageLoops];
                            double[] statsBx = new double[averageLoops];
                            double[] statsBy = new double[averageLoops];
                            double[] statsBz = new double[averageLoops];

                            double SDx = 0, SDy = 0, SDz = 0, SDm = 0;

                            // Perform averaging if enabled
                            if (checkBoxAveraging.Checked)
                            {
                                statsBMag[0] = UnitFieldMagnitude;
                                statsBx[0] = UnitFieldX;
                                statsBy[0] = UnitFieldY;
                                statsBz[0] = UnitFieldZ;
                                TeslameterF71.ReadbacksAvailable = false;

                                for (int aveLoop = 1; aveLoop < averageLoops; aveLoop++)
                                {
                                    Thread.Sleep(averageTime); // Wait before reading again
                                    while (!TeslameterF71.ReadbacksAvailable)
                                    {
                                        Thread.Sleep(10); // Wait until readback is available
                                    }

                                    // Store the measured values
                                    statsBMag[aveLoop] = TeslameterF71.UnitFieldMagnitude * 1000;
                                    statsBx[aveLoop] = TeslameterF71.UnitFieldX * 1000;
                                    statsBy[aveLoop] = TeslameterF71.UnitFieldY * 1000;
                                    statsBz[aveLoop] = TeslameterF71.UnitFieldZ * 1000;
                                    TeslameterF71.ReadbacksAvailable = false;
                                }

                                // Average the results and calculate standard deviations
                                UnitFieldMagnitude = statsBMag.Average();
                                UnitFieldX = statsBx.Average();
                                UnitFieldY = statsBy.Average();
                                UnitFieldZ = statsBz.Average();

                                SDx = GetStandardDeviation(statsBMag);
                                SDy = GetStandardDeviation(statsBx);
                                SDz = GetStandardDeviation(statsBy);
                                SDm = GetStandardDeviation(statsBz);
                            }

                            // Final measurement values
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

                            // Calculate remaining time and percentage complete
                            DateTime nowTime = DateTime.Now;
                            measTime = nowTime;
                            TimeSpan timeBetweenMeasurements = nowTime.Subtract(startTime);
                            int pointsRemaining = ((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1)) - (pointsMeasured + 1);

                            if (pointsMeasured > 0)
                            {
                                double timeRemaining = pointsRemaining * (timeBetweenMeasurements.TotalSeconds / pointsMeasured);
                                TimeSpan timeSpanRemaining = TimeSpan.FromSeconds(timeRemaining);
                                timeRemainString = timeSpanRemaining.ToString(@"dd\:HH\:mm\:ss");
                            }

                            pointsMeasured++;
                            percentageComplete = 100 * pointsMeasured / ((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1));

                            // Update the chart if needed
                            pointsMeasuredX = Math.Max(pointsMeasuredX, i + 1);
                            pointsMeasuredY = Math.Max(pointsMeasuredY, j + 1);
                            updateChart = true;
                            newData = true;

                            // Stop the measurement if the flag is set
                            if (stopMeasure)
                            {
                                measurementRunning = false;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the measurement process
                Console.WriteLine($"Error in StartCubeMeasure: {ex.Message}");
            }
            finally
            {
                // Ensure the measurement is properly closed
                tsw.Close();
            }
        }


        private void StartTimeMeasure()
        {
            try
            {
                // Parse measurement coordinates from the UI textboxes
                double setX = Double.Parse(textBoxTimeMeasX.Text);
                double setY = Double.Parse(textBoxTimeMeasY.Text);
                double setZ = Double.Parse(textBoxTimeMeasZ.Text);

                // Go to the initial measurement position
                GoToXYZ(setX, setY, setZ);

                // Store the start time of the measurement process
                startTime = DateTime.Now;

                // Calculate the end time based on user input for hours, minutes, and seconds
                DateTime endTime = startTime
                    .AddHours(double.Parse(textBoxTimeMeasHour.Text))
                    .AddMinutes(double.Parse(textBoxTimeMeasMin.Text))
                    .AddSeconds(double.Parse(textBoxTimeMeasSec.Text));

                // Set up the averaging parameters
                int averageLoops = int.Parse(textBoxAverageMeasure.Text);
                int averageTime = TeslameterF71.UnitAveragingWindow * 10;

                // Measurement loop until the current time reaches or exceeds the end time
                while (DateTime.Now < endTime)
                {
                    // Sleep for the averaging time to maintain consistency in measurements
                    Thread.Sleep(averageTime);

                    // Get current positions from the kCube devices
                    double posX = (double)kCubeX.Position;
                    double posY = (double)kCubeY.Position;
                    double posZ = -((double)kCubeZ.Position - 50);

                    // Set the measured coordinates
                    Sx = setX;
                    Sy = setY;
                    Sz = setZ;
                    Px = posX;
                    Py = posY;
                    Pz = posZ;

                    // Wait until the Tesla meter readbacks are available
                    while (!TeslameterF71.ReadbacksAvailable)
                    {
                        Thread.Sleep(10); // Small delay before retrying
                    }

                    // Read the magnetic field measurements and temperature
                    double UnitFieldMagnitude = TeslameterF71.UnitFieldMagnitude * 1000;
                    double UnitFieldX = TeslameterF71.UnitFieldX * 1000;
                    double UnitFieldY = TeslameterF71.UnitFieldY * 1000;
                    double UnitFieldZ = TeslameterF71.UnitFieldZ * 1000;
                    double UnitFieldTemp = TeslameterF71.UnitFieldTemp;

                    // Early warning for out-of-range magnetic field magnitude
                    if (UnitFieldMagnitude > 200)
                    {
                        Console.WriteLine("Warning: Magnetic field magnitude exceeds the limit!");
                    }

                    // Arrays for storing measurement values for averaging
                    double[] statsBMag = new double[averageLoops];
                    double[] statsBx = new double[averageLoops];
                    double[] statsBy = new double[averageLoops];
                    double[] statsBz = new double[averageLoops];

                    double SDx = 0, SDy = 0, SDz = 0, SDm = 0;

                    // Perform averaging if the corresponding checkbox is checked
                    if (checkBoxAveraging.Checked)
                    {
                        // Store the first read for averaging
                        statsBMag[0] = UnitFieldMagnitude;
                        statsBx[0] = UnitFieldX;
                        statsBy[0] = UnitFieldY;
                        statsBz[0] = UnitFieldZ;

                        TeslameterF71.ReadbacksAvailable = false;

                        // Perform averaging over multiple loops
                        for (int aveLoop = 1; aveLoop < averageLoops; aveLoop++)
                        {
                            Thread.Sleep(averageTime); // Wait before next measurement

                            // Wait until the Tesla meter readbacks are available
                            while (!TeslameterF71.ReadbacksAvailable)
                            {
                                Thread.Sleep(10);
                            }

                            // Store the read values
                            statsBMag[aveLoop] = TeslameterF71.UnitFieldMagnitude * 1000;
                            statsBx[aveLoop] = TeslameterF71.UnitFieldX * 1000;
                            statsBy[aveLoop] = TeslameterF71.UnitFieldY * 1000;
                            statsBz[aveLoop] = TeslameterF71.UnitFieldZ * 1000;
                            TeslameterF71.ReadbacksAvailable = false;
                        }

                        // Calculate the averages of the measurements
                        UnitFieldMagnitude = statsBMag.Average();
                        UnitFieldX = statsBx.Average();
                        UnitFieldY = statsBy.Average();
                        UnitFieldZ = statsBz.Average();

                        // Calculate standard deviations of the measurements
                        SDx = GetStandardDeviation(statsBMag);
                        SDy = GetStandardDeviation(statsBx);
                        SDz = GetStandardDeviation(statsBy);
                        SDm = GetStandardDeviation(statsBz);
                    }

                    // Finalize the measurements
                    TeslameterF71.ReadbacksAvailable = false;

                    // Store the final results
                    Bm = UnitFieldMagnitude;
                    Bx = UnitFieldX;
                    By = UnitFieldY;
                    Bz = UnitFieldZ;

                    // Store the standard deviations
                    BmSD = SDm;
                    BxSD = SDx;
                    BySD = SDy;
                    BzSD = SDz;

                    // Store the temperature
                    Temp = UnitFieldTemp;

                    // Calculate remaining time, total time, and elapsed time
                    DateTime nowTime = DateTime.Now;
                    measTime = nowTime;
                    TimeSpan timeRemaining = endTime - nowTime;
                    TimeSpan timeTotal = endTime - startTime;
                    TimeSpan timeElapsed = nowTime - startTime;

                    // If measurements have been taken, calculate percentage completed
                    if (pointsMeasured > 0)
                    {
                        timeRemainString = timeRemaining.ToString();
                        percentageComplete = Math.Round(timeElapsed.TotalSeconds / timeTotal.TotalSeconds, 3);
                    }

                    // Increment the measurement counter
                    pointsMeasured++;

                    // Update chart or UI with new data
                    updateChart = true;
                    newData = true;

                    // If the stop flag is set, stop the measurement process
                    if (stopMeasure)
                    {
                        measurementRunning = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during measurement
                Console.WriteLine($"Error in StartTimeMeasure: {ex.Message}");
            }
            finally
            {
                // Ensure that any resources are properly closed
                tsw.Close();
            }
        }


        private double GetStandardDeviation(double[] values)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("The array cannot be null or empty.", nameof(values));
            }

            // Calculate the average (mean) of the values
            double average = values.Average();

            // Calculate the sum of the squared differences from the mean
            double sumOfSquaresOfDifferences = values
                .Select(val => (val - average) * (val - average)) // Squared difference from the mean
                .Sum(); // Sum up all the squared differences

            // Compute the standard deviation (sqrt of the average squared differences)
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / values.Length);

            return standardDeviation;
        }

        private void GoToXYZ(double posX, double posY, double posZ)
        {
            #region ParallelTasks

            // Adjust Z-coordinate as per the conversion logic
            posZ = 50 - posZ;

            // Perform three tasks in parallel to move the kCube along X, Y, and Z axes
            Parallel.Invoke(
                () =>
                {
                    try
                    {
                        // Move kCube along X axis
                        kCubeX.MoveTo((decimal)posX, 60000); // Move to specified position with a timeout of 60 seconds
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exceptions as needed
                        Console.WriteLine($"Error moving X axis: {ex.Message}");
                    }
                },  // End of first Action

                () =>
                {
                    try
                    {
                        // Move kCube along Y axis
                        kCubeY.MoveTo((decimal)posY, 60000); // Move to specified position with a timeout of 60 seconds
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exceptions as needed
                        Console.WriteLine($"Error moving Y axis: {ex.Message}");
                    }
                }, // End of second Action

                () =>
                {
                    try
                    {
                        // Move kCube along Z axis
                        kCubeZ.MoveTo((decimal)posZ, 60000); // Move to adjusted Z position with a timeout of 60 seconds
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exceptions as needed
                        Console.WriteLine($"Error moving Z axis: {ex.Message}");
                    }
                } // End of third Action
            ); // End of Parallel.Invoke

            #endregion
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            try
            {
                // Update Tesla Meter readings (in mT) and units
                labelFieldX.Text = $"x: {(TeslameterF71.UnitFieldX * 1000):00.000} m{TeslameterF71.UnitUnits}";
                labelFieldY.Text = $"y: {(TeslameterF71.UnitFieldY * 1000):00.000} m{TeslameterF71.UnitUnits}";
                labelFieldZ.Text = $"z: {(TeslameterF71.UnitFieldZ * 1000):00.000} m{TeslameterF71.UnitUnits}";
                labelFieldMag.Text = $"{(TeslameterF71.UnitFieldMagnitude * 1000):00.000} m{TeslameterF71.UnitUnits}";
                labelFieldTemp.Text = $"{TeslameterF71.UnitFieldTemp:00.00} C";
                labelAveWindow.Text = $"Averaging: {Math.Round((double)TeslameterF71.UnitAveragingWindow / 100, 4)} s";

                // Update kCube positions
                labelXpos.Text = kCubeX.DevicePosition.ToString("0.00");
                labelYpos.Text = kCubeY.Position.ToString("0.00");
                labelZpos.Text = (50 - kCubeZ.Position).ToString("0.00");

                // If new data is available, update the UI and CSV file
                if (updateChart && newData)
                {
                    string dateString = measTime.ToString(@"yyyy\-MM\-dd");
                    string timeString = measTime.ToString(@"HH\:mm\:ss\.fff");

                    if (pointsMeasured > dataGridView1.Rows.Count)
                    {
                        // Prepare data to be written to CSV and DataGridView
                        csv += $"{pointsMeasured},{dateString},{timeString},{Sx},{Sy},{Sz},{Px},{Py},{Pz},{Bm},{BmSD},{Bx},{BxSD},{By},{BySD},{Bz},{BzSD},{Temp},{ArrayIndexX},{ArrayIndexY},{ArrayIndexZ}";
                        tsw.WriteLine(csv);  // Write to CSV
                        csv = "";  // Reset csv string
                        dataGridView1.Rows.Add(new object[]
                        {
                    pointsMeasured, dateString, timeString, Sx, Sy, Sz, Px, Py, Pz, Bm, BmSD, Bx, BxSD, By, BySD, Bz, BzSD, Temp, ArrayIndexX, ArrayIndexY, ArrayIndexZ
                        });

                        // Update progress bar
                        progressBarMeasure.Value = (int)percentageComplete;

                        // Update progress label
                        if (bStartCubeMeasure)
                        {
                            labelProgress.Text = $"Point {pointsMeasured} of {((measurePointsX + 1) * (measurePointsY + 1) * (measurePointsZ + 1))} {Math.Round(percentageComplete, 1)}% {timeRemainString}";
                        }
                        else
                        {
                            labelProgress.Text = $"Point {pointsMeasured} {Math.Round(percentageComplete, 1)}% {timeRemainString}";
                        }
                    }

                    newData = false;  // Reset new data flag

                    // Reinitialize plot shapes if measurement has started
                    if (bStartCubeMeasure)
                    {
                        DrawDemo();
                    }

                    // Plot data for time-based measurements if started
                    if (bStartTimeMeasure)
                    {
                        DateTime[] xS = new DateTime[dataGridView1.Rows.Count];
                        double[] xY = new double[dataGridView1.Rows.Count];

                        int i = 0;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            xS[i] = DateTime.Parse($"{row.Cells[1].Value} {row.Cells[2].Value}");
                            xY[i] = double.Parse(row.Cells[9].Value.ToString());
                            i++;
                        }

                        // Update the plot with the new data
                        FormsPlot1.Plot.Clear();
                        FormsPlot1.Plot.Add.SignalXY(xS, xY);
                        FormsPlot1.Plot.Axes.DateTimeTicksBottom();
                        FormsPlot1.Refresh();
                    }
                }

                // Handling cube measurement plotting
                if (bStartCubeMeasure)
                {
                    // Update the combo box when Z index changes
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

                    // Update position if needed
                    if (nextPos)
                    {
                        txtBoxSetPosX.Text = Math.Round(nextXpos, 2).ToString();
                        txtBoxSetPosY.Text = Math.Round(nextYpos, 2).ToString();
                        txtBoxSetPosZ.Text = Math.Round(nextZpos, 2).ToString();
                        nextPos = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors gracefully
                Console.WriteLine($"Error in TimerUI_Tick: {ex.Message}");
            }
        }









    }
}
