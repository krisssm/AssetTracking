using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetTracking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Initial factory
            var factory = new FlurlTbClientFactory
            {
                Options = new ThingsboardNetFlurlOptions()
                {
                    BaseUrl = "http://localhost:8080",
                    Username = "kristjanmaetaly1.2.3.4@gmail.com",
                    Password = "KristjanjaKregor",
                }
            };

            // Get the client
            var authClient = factory.CreateAuthClient();
            var userInfo = await authClient.GetCurrentUserAsync();
            Console.WriteLine($"Hello {userInfo.Email}");
        }

        InitializeComponent();
        }
    }
}
