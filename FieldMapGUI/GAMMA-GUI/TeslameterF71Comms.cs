using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace FieldMapGUI
{
    partial class TeslameterF71Comms
    {
        // Command-related fields
        private string sendString;
        private string commandString;

        // Public data fields
        public bool ReadbacksAvailable;
        public string ReplyFromUnit;

        public double UnitFieldMagnitude;
        public double UnitFieldX;
        public double UnitFieldY;
        public double UnitFieldZ;
        public double UnitFieldTemp;
        public int UnitAveragingWindow;
        public int UnitProbeTempCompEnabled;
        public int UnitFilterEnabled;

        public string UnitSensorMode;
        public string UnitFilterType;
        public string UnitProbeSerial;
        public string UnitProbeCalDate;
        public string UnitProbeModel;
        public string UnitProbeOrientation;
        public string UnitProbeType;
        public string UnitUnits;

        // Serial port and communication flags
        private SerialPort m_ioSPIO;
        private bool m_bOnline = false;
        private bool m_bConnected;

        // Thread and synchronization
        private Thread m_thIOThread;
        public static readonly object m_oIOLock = new object();

        // Logging and configuration
        private readonly string m_sLogDirectory;
        private readonly string m_sLogFilePath;
        private const string LOG_FOLDER = "\\Log";
        private bool m_bLogToDebugFile;
        private bool m_bDataDumpToFile;
        private int m_iDataDumpFrequencyMS;
        private DateTime m_dtLastDataDump;
   

        // Constructor
        public TeslameterF71Comms()
        {
            string userProfilePath = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = Path.Combine(userProfilePath, "My Documents", "FieldMapGUI", "Teslameter", LOG_FOLDER);
            m_sLogFilePath = Path.Combine(m_sLogDirectory, "Teslameter-IO.txt");

            LogIOToFile = false;
            LogFrequencyMS = 10000; // Log every 10 seconds
            m_dtLastDataDump = DateTime.Now;
            DumpPropertiesToFile = false;
        }

        // Initialization
        public void Init()
        {
            ReadbacksAvailable = false;
            commandString = "";
            m_thIOThread = new Thread(MainIOThread) { Name = "Teslameter_IOThread" };

            m_bOnline = false;
            m_bConnected = false;

            // Try connecting to available serial ports
            foreach (string port in SerialPort.GetPortNames())
            {
                if (port.Contains("/dev/cu.usbmodem") || port.Contains("/dev/tty.usbmodem") || port.Contains("COM"))
                {
                    m_ioSPIO = new SerialPort(port, 115200)
                    {
                        NewLine = "\r\n",
                        RtsEnable = true,
                        WriteTimeout = 500,
                        ReadTimeout = 500,
                        ReadBufferSize = 4096,
                        WriteBufferSize = 4096
                    };

                    try
                    {
                        m_ioSPIO.Open();
                        if (TeslameterUnitPresent() == 1)
                        {
                            LogError("Teslameter IO - Initialized");
                            m_bOnline = true;
                            Connected = true;
                            break;
                        }
                        else
                        {
                            m_ioSPIO.Close();
                            LogError("Teslameter IO - Initialization Failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during initialization: {ex.Message}");
                    }
                }
            }

            // Start the IO thread if connected
            if (Connected)
            {
                StartIOThread();
            }
        }

        // Check if the Teslameter unit is present
        private int TeslameterUnitPresent()
        {
            m_bOnline = true; // Assume comms are possible
            string response = SendToUnit("*:PROB:MOD?;:PROB:SNUM?;:PROB:CALD?;:PROB:SOR?;:PROB:STYP?;");

            if (!string.IsNullOrEmpty(response) && response.Contains("FP-2X-250-ZS30M-15"))
            {
                LogError("TeslameterF71 Unit Found!");
                string[] readbacks = response.Split(';');
                UnitProbeModel = readbacks[0];
                UnitProbeSerial = readbacks[1];
                UnitProbeCalDate = readbacks[2];
                UnitProbeOrientation = readbacks[3];
                UnitProbeType = readbacks[4];
                return 1;
            }

            LogError("Teslameter Unit Not Found");
            return 0;
        }

        // Send a command to the Teslameter unit
        private string SendToUnit(string command)
        {
            try
            {
                m_ioSPIO.DiscardInBuffer();
                m_ioSPIO.DiscardOutBuffer();

                m_ioSPIO.WriteLine(command);
                Thread.Sleep(200);
                string response = m_ioSPIO.ReadLine();
                SaveToDebugFile(command);
                Connected = true;

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command: {ex.Message}");
                return null;
            }
        }

        // Start the IO thread
        public void StartIOThread()
        {
            if (!m_thIOThread.IsAlive)
            {
                Console.WriteLine("Starting IOThread");
                m_thIOThread.Start();
            }
        }

        // Shutdown and cleanup
        public void Closedown()
        {
         

            if (m_thIOThread.IsAlive)
            {
                m_thIOThread.Join(2000);
            }

            m_bOnline = false;
            m_ioSPIO.Close();
            m_bConnected = false;
        }

        // Main IO thread logic
        private void MainIOThread()
        {
            while (m_bOnline)
            {
                lock (m_oIOLock)
                {
                    if (!string.IsNullOrEmpty(commandString))
                    {
                        Console.WriteLine($"Queued Command: {commandString}");
                    }

                    sendString = $"FETC:DC?;*FETC:DC? X;*FETC:DC? Y;*FETC:DC? Z;*FETC:TEMP?;:SENS:MODE?;:SENS:AVER:COUN?;:SENS:FILT:STAT?;:SENS:FILT:TYPE?;:UNIT?;:PROB:TCOM?{commandString};:*OPC;";
                    commandString = ""; // Reset command queue
                }

                // Process response
                ReplyFromUnit = SendToUnit(sendString);
                string[] readbacks = ReplyFromUnit?.Split(';');

                if (readbacks?.Length == 11 && readbacks[9] == "TESLA")
                {
                    try
                    {
                        // Parse readbacks
                        UnitFieldMagnitude = double.Parse(readbacks[0]);
                        UnitFieldX = -double.Parse(readbacks[2]);
                        UnitFieldY = -double.Parse(readbacks[3]);
                        UnitFieldZ = -double.Parse(readbacks[1]);
                        UnitFieldTemp = double.Parse(readbacks[4]);
                        UnitSensorMode = readbacks[5];
                        UnitAveragingWindow = int.Parse(readbacks[6]);
                        UnitFilterEnabled = int.Parse(readbacks[7]);
                        UnitFilterType = readbacks[8];
                        UnitProbeTempCompEnabled = int.Parse(readbacks[10]);
                        UnitUnits = readbacks[9];
                        ReadbacksAvailable = true;
                    }
                    catch
                    {
                        LogError("Error parsing response from unit.");
                    }
                }
            }
        }

        // Queue a command for execution
        public void QueueCommand(string command)
        {
            lock (m_oIOLock)
            {
                commandString += ";" + command;
            }
        }

        // Error logging
        private void LogError(string error)
        {
            Console.WriteLine($"Error: {error}");
            SaveToDebugFile(error);
        }

        // Save debug information to a file
        private void SaveToDebugFile(string text)
        {
            if (!m_bLogToDebugFile) return;

            try
            {
                Directory.CreateDirectory(m_sLogDirectory);
                StreamWriter file = new StreamWriter(Path.Combine(m_sLogDirectory, "RC-IO.txt"), true);
                file.WriteLine($"{DateTime.Now}: {text}");
            }
            catch
            {
                Console.WriteLine("Failed to save debug file.");
            }
        }

        // Properties
        public bool Connected
        {
            get => m_bConnected;
            set => m_bConnected = value;
        }

        public bool LogIOToFile
        {
            get => m_bLogToDebugFile;
            set => m_bLogToDebugFile = value;
        }

        public bool DumpPropertiesToFile
        {
            get => m_bDataDumpToFile;
            set => m_bDataDumpToFile = value;
        }

        public int LogFrequencyMS
        {
            get => m_iDataDumpFrequencyMS;
            set => m_iDataDumpFrequencyMS = value;
        }
    }
}
