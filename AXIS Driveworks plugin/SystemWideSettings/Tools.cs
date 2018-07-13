using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AAValidation
{
    public static class Validation
    {
        public static class SO
        {
            public static Boolean IsValid(String soNumber)
            {
                return new Regex(@"^\d{5,6}$").IsMatch(soNumber);
            }
        }        

        public static class FileNaming
        {
            public static String ReplaceInvalidCharactersWithUnderscore(String FilePath)
            {
                return Regex.Replace(FilePath, @"[<>:""\/|?*]", "_");
            }
            public static Boolean FilenameIsValid(String FileName, String ConstrainingExtension = "")
            {
                if (Regex.IsMatch(FileName, @"[<>:""/\\|?*]")) { throw new AAExceptions.InvalidCharacterInFileName(); }
                String ExtensionMatch = String.Format(@"{0}$", ConstrainingExtension);
                if (!Regex.IsMatch(FileName, ExtensionMatch, RegexOptions.IgnoreCase)) { throw new AAExceptions.InvalidExtensionForRequestedFileType(ConstrainingExtension); }

                return true;
            }
        }
    }
}

namespace Tools
{
    public static class RTF
    {
        public static String EscapeSlashes(String InputString)
        {
            if(InputString== null) { return null; }
            return Regex.Replace(InputString, @"\\", @"\\");
        }
    }
}
