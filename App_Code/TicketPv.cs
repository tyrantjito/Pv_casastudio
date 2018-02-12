﻿using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.io;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Descripción breve de TicketPv
/// </summary>
public class TicketPv
{
    BaseDatos ejecuta = new BaseDatos();
    ImprimeTicket detTicket = new ImprimeTicket();
    int _ticket;
    private string _fecha;
    private string _hora;
    int _pv;
    int _caja;
    string _usuario, _forma, _refe, _ban, _encabezado, _notas, _notasTicket,_cliente,_correo;
    decimal _sub, _tot, _porc_dctoGral, _iva, _porcIva,_montoDecto;
    bool _esVtaCredito;

	public TicketPv()
	{
        _ticket = 0;
        _pv = 0;
        _caja = 0;
        _sub = _tot = _porc_dctoGral = _iva = _porcIva = _montoDecto = 0;
        _usuario = _refe = _forma = _ban = _fecha = _hora = _encabezado = _notas = _notasTicket = string.Empty;
        _esVtaCredito = false;
	}

    public int Ticket{        
        set { _ticket = value; }
        get { return _ticket; }
    }


    public string Cliente
    {
        set { _cliente = value; }
        get { return _cliente; }
    }

    public string Correo
    {
        set { _correo = value; }
        get { return _correo; }
    }

    public int PuntoVenta
    {
        set { _pv = value; }
        get { return _pv; }
    }

    public int Caja
    {
        set { _caja = value; }
        get { return _caja; }
    }

    

    public string GenerarTicket(bool esVtaTaller)
    {

        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteBb = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente6 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("Ticket de Compra");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Ticket_" + _pv.ToString() + "_" + _ticket.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }


        if (archivo.Trim() != "")
        {
            FileInfo info = new FileInfo(archivo);
            if (info.Exists)
                info.Delete();

            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            //Insertar logo o imagen            
            string rutaLogo = HttpContext.Current.Server.MapPath("~/img/logo.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
            logo.ScaleToFit(100, 50);
            logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

            PdfPTable enc1 = new PdfPTable(3);
            enc1.DefaultCell.Border = 0;
            enc1.WidthPercentage = 100f;
            int[] enc1cellwidth = { 20, 40, 40 };
            enc1.SetWidths(enc1cellwidth);


            PdfPCell img1 = new PdfPCell(logo);
            img1.Border = 0;
            img1.HorizontalAlignment = Element.ALIGN_CENTER;
            enc1.AddCell(img1);


            PdfPCell enca = new PdfPCell(new Phrase("\n CASA STUDIO EUROPA SA DE CV \n BLEND CENTER DESIGN \n AVENIDA DE LAS PALMAS 520 \n PISO 1 LOCAL 102 \n TEL 55-5281-5381  / 4623", fuente));
            enca.HorizontalAlignment = Element.ALIGN_CENTER;

            enca.Border = 0;
            enc1.AddCell(enca);


            ImprimeTicket imp = new ImprimeTicket();
            imp.ticketc = _ticket;
            object[] camposcab = detTicket.obtieneenca(_ticket);
            DateTime fechav;

            if (Convert.ToBoolean(camposcab[0]))
            {
                DataSet datos = (DataSet)camposcab[1];


                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    fechav = Convert.ToDateTime(fila[0]);

                    PdfPCell enca2 = new PdfPCell(new Phrase("\n\n Fecha: " + Convert.ToDateTime(fechav).ToString("dd/MM/yyyy")+"\n Ejecutivo: \n Email: ", fuente6));
                    enca2.HorizontalAlignment = Element.ALIGN_LEFT;
                    enca2.Border = 0;
                    enc1.AddCell(enca2);

                }
            }

            documento.Add(enc1);
            documento.Add(new Paragraph(" "));

            PdfPTable enc2 = new PdfPTable(1);
            enc2.DefaultCell.Border = 0;
            enc2.WidthPercentage = 100f;
            int[] enc2cellwidth = { 50};
            enc2.SetWidths(enc2cellwidth);

            PdfPCell enca3 = new PdfPCell(new Phrase("Nombre del Cliente:"+_cliente+" \n Telefono :  \n Celular : \n Email:"+_correo+" \n", fuente6));
            enca3.HorizontalAlignment = Element.ALIGN_LEFT;
            enca3.Border = 0;
            enc2.AddCell(enca3);

            documento.Add(enc2);


            string noOrden = "";
                    string nomTaller = "";
                    string folio = "";
                    string cliente = "";
                    if (esVtaTaller)
                    {
                        object[] datosVtaTaller = detTicket.datosVtaTaller();
                        DataSet valVtaTaller = (DataSet)datosVtaTaller[1];
                        foreach (DataRow f in valVtaTaller.Tables[0].Rows)
                        {
                            folio = f[0].ToString();
                            noOrden = f[1].ToString();
                            nomTaller = f[2].ToString();
                            cliente = f[3].ToString();
                        }
                    }

                    object[] datosTicket = detTicket.datosTicket();
                    if (Convert.ToBoolean(datosTicket[0]))
                    {
                        DataSet valores = (DataSet)datosTicket[1];
                        DateTime fecha;
                        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        foreach (DataRow fila in valores.Tables[0].Rows)
                        {
                            try
                            {
                                fecha = Convert.ToDateTime(fila[0].ToString() + " " + fila[1].ToString());
                                _fecha = fecha.ToString("yyyy-MM-dd");
                                _hora = fecha.ToString("HH:mm:ss");
                            }
                            catch (Exception)
                            {
                                _fecha = fila[0].ToString();
                                _hora = fila[1].ToString().Substring(0, 10);
                            }

                            _usuario = fila[3].ToString();
                            _forma = fila[8].ToString();
                            _refe = fila[9].ToString();
                            _ban = fila[11].ToString();
                            _sub = Convert.ToDecimal(fila[4].ToString());
                            _iva = Convert.ToDecimal(fila[5].ToString());
                            _tot = Convert.ToDecimal(fila[6].ToString());
                            _porc_dctoGral = Convert.ToDecimal(fila[13].ToString()) / 100;
                            _montoDecto = Convert.ToDecimal(fila[14].ToString());
                            _porcIva = Convert.ToDecimal(fila[7].ToString());
                            _notasTicket = fila[12].ToString();
                            try { _esVtaCredito = bool.Parse(fila[15].ToString()); } catch (Exception) { _esVtaCredito = false; }
                            Encabezado(documento);

                            if (esVtaTaller)
                            {
                                Paragraph vtaTall = new Paragraph("Folio: " + folio.Trim() + " - Orden: " + noOrden + " - Cliente: " + cliente + " - Taller: " + nomTaller, fuente);
                                vtaTall.Alignment = Element.ALIGN_CENTER;
                                documento.Add(vtaTall);
                            }

                            Paragraph aten = new Paragraph("Atendio: " + _usuario.ToString(), fuente);
                            aten.Alignment = Element.ALIGN_CENTER;
                            documento.Add(aten);

                            if (_esVtaCredito)
                            {
                                Paragraph vtaCred = new Paragraph("-Venta a Crédito-", fuente);
                                vtaCred.Alignment = Element.ALIGN_CENTER;
                                documento.Add(vtaCred);
                            }

                            Paragraph pag = new Paragraph("Forma Pago: " + _forma.ToString(), fuente);
                            pag.Alignment = Element.ALIGN_CENTER;
                            documento.Add(pag);
                            if (_forma != "Efectivo" && _forma != "")
                            {
                                /*Paragraph refe = new Paragraph("Referencia Pago: " + _refe.ToString(), fuente);
                                refe.Alignment = Element.ALIGN_CENTER;
                                documento.Add(refe);*/
                                Paragraph banc = new Paragraph("Banco: " + _ban.ToString(), fuente);
                                banc.Alignment = Element.ALIGN_CENTER;
                                documento.Add(banc);
                            }
                        }


                        documento.AddCreationDate();
                        documento.Add(new Paragraph(" "));
                        documento.Add(new Paragraph(" "));
                        ProductosTicket(documento);

                        Paragraph lin = new Paragraph("--------------------------------------------------------", fuente);
                        lin.Alignment = Element.ALIGN_CENTER;
                        //documento.Add(lin);                
                        Paragraph subtotal = new Paragraph("ESTOY ENTERADO Y ACEPTO LAS OBSERVACIONES Y CLAUSULAS MENCIONADAS ", fuenteBb);
                        subtotal.Alignment = Element.ALIGN_CENTER;

                        Paragraph liva = new Paragraph("", fuenteB);
                        liva.Alignment = Element.ALIGN_CENTER;

                        documento.Add(new Paragraph(" "));
                        documento.Add(new Paragraph(" "));
                        Paragraph ltotal = new Paragraph("", fuenteT);
                        ltotal.Alignment = Element.ALIGN_CENTER;
                        Paragraph dctoGral = new Paragraph("__________________________________________________________________________", fuenteT);
                        dctoGral.Alignment = Element.ALIGN_CENTER;
                        documento.Add(dctoGral);
                        documento.Add(subtotal);
                        documento.Add(liva);
                        documento.Add(ltotal);

                        Pagos pagosTicket = new Pagos();
                        pagosTicket.ticket = _ticket;
                        pagosTicket.caja = _caja;
                        pagosTicket.punto = _pv;
                        pagosTicket.obtienePagos();
                        object[] datosTpagos = pagosTicket.retorno;
                        if (Convert.ToBoolean(datosTpagos[0]))
                        {
                            DataSet infoPago = (DataSet)datosTpagos[1];
                            foreach (DataRow fila in infoPago.Tables[0].Rows)
                            {

                                Paragraph pagf = new Paragraph("", fuente);
                                pagf.Alignment = Element.ALIGN_CENTER;
                                documento.Add(pagf);
                                /*if (fila[0].ToString() != "E")
                                {
                                    Paragraph refef = new Paragraph( "T.: "+fila[5]+  " Referencia Pago: " + fila[7].ToString(), fuente);
                                    refef.Alignment = Element.ALIGN_CENTER;
                                    documento.Add(refef);                           
                                }*/

                                Paragraph lpago = new Paragraph("", fuenteB);
                                lpago.Alignment = Element.ALIGN_CENTER;
                                documento.Add(lpago);
                                Paragraph lcambio = new Paragraph("", fuente);
                                lcambio.Alignment = Element.ALIGN_CENTER;
                                documento.Add(lcambio);
                            }
                        }

                        documento.Add(new Paragraph(" "));
                        //importe con letra
                        ConvertirNumerosLetras conversion = new ConvertirNumerosLetras();
                        conversion.IMporte = (_tot + _iva).ToString();
                        string textoLetras = conversion.enletras();
                        Paragraph importeLetra = new Paragraph("", fuente);
                        importeLetra.Alignment = Element.ALIGN_CENTER;
                        documento.Add(importeLetra);

                        if (_notasTicket != "")
                        {
                            documento.Add(new Paragraph(" "));
                            Paragraph notTi = new Paragraph("", fuente);
                            notTi.Alignment = Element.ALIGN_CENTER;
                            documento.Add(notTi);
                            documento.Add(new Paragraph(" "));
                            Paragraph notTciket = new Paragraph("", fuenteS);
                            notTciket.Alignment = Element.ALIGN_CENTER;
                            documento.Add(notTciket);
                        }

                        documento.Add(new Paragraph(" "));
                        Paragraph not = new Paragraph(_notas, fuenteS);
                        not.Alignment = Element.ALIGN_CENTER;
                        documento.Add(not);
                    }
                    documento.Close();

                }
                return archivo;
            }

        
    private void Encabezado(Document document)
    {

        // FUENTE
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblCab = new PdfPTable(1);

        Paragraph plan = new Paragraph(_encabezado, fuente);
        plan.Alignment = Element.ALIGN_CENTER;
        document.Add(plan);
                
        document.Add(new Paragraph(""));
        document.Add(new Paragraph(""));
        Islas isla = new Islas();
        isla.Almacen = _pv;
        isla.obtieneUbicacion();
        string ubicacion = isla.Ubicacion;
        if (ubicacion == "")
            ubicacion = "Terminal: " + _pv.ToString() + ": Sin Ubicación";
        else
            ubicacion = "Terminal: " + _pv.ToString() + ": " + ubicacion;

        Paragraph ubica = new Paragraph(ubicacion, fuente);
        ubica.Alignment = Element.ALIGN_CENTER;
        document.Add(ubica);
        Paragraph fech = new Paragraph("Fecha: "+ _fecha+"          Hora: "+_hora, fuente);
        fech.Alignment = Element.ALIGN_CENTER;
        document.Add(fech);
        Paragraph caj = new Paragraph("Caja: " + _caja.ToString() + "          Ticket: #" + _ticket, fuente);
        caj.Alignment = Element.ALIGN_CENTER;
        document.Add(caj);
    }

    private void ProductosTicket(Document document)
    {

        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenten = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.RED);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblProductos = new PdfPTable(8);
        tblProductos.WidthPercentage = 100f;


        PdfPCell cllcan = new PdfPCell(new Phrase("CANT", fuenteB));
        cllcan.BorderWidthBottom = 1;
        cllcan.HorizontalAlignment = Element.ALIGN_MIDDLE;
        cllcan.VerticalAlignment = Element.ALIGN_MIDDLE;
        cllcan.BackgroundColor = BaseColor.LIGHT_GRAY;
        cllcan.Padding = 1;
        PdfPCell cldesc = new PdfPCell(new Phrase("CÓDIGO", fuenteB));
        cldesc.BorderWidthBottom = 1;
        cldesc.VerticalAlignment = Element.ALIGN_MIDDLE;
        cldesc.HorizontalAlignment = Element.ALIGN_MIDDLE;
        cldesc.VerticalAlignment = 1;
        cldesc.BackgroundColor = BaseColor.LIGHT_GRAY;
        cldesc.Padding = 1;
        PdfPCell clpre = new PdfPCell(new Phrase("MEDIDAS", fuenteB));
        clpre.BorderWidthBottom = 1;
        clpre.HorizontalAlignment = Element.ALIGN_MIDDLE;
        clpre.VerticalAlignment = Element.ALIGN_MIDDLE;
        clpre.BackgroundColor = BaseColor.LIGHT_GRAY;
        clpre.Padding = 1;
        PdfPCell clDcto = new PdfPCell(new Phrase("DETALLE", fuenteB));
        clDcto.BorderWidthBottom = 1;
        clDcto.HorizontalAlignment = Element.ALIGN_MIDDLE;
        clDcto.VerticalAlignment = Element.ALIGN_MIDDLE;
        clDcto.BackgroundColor = BaseColor.LIGHT_GRAY;
        clDcto.Padding = 1;
        PdfPCell clCantDcto = new PdfPCell(new Phrase("FOTO", fuenteB));
        clCantDcto.BorderWidthBottom = 1;
        clCantDcto.HorizontalAlignment = Element.ALIGN_MIDDLE;
        clCantDcto.VerticalAlignment = Element.ALIGN_MIDDLE;
        clCantDcto.BackgroundColor = BaseColor.LIGHT_GRAY;
        clCantDcto.Padding = 1;
        PdfPCell cltotal = new PdfPCell(new Phrase("MONEDA", fuenteB));
        cltotal.BorderWidthBottom = 1;
        cltotal.HorizontalAlignment = 1;
        cltotal.VerticalAlignment = 1;
        cltotal.BackgroundColor = BaseColor.LIGHT_GRAY;
        cltotal.Padding = 1;
        PdfPCell pre = new PdfPCell(new Phrase("PRECIO", fuenteB));
        pre.BorderWidthBottom = 1;
        pre.HorizontalAlignment = Element.ALIGN_MIDDLE;
        pre.VerticalAlignment = Element.ALIGN_MIDDLE;
        pre.BackgroundColor = BaseColor.LIGHT_GRAY;
        pre.Padding = 1;
        PdfPCell tota = new PdfPCell(new Phrase("TOTAL", fuenteB));
        tota.BorderWidthBottom = 1;
        tota.HorizontalAlignment = Element.ALIGN_MIDDLE;
        tota.VerticalAlignment = Element.ALIGN_MIDDLE;
        tota.BackgroundColor = BaseColor.LIGHT_GRAY;
        tota.Padding = 1;

       

        tblProductos.AddCell(cllcan);
        tblProductos.AddCell(cldesc);
        tblProductos.AddCell(clpre);
        tblProductos.AddCell(clDcto);
        tblProductos.AddCell(clCantDcto);
        tblProductos.AddCell(cltotal);
        tblProductos.AddCell(pre);
        tblProductos.AddCell(tota);

        ImprimeTicket imp = new  ImprimeTicket();

        imp.ticketc = _ticket;


        int tamañodatos= 0;
        int cantidad;
        string codigo;
        string medidas;
        string descripcion;
        string foto;
        decimal ventu;
        decimal importe;
        object[] camposcab = detTicket.obtieneDetalle(_ticket); 
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];
         

            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                cantidad = Convert.ToInt32( fila[0]);
                codigo = Convert.ToString(fila[1]);
                medidas = Convert.ToString(fila[2]);
                descripcion = Convert.ToString(fila[3]);
                foto = Convert.ToString(fila[4]);
                ventu = Convert.ToInt32(fila[5]);
                importe = Convert.ToInt32(fila[6]);

                if(foto == "")
                {
                    PdfPCell canti = new PdfPCell(new Phrase("" + cantidad, fuenteS));
                    canti.HorizontalAlignment = Element.ALIGN_CENTER;
                    canti.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(canti);


                    PdfPCell codi = new PdfPCell(new Phrase("" + codigo, fuenteS));
                    codi.HorizontalAlignment = Element.ALIGN_CENTER;
                    codi.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(codi);

                    PdfPCell deta = new PdfPCell(new Phrase("" + medidas, fuenteS));
                    deta.HorizontalAlignment = Element.ALIGN_CENTER;
                    deta.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(deta);

                    PdfPCell desc = new PdfPCell(new Phrase("" + descripcion, fuenteS));
                    desc.HorizontalAlignment = Element.ALIGN_CENTER;
                    desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(desc);

                    PdfPCell fot = new PdfPCell(new Phrase("No cuenta con imagen", fuenteS));
                    fot.HorizontalAlignment = Element.ALIGN_CENTER;
                    fot.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(fot);

                    PdfPCell mon = new PdfPCell(new Phrase("MX", fuenteS));
                    mon.HorizontalAlignment = Element.ALIGN_CENTER;
                    mon.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(mon);

                    PdfPCell vu = new PdfPCell(new Phrase("" + ventu.ToString("C2"), fuenteS));
                    vu.HorizontalAlignment = Element.ALIGN_CENTER;
                    vu.VerticalAlignment = Element.ALIGN_CENTER;
                    tblProductos.AddCell(vu);

                    PdfPCell im = new PdfPCell(new Phrase("" + importe.ToString("C2"), fuenteS));
                    im.HorizontalAlignment = Element.ALIGN_CENTER;
                    im.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(im);
                }
                else { 
              /* string rutaLogo = HttpContext.Current.Server.MapPath("~/TMP/"+foto);
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                logo.ScaleToFit(100, 50);
                logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

                    // iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("~/TMP/"+foto);
                    logo.BorderWidth = 0;
                    logo.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 150 / logo.Width;
                    logo.ScalePercent(percentage * 100);*/

                    PdfPCell canti = new PdfPCell(new Phrase("" + cantidad, fuenteS));
                canti.HorizontalAlignment = Element.ALIGN_CENTER;
                canti.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(canti);


                PdfPCell codi = new PdfPCell(new Phrase("" + codigo, fuenteS));
                codi.HorizontalAlignment = Element.ALIGN_CENTER;
                codi.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(codi);

                PdfPCell deta = new PdfPCell(new Phrase("" + medidas, fuenteS));
                deta.HorizontalAlignment = Element.ALIGN_CENTER;
                deta.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(deta);

                PdfPCell desc = new PdfPCell(new Phrase("" + descripcion, fuenteS));
                desc.HorizontalAlignment = Element.ALIGN_CENTER;
                desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(desc);

                    string rutaLogo = HttpContext.Current.Server.MapPath("~/TMP/"+foto);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(rutaLogo);
                    image.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    PdfPCell cell = new PdfPCell(image);
                    cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                    tblProductos.AddCell(cell);

                   /* PdfPCell fot = new PdfPCell(new Phrase("" + imagen, fuenteS));
                fot.HorizontalAlignment = Element.ALIGN_CENTER;
                fot.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(fot);*/

                PdfPCell mon = new PdfPCell(new Phrase("MX", fuenteS));
                mon.HorizontalAlignment = Element.ALIGN_CENTER;
                mon.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(mon);

                PdfPCell vu = new PdfPCell(new Phrase("" + ventu.ToString("C2"), fuenteS));
                vu.HorizontalAlignment = Element.ALIGN_CENTER;
                vu.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(vu);

                PdfPCell im = new PdfPCell(new Phrase("" + importe.ToString("C2"), fuenteS));
                im.HorizontalAlignment = Element.ALIGN_CENTER;
                im.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblProductos.AddCell(im);


                }


            }
        }

        PdfPTable det2 = new PdfPTable(1);
        det2.DefaultCell.Border = 0;
        det2.WidthPercentage = 100f;
        int[] det2cellwidth = { 100 };
        det2.SetWidths(det2cellwidth);

        PdfPCell enca3 = new PdfPCell(new Phrase("PARA SOLICITAR FACTURA DEBERA PROPORCIONARSE  A SU VENDEDOR EL MISMO DIA DE LA VENTA TODOS LOS DATOS FISCALES Y EL CORREO ELECTRONICO PARA ENVIARLES SU FACTURA, EN CASO DE NO HACERLO SE FACTURARA COMO PUBLICO EN GENERAL Y POR NINGUN MOTIVO ABRA REFACTURACION", fuenter));
        enca3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        enca3.BackgroundColor = BaseColor.LIGHT_GRAY;
        enca3.Border = 0;
        det2.AddCell(enca3);

        PdfPCell obs = new PdfPCell(new Phrase("OBSERVACIONES", fuenter));
        obs.HorizontalAlignment = Element.ALIGN_LEFT;
        obs.Border = 0;
        det2.AddCell(obs);

        PdfPCell obs2 = new PdfPCell(new Phrase("A) Esta cotizacion carece de valor fiscal. Es valida por 15 dias. Los precios estan cotizados en pesos. PRECIOS MAS IVA", fuenten));
        obs2.HorizontalAlignment = Element.ALIGN_LEFT;
        obs2.Border = 0;
        det2.AddCell(obs2);

        PdfPCell obs3 = new PdfPCell(new Phrase("B) Una vez levantado el pedido no se aceptan cambios ni devoluciones.\nC) En mercancia sobre pedido no hay descuentos.\nD) Las imágenes no corresponden al 100% con el pedido, las especificaciones escritas si, ya que las telas pueden variar en color y textura dependiendo de la partida (sin previo aviso).\nE) En las ordenes de productos italianos, los meses de Agosto y Diciembre no se consideran dentro de la semanas de Produccion, ya que las empresas Italianas cierran.\nF) Es requisito la firma de conformidad del cliente para hacer el pedido.", fuente));
        obs3.HorizontalAlignment = Element.ALIGN_LEFT;
        obs3.Border = 0;
        det2.AddCell(obs3);

        PdfPCell obs4 = new PdfPCell(new Phrase("FORMA DE PAGO", fuenter));
        obs4.HorizontalAlignment = Element.ALIGN_LEFT;
        obs4.Border = 0;
        det2.AddCell(obs4);

        PdfPCell obs5 = new PdfPCell(new Phrase("A) Pago de 70% de anticipo y el saldo del 30% restante,debera ser pagado 5 dias antes de la llegada de la mercancia a puerto. \n B) En caso de una orden en  moneda extranjera,  el tipo de cambio bancario, corresponde al dia en que se realice cada operación al tipo de cambio de venta de la moneda .  (BANAMEX) \n C) Se cobrara un  costo extra por almacenaje de  $4,000 pesos M.N por mes , en caso de no entregarse por causas personales del cliente. La empresa no se hace responsible del deterioro de la mercancia en almacenaje. \n D) No nos hacemos responsables por el deterioro de la mercancia en almacenaje.", fuente));
        obs5.HorizontalAlignment = Element.ALIGN_LEFT;
        obs5.Border = 0;
        det2.AddCell(obs5);

        PdfPCell obs6 = new PdfPCell(new Phrase("CLAUSULAS DE ENTREGA", fuenter));
        obs6.HorizontalAlignment = Element.ALIGN_LEFT;
        obs6.Border = 0;
        det2.AddCell(obs6);

        PdfPCell obs7 = new PdfPCell(new Phrase("A) La entrega sera programada por bodega despues del pago total. No hay entregas en fin de semana. Las entregas solo son en LA CIUDAD DE MEXICO \n B) Horario de entrega abierto y bodega se comunica para confirmar dia y hora. \n C) La entrega es  gratuita en LA CIUDAD DE MEXICO solo en planta baja, o si hay elevador por donde quepa la mercancia. NO SE VUELAN MUEBLES O SUBEN POR ESCALERAS.  El cliente debera asegurarse que los muebles caben por el elevador de servicio, en caso contrario, estos servicios deberan ser contratados con anterioridad  por el cliente con el costo y riesgo del volado siendo esponsabilidad del cliente unicamente. \n D) El area donde se colocaran los muebles deberá estar despejada. \n E) Fuera de LA CIUDAD DE MEXICO, el flete es  por cuenta del cliente desde la bodega hasta el destino final.", fuente));
        obs7.HorizontalAlignment = Element.ALIGN_LEFT;
        obs7.Border = 0;
        det2.AddCell(obs7);

        PdfPCell obs8 = new PdfPCell(new Phrase("F) El cliente debe de reportar anomalias en la mercancia a la hora de la entrega. Una vez firmado de recibido, no se aceptan reclamaciones. ", fuenten));
        obs8.HorizontalAlignment = Element.ALIGN_LEFT;
        obs8.Border = 0;
        det2.AddCell(obs8);

        PdfPCell obs9 = new PdfPCell(new Phrase("G) No se aceptan cancelaciones, devoluciones o cambios en mercancia de armado y pedidos especiales.", fuente));
        obs9.HorizontalAlignment = Element.ALIGN_LEFT;
        obs9.Border = 0;
        det2.AddCell(obs9);


        document.Add(tblProductos);
        document.Add(new Paragraph(" "));
        document.Add(new Paragraph(" "));
        document.Add(det2);

    }

}