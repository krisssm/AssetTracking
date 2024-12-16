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
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Messaging;

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        private PictureBox pictureFloorplan;

        int rssi1;
        int rssi2;
        int avgrssi1;
        int avgrssi2;
        int horizontal;

        int count1 = 1;
        int count2 = 1;
        int errcount1 = 1;
        int errcount2 = 1;

        int errMax = 10; //Error count to check if asset exists
        int averageCount = 5; //Amount of average signals to calculate



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
                count1 = 0;
                count2 = 0;
                rssi1 = 0;
                rssi2 = 0;
                avgrssi1 = 0;
                avgrssi2 = 0;
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
            // Get the Graphics object
            Graphics g = e.Graphics;

            // Define the circle's position and size
            int centerX = pictureFloorplan.Width / 2 - horizontal; // Center of the PictureBox
            int centerY = pictureFloorplan.Height / 2; // Center of the PictureBox
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

            //Runs to get average rssi signals
            if (count1 != averageCount && count2 != averageCount)
            {
                Console.WriteLine("CASE1");
                count1 += 1;
                count2 += 1;

            }
            else if (count1 != averageCount && count2 == averageCount)
            {
                Console.WriteLine("CASE2");
                count1 += 1;
            }
            else if (count1 == averageCount && count2 != averageCount)
            {
                Console.WriteLine("CASE3");
                count2 += 1;
            }
            else if (count1 == averageCount && count2 == averageCount)
            {
                Console.WriteLine("CASE4");
                count1 = 1;
                count2 = 1;
                avgrssi1 = rssi1 / averageCount;
                avgrssi2 = rssi2 / averageCount;
                rssi1 = 0;
                rssi2 = 0;

                if (avgrssi1 > avgrssi2)
                {
                    label7.Text = "Pulmonology";
                }
                else
                {
                    label7.Text = "Neurology";
                }
                calculateLocation();
            }

            if (errcount1 > errMax && errcount2 > errMax) //Checks if asset exists
            {
                errcount1 = 1;
                errcount2 = 1;
                timer1.Enabled = false;
                MessageBox.Show("Asset not found!");           
                return;
            }

            Console.WriteLine($"The count1: {count1}");
                Console.WriteLine($"The count2: {count2}");
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
                    errcount1 = 1;
                    if (count1 != averageCount)
                    {

                        rssi1 += int.Parse(match.Groups[1].Value);
                    }
                    //Console.WriteLine($"The value of rssi on Gateway 1 is: {rssi1}");
                }
                else
                {
                    count1 -= 1;
                    errcount1 += 1;
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
                    errcount2 = 1;
                    if (count2 != averageCount)
                    {
                        rssi2 += int.Parse(match1.Groups[1].Value);
                    }
                    //Console.WriteLine($"The value of rssi on Gateway 2 is: {rssi2}");
                }
                else
                {
                    count2 -= 1;
                    errcount2 += 1;
                    Console.WriteLine("Rssi not found in the string on Gateway 2.");
                }
            }
        }

      

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            if (timer1.Enabled == true) //Kui assetit muuta samal ajal kui otsib, lõpetab otsimise
            { 
                timer1.Enabled = false; 
            }
        }

        private void calculateLocation()
        {
            Console.WriteLine($"The average of rssi on Gateway 1 is: {avgrssi1}");
            Console.WriteLine($"The average of rssi on Gateway 2 is: {avgrssi2}");
            if (avgrssi2 > -71 && avgrssi1 > -71) //Saatja on kahe gateway vahel enamvähem keskel
            {
                if (avgrssi1 > avgrssi2) //Saatja on kahe gateway vahel ja rohkem gateway 1 poole
                {
                    Console.WriteLine("CASE 1");
                    horizontal = -1 * avgrssi2;
                }
                else if (avgrssi2 > avgrssi1) //Saatja on kahe gateway vahel ja rohkem gateway 2 poole
                {
                    Console.WriteLine("CASE 2");
                    horizontal = avgrssi1;
                }
                else
                {
                    Console.WriteLine("CASE 5");
                    horizontal = 0;
                }
                
            } 
            else if ((avgrssi1 < -19 && avgrssi1 > -71) && avgrssi2 < -71) //Saatja on möödas gateway 1st
            {
             Console.WriteLine("CASE 3");
              horizontal = 40 - 3 * avgrssi1;
            }
            else if ((avgrssi2 < -19 && avgrssi2 > -71) && avgrssi1 < -71) //Saatja on möödas gateway 2st
            {
               Console.WriteLine("CASE 4");
                horizontal = -40 + 3 * avgrssi2;
            }
            pictureFloorplan.Invalidate();
        }

        
    }
}
