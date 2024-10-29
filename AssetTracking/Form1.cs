using MikrotikDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        public Form1()
        {

   
            using (var conn = new MKConnection("192.168.1.74", "admin", "kristjanjakregor"))
            {
                conn.Open();
                var cmd = conn.CreateCommand("iot bluetooth scanners advertisements print where address=D4:01:C3:6B:9F:8E");
                var result = cmd.ExecuteReader();
                foreach (var line in result)
                    Console.WriteLine(line);

                var cmd1 = conn.CreateCommand("iot bluetooth scanners advertisements print where address=DC:2C:6E:73:A0:D4");
                var result1 = cmd1.ExecuteReader();
                foreach (var line1 in result1)
                    Console.WriteLine(line1);

            }




            InitializeComponent();
        }
    }
}
