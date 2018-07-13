using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAExceptions
{
    public class InvalidSONumberSyntax : Exception
    {
        public InvalidSONumberSyntax()
            : base(message: "The requested SO number syntax is invalid.")
        {
        }

        public InvalidSONumberSyntax(string RequestString)
            : base(message: String.Format("The requested SO number syntax, {0}, is invalid.", RequestString))
        {   
        }
    }

    public class InvalidCharacterInFileName : Exception
    {
        public InvalidCharacterInFileName()
            : base(message: "<, >, :, \", \\, /, |, ?, *  cannot be used in a filename.")
        {
        }

        //public InvalidCharacterInFileName(string RequestString)
        //    : base(message: String.Format("The requested SO number syntax, {0}, is invalid.", RequestString))
        //{
        //}
    }

    public class InvalidExtensionForRequestedFileType : Exception
    {
        public InvalidExtensionForRequestedFileType()
            : base(message: "<, >, :, \", \\, /, |, ?, *  cannot be used in a filename.")
        {
        }

        public InvalidExtensionForRequestedFileType(string RequestedExtensionType)
            : base(message: String.Format("The filename does not have the correct extension for the requested file type. {0}", RequestedExtensionType))
        {
        }
    }

    public class NoPACAFAssociationWithInstallationSheet : Exception
    {
        public NoPACAFAssociationWithInstallationSheet()
            : base(message: "Selection from Parsed FixtureSetupCode has no associations with a specific Installation Sheet.")
        {
        }
        public NoPACAFAssociationWithInstallationSheet(String MountingParameterCode, String FixtureCode)
            : base(message: String.Format(@"{0} from Mounting-type category of {1} has no associations with a specific Installation Sheet.",MountingParameterCode, FixtureCode))
        {
        }
    }

    public class CategoryTypeNotPresent : Exception
    {
        public CategoryTypeNotPresent(String CategoryType)
            : base(message: String.Format("FixtureSetupCode Parsing result contains no category of type {0}", CategoryType))
        {
        }

        public CategoryTypeNotPresent()
            : base(message: "FixtureSetupCode Parsing result contains no category of the requested type.")
        {
        }
    }

    public class NoFixtureAssociationWithInstallationSheet : Exception
    {
        public NoFixtureAssociationWithInstallationSheet()
            : base(message: "No Installation sheet is associated with requested fixture code.")
        {
        }
        public NoFixtureAssociationWithInstallationSheet(String FixtureCode)
            : base(message: String.Format(@"Fixture code {0} has no associations with a specific installation sheet.", FixtureCode))
        {
        }
    }
}
