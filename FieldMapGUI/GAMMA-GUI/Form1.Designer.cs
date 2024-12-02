namespace FieldMapGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timerReadbacks = new System.Windows.Forms.Timer(this.components);
            this.timerCubeMeasure = new System.Windows.Forms.Timer(this.components);
            this.panelField = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonConnectTeslameter = new System.Windows.Forms.Button();
            this.comboBoxTeslaAveraging = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.labelAveWindow = new System.Windows.Forms.Label();
            this.labelFieldMag = new System.Windows.Forms.Label();
            this.labelFieldTemp = new System.Windows.Forms.Label();
            this.labelFieldZ = new System.Windows.Forms.Label();
            this.labelFieldY = new System.Windows.Forms.Label();
            this.labelFieldX = new System.Windows.Forms.Label();
            this.buttonkCubeHomeX = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.labelZpos = new System.Windows.Forms.Label();
            this.labelYpos = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBoxSetPosZ = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBoxSetPosY = new System.Windows.Forms.TextBox();
            this.txtBoxSetPosX = new System.Windows.Forms.TextBox();
            this.buttonGoTo = new System.Windows.Forms.Button();
            this.labelXpos = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasSec = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasMin = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasHour = new System.Windows.Forms.TextBox();
            this.buttonStartTimeMeasure = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasX = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasY = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxTimeMeasZ = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBoxStopToMeasure = new System.Windows.Forms.CheckBox();
            this.checkBoxDwell = new System.Windows.Forms.CheckBox();
            this.checkBoxAveraging = new System.Windows.Forms.CheckBox();
            this.textBoxAverageMeasure = new System.Windows.Forms.TextBox();
            this.buttonMeasure = new System.Windows.Forms.Button();
            this.textBoxDwell = new System.Windows.Forms.TextBox();
            this.labelStepSizeZ = new System.Windows.Forms.Label();
            this.labelStepSizeY = new System.Windows.Forms.Label();
            this.labelStepSizeX = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMeasMaxZ = new System.Windows.Forms.TextBox();
            this.textBoxMeasMaxY = new System.Windows.Forms.TextBox();
            this.textBoxMeasMaxX = new System.Windows.Forms.TextBox();
            this.textBoxMeasStepZ = new System.Windows.Forms.TextBox();
            this.textBoxMeasStepY = new System.Windows.Forms.TextBox();
            this.textBoxMeasStepX = new System.Windows.Forms.TextBox();
            this.textBoxMeasMinZ = new System.Windows.Forms.TextBox();
            this.textBoxMeasMinY = new System.Windows.Forms.TextBox();
            this.textBoxMeasMinX = new System.Windows.Forms.TextBox();
            this.labelProgress = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarMeasure = new System.Windows.Forms.ProgressBar();
            this.buttonStopMeasure = new System.Windows.Forms.Button();
            this.buttonSaveData = new System.Windows.Forms.Button();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBoxPlotSelect = new System.Windows.Forms.ComboBox();
            this.checkBoxPlotAll = new System.Windows.Forms.CheckBox();
            this.timerShutdown = new System.Windows.Forms.Timer(this.components);
            this.comboBoxFieldSelect = new System.Windows.Forms.ComboBox();
            this.textBoxZdist = new System.Windows.Forms.TextBox();
            this.labelZdist = new System.Windows.Forms.Label();
            this.timerTimeMeasure = new System.Windows.Forms.Timer(this.components);
            this.panelScott = new System.Windows.Forms.Panel();
            this.editor3D = new Plot3D.Editor3D();
            this.panelField.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerCubeMeasure
            // 
            this.timerCubeMeasure.Interval = 20;
            this.timerCubeMeasure.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // panelField
            // 
            this.panelField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelField.Controls.Add(this.label23);
            this.panelField.Controls.Add(this.buttonConnectTeslameter);
            this.panelField.Controls.Add(this.comboBoxTeslaAveraging);
            this.panelField.Controls.Add(this.label12);
            this.panelField.Controls.Add(this.labelAveWindow);
            this.panelField.Controls.Add(this.labelFieldMag);
            this.panelField.Controls.Add(this.labelFieldTemp);
            this.panelField.Controls.Add(this.labelFieldZ);
            this.panelField.Controls.Add(this.labelFieldY);
            this.panelField.Controls.Add(this.labelFieldX);
            this.panelField.Location = new System.Drawing.Point(12, 12);
            this.panelField.Name = "panelField";
            this.panelField.Size = new System.Drawing.Size(292, 305);
            this.panelField.TabIndex = 24;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(232, 152);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(21, 14);
            this.label23.TabIndex = 88;
            this.label23.Text = "ms";
            // 
            // buttonConnectTeslameter
            // 
            this.buttonConnectTeslameter.Location = new System.Drawing.Point(209, 11);
            this.buttonConnectTeslameter.Name = "buttonConnectTeslameter";
            this.buttonConnectTeslameter.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectTeslameter.TabIndex = 87;
            this.buttonConnectTeslameter.Text = "Connect";
            this.buttonConnectTeslameter.UseVisualStyleBackColor = true;
            this.buttonConnectTeslameter.Click += new System.EventHandler(this.buttonConnectTeslameter_Click);
            // 
            // comboBoxTeslaAveraging
            // 
            this.comboBoxTeslaAveraging.FormattingEnabled = true;
            this.comboBoxTeslaAveraging.Location = new System.Drawing.Point(146, 149);
            this.comboBoxTeslaAveraging.Name = "comboBoxTeslaAveraging";
            this.comboBoxTeslaAveraging.Size = new System.Drawing.Size(80, 22);
            this.comboBoxTeslaAveraging.TabIndex = 2;
            this.comboBoxTeslaAveraging.SelectedIndexChanged += new System.EventHandler(this.comboBoxTeslaAveraging_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(79, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 24);
            this.label12.TabIndex = 87;
            this.label12.Text = "Field Probe";
            // 
            // labelAveWindow
            // 
            this.labelAveWindow.AutoSize = true;
            this.labelAveWindow.Location = new System.Drawing.Point(27, 153);
            this.labelAveWindow.Name = "labelAveWindow";
            this.labelAveWindow.Size = new System.Drawing.Size(89, 14);
            this.labelAveWindow.TabIndex = 67;
            this.labelAveWindow.Text = "labelAveWindow";
            // 
            // labelFieldMag
            // 
            this.labelFieldMag.AutoSize = true;
            this.labelFieldMag.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFieldMag.Location = new System.Drawing.Point(26, 46);
            this.labelFieldMag.Name = "labelFieldMag";
            this.labelFieldMag.Size = new System.Drawing.Size(131, 21);
            this.labelFieldMag.TabIndex = 66;
            this.labelFieldMag.Text = "labelFieldMag";
            // 
            // labelFieldTemp
            // 
            this.labelFieldTemp.AutoSize = true;
            this.labelFieldTemp.Location = new System.Drawing.Point(27, 135);
            this.labelFieldTemp.Name = "labelFieldTemp";
            this.labelFieldTemp.Size = new System.Drawing.Size(76, 14);
            this.labelFieldTemp.TabIndex = 65;
            this.labelFieldTemp.Text = "labelFieldTemp";
            // 
            // labelFieldZ
            // 
            this.labelFieldZ.AutoSize = true;
            this.labelFieldZ.Location = new System.Drawing.Point(27, 117);
            this.labelFieldZ.Name = "labelFieldZ";
            this.labelFieldZ.Size = new System.Drawing.Size(58, 14);
            this.labelFieldZ.TabIndex = 64;
            this.labelFieldZ.Text = "labelFieldZ";
            // 
            // labelFieldY
            // 
            this.labelFieldY.AutoSize = true;
            this.labelFieldY.Location = new System.Drawing.Point(27, 99);
            this.labelFieldY.Name = "labelFieldY";
            this.labelFieldY.Size = new System.Drawing.Size(59, 14);
            this.labelFieldY.TabIndex = 63;
            this.labelFieldY.Text = "labelFieldY";
            // 
            // labelFieldX
            // 
            this.labelFieldX.AutoSize = true;
            this.labelFieldX.Location = new System.Drawing.Point(27, 80);
            this.labelFieldX.Name = "labelFieldX";
            this.labelFieldX.Size = new System.Drawing.Size(58, 14);
            this.labelFieldX.TabIndex = 62;
            this.labelFieldX.Text = "labelFieldX";
            // 
            // buttonkCubeHomeX
            // 
            this.buttonkCubeHomeX.Location = new System.Drawing.Point(204, 82);
            this.buttonkCubeHomeX.Name = "buttonkCubeHomeX";
            this.buttonkCubeHomeX.Size = new System.Drawing.Size(75, 23);
            this.buttonkCubeHomeX.TabIndex = 25;
            this.buttonkCubeHomeX.Text = "Home";
            this.buttonkCubeHomeX.UseVisualStyleBackColor = true;
            this.buttonkCubeHomeX.Click += new System.EventHandler(this.buttonkCubeHomeX_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.labelZpos);
            this.panel1.Controls.Add(this.labelYpos);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtBoxSetPosZ);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtBoxSetPosY);
            this.panel1.Controls.Add(this.txtBoxSetPosX);
            this.panel1.Controls.Add(this.buttonGoTo);
            this.panel1.Controls.Add(this.buttonkCubeHomeX);
            this.panel1.Controls.Add(this.labelXpos);
            this.panel1.Location = new System.Drawing.Point(12, 323);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 185);
            this.panel1.TabIndex = 68;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(69, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(148, 24);
            this.label11.TabIndex = 68;
            this.label11.Text = "Motor Control";
            // 
            // labelZpos
            // 
            this.labelZpos.AutoSize = true;
            this.labelZpos.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZpos.Location = new System.Drawing.Point(157, 85);
            this.labelZpos.Name = "labelZpos";
            this.labelZpos.Size = new System.Drawing.Size(16, 18);
            this.labelZpos.TabIndex = 86;
            this.labelZpos.Text = "0";
            this.labelZpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelYpos
            // 
            this.labelYpos.AutoSize = true;
            this.labelYpos.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYpos.Location = new System.Drawing.Point(99, 85);
            this.labelYpos.Name = "labelYpos";
            this.labelYpos.Size = new System.Drawing.Size(16, 18);
            this.labelYpos.TabIndex = 85;
            this.labelYpos.Text = "0";
            this.labelYpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(157, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 18);
            this.label8.TabIndex = 84;
            this.label8.Text = "Z";
            // 
            // txtBoxSetPosZ
            // 
            this.txtBoxSetPosZ.Location = new System.Drawing.Point(135, 108);
            this.txtBoxSetPosZ.Name = "txtBoxSetPosZ";
            this.txtBoxSetPosZ.Size = new System.Drawing.Size(55, 20);
            this.txtBoxSetPosZ.TabIndex = 70;
            this.txtBoxSetPosZ.Text = "0";
            this.txtBoxSetPosZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxSetPosZ.TextChanged += new System.EventHandler(this.txtBoxSetPosZ_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 18);
            this.label9.TabIndex = 83;
            this.label9.Text = "Y";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(36, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 18);
            this.label10.TabIndex = 82;
            this.label10.Text = "X";
            // 
            // txtBoxSetPosY
            // 
            this.txtBoxSetPosY.Location = new System.Drawing.Point(74, 108);
            this.txtBoxSetPosY.Name = "txtBoxSetPosY";
            this.txtBoxSetPosY.Size = new System.Drawing.Size(55, 20);
            this.txtBoxSetPosY.TabIndex = 69;
            this.txtBoxSetPosY.Text = "0";
            this.txtBoxSetPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxSetPosY.TextChanged += new System.EventHandler(this.txtBoxSetPosY_TextChanged);
            // 
            // txtBoxSetPosX
            // 
            this.txtBoxSetPosX.Location = new System.Drawing.Point(14, 108);
            this.txtBoxSetPosX.Name = "txtBoxSetPosX";
            this.txtBoxSetPosX.Size = new System.Drawing.Size(55, 20);
            this.txtBoxSetPosX.TabIndex = 68;
            this.txtBoxSetPosX.Text = "0";
            this.txtBoxSetPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxSetPosX.TextChanged += new System.EventHandler(this.txtBoxSetPosX_TextChanged);
            // 
            // buttonGoTo
            // 
            this.buttonGoTo.Location = new System.Drawing.Point(204, 108);
            this.buttonGoTo.Name = "buttonGoTo";
            this.buttonGoTo.Size = new System.Drawing.Size(75, 23);
            this.buttonGoTo.TabIndex = 67;
            this.buttonGoTo.Text = "Move";
            this.buttonGoTo.UseVisualStyleBackColor = true;
            this.buttonGoTo.Click += new System.EventHandler(this.buttonGoTo_Click);
            // 
            // labelXpos
            // 
            this.labelXpos.AutoSize = true;
            this.labelXpos.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXpos.Location = new System.Drawing.Point(36, 85);
            this.labelXpos.Name = "labelXpos";
            this.labelXpos.Size = new System.Drawing.Size(16, 18);
            this.labelXpos.TabIndex = 66;
            this.labelXpos.Text = "0";
            this.labelXpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 854);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1552, 22);
            this.statusStrip1.TabIndex = 70;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(112, 17);
            this.statusLabel.Text = "toolStripStatusLabel";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.labelProgress);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.progressBarMeasure);
            this.panel2.Controls.Add(this.buttonStopMeasure);
            this.panel2.Location = new System.Drawing.Point(1261, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 525);
            this.panel2.TabIndex = 69;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label22);
            this.panel4.Controls.Add(this.textBoxTimeMeasSec);
            this.panel4.Controls.Add(this.label21);
            this.panel4.Controls.Add(this.textBoxTimeMeasMin);
            this.panel4.Controls.Add(this.label20);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Controls.Add(this.textBoxTimeMeasHour);
            this.panel4.Controls.Add(this.buttonStartTimeMeasure);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.textBoxTimeMeasX);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.textBoxTimeMeasY);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Controls.Add(this.textBoxTimeMeasZ);
            this.panel4.Location = new System.Drawing.Point(3, 38);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(272, 159);
            this.panel4.TabIndex = 121;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(243, 94);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(16, 17);
            this.label22.TabIndex = 133;
            this.label22.Text = "s";
            // 
            // textBoxTimeMeasSec
            // 
            this.textBoxTimeMeasSec.Location = new System.Drawing.Point(200, 93);
            this.textBoxTimeMeasSec.Name = "textBoxTimeMeasSec";
            this.textBoxTimeMeasSec.Size = new System.Drawing.Size(40, 20);
            this.textBoxTimeMeasSec.TabIndex = 132;
            this.textBoxTimeMeasSec.Text = "0";
            this.textBoxTimeMeasSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(172, 94);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(21, 17);
            this.label21.TabIndex = 131;
            this.label21.Text = "m";
            // 
            // textBoxTimeMeasMin
            // 
            this.textBoxTimeMeasMin.Location = new System.Drawing.Point(129, 93);
            this.textBoxTimeMeasMin.Name = "textBoxTimeMeasMin";
            this.textBoxTimeMeasMin.Size = new System.Drawing.Size(40, 20);
            this.textBoxTimeMeasMin.TabIndex = 130;
            this.textBoxTimeMeasMin.Text = "1";
            this.textBoxTimeMeasMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(103, 94);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(16, 17);
            this.label20.TabIndex = 129;
            this.label20.Text = "h";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 96);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 14);
            this.label19.TabIndex = 123;
            this.label19.Text = "Duration";
            // 
            // textBoxTimeMeasHour
            // 
            this.textBoxTimeMeasHour.Location = new System.Drawing.Point(60, 93);
            this.textBoxTimeMeasHour.Name = "textBoxTimeMeasHour";
            this.textBoxTimeMeasHour.Size = new System.Drawing.Size(40, 20);
            this.textBoxTimeMeasHour.TabIndex = 122;
            this.textBoxTimeMeasHour.Text = "0";
            this.textBoxTimeMeasHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonStartTimeMeasure
            // 
            this.buttonStartTimeMeasure.Location = new System.Drawing.Point(186, 131);
            this.buttonStartTimeMeasure.Name = "buttonStartTimeMeasure";
            this.buttonStartTimeMeasure.Size = new System.Drawing.Size(75, 23);
            this.buttonStartTimeMeasure.TabIndex = 122;
            this.buttonStartTimeMeasure.Text = "Start Measurements";
            this.buttonStartTimeMeasure.UseVisualStyleBackColor = true;
            this.buttonStartTimeMeasure.Click += new System.EventHandler(this.buttonStartTimeMeasure_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 14);
            this.label15.TabIndex = 123;
            this.label15.Text = "Set";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(87, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(111, 18);
            this.label14.TabIndex = 122;
            this.label14.Text = "Point Measure";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(220, 33);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 18);
            this.label16.TabIndex = 128;
            this.label16.Text = "Z";
            // 
            // textBoxTimeMeasX
            // 
            this.textBoxTimeMeasX.Location = new System.Drawing.Point(58, 60);
            this.textBoxTimeMeasX.Name = "textBoxTimeMeasX";
            this.textBoxTimeMeasX.Size = new System.Drawing.Size(65, 20);
            this.textBoxTimeMeasX.TabIndex = 122;
            this.textBoxTimeMeasX.Text = "0";
            this.textBoxTimeMeasX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(150, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(18, 18);
            this.label17.TabIndex = 127;
            this.label17.Text = "Y";
            // 
            // textBoxTimeMeasY
            // 
            this.textBoxTimeMeasY.Location = new System.Drawing.Point(129, 60);
            this.textBoxTimeMeasY.Name = "textBoxTimeMeasY";
            this.textBoxTimeMeasY.Size = new System.Drawing.Size(65, 20);
            this.textBoxTimeMeasY.TabIndex = 124;
            this.textBoxTimeMeasY.Text = "0";
            this.textBoxTimeMeasY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(79, 33);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 18);
            this.label18.TabIndex = 126;
            this.label18.Text = "X";
            // 
            // textBoxTimeMeasZ
            // 
            this.textBoxTimeMeasZ.Location = new System.Drawing.Point(200, 60);
            this.textBoxTimeMeasZ.Name = "textBoxTimeMeasZ";
            this.textBoxTimeMeasZ.Size = new System.Drawing.Size(65, 20);
            this.textBoxTimeMeasZ.TabIndex = 125;
            this.textBoxTimeMeasZ.Text = "0";
            this.textBoxTimeMeasZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.checkBoxStopToMeasure);
            this.panel3.Controls.Add(this.checkBoxDwell);
            this.panel3.Controls.Add(this.checkBoxAveraging);
            this.panel3.Controls.Add(this.textBoxAverageMeasure);
            this.panel3.Controls.Add(this.buttonMeasure);
            this.panel3.Controls.Add(this.textBoxDwell);
            this.panel3.Controls.Add(this.labelStepSizeZ);
            this.panel3.Controls.Add(this.labelStepSizeY);
            this.panel3.Controls.Add(this.labelStepSizeX);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.textBoxMeasMaxZ);
            this.panel3.Controls.Add(this.textBoxMeasMaxY);
            this.panel3.Controls.Add(this.textBoxMeasMaxX);
            this.panel3.Controls.Add(this.textBoxMeasStepZ);
            this.panel3.Controls.Add(this.textBoxMeasStepY);
            this.panel3.Controls.Add(this.textBoxMeasStepX);
            this.panel3.Controls.Add(this.textBoxMeasMinZ);
            this.panel3.Controls.Add(this.textBoxMeasMinY);
            this.panel3.Controls.Add(this.textBoxMeasMinX);
            this.panel3.Location = new System.Drawing.Point(3, 203);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 255);
            this.panel3.TabIndex = 94;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(87, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(112, 18);
            this.label13.TabIndex = 121;
            this.label13.Text = "Cube Measure";
            // 
            // checkBoxStopToMeasure
            // 
            this.checkBoxStopToMeasure.AutoSize = true;
            this.checkBoxStopToMeasure.Checked = true;
            this.checkBoxStopToMeasure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStopToMeasure.Location = new System.Drawing.Point(21, 228);
            this.checkBoxStopToMeasure.Name = "checkBoxStopToMeasure";
            this.checkBoxStopToMeasure.Size = new System.Drawing.Size(135, 18);
            this.checkBoxStopToMeasure.TabIndex = 119;
            this.checkBoxStopToMeasure.Text = "Stop For Measurement";
            this.checkBoxStopToMeasure.UseVisualStyleBackColor = true;
            // 
            // checkBoxDwell
            // 
            this.checkBoxDwell.AutoSize = true;
            this.checkBoxDwell.Checked = true;
            this.checkBoxDwell.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDwell.Location = new System.Drawing.Point(21, 181);
            this.checkBoxDwell.Name = "checkBoxDwell";
            this.checkBoxDwell.Size = new System.Drawing.Size(78, 18);
            this.checkBoxDwell.TabIndex = 118;
            this.checkBoxDwell.Text = "Dwell (ms)";
            this.checkBoxDwell.UseVisualStyleBackColor = true;
            // 
            // checkBoxAveraging
            // 
            this.checkBoxAveraging.AutoSize = true;
            this.checkBoxAveraging.Checked = true;
            this.checkBoxAveraging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAveraging.Location = new System.Drawing.Point(21, 204);
            this.checkBoxAveraging.Name = "checkBoxAveraging";
            this.checkBoxAveraging.Size = new System.Drawing.Size(76, 18);
            this.checkBoxAveraging.TabIndex = 94;
            this.checkBoxAveraging.Text = "Averaging";
            this.checkBoxAveraging.UseVisualStyleBackColor = true;
            // 
            // textBoxAverageMeasure
            // 
            this.textBoxAverageMeasure.Location = new System.Drawing.Point(111, 202);
            this.textBoxAverageMeasure.Name = "textBoxAverageMeasure";
            this.textBoxAverageMeasure.Size = new System.Drawing.Size(65, 20);
            this.textBoxAverageMeasure.TabIndex = 117;
            this.textBoxAverageMeasure.Text = "5";
            this.textBoxAverageMeasure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMeasure
            // 
            this.buttonMeasure.Location = new System.Drawing.Point(190, 223);
            this.buttonMeasure.Name = "buttonMeasure";
            this.buttonMeasure.Size = new System.Drawing.Size(75, 23);
            this.buttonMeasure.TabIndex = 95;
            this.buttonMeasure.Text = "Start Measurements";
            this.buttonMeasure.UseVisualStyleBackColor = true;
            this.buttonMeasure.Click += new System.EventHandler(this.buttonMeasure_Click);
            // 
            // textBoxDwell
            // 
            this.textBoxDwell.Location = new System.Drawing.Point(112, 179);
            this.textBoxDwell.Name = "textBoxDwell";
            this.textBoxDwell.Size = new System.Drawing.Size(65, 20);
            this.textBoxDwell.TabIndex = 116;
            this.textBoxDwell.Text = "50";
            this.textBoxDwell.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelStepSizeZ
            // 
            this.labelStepSizeZ.AutoSize = true;
            this.labelStepSizeZ.Location = new System.Drawing.Point(211, 146);
            this.labelStepSizeZ.Name = "labelStepSizeZ";
            this.labelStepSizeZ.Size = new System.Drawing.Size(40, 14);
            this.labelStepSizeZ.TabIndex = 114;
            this.labelStepSizeZ.Text = "label11";
            this.labelStepSizeZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStepSizeY
            // 
            this.labelStepSizeY.AutoSize = true;
            this.labelStepSizeY.Location = new System.Drawing.Point(141, 146);
            this.labelStepSizeY.Name = "labelStepSizeY";
            this.labelStepSizeY.Size = new System.Drawing.Size(40, 14);
            this.labelStepSizeY.TabIndex = 113;
            this.labelStepSizeY.Text = "label11";
            this.labelStepSizeY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStepSizeX
            // 
            this.labelStepSizeX.AutoSize = true;
            this.labelStepSizeX.Location = new System.Drawing.Point(71, 146);
            this.labelStepSizeX.Name = "labelStepSizeX";
            this.labelStepSizeX.Size = new System.Drawing.Size(40, 14);
            this.labelStepSizeX.TabIndex = 97;
            this.labelStepSizeX.Text = "label11";
            this.labelStepSizeX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 14);
            this.label7.TabIndex = 111;
            this.label7.Text = "Max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 14);
            this.label6.TabIndex = 110;
            this.label6.Text = "Steps";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 14);
            this.label5.TabIndex = 98;
            this.label5.Text = "Min";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(220, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 18);
            this.label4.TabIndex = 109;
            this.label4.Text = "Z";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(150, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 18);
            this.label3.TabIndex = 108;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(79, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 18);
            this.label2.TabIndex = 107;
            this.label2.Text = "X";
            // 
            // textBoxMeasMaxZ
            // 
            this.textBoxMeasMaxZ.Location = new System.Drawing.Point(200, 96);
            this.textBoxMeasMaxZ.Name = "textBoxMeasMaxZ";
            this.textBoxMeasMaxZ.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMaxZ.TabIndex = 106;
            this.textBoxMeasMaxZ.Text = "50";
            this.textBoxMeasMaxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMaxZ.TextChanged += new System.EventHandler(this.textBoxMeasMaxZ_TextChanged);
            // 
            // textBoxMeasMaxY
            // 
            this.textBoxMeasMaxY.Location = new System.Drawing.Point(129, 96);
            this.textBoxMeasMaxY.Name = "textBoxMeasMaxY";
            this.textBoxMeasMaxY.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMaxY.TabIndex = 105;
            this.textBoxMeasMaxY.Text = "50";
            this.textBoxMeasMaxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMaxY.TextChanged += new System.EventHandler(this.textBoxMeasMaxY_TextChanged);
            // 
            // textBoxMeasMaxX
            // 
            this.textBoxMeasMaxX.Location = new System.Drawing.Point(58, 96);
            this.textBoxMeasMaxX.Name = "textBoxMeasMaxX";
            this.textBoxMeasMaxX.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMaxX.TabIndex = 104;
            this.textBoxMeasMaxX.Text = "50";
            this.textBoxMeasMaxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMaxX.TextChanged += new System.EventHandler(this.textBoxMeasMaxX_TextChanged);
            // 
            // textBoxMeasStepZ
            // 
            this.textBoxMeasStepZ.Location = new System.Drawing.Point(200, 122);
            this.textBoxMeasStepZ.Name = "textBoxMeasStepZ";
            this.textBoxMeasStepZ.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasStepZ.TabIndex = 103;
            this.textBoxMeasStepZ.Text = "51";
            this.textBoxMeasStepZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasStepZ.TextChanged += new System.EventHandler(this.textBoxMeasStepZ_TextChanged);
            // 
            // textBoxMeasStepY
            // 
            this.textBoxMeasStepY.Location = new System.Drawing.Point(129, 122);
            this.textBoxMeasStepY.Name = "textBoxMeasStepY";
            this.textBoxMeasStepY.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasStepY.TabIndex = 102;
            this.textBoxMeasStepY.Text = "51";
            this.textBoxMeasStepY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasStepY.TextChanged += new System.EventHandler(this.textBoxMeasStepY_TextChanged);
            // 
            // textBoxMeasStepX
            // 
            this.textBoxMeasStepX.Location = new System.Drawing.Point(58, 122);
            this.textBoxMeasStepX.Name = "textBoxMeasStepX";
            this.textBoxMeasStepX.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasStepX.TabIndex = 101;
            this.textBoxMeasStepX.Text = "51";
            this.textBoxMeasStepX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasStepX.TextChanged += new System.EventHandler(this.textBoxMeasStepX_TextChanged);
            // 
            // textBoxMeasMinZ
            // 
            this.textBoxMeasMinZ.Location = new System.Drawing.Point(200, 70);
            this.textBoxMeasMinZ.Name = "textBoxMeasMinZ";
            this.textBoxMeasMinZ.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMinZ.TabIndex = 100;
            this.textBoxMeasMinZ.Text = "0";
            this.textBoxMeasMinZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMinZ.TextChanged += new System.EventHandler(this.textBoxMeasMinZ_TextChanged);
            // 
            // textBoxMeasMinY
            // 
            this.textBoxMeasMinY.Location = new System.Drawing.Point(129, 70);
            this.textBoxMeasMinY.Name = "textBoxMeasMinY";
            this.textBoxMeasMinY.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMinY.TabIndex = 99;
            this.textBoxMeasMinY.Text = "0";
            this.textBoxMeasMinY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMinY.TextChanged += new System.EventHandler(this.textBoxMeasMinY_TextChanged);
            // 
            // textBoxMeasMinX
            // 
            this.textBoxMeasMinX.Location = new System.Drawing.Point(58, 70);
            this.textBoxMeasMinX.Name = "textBoxMeasMinX";
            this.textBoxMeasMinX.Size = new System.Drawing.Size(65, 20);
            this.textBoxMeasMinX.TabIndex = 96;
            this.textBoxMeasMinX.Text = "0";
            this.textBoxMeasMinX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMeasMinX.TextChanged += new System.EventHandler(this.textBoxMeasMinX_TextChanged);
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(11, 473);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(73, 14);
            this.labelProgress.TabIndex = 120;
            this.labelProgress.Text = "labelProgress";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 24);
            this.label1.TabIndex = 66;
            this.label1.Text = "Measurement Setup";
            // 
            // progressBarMeasure
            // 
            this.progressBarMeasure.Location = new System.Drawing.Point(11, 493);
            this.progressBarMeasure.Name = "progressBarMeasure";
            this.progressBarMeasure.Size = new System.Drawing.Size(254, 23);
            this.progressBarMeasure.TabIndex = 115;
            // 
            // buttonStopMeasure
            // 
            this.buttonStopMeasure.Location = new System.Drawing.Point(190, 464);
            this.buttonStopMeasure.Name = "buttonStopMeasure";
            this.buttonStopMeasure.Size = new System.Drawing.Size(75, 23);
            this.buttonStopMeasure.TabIndex = 112;
            this.buttonStopMeasure.Text = "Stop Measurements";
            this.buttonStopMeasure.UseVisualStyleBackColor = true;
            this.buttonStopMeasure.Click += new System.EventHandler(this.buttonStopMeasure_Click);
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Location = new System.Drawing.Point(93, 514);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveData.TabIndex = 87;
            this.buttonSaveData.Text = "Save";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Location = new System.Drawing.Point(12, 514);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadData.TabIndex = 86;
            this.buttonLoadData.Text = "Load";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadData_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 543);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1528, 321);
            this.dataGridView1.TabIndex = 71;
            // 
            // comboBoxPlotSelect
            // 
            this.comboBoxPlotSelect.FormattingEnabled = true;
            this.comboBoxPlotSelect.Location = new System.Drawing.Point(1171, 39);
            this.comboBoxPlotSelect.Name = "comboBoxPlotSelect";
            this.comboBoxPlotSelect.Size = new System.Drawing.Size(80, 22);
            this.comboBoxPlotSelect.TabIndex = 1;
            this.comboBoxPlotSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlotSelect_SelectedIndexChanged);
            // 
            // checkBoxPlotAll
            // 
            this.checkBoxPlotAll.AutoSize = true;
            this.checkBoxPlotAll.BackColor = System.Drawing.Color.White;
            this.checkBoxPlotAll.Location = new System.Drawing.Point(1177, 17);
            this.checkBoxPlotAll.Name = "checkBoxPlotAll";
            this.checkBoxPlotAll.Size = new System.Drawing.Size(67, 18);
            this.checkBoxPlotAll.TabIndex = 0;
            this.checkBoxPlotAll.Text = "Plot All Z";
            this.checkBoxPlotAll.UseVisualStyleBackColor = false;
            this.checkBoxPlotAll.CheckedChanged += new System.EventHandler(this.checkBoxPlotAll_CheckedChanged);
            // 
            // timerShutdown
            // 
            this.timerShutdown.Interval = 1000;
            this.timerShutdown.Tick += new System.EventHandler(this.timerShutdown_Tick);
            // 
            // comboBoxFieldSelect
            // 
            this.comboBoxFieldSelect.FormattingEnabled = true;
            this.comboBoxFieldSelect.Items.AddRange(new object[] {
            "Bmagnitude",
            "Bx",
            "By",
            "Bz",
            "Bmag SD",
            "Bx SD",
            "By SD",
            "Bz SD",
            "Temp"});
            this.comboBoxFieldSelect.Location = new System.Drawing.Point(1171, 67);
            this.comboBoxFieldSelect.Name = "comboBoxFieldSelect";
            this.comboBoxFieldSelect.Size = new System.Drawing.Size(80, 22);
            this.comboBoxFieldSelect.TabIndex = 88;
            this.comboBoxFieldSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxFieldSelect_SelectedIndexChanged);
            // 
            // textBoxZdist
            // 
            this.textBoxZdist.Location = new System.Drawing.Point(1206, 93);
            this.textBoxZdist.Name = "textBoxZdist";
            this.textBoxZdist.Size = new System.Drawing.Size(45, 20);
            this.textBoxZdist.TabIndex = 94;
            this.textBoxZdist.Text = "15";
            this.textBoxZdist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelZdist
            // 
            this.labelZdist.AutoSize = true;
            this.labelZdist.Location = new System.Drawing.Point(1174, 96);
            this.labelZdist.Name = "labelZdist";
            this.labelZdist.Size = new System.Drawing.Size(31, 14);
            this.labelZdist.TabIndex = 94;
            this.labelZdist.Text = "Zdist";
            this.labelZdist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelScott
            // 
            this.panelScott.Location = new System.Drawing.Point(310, 12);
            this.panelScott.Name = "panelScott";
            this.panelScott.Size = new System.Drawing.Size(945, 525);
            this.panelScott.TabIndex = 95;
            this.panelScott.Visible = false;
            // 
            // editor3D
            // 
            this.editor3D.BackColor = System.Drawing.Color.White;
            this.editor3D.BorderColorFocus = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.editor3D.BorderColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editor3D.LegendPos = Plot3D.Editor3D.eLegendPos.BottomLeft;
            this.editor3D.Location = new System.Drawing.Point(310, 12);
            this.editor3D.Name = "editor3D";
            this.editor3D.Normalize = Plot3D.Editor3D.eNormalize.Separate;
            this.editor3D.Raster = Plot3D.Editor3D.eRaster.Labels;
            this.editor3D.Size = new System.Drawing.Size(945, 525);
            this.editor3D.TabIndex = 69;
            this.editor3D.TooltipMode = ((Plot3D.Editor3D.eTooltip)((Plot3D.Editor3D.eTooltip.UserText | Plot3D.Editor3D.eTooltip.Coord)));
            this.editor3D.TopLegendColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1552, 876);
            this.Controls.Add(this.panelScott);
            this.Controls.Add(this.labelZdist);
            this.Controls.Add(this.textBoxZdist);
            this.Controls.Add(this.comboBoxFieldSelect);
            this.Controls.Add(this.comboBoxPlotSelect);
            this.Controls.Add(this.checkBoxPlotAll);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(this.editor3D);
            this.Controls.Add(this.buttonLoadData);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelField);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FieldMapGUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelField.ResumeLayout(false);
            this.panelField.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerReadbacks;
        private System.Windows.Forms.Timer timerCubeMeasure;
       
        private System.Windows.Forms.Panel panelField;
     
        private System.Windows.Forms.Label labelFieldX;
        private System.Windows.Forms.Label labelFieldTemp;
        private System.Windows.Forms.Label labelFieldZ;
        private System.Windows.Forms.Label labelFieldY;
        private System.Windows.Forms.Label labelFieldMag;
        private System.Windows.Forms.Label labelAveWindow;
        private System.Windows.Forms.Button buttonkCubeHomeX;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelXpos;
        private System.Windows.Forms.TextBox txtBoxSetPosX;
        private System.Windows.Forms.Button buttonGoTo;
        private Plot3D.Editor3D editor3D;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxSetPosZ;
        private System.Windows.Forms.TextBox txtBoxSetPosY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelZpos;
        private System.Windows.Forms.Label labelYpos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxPlotAll;
        private System.Windows.Forms.ComboBox comboBoxPlotSelect;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.Button buttonSaveData;
        private System.Windows.Forms.ComboBox comboBoxTeslaAveraging;
        private System.Windows.Forms.Timer timerShutdown;
        private System.Windows.Forms.ComboBox comboBoxFieldSelect;
        private System.Windows.Forms.TextBox textBoxZdist;
        private System.Windows.Forms.Label labelZdist;
        private System.Windows.Forms.Button buttonConnectTeslameter;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxTimeMeasHour;
        private System.Windows.Forms.Button buttonStartTimeMeasure;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxTimeMeasX;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxTimeMeasY;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxTimeMeasZ;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBoxStopToMeasure;
        private System.Windows.Forms.CheckBox checkBoxDwell;
        private System.Windows.Forms.CheckBox checkBoxAveraging;
        private System.Windows.Forms.TextBox textBoxAverageMeasure;
        private System.Windows.Forms.Button buttonMeasure;
        private System.Windows.Forms.TextBox textBoxDwell;
        private System.Windows.Forms.Label labelStepSizeZ;
        private System.Windows.Forms.Label labelStepSizeY;
        private System.Windows.Forms.Label labelStepSizeX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMeasMaxZ;
        private System.Windows.Forms.TextBox textBoxMeasMaxY;
        private System.Windows.Forms.TextBox textBoxMeasMaxX;
        private System.Windows.Forms.TextBox textBoxMeasStepZ;
        private System.Windows.Forms.TextBox textBoxMeasStepY;
        private System.Windows.Forms.TextBox textBoxMeasStepX;
        private System.Windows.Forms.TextBox textBoxMeasMinZ;
        private System.Windows.Forms.TextBox textBoxMeasMinY;
        private System.Windows.Forms.TextBox textBoxMeasMinX;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarMeasure;
        private System.Windows.Forms.Button buttonStopMeasure;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxTimeMeasSec;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxTimeMeasMin;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Timer timerTimeMeasure;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panelScott;
    }
}

