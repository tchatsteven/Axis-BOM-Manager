using System;

namespace AASettings
{ 
    public static class RichTextFormat
    {
        private static String Header = @"{\rtf1\ansi \fs20 {\colortbl
;
\red255\green118\blue0;
\red255\green0\blue0;}\cf0 ";

        public static String AddRTFHeader(String Input)
        {
            return String.Format(@"{0}{1} }}", Header, Input);
        }
    }


}
