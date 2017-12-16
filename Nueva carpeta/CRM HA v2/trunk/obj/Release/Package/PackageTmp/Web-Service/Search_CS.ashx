<%@ WebHandler Language="C#" Class="Search_CS" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

public class Search_CS : IHttpHandler {

    private string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
    public void ProcessRequest (HttpContext context) {
      
        string prefixText = context.Request.QueryString["q"];
                
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = connectionString;
            conn.Open();
            
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT e.Nombre, e.Apellido FROM tEmpresa AS e WHERE (e.Nombre LIKE + '%' + @SearchText + '%') OR (e.Apellido LIKE + '%' + @SearchText + '%' ) ORDER BY e.Nombre";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = conn;
                StringBuilder sb = new StringBuilder();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        sb.Append(sdr["Nombre"] + " " + sdr["Apellido"])
                            .Append(Environment.NewLine);
                    }
                }
                conn.Close();
                context.Response.Write(sb.ToString());
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}