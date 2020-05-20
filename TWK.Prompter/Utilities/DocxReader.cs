/*  
 *  Source: https://www.codeproject.com/Articles/649064/Show-Word-File-in-WPF
 *  Author: Mario Z 
 *  License: CPOL (https://www.codeproject.com/info/cpol10.aspx)
 */

using System;
using System.IO;
using System.IO.Packaging;
using System.Xml;

namespace TWK.Prompter.Utilities
{

    public class DocxReader : IDisposable
    {
        protected const string

            MainDocumentRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument",

            // XML namespaces
            WordprocessingMLNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main",
            RelationshipsNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships",

            // Miscellaneous elements
            DocumentElement = "document",
            BodyElement = "body",

            // Block-Level elements
            ParagraphElement = "p",
            TableElement = "tbl",

            // Inline-Level elements
            SimpleFieldElement = "fldSimple",
            HyperlinkElement = "hyperlink",
            RunElement = "r",

            // Run content elements
            BreakElement = "br",
            TabCharacterElement = "tab",
            TextElement = "t",

            // Table elements
            TableRowElement = "tr",
            TableCellElement = "tc",

            // Properties elements
            ParagraphPropertiesElement = "pPr",
            RunPropertiesElement = "rPr";
        // Note: new members should also be added to nameTable in CreateNameTable method.

        protected virtual XmlNameTable CreateNameTable()
        {
            var nameTable = new NameTable();

            nameTable.Add(WordprocessingMLNamespace);
            nameTable.Add(RelationshipsNamespace);
            nameTable.Add(DocumentElement);
            nameTable.Add(BodyElement);
            nameTable.Add(ParagraphElement);
            nameTable.Add(TableElement);
            nameTable.Add(ParagraphPropertiesElement);
            nameTable.Add(SimpleFieldElement);
            nameTable.Add(HyperlinkElement);
            nameTable.Add(RunElement);
            nameTable.Add(BreakElement);
            nameTable.Add(TabCharacterElement);
            nameTable.Add(TextElement);
            nameTable.Add(RunPropertiesElement);
            nameTable.Add(TableRowElement);
            nameTable.Add(TableCellElement);

            return nameTable;
        }

        private readonly Package package;
        private readonly PackagePart mainDocumentPart;

        protected PackagePart MainDocumentPart
        {
            get { return mainDocumentPart; }
        }

        public DocxReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            package = Package.Open(stream, FileMode.Open, FileAccess.Read);

            foreach (var relationship in package.GetRelationshipsByType(MainDocumentRelationshipType))
            {
                mainDocumentPart = package.GetPart(PackUriHelper.CreatePartUri(relationship.TargetUri));
                break;
            }
        }

        public void Read()
        {
            using (var mainDocumentStream = mainDocumentPart.GetStream(FileMode.Open, FileAccess.Read))
            using (var reader = XmlReader.Create(mainDocumentStream, new XmlReaderSettings()
            {
                NameTable = CreateNameTable(),
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            }))
                ReadMainDocument(reader);
        }

        private static void ReadXmlSubtree(XmlReader reader, Action<XmlReader> action)
        {
            using (var subtreeReader = reader.ReadSubtree())
            {
                // Position on the first node.
                subtreeReader.Read();

                if (action != null)
                    action(subtreeReader);
            }
        }

        private void ReadMainDocument(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == DocumentElement)
                {
                    ReadXmlSubtree(reader, ReadDocument);
                    break;
                }
        }

        protected virtual void ReadDocument(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == BodyElement)
                {
                    ReadXmlSubtree(reader, ReadBody);
                    break;
                }
        }

        private void ReadBody(XmlReader reader)
        {
            while (reader.Read())
                ReadBlockLevelElement(reader);
        }

        private void ReadBlockLevelElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == WordprocessingMLNamespace)
                    switch (reader.LocalName)
                    {
                        case ParagraphElement:
                            action = ReadParagraph;
                            break;

                        case TableElement:
                            action = ReadTable;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }

        protected virtual void ReadParagraph(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == ParagraphPropertiesElement)
                    ReadXmlSubtree(reader, ReadParagraphProperties);
                else
                    ReadInlineLevelElement(reader);
            }
        }

        protected virtual void ReadParagraphProperties(XmlReader reader)
        {

        }

        private void ReadInlineLevelElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == WordprocessingMLNamespace)
                    switch (reader.LocalName)
                    {
                        case SimpleFieldElement:
                            action = ReadSimpleField;
                            break;

                        case HyperlinkElement:
                            action = ReadHyperlink;
                            break;

                        case RunElement:
                            action = ReadRun;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }

        private void ReadSimpleField(XmlReader reader)
        {
            while (reader.Read())
                ReadInlineLevelElement(reader);
        }

        protected virtual void ReadHyperlink(XmlReader reader)
        {
            while (reader.Read())
                ReadInlineLevelElement(reader);
        }

        protected virtual void ReadRun(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == RunPropertiesElement)
                    ReadXmlSubtree(reader, ReadRunProperties);
                else
                    ReadRunContentElement(reader);
            }
        }

        protected virtual void ReadRunProperties(XmlReader reader)
        {

        }

        private void ReadRunContentElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == WordprocessingMLNamespace)
                    switch (reader.LocalName)
                    {
                        case BreakElement:
                            action = ReadBreak;
                            break;

                        case TabCharacterElement:
                            action = ReadTabCharacter;
                            break;

                        case TextElement:
                            action = ReadText;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }

        protected virtual void ReadBreak(XmlReader reader)
        {

        }

        protected virtual void ReadTabCharacter(XmlReader reader)
        {

        }

        protected virtual void ReadText(XmlReader reader)
        {

        }

        protected virtual void ReadTable(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == TableRowElement)
                    ReadXmlSubtree(reader, ReadTableRow);
        }

        protected virtual void ReadTableRow(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == WordprocessingMLNamespace && reader.LocalName == TableCellElement)
                    ReadXmlSubtree(reader, ReadTableCell);
        }

        protected virtual void ReadTableCell(XmlReader reader)
        {
            while (reader.Read())
                ReadBlockLevelElement(reader);
        }

        public void Dispose()
        {
            package.Close();
        }
    }
}