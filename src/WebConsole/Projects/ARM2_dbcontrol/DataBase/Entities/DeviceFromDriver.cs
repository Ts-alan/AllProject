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

        public CLASS_INFO(Byte _class, Byte _mount)
        {
            dev_class = _class;
            mount = _mount;
        }
    }

    public struct NET_DEVICE_INFO
    {
        public UInt32 size;

        public Byte[] time;

        public Byte mount;
        public Byte insert;

        public UInt16 dev_descr_length;
        public UInt16 hardware_id_length;
        public UInt16 friendly_name_length;
        public String strings;
    }
}
