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

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        private PictureBox pictureFloorplan;

        string rssiValue = "-53";

        public Form1()
        {


            //using (var conn = new MKConnection("192.168.1.74", "admin", "kristjanjakregor"))
            //{
            //    conn.Open();
            //    var cmd = conn.CreateCommand("iot bluetooth scanners advertisements print where address=D4:01:C3:6B:9F:8E");
            //    var result = cmd.ExecuteReader();
            //    foreach (var line in result)
            //    {
            //        Console.WriteLine(line);
            //        Console.WriteLine("TTT");
            //    }

            //    var cmd1 = conn.CreateCommand("iot bluetooth scanners advertisements print where address=DC:2C:6E:73:A0:D4");
            //    var result1 = cmd1.ExecuteReader();
            //    foreach (var line1 in result1)
            //        Console.WriteLine(line1);
            //    Console.WriteLine("Tere");
            //    conn.Close();
            //}

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

        //private void btnFloorplan_Click(object sender, EventArgs e)
        //{
        //    if (pictureFloorplan1.Visible == false)
        //    {
        //        pictureFloorplan1.Visible = true;
        //    }
        //    else if (pictureFloorplan1.Visible == true)
        //    {
        //        pictureFloorplan1.Visible = false;
        //    }
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {

            using (var conn = new MKConnection("192.168.1.74", "admin", "kristjanjakregor"))
            {
                conn.Open();
 

                string assetName = textSearch.Text; // Your variable that holds the MAC address
                var cmd = conn.CreateCommand(string.Format("iot bluetooth scanners advertisements print where address={0}", assetName));

                var result = cmd.ExecuteReader();
                string originalString = "";
                foreach (var line in result)
                { 
                    Console.WriteLine(line);
                    originalString = line;
 
                }
                    
                Console.WriteLine("Tere1");
                conn.Close();


                // Regular expression to match rssi value
                var match = Regex.Match(originalString, @"rssi=(.*?)=");

                if (match.Success)
                {
                    rssiValue = match.Groups[1].Value;
                    Console.WriteLine($"The value of rssi is: {rssiValue}");
                    lblRSSI.Text = rssiValue;
                    pictureFloorplan.Invalidate();
                }
                else
                {
                    Console.WriteLine("Rssi not found in the string.");
                }
            }
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
    }
}
