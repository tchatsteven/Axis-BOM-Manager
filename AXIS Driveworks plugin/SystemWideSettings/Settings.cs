using System;
using System.IO;

namespace AASettings
{
    public static class ProjectRootPaths
    {
        public static String FixtureRepository = @"\\fs\data\RESEARCH + DEVELOPMENT\6 DW\Specifications\Fixture Repository";  
    }

    public static class ShopDrawings
    {
        public static String GeneratedSDDocumentationRootFolder = @"\\fs\data\Groups\Shop Drawing Department\8 - DriveWorks Specifications";
        public static String OrdersRootFolder = Path.Combine(GeneratedSDDocumentationRootFolder, "Orders");
        public static String IndividualFixturesRootFolder = Path.Combine(GeneratedSDDocumentationRootFolder, "Single Fixtures");
        public static String FallbackDrawingImage = @"C:\Axis Vault\7-Driveworks\Group Content\Images\Errors\ShopDrawingNotFound.pdf";

        public static String GetAssembledOrderPDFPath(String SalesOrderNumber)
        {
            return Path.Combine(OrdersRootFolder, SalesOrderNumber, String.Format(@"{0} Shop Drawings.PDF", SalesOrderNumber));
        }

        public static class ImagePaths
        {
            public static String AXISLogo = @"C:\Axis Vault\7-Driveworks\Group Content\Images\Logos\General\AXIS Detailed Logo.ai";
        }
    }

    public static class InstallationSheets
    {
        public static String RootFolder = @"C:\Axis Vault\7-Driveworks\Group Content\Installation Sheets";
        public static String GetFullPath(String InstallSheetFileName)
        {
            try
            {
                return Path.Combine(RootFolder, InstallSheetFileName);
            }
            catch(ArgumentNullException Err)
            {
                return null;
            }
        }
    }

    public static class ImagePaths
    {       
        public static String AXISLogo = @"C:\Axis Vault\7-Driveworks\Group Content\Images\Logos\General\AXIS Detailed Logo.ai";
        public static String FixtureLogosRootPath = @"C:\Axis Vault\7-Driveworks\Group Content\Images\Logos\Products";
        

        public static class HeaderImages
        {
            public static String Root = @"C:\Axis Vault\7-Driveworks\Group Content\Images\HeaderImages";
            public static String ErrorImagesFolder = Path.Combine(Root, "Errors");
            public static String FallbackHeaderImageFileName = @"RequestedImageDoesNotExist.ai";
            public static String FallbackHeaderImageFullpath = Path.Combine(ErrorImagesFolder, FallbackHeaderImageFileName);
            public static String WiringDiagramsRootPath = @"C:\Axis Vault\7-Driveworks\Group Content\Images\HeaderImages\Wiring Diagrams";
            public static String WiringFallbackDiagramFileName = @"WiringDiagramNotFound.ai";
            public static String WiringDiagramFallbackImageFullpath = Path.Combine(WiringDiagramsRootPath, WiringFallbackDiagramFileName);
        }
    }

    public static class DWSpecifications
    {
        public static String RootPath = @"\\fs\data\RESEARCH + DEVELOPMENT\6 DW\Specifications";
        private static String FixtureRepositoryFolderName = "Fixture Repository";
        public static String FixtureRepositoryRootPath = Path.Combine(RootPath, FixtureRepositoryFolderName);
    }
}