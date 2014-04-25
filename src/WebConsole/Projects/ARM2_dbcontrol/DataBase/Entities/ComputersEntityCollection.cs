using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ComputersEntityCollection: List<ComputersEntity>
    {
        public ComputersEntityCollection()
        {            
        }

        public List<String> GetComputerNames()
        {
            List<String> list = new List<String>();
            foreach (ComputersEntity comp in this)
            {
                list.Add(comp.ComputerName);
            }
            return list;
        }

        public List<String> GetIPAddresses()
        {
            List<String> list = new List<String>();
            foreach (ComputersEntity comp in this)
            {
                list.Add(comp.IPAddress);
            }
            return list;
        }

        public List<String> GetOSNames()
        {
            List<String> list = new List<String>();
            foreach (ComputersEntity comp in this)
            {
                list.Add(comp.OSName);
            }
            return list;
        }

        public List<String> GetVbaVersions()
        {
            List<String> list = new List<String>();
            foreach (ComputersEntity comp in this)
            {
                list.Add(comp.Vba32Version);
            }
            return list;
        }
    }
}
