using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO.Ports;

namespace Advanced_Drive_GUI
{
    public class DriveControlObject
    {
        public SerialPort? drivePort;

        public void ConnectToDrive()
        {
            string drivePortName = "N/A";
            string[] driveNames = { "TestName" }; //array of all drive names to search for.
            var searcher = new ManagementObjectSearcher("Select * From Win32_SerialPort");
            ManagementObjectCollection collection = searcher.Get(); //get all devices connected via serial port
            foreach (var item in collection) //for each device found
            {
                //could look for something like "DeviceID" instead of "Name"?
                string deviceName = (string)item.GetPropertyValue("Name"); //get the name of the current device
                if (driveNames.Contains(deviceName)) //if the device name matches one of the drive names
                {
                    drivePortName = deviceName; //set the port name to match the device name
                }
            }
            if (drivePortName != "N/A")
            {
                SerialPort serialPort = new(drivePortName); //instantiate SerialPort
                serialPort.PortName = drivePortName; //Change port name to match the drive's 
                drivePort = serialPort;
            }
            drivePort = null;
        }
    }
}
