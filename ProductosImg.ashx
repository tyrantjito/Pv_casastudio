<%@ WebHandler Language="C#" Class="ProductosImg" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class ProductosImg : IHttpHandler {
    
   public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string[] datos = context.Request.QueryString["id"].ToString().Split(';');
            string id_producto = Convert.ToString(datos[0]);
            int id_imagen = Convert.ToInt32(datos[1]);
            Image imagen = GetImagen(id_producto,id_imagen);
            context.Response.ContentType = "image/jpeg";
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }


         private Image GetImagen(string id_producto,int id_imagen)
    {
        Image logo = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["PVW"].ToString();
        string sql = "select imagen from Fotografias_Productos where id_producto='" + id_producto+ "' and id_imagen=" + id_imagen ;
        try
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                byte[] imagenPerfil = (byte[])lectura[0];
                memoryStream = new MemoryStream(imagenPerfil, false);
            }
        }
        catch (Exception x )
        {

        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }
        try
        {
            logo = Image.FromStream(memoryStream);
        }
        catch (Exception x )
        {
            logo = null;
        }
        return logo;
    }
}