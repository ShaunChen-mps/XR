using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelManager
{
    [Serializable]
    public class Register
    {
        public Register(bool isRom, string name, string address, int byteCount, int bitCount, ushort page, int value, int cfgN)
        {
            this.IsRom = isRom;
            this.Name = name;
            this.Address = address;
            this.ByteCount = byteCount;
            this.BitsCount = bitCount;
            this.Page = page;
            this.RegisterValue = value;
            this.CfgN = cfgN;
        }
        [Excel("ROM", "Register Name", 0)]
        [Excel("RAM", "Register Name", 0)]
        [Description("寄存器名称")]
        [XmlAttribute]
        public string Name { get; set; }

        [Excel("ROM", "Register Address", 2)]
        [Excel("RAM", "Register Address", 2)]
        [Description("寄存器地址")]
        [XmlAttribute]
        public string Address { get; set; }

        [Excel("RAM", "Register Byte Count", 3)]
        [Description("寄存器Byte长度")]
        [XmlAttribute]
        public int ByteCount { get; set; }

        [Excel("ROM", "Register Bit Count", 3)]
        [Description("寄存器Bit长度")]
        [XmlAttribute]
        public int BitsCount { get; set; }

        [Description("寄存器读写访问类型")]
        [XmlAttribute]
        public string RW { get; set; }

        [Excel("ROM", "Register Page", 1)]
        [Excel("RAM", "Register Page", 1)]
        [Description("寄存器的Page信息")]
        [XmlAttribute]
        public ushort Page { get; set; }

        [Description("寄存器的相关描述")]
        [XmlAttribute]
        public string Description { get; set; }

        [Excel("RAM", "Register Value", 4)]
        [Description("寄存器值")]
        [XmlAttribute]
        public int RegisterValue { get; set; }


        [Description("寄存器是否需要导出")]
        [XmlAttribute]
        public bool IsExport { get; set; }

        [Description("寄存器是否属于ROM")]
        [XmlAttribute]
        //public bool IsROM { get; set; }
        public bool IsRom { get; set; }

        [Description("寄存器是否属于Monitor")]
        [XmlAttribute]
        public bool IsMonitor { get; set; }

        [Description("BitControls中是否有OTP的寄存器")]
        [XmlAttribute]
        public bool IsContainsOTPBits { get; set; }

        [Description("寄存器所属类型")]
        private string _Category;
        [XmlAttribute]
        public string CategoryType { get; set; }
        [Excel("ROM", "Register Cfg", 4)]
        public int CfgN { get; set; }

    }
}
