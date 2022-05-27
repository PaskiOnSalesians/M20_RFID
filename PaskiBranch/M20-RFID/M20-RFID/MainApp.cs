using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AccesDades;

namespace M20_RFID
{
    public partial class MainApp : Form
    {
        Dades _data = new Dades();

        SerialPort ArduinoPort;
        string[] PuertosDisponibles;

        Thread arduinoListener;

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
            Control.CheckForIllegalCrossThreadCalls = false;
            arduinoListener = new Thread(portArduinoListener);
            arduinoListener.Start();

            _data.ConnectDB();

            DataSet dts = new DataSet();

            string tabla = "ActiveBeacons";
            string query = "select codeBeacon from " + tabla;

            dts = _data.PortarPerConsulta(query, tabla);

            int pos = 0;
            foreach(DataRow dr in dts.Tables[0].Rows)
            {
                cbxBeacons.Items.Add(dr[pos].ToString());
            }
        }

        private void OpenArduinoPort()
        {
            ArduinoPort.PortName = lsxArduino.GetItemText(lsxArduino.SelectedItem.ToString());
            ArduinoPort.BaudRate = 9600;

            ArduinoPort.Open();
            ArduinoPort.Write(cbxBeacons.Text);
            //ArduinoPort.Close();

            //try
            //{


            //    //try
            //    //{
            //    //    if(cbxBeacons.SelectedItem != null)
            //    //    {

            //    //    }
            //    //}
            //    //catch (Exception)
            //    //{
            //    //    MessageBox.Show("No hay una baliza seleccionada!");
            //    //}
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Puerto no seleccionado!");
            //}
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

        private void MainApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (arduinoListener.IsAlive)
            {
                arduinoListener.Abort();
            }
        }
    }
}
