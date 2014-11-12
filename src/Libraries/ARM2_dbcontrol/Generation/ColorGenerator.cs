using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Collections;

namespace ARM2_dbcontrol.Generation
{
    /// <summary>
    /// Создает набор цветов
    /// </summary>
    public class ColorGenerator
    {
        public ColorGenerator()
        {

        }

        public List<string> GetMainColorList()
        {
            List<string> list = new List<string>();
            list.Add(Color.Blue.Name);
            list.Add(Color.Red.Name);
            list.Add(Color.Green.Name);
            list.Add(Color.WhiteSmoke.Name);
            list.Add(Color.Yellow.Name);
            list.Add(Color.Tomato.Name);
            list.Add(Color.SteelBlue.Name);
            list.Add(Color.Silver.Name);
            list.Add(Color.SeaGreen.Name);
            list.Add(Color.Purple.Name);
            return list;
        }

        public List<string> GetFinalColorList()
        {
            string[] allColors = Enum.GetNames(typeof(System.Drawing.KnownColor));
            string[] systemEnvironmentColors =
                new string[(
                typeof(System.Drawing.SystemColors)).GetProperties().Length];

            int index = 0;

            foreach (MemberInfo member in (
                typeof(System.Drawing.SystemColors)).GetProperties())
            {
                systemEnvironmentColors[index++] = member.Name;
            }

            List<string> finalColorList = new List<string>();

            foreach (string color in allColors)
            {
                if (Array.IndexOf(systemEnvironmentColors, color) < 0)
                {
                    finalColorList.Add(color);
                }
            }

            return finalColorList;
        }
    }
}
