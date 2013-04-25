using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Filters.Primitive
{
    public class Range
    {
        private int _start = 0;
        private int _stop = int.MaxValue;
        public int Start
        {
            get { return _start; }
        }

        public int Stop
        {
            get { return _stop; }
        }

        public Range(int start, int stop)
        {
            _start = start;
            _stop = stop;  
        }

        public Range(string range)
        {
            if (string.IsNullOrEmpty(range)) return;
            bool correct = true;
            string[] r = range.Split(new char[] { '-' });
            if (r.Length != 2)
            {
                correct = false;
            }
            int start = 0;
            int stop = int.MaxValue;
            if (correct)
            {
                correct = int.TryParse(r[0], out start);
            }
            if (correct)
            {
                correct = int.TryParse(r[1], out stop);
            }
            if (!correct)
            {
                throw new ArgumentException("Range: range is in incorrect format");
            }

            _start = start;
            _stop = stop;  
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", _start, _stop);
        }
    }
}
