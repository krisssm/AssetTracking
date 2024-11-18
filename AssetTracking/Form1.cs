using MikrotikDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.IO;
using Polly.Caching;
using System.Linq.Expressions;

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        private PictureBox pictureFloorplan;

        string rssiValue = "-53";
        int rssi1;
        int rssi2;
        int horizontal;



        public Form1()
        {

            this.Text = "Draw Empty Circle on PictureBox Example";
            this.Size = new Size(400, 400);

            // Create and configure the PictureBox
            pictureFloorplan = new PictureBox
            {
                Location = new Point(270, 0), // Position of the PictureBox
                Size = new Size(750, 750), // Size of the PictureBox
                BorderStyle = BorderStyle.FixedSingle,
                Image = Image.FromFile("C:\\Users\\Kris\\source\\repos\\AssetTracking\\Plaan.png"), // Set the image path
                SizeMode = PictureBoxSizeMode.Zoom // Adjust the image to fit the PictureBox
            };

            // Subscribe to the Paint event of the PictureBox
            pictureFloorplan.Paint += new PaintEventHandler(PictureFloorplan_Paint);

            // Add the PictureBox to the form
            this.Controls.Add(pictureFloorplan);

            InitializeComponent();
        }

  
        private void Form1_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textSearch.Text))
            {
                timer1.Enabled = true;
            }
            else
            {
                // TextBox is empty
                MessageBox.Show("Please enter an asset.");
            }
            
        }
        private void PictureFloorplan_Paint(object sender, PaintEventArgs e)
        {
            // Get the Graphics object from the PaintEventArgs
            Graphics g = e.Graphics;

            // Define the circle's position and size
            int centerX = pictureFloorplan.Width / 2 - horizontal; // Center of the PictureBox
            int centerY = pictureFloorplan.Height / 2; // Center of the PictureBox
            //int radius = (int.Parse(rssiValue) -20) * -3; // Radius of the circle
            int radius = 30; // Radius of the circle

            // Create a pen to draw the circle
            using (Pen pen = new Pen(Color.Red, 2)) // Color and width of the pen
            {
                // Draw the empty circle
                g.DrawEllipse(pen, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                timer1.Enabled = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string assetName = textSearch.Text; // Variable that holds the MAC address
            int strongestRssi = int.MinValue; // Tracks the strongest RSSI value
            string strongestRssiGateway = ""; //Tracks which gateway has the strongest signal

            // Gateway 1
            using (var conn = new MKConnection("192.168.1.74", "admin", "KristjanJaKregor"))
            {
                conn.Open();

                string originalString = "";
                var cmd = conn.CreateCommand(string.Format("iot bluetooth scanners advertisements print where address={0}", assetName));
                try
                {
                    var result = cmd.ExecuteReader();
                    
                    foreach (var line in result)
                    {
                        Console.WriteLine(line);
                        originalString = line;

                    }
                }

                catch (Exception ex)
                {
                    // Handle any other errors here
                    Console.WriteLine($"Error: {ex.Message}");
                }
                


                conn.Close();


                // Regular expression to match rssi value
                var match = Regex.Match(originalString, @"rssi=(.*?)=");

                
                if (match.Success)
                {
                    rssi1 = int.Parse(match.Groups[1].Value);
                    Console.WriteLine($"The value of rssi on Gateway 1 is: {rssi1}");
                    if (rssi1 > strongestRssi)
                    {
                        strongestRssi = rssi1;
                        strongestRssiGateway = "Gateway 1";
                    }
                }
                

                if (match.Success)
                {
                    rssiValue = match.Groups[1].Value;
                    Console.WriteLine($"The value of rssi on Gateway 1 is: {rssiValue}");
                    label6.Text = Convert.ToString(rssi1);
                    pictureFloorplan.Invalidate();
                }
                
                else
                {
                    Console.WriteLine("Rssi not found in the string on Gateway 1.");
                }
                
            }

            // Gateway 2
            using (var conn = new MKConnection("192.168.1.161", "admin", "KristjanJaKregor"))
            {

                conn.Open();

                string originalString1 = "";
                var cmd = conn.CreateCommand(string.Format("iot bluetooth scanners advertisements print where address={0}", assetName));
                
                try
                {
                    var result = cmd.ExecuteReader();
                    foreach (var line1 in result)
                    {
                        Console.WriteLine(line1);
                        originalString1 = line1;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any other errors here
                    Console.WriteLine($"Error: {ex.Message}");
                }
              
                conn.Close();

                // Regular expression to match rssi value
                var match1 = Regex.Match(originalString1, @"rssi=(.*?)=");

                if (match1.Success)
                {
                    rssi2 = int.Parse(match1.Groups[1].Value);
                    //rssiValue = match1.Groups[1].Value;
                    Console.WriteLine($"The value of rssi on Gateway 2 is: {rssi2}");
                    label5.Text = Convert.ToString(rssi2);
                    //pictureFloorplan.Invalidate();
                    if (rssi2 > strongestRssi)
                    {
                        strongestRssi = rssi2;
                        strongestRssiGateway = "Gateway 2";
                    }
                }
                else
                {
                    Console.WriteLine("Rssi not found in the string on Gateway 2.");
                }
            }
            // Display the strongest RSSI and update the UI
            /*if (strongestRssiGateway != "")
            {
                Console.WriteLine($"The strongest RSSI is {strongestRssi} from {strongestRssiGateway}.");
                label6.Text = $"Strongest RSSI: {strongestRssi} ({strongestRssiGateway})";
                rssiValue = strongestRssi.ToString(); // Update the global RSSI value
                pictureFloorplan.Invalidate(); // Redraw the circle on the PictureBox
            }
            else
            {
                Console.WriteLine("No RSSI values found.");
            }
            */

            switch (strongestRssiGateway)
            {
                case "Gateway 1":
                    label7.Text = "1";
                    break;
                case "Gateway 2":
                    label7.Text = "2";
                    break;
                default:
                    label7.Text = "None";
                    break;
            }
        }

        private void btnFloorplan_Click(object sender, EventArgs e)
        {
            rssi1 = Convert.ToInt16(textBox1.Text);
            rssi2 = Convert.ToInt16(textBox2.Text);

            if (rssi2 > -60 && rssi1 > -60 && rssi2 < -45 && rssi1 < -45) //Saatja on kahe gateway vahel enamvähem keskel
            {
                Console.WriteLine("CASE 5");
                horizontal = 0;
            }
            if (rssi1 > -50 && rssi2 < -50 && rssi2 > -70) //Saatja on kahe gateway vahel ja rohkem gateway 1 poole
            {
                Console.WriteLine("CASE 1");
                horizontal = -2 * rssi2;
            }
            else if(rssi2 > -60 && rssi1 < -60 && rssi1 > -70) //Saatja on kahe gateway vahel ja rohkem gateway 2 poole
            {
                Console.WriteLine("CASE 2");
                horizontal = -2 * rssi1;
            }
            else if (rssi1 < -30 && rssi2 < -70) //Saatja on möödas gateway 1st
            {
                Console.WriteLine("CASE 3");
                horizontal = 40 - 3 * rssi1;
            }
            else if (rssi2 < -30 && rssi1 < -70) //Saatja on möödas gateway 2st
            {
                Console.WriteLine("CASE 4");
                horizontal = -40 + 3 * rssi1;
            }
            pictureFloorplan.Invalidate();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            if (timer1.Enabled == true) //Kui assetit muuta samal ajal kui otsib, lõpetab otsimise
            { 
                timer1.Enabled = false; 
            }
        }

      
    }
}
