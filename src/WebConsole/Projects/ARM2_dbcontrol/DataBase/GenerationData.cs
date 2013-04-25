using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace ARM2_dbcontrol.DataBase
{
    class GenerationData
    {
        private string  prefix;
        private string  suffix;
        private int     starting;
        private int     count;

        public GenerationData(string prefix, string suffix, int starting, int count)
        {

        }

        public List<string> GenerateComputerName()
        {
            throw new Exception("Method GenerateComputerName() is empty!");
        }

        public List<string> GenerateLogin()
        {
            throw new Exception("Method GenerateLogin() is empty!");
        }

        public List<string> GenerateIP()
        {
            throw new Exception("Method GenerateIP() is empty!");
        }
    }
}
