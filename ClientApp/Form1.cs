using ClientApp.ClientServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class Form1 : Form
    {

        TcpClient socketForServer;
        NetworkStream networkStream;
        StreamReader streamReader;
        StreamWriter streamWriter;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                socketForServer = new TcpClient("localHost", 1024);
            }
            catch
            {
                MessageBox.Show(
                "Failed to connect to server at {0}:999", "localhost");
                return;
            }

            networkStream = socketForServer.GetStream();
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
            MessageBox.Show("*******This is client program who is connected to localhost on port No:10*****");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            networkStream?.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = ClientServer.ClientServer.WriteResponse(MessageType.Authentification, textBox3.Text, textBox4.Text);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();
                textBox1.Text = "";
                MessageType messageType;
                var readMessage = streamReader.ReadLine();
                string response = ClientServer.ClientServer.ParseMessage(msg, out messageType);
                MessageBox.Show(response);
                if (messageType == MessageType.Authentification)
                {
                    panel2.Visible = false;
                    panel1.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("Exception reading from Server");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string msg = ClientServer.ClientServer.WriteResponse(MessageType.Message, textBox1.Text, textBox3.Text, textBox2.Text);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();
                MessageType messageType;
                var readMessage = streamReader.ReadLine();
                string response = ClientServer.ClientServer.ParseMessage(msg, out messageType);
                MessageBox.Show(response);
                textBox1.Text = "";
                textBox2.Text = "";
            }
            catch
            {
                MessageBox.Show("Exception reading from Server");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = ClientServer.ClientServer.WriteResponse(MessageType.ListMessages, textBox3.Text);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();
                textBox1.Text = "";
                MessageType messageType;
                var readMessage = streamReader.ReadLine();
                string response = ClientServer.ClientServer.ParseMessage(readMessage, out messageType);

                if (messageType == MessageType.ListMessages)
                {
                    string[] resp = response.Split(',');
                    if (resp.Length % 2 != 0) MessageBox.Show("Неверный формат возвращаемых данных");
                    //dataGridView1.Rows[0].Cells.Clear();
                    //dataGridView1.Rows[1].Cells.Clear();
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < resp.Length / 2; i++)
                    {
                        dataGridView1.Rows.Add();
                        //dataGridView1.Rows[i].Cells.Clear();
                        dataGridView1.Rows[i].Cells[0].Value = resp[2 * i];
                        dataGridView1.Rows[i].Cells[1].Value = resp[2 * i + 1];
                    }

                }
                else
                    MessageBox.Show(response);
            }
            catch
            {
                MessageBox.Show("Exception reading from Server");
            }
        }
    }
}
