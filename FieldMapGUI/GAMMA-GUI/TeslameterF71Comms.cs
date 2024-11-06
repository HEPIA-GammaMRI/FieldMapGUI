using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;


namespace FieldMapGUI
{

    partial class TeslameterF71Comms
    {
        string sendString;
        string commandString;

        public bool ReadbacksAvailable;

        public string replyFromUnit;

        public double UnitFieldMagnitude;
        public double UnitFieldX;
        public double UnitFieldY;
        public double UnitFieldZ;
        public double UnitFieldTemp;
        public int UnitAveragingWindow;
        public int UnitProbeTempCOmpEnabled;
        public int UnitFilterEnabled;

        public String UnitSensorMode,  UnitFilterType, UnitProbeSerial, UnitProbeCalDate, UnitProbeModel, UnitProbeOrientation, UnitProbeType,  UnitUnits;

        private SerialPort m_ioSPIO;
        private bool m_bOnline = false;
        private bool m_bConnected;


        private Thread m_thIOThread;

        public static Object m_oIOLock = new Object();


        private string m_sLogDirectory;
        private string m_sLogFilePath;
        private const string LOG_FOLDER = "\\Log";
        private bool m_bLogToDebugFile;
        private bool m_bDataDumpToFile;
        private int m_iDataDumpFrequencyMS;
        private DateTime m_dtLastDataDump;
        private bool m_ContinueMonitoring;

        public TeslameterF71Comms()
        {
            string sUserProfilePATH = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = sUserProfilePATH + "\\My Documents\\FieldMapGUI\\Teslameter" + LOG_FOLDER + "\\";
            m_sLogFilePath = m_sLogDirectory + "Teslameter-IO.txt";
            LogIOToFile = false;
            LogFrequencyMS = 10000; // log every 10s
            m_dtLastDataDump = DateTime.Now;
            DumpPropertiesToFile = false;
        }

        public void Init()
        {
            ReadbacksAvailable = false;

            commandString = "";



           

        m_thIOThread = new Thread(MainIOThread);
            m_thIOThread.Name = "Teslameter_IOThread";

            int TeslameterStatus = 0;
            m_bOnline = false;
            m_bConnected = false;

            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                if (port.Contains("/dev/cu.usbmodem") || port.Contains("/dev/tty.usbmodem") || port.Contains("COM"))
                {
                    m_ioSPIO = new SerialPort(port, 115200);

                    if (!m_ioSPIO.IsOpen)
                    {
                        try
                        {
                            m_ioSPIO.NewLine = "\r\n";
                            //m_ioSPIO.DtrEnable = true;
                            m_ioSPIO.RtsEnable = true;
                            //m_ioSPIO.Handshake = Handshake.RequestToSend;
                            m_ioSPIO.WriteTimeout = 500;
                            m_ioSPIO.ReadTimeout = 500;
                            m_ioSPIO.ReadBufferSize = 4096;
                            m_ioSPIO.WriteBufferSize = 4096;
                            m_ioSPIO.Open();


                            TeslameterStatus = TeslameterUnitPresent();
                            if (TeslameterStatus == 1)
                            {
                                LogError("Teslameter IO - Initialised()");
                                m_bOnline = (TeslameterStatus == 1);
                                Connected = m_bOnline;
                                break;
                            }
                            else
                            {
                                m_ioSPIO.Close();
                                LogError("Teslameter IO - Failed to Init()");
                                m_bOnline = false;
                                TeslameterStatus = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            if (Connected)
            {
                StartIOThread();
            }
            //return TeslameterStatus;
        }

        private int TeslameterUnitPresent()
        {
            int temp = 0;
            m_bOnline = true; // COMMS are possible via SerialPort 
            string rv = SendToUnit("*:PROB:MOD?;:PROB:SNUM?;:PROB:CALD?;:PROB:SOR?;:PROB:STYP?;");
            if (rv != null)
            {
                if (rv.Contains("FP-2X-250-ZS30M-15"))  
                {
                    LogError("TeslameterF71 Unit Found!");

                    String[] readbacks = rv.Split(';');
                    UnitProbeModel = readbacks[0];
                    UnitProbeSerial = readbacks[1];
                    UnitProbeCalDate = readbacks[2];
                    UnitProbeOrientation = readbacks[3];
                    UnitProbeType = readbacks[4];
                    temp = 1;
                }
                else
                {
                    LogError(">>>>> It's all gone a bit Pete Tong!");
                    temp = 0;
                }
            }
            return temp;
        }

        private string SendToUnit(string sCmd)
        {
            string rv;
           
            rv = "";
            

            

            try
            {

                m_ioSPIO.DiscardInBuffer();
                m_ioSPIO.DiscardOutBuffer();

                m_ioSPIO.WriteLine(sCmd);
                
                Thread.Sleep(200);
                rv = m_ioSPIO.ReadLine();

             




                Connected = true;
                //Console.WriteLine("Reply: " + rv);
            }
            catch (Exception ex)
            {
               // Console.WriteLine("SendToUnit(): IO Status = {0}", ex.Message);
      
            }
            SaveToDebugFile(sCmd);
            //  Console.WriteLine("SendToUnit(): {0}", sCmd);
            
            return rv;
        }

        public void StartIOThread()
        {
            if (m_thIOThread.IsAlive == false)
            {
                Console.WriteLine("RC_IO: Starting IOThread");
                m_thIOThread.Start();
            }
        }

        public void Closedown()
        {
            m_ContinueMonitoring = false; // Signal to IOThread() that it should terminate
            if (m_thIOThread.IsAlive)
            {
                m_thIOThread.Join(2000);
            }
            m_bOnline = false;
            m_ioSPIO.Close();
            m_bConnected = false;
        }//Closedown()

        private void LogError(string sError)

        {
            Console.WriteLine("RC_IO - " + sError);
            SaveToDebugFile(sError);
        }

        public bool Connected
        {
            set
            {
                if (m_bConnected == value)
                {
                    return;
                }
                m_bConnected = value;
            }
            get { return m_bConnected; }
        }

        private void MainIOThread()
        {
            while (m_bOnline)
            {
                //m_ioSPIO.DiscardOutBuffer();
                //m_ioSPIO.DiscardInBuffer();
                //m_ioSPIO.WriteTimeout = 500;
                //m_ioSPIO.ReadTimeout = 500;
                //ReadbacksAvailable = false;
                lock (m_oIOLock)

                {

                    if (commandString != "")
                    {
                        Console.WriteLine(commandString);
                    }
                    sendString = "FETC:DC? ;*FETC:DC? X;*FETC:DC? Y;*FETC:DC? Z;*FETC:TEMP?;:SENS:MODE?;:SENS:AVER:COUN?;:SENS:FILT:STAT?;:SENS:FILT:TYPE?;:UNIT?;:PROB:TCOM?" + commandString + ";:*OPC;";//;:*CLS;:*RST
                    //
                    commandString = "";
                }
                replyFromUnit = SendToUnit(sendString); 
                //Console.WriteLine(replyFromUnit);
                String[] readbacks = replyFromUnit.Split(';');
                Thread.Sleep(50);
               

                double tempVar, tempVarM, tempVarX, tempVarY, tempVarZ;
                
                
                try
                {
                   
                    
                  

                    tempVarM = double.Parse(readbacks[0]);
                    tempVarX = double.Parse(readbacks[1]);
                    tempVarY = double.Parse(readbacks[2]);
                    tempVarZ = double.Parse(readbacks[3]);


                    tempVar = 0;

                    if (Math.Round(tempVarM,4) != Math.Round(Math.Sqrt(tempVarX* tempVarX + tempVarY*tempVarY + tempVarZ* tempVarZ),4)) 
                    { 
                        tempVar = 1; 
                    }

                    
                    if (readbacks.Length != 11)
                    {
                        tempVar = 1;
                    }
                    if (readbacks[9] != "TESLA")
                    {
                        tempVar = 1;
                    }




                }
                catch
                {
                    tempVar = 1;
                }


                if (tempVar == 0)
                {
                    UnitFieldMagnitude = double.Parse(readbacks[0]);
                    UnitFieldX = -double.Parse(readbacks[2]);
                    UnitFieldY = -double.Parse(readbacks[3]);
                    UnitFieldZ = -double.Parse(readbacks[1]);
                    UnitFieldTemp = double.Parse(readbacks[4]);

                    UnitSensorMode = readbacks[5];
                    UnitAveragingWindow = int.Parse(readbacks[6]);
                    UnitFilterEnabled = int.Parse(readbacks[7]);
                    UnitFilterType = readbacks[8];
                    
                    UnitProbeTempCOmpEnabled = int.Parse(readbacks[10]);
                    UnitUnits = readbacks[9];
                    ReadbacksAvailable = true;
                }





                







            }


        }//IOThread()

        public void QueueCommand(string sCommand)
        {
            lock (m_oIOLock)
            {
                commandString = commandString + ";" + sCommand;
            }

        }//SendDGCommand()

        public bool LogIOToFile
        {
            set { m_bLogToDebugFile = value; }
            get { return m_bLogToDebugFile; }
        }

        private void SaveToDebugFile(string sText)
        {
            if (!m_bLogToDebugFile)
            {
                return;
            }
            try
            {
                StreamWriter file = new System.IO.StreamWriter(m_sLogDirectory + "RC-IO.txt", true); // Append if present, create if not
                file.WriteLine(string.Format("{0} : {1}", DateTime.Now, sText));
                file.Close();
            }
            catch { } // ## TODO - report fault on saving to file!
        }

        public bool DumpPropertiesToFile
        {
            set { m_bDataDumpToFile = value; }
            get { return m_bDataDumpToFile; }
        }

        public int LogFrequencyMS
        {
            set { m_iDataDumpFrequencyMS = value; }
            get { return m_iDataDumpFrequencyMS; }
        }

    }


}
