using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M20_RFID
{
    public partial class MainApp : Form
    {
        SerialPort ArduinoPort;
        string[] PuertosDisponibles;

        public MainApp()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            portArduinoListener();
        }

        private void OpenArduinoPort()
        {
            try
            {
                ArduinoPort.PortName = lsxArduino.GetItemText(lsxArduino.SelectedItem);
                ArduinoPort.BaudRate = 9600;
                ArduinoPort.Open();
                ArduinoPort.Write("Beacon Name");
                ArduinoPort.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        private void portArduinoListener()
        {
            PuertosDisponibles = SerialPort.GetPortNames();

            foreach (var item in PuertosDisponibles)
            {
                lsxArduino.Items.Add(item);
            }

            ArduinoPort = new SerialPort();
        }

        private void btnSerialConnect_Click(object sender, EventArgs e)
        {
            OpenArduinoPort();
        }
    }
}
