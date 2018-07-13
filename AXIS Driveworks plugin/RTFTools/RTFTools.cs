using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTF
{
    public static class Tools
    {
        private static class Style1
        {
            public static String RTFHeader = @"{\rtf1\ansi\fs20{{\colortbl;\red255\green118\blue0;\red255\green0\blue0;\red189\green174\blue137;}";
        }

        public static String FormatAsRTF(String InputText, String RTFStyle = "Style1")
        {
            String RTFHeader;
            switch (RTFStyle)
            { 
                default:
                    RTFHeader = Style1.RTFHeader;
                    break;
            }

            return String.Format(@"{0}{1}}}", RTFHeader, InputText);
        }

    }
}
