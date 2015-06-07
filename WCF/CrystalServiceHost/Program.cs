using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WcfGetDataLib;

namespace GetData
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (ServiceHost ServiceHost = new ServiceHost(typeof(GetDataService)))
            {
                // Открыть хост и начать прослушивание входных сообщений. 
                ServiceHost.Open();
                    // Оставить службу в действии до тех пор, пока не будет нажата клавиша <Enter>. 
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press the Enter key to terminate service.");
                Console.ReadLine();
            } 

        }

    }
}

