using ServiceAssembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    static class Program
    {
        //public static Game game;
        public static ServerUI form1;
        public static ServiceHost serviceHost;
        private static CommsService service;

        public static CommsService Service { get => service; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            service = new CommsService();
            service.Game = new Game();
            serviceHost = new ServiceHost(Service);
            
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Startup());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //lol, you screwed 
            }
        }
    }



}
