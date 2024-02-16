using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp;
using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf.draw;
using System.Data;
using System.Xml.Linq;
using System.Configuration;

namespace As_PDF
{
    public class Pdf
    {
        public static void CrearCertificacionHoras(string clienteResponsable, string razonSocial, string direccion, string localidad, string provincia, string leyenda, DataTable dtData, string documentName)
        {
            //Creamos el docuento
            Document doc = new Document(PageSize.A4);

            // Indicamos donde vamos a guardar el documento
            string _pdfFile = string.Format("{0}\\{1}", ConfigurationManager.AppSettings["pdfFile"], documentName);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(_pdfFile, FileMode.Create));
            writer.PageEvent = new HedearFooter();
            


            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Certificación de Horas");
            doc.AddCreator("Alusoft");

            // Abrimos el archivo
            doc.Open();

            addTitulo(doc, "Certificación de horas");

            addEncabezado(doc, clienteResponsable, razonSocial, direccion, localidad, provincia);

            addFecha(doc);

            addLeyenda(doc, leyenda);

            addTable(doc, dtData);

            doc.Close();

            writer.Close();
        }

        static void addTitulo(Document doc, string titulo)
        {
            Font _fontTitulo = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLDITALIC, BaseColor.BLACK);
            //Titulo
            var parrafo2 = new Paragraph(new Phrase(titulo.ToUpper(), _fontTitulo));
            parrafo2.SpacingBefore = 0;
            parrafo2.SpacingAfter = 20;
            parrafo2.Alignment = 2; //0-Left, 1 middle,2 Right
            doc.Add(parrafo2);
            doc.Add(Chunk.NEWLINE);
        }

        static void addFecha(Document doc)
        {
            Font _font = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, new BaseColor(0, 32, 99));
            //Titulo
            var parrafo2 = new Paragraph(new Phrase(DateTime.Now.Date.ToLongDateString(), _font));
            parrafo2.SpacingBefore = 5;
            parrafo2.SpacingAfter = 5;
            parrafo2.Alignment = 2; //0-Left, 1 middle,2 Right
            doc.Add(parrafo2);
            doc.Add(Chunk.NEWLINE);
        }

        static void addEncabezado(Document doc, string clienteResponsable, string razonSocial, string direccion, string localidad, string provincia)
        {
            Font _fontEncabezado = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, new BaseColor(0, 32, 99));
            Font _fontDireccion = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.GRAY);

            PdfPTable _tEncabezado = new PdfPTable(2);
            _tEncabezado.WidthPercentage = 80;
            _tEncabezado.HorizontalAlignment = 1;


            PdfPCell _colEmpty = new PdfPCell(new Phrase("", _fontEncabezado));
            _colEmpty.BorderWidth = 0;

            //Escribe razon social y resposable
            bool _escribeRazonSocial = true;

            foreach (var _resp in clienteResponsable.Split(char.Parse(",")))
            {
                PdfPCell _cResponsable = new PdfPCell(new Phrase(_resp.Trim(), _fontEncabezado));
                _cResponsable.BorderWidth = 0;

                PdfPCell _cRazonSocial = new PdfPCell(new Phrase(razonSocial.ToUpper(), _fontEncabezado));
                _cRazonSocial.BorderWidth = 0;

                _tEncabezado.AddCell(_cResponsable);
                if (_escribeRazonSocial)
                    _tEncabezado.AddCell(_cRazonSocial);
                else
                    _tEncabezado.AddCell(_colEmpty);

                _escribeRazonSocial = false;
            }

            //Escribe la Direccion
            PdfPCell _cDireccion = new PdfPCell(new Phrase(direccion, _fontDireccion));
            _cDireccion.BorderWidth = 0;

            PdfPCell _cLocalidad = new PdfPCell(new Phrase(string.Format("{0} {1}", localidad, provincia), _fontDireccion));
            _cLocalidad.BorderWidth = 0;


            _tEncabezado.AddCell(_colEmpty);
            _tEncabezado.AddCell(_cDireccion);
            _tEncabezado.AddCell(_colEmpty);
            _tEncabezado.AddCell(_cLocalidad);

            doc.Add(_tEncabezado);
            addSeparador(doc);

        }

        static void addLeyenda(Document doc, string leyenda)
        {
            Font _font = new Font(Font.FontFamily.HELVETICA, 9, Font.ITALIC, BaseColor.GRAY);

            var parrafo2 = new Paragraph(new Phrase(leyenda, _font));
            parrafo2.SpacingBefore = 5;
            parrafo2.SpacingAfter = 5;
            parrafo2.Alignment = 0; //0-Left, 1 middle,2 Right            
            doc.Add(parrafo2);
            doc.Add(Chunk.NEWLINE);
        }

        static void addTable(Document doc, DataTable data)
        {
            Font _fontHeader = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE);
            Font _fontRows = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.GRAY);

            //Crea la tabla
            PdfPTable tbl = new PdfPTable(data.Columns.Count);
            tbl.WidthPercentage = 100;

            
            //Escribe los Headers
            foreach (DataColumn _cName in data.Columns)
            {
                PdfPCell colName = new PdfPCell(new Phrase(_cName.ColumnName, _fontHeader));
                colName.BorderWidth = 0.01f;
                colName.BorderWidthBottom = 0.01f;
                colName.BackgroundColor = new BaseColor(68, 114, 196);


                if (_cName.ColumnName.ToLower().Contains("fecha") || _cName.DataType == typeof(int))
                    colName.HorizontalAlignment = 1;
                else
                    colName.HorizontalAlignment = 0;

                tbl.AddCell(colName);
            }
           
            int Index = 0;
            //Escribe los Rows
            foreach (DataRow _r in data.Rows)
            {
                Index++;
                foreach (DataColumn _col in data.Columns)
                {
                    //arma salto de pagina
                    if (Index > 30)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            PdfPCell colSalto = new PdfPCell(new Phrase(" ", _fontRows));
                            colSalto.BorderWidth = 0;
                            colSalto.BorderWidthBottom = 0;
                            tbl.AddCell(colSalto);
                        }                        
                    }
                    else
                    {
                        PdfPCell colName = new PdfPCell(new Phrase(_r[_col].ToString(), _fontRows));
                        colName.BorderWidth = 0.01f;
                        colName.BorderWidthBottom = 0.01f;
                        if (_col.ColumnName.ToLower().Contains("fecha") || _col.DataType == typeof(int))
                            colName.HorizontalAlignment = 1;
                        else
                            colName.HorizontalAlignment = 0;

                        tbl.AddCell(colName);
                    }
                }
                if (Index > 30)
                {                   
                    Index = 0;
                }
            }

            doc.Add(tbl);

          
        }
        static void addSeparador(Document doc)
        {
            Font _fontEncabezado = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, new BaseColor(0, 32, 99));
            var Encabezado = new Paragraph(new Phrase("", _fontEncabezado));

            iTextSharp.text.pdf.draw.LineSeparator lineseparator = new iTextSharp.text.pdf.draw.LineSeparator();
            lineseparator.LineColor = new BaseColor(0, 32, 99);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = lineseparator;

            Encabezado.Add(seperator);

            doc.Add(Encabezado);
            doc.Add(Chunk.NEWLINE);
        }

    }

    class HedearFooter : PdfPageEventHelper
    {
        public string titulo { get; set; }
        public override void OnEndPage(PdfWriter writer, Document document)
        {

            Image img = Image.GetInstance(string.Format("{0}\\{1}",ConfigurationManager.AppSettings["headerFile"],"Encabezado.jpg"));
            Image imgPie = Image.GetInstance(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["headerFile"], "pie.jpg")); 
            Image imgMarca = Image.GetInstance(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["headerFile"], "marcaAgua.jpg")); 

            //Encabezado Pagina
            img.ScaleToFit(610f, 900F);
            //Imagen - Esquina inferior izquierda
            img.SetAbsolutePosition(0, 750);
            document.Add(img);

            //Pie Marca Agua
            imgMarca.ScaleToFit(400f, 400F);
            //Imagen - Esquina inferior izquierda
            imgMarca.SetAbsolutePosition(80, 200);
            document.Add(imgMarca);

            //Pie Pagina
            imgPie.ScaleToFit(610f, 900F);
            //Imagen - Esquina inferior izquierda
            imgPie.SetAbsolutePosition(0, 0);
            document.Add(imgPie);

        }
    }
}
