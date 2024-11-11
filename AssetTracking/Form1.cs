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

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        private PictureBox pictureFloorplan;

        string rssiValue = "-53";
        

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
                Image = Image.FromFile("C:\\Users\\Kris\\Desktop\\Plaan.png"), // Set the image path
                SizeMode = PictureBoxSizeMode.StretchImage // Adjust the image to fit the PictureBox
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
            timer1.Enabled = true;
        }
        private void PictureFloorplan_Paint(object sender, PaintEventArgs e)
        {
            // Get the Graphics object from the PaintEventArgs
            Graphics g = e.Graphics;

            // Define the circle's position and size
            int centerX = pictureFloorplan.Width / 2; // Center of the PictureBox
            int centerY = pictureFloorplan.Height / 2; // Center of the PictureBox
            int radius = (int.Parse(rssiValue) -20) * -3; // Radius of the circle

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
                    rssiValue = match.Groups[1].Value;
                    Console.WriteLine($"The value of rssi on Gateway 1 is: {rssiValue}");
                    label6.Text = rssiValue;
                    pictureFloorplan.Invalidate();
                }
                else
                {
                    Console.WriteLine("Rssi not found in the string on Gateway 1.");
                }
            }


            using (var conn = new MKConnection("192.168.1.161", "admin", "KristjanJaKregor"))
            {

                conn.Open();

                string assetName1 = textSearch.Text; // Your variable that holds the MAC address
                var cmd = conn.CreateCommand(string.Format("iot bluetooth scanners advertisements print where address={0}", assetName));
                string originalString1 = "";
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
                    rssiValue = match1.Groups[1].Value;
                    Console.WriteLine($"The value of rssi on Gateway 2 is: {rssiValue}");
                    label5.Text = rssiValue;
                    pictureFloorplan.Invalidate();
                }
                else
                {
                    Console.WriteLine("Rssi not found in the string on Gateway 2.");
                }
            }
        }
    }
}
