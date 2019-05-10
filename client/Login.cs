using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            makeConnection();
        }

        private void makeConnection()
        {
            InstanceContext instanceContext = new InstanceContext(new CallBackImlementation());
            Program.serviceClient = new SVC.CommsServiceClient(instanceContext);
            Program.Client  = new SVC.Client();

            Program.Client.Name = tbClientName.Text.ToUpper().Trim();
            if (Program.serviceClient.Connect(Program.Client))
            {
                //good
                Console.WriteLine(Program.Client.Name + " Connected");
                Program.game = new SVC.Game();
                Program.form1 = new Form1();
                this.Hide();
                Program.form1.Show();
            }
            else
            {
                //bad
                Console.WriteLine("Not Connected");
                lblStatus.Text = "Not Connected";
                lblStatus.ForeColor = Color.Red;
            }

        }
    }
}
