using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32CC
{
    static class Anchor
    {
        private static string FromBase64String(string source)
        {
            byte[] bs = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(bs);
        }

        public static string GetCommentFromSerial(string serial)
        {
            string comment = "";
            try
            {
                comment = FromBase64String(serial);
            }
            catch
            {
            }
            return comment;
        }
    }
}
