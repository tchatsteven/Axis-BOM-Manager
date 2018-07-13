using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using System.Xml.XPath;
using System.Linq;
using System.Xml.Linq;
using ShopDrawingPDFAssembler.Settings;
using System.Text.RegularExpressions;

namespace PDFGeneration
{  
    public class _SpecSheet
    {
        PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
        Document SpecSheetDocument = new Document(); // this variable stores the Shop Drawing PDF documment object.
        //String TargetPath;


        //DocumentPages
        List<Section> SectionsList = new List<Section>();


        public _SpecSheet(String productCode)
        {
            //if (IsSoNumber(SONumberOrFixtureSetupCode)) { ProcessAsOrder(SONumberOrFixtureSetupCode); } else { ProcessAsSingleFixture(SONumberOrFixtureSetupCode); };
        }

        private Boolean Is(String InputString)
        {
           return new Regex(@"^\d{5,6}$", RegexOptions.IgnoreCase).IsMatch(InputString);
        }

        private void ProcessAsOrder(String SONumber)
        {
            //create new order data object. we pass the SO number, class gets data from EPICOR
            order = new _PDFAssemblerOrder(SONumber);

            //Solve BOM Pages
            GenerateBOMPages(order);

            //Start solving all fixtures
            order.SolveAllFixtures();

            //Add BOM Pages. This is out of order as we need the order fixtures to be solved so that we can accurately number the pages in the BOM
            AddBOMPages();

            //Add All Shop Drawing Pages To Pages List
            order.OrderLines.ForEach(o => SectionsList.AddRange(o.ShopDrawing.Pages));            

            //Add All pages from pages list to document;
            AssembleDocument();

            //Get targetFileName
            TargetPath = Path.Combine(Settings.Paths.GeneratedShopDrawingOrderRoot, SONumber, String.Format(@"{0} Shop Drawings.PDF", SONumber));

            //SaveFile
            Save(TargetPath);

            //add all installation sheet pages to pages list
            AddInstallationSheets(TargetPath);
        }

        private void ProcessAsSingleFixture(String FixtureSetupCode)
        {
            //create new order data object. Since we are generating a single fixture, there is really no context of order here, bt we fake the order data with defaults.
            order = new _PDFAssemblerOrder();

            //add single FixtureSetupCode To Order Data object
            order.AddLine(FixtureSetupCode);            

            //Start solving all fixtures
            order.SolveAllFixtures();

            //Add All Shop Drawing Pages To Pages List
            order.OrderLines.ForEach(o => SectionsList.AddRange(o.ShopDrawing.Pages));

            //Add All pages from pages list to document;
            AssembleDocument();
            String ProductID = order.OrderLines[0].ProductID;

            //Get targetFileName
            //TargetPath = Path.Combine(Settings.Paths.GeneratedSingleFixtureShopDrawing, ProductID, FixtureSetupCode, @"Shop Drawings.PDF");
            TargetPath = order.OrderLines[0].fixtureSetup.ShopDrawingData.FilePath;

            //SaveFile
            if (TargetPath != null) { Save(TargetPath); };
        }

        private void GenerateBOMPages(_PDFAssemblerOrder order)
        {
            bomPages = new BomPages(order);
        }

        private void AddBOMPages()
        {
            bomPages.FillLineNumbering();
            SectionsList.AddRange(bomPages.Pages);
        }

        private void AddInstallationSheets(String MainSDPDFFilePath)
        {
            order.OrderLines.ForEach(o => InstallationSheetFullPaths.Add(o.InstallationSheetFullPath));
            PdfDocument TargetSDDoc = PdfReader.Open(MainSDPDFFilePath, PdfDocumentOpenMode.Modify);

            foreach (String InstallationSheetFullPathName in InstallationSheetFullPaths.Distinct().Where(o=> o != null))
            {                
                if (File.Exists(InstallationSheetFullPathName))
                {
                    PdfDocument SourceDoc = PdfReader.Open(InstallationSheetFullPathName, PdfDocumentOpenMode.Import);

                    foreach (PdfPage CurrentPage in SourceDoc.Pages)
                    {
                        TargetSDDoc.AddPage(CurrentPage);
                    }
                }                
            }

            TargetSDDoc.Save(MainSDPDFFilePath);
        }

        private void AssembleDocument()
        {
            foreach(Section currentSection in SectionsList )
            {
                ShopDrawingPDF.Add(currentSection);
            }
        }       

        public void RenderDoc()
        {
            pdfRenderer.Document = ShopDrawingPDF;
            pdfRenderer.RenderDocument();
        }

        public void Save(String targetPath)
        {
            RenderDoc();

            String FolderPath = new System.IO.FileInfo(targetPath).DirectoryName;
            Boolean FolderExists = Directory.Exists(FolderPath);

            if (!FolderExists) { Directory.CreateDirectory(FolderPath); }

            pdfRenderer.PdfDocument.Save(targetPath);
        }

        public void Open(String targetFilePath)
        {
            if (targetFilePath!=null) { Process.Start(targetFilePath);  }
        }
    }    
}
