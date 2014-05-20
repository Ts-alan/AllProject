using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public struct DEVICE_INFO
    {
        public UInt32 size;

        // = new Byte[8]
        public Byte[] time;

        public Byte mount;
        // = 0
        public Byte insert;

        public Byte dev_class;
        public Byte dev_subclass;
        public Byte dev_protocol;

        public UInt16 id_vendor;
        public UInt16 id_product;

        public UInt16 manufacturer_length;
        public UInt16 product_length;
        public UInt16 serial_number_length;
        public String strings;
    }

    public struct CLASS_INFO
    {
        public Byte dev_class;
        public Byte mount;
    }
}
