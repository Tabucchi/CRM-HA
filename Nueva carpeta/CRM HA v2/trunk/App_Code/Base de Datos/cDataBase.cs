using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;


public class cDataBase : IDisposable
{
    private string connectionString = "server=APPSERVER2\\PRODUCTION;database=NaexCRM;; User id=sa; Password=nu43cp33;";
    private SqlConnection cnn = null;
    private static cDataBase instace = null;


    // Devuelve una instancia de esta clase
    public static cDataBase GetInstance()
    {
        if (instace == null)
            instace = new cDataBase();
        return instace;
    }


    // Conecta al origen de datos
    public bool Conectar()
    {
        Desconectar();
        if (cnn == null)
            cnn = new SqlConnection();
        cnn.ConnectionString = connectionString;
        cnn.Open();
        return true;
    }


    // Cierra la conexion 
    public bool Desconectar()
    {
        if (cnn == null)
            return false;
        cnn.Close();
        return true;
    }


    // Insert Generico, Devuelve el ID del registro ingresado.
    public int InsertObject(string nombreTabla, List<cAtributo> atributos)
    {
        try
        {
            Conectar();
            string select = "INSERT INTO " + nombreTabla;
            string campos = " (";
            string values = "VALUES (";
            SqlCommand cmd = new SqlCommand();
            foreach (cAtributo atributo in atributos)
            {
                campos += atributo.Nombre + " ,";
                values += "@" + atributo.Nombre + " ,";
                cmd.Parameters.Add(new SqlParameter(atributo.Nombre, atributo.Valor));
            }
            // Reemplazamos la ultima coma por un parentesis
            campos = campos.Substring(0, (campos.Length - 1)) + " )";
            values = values.Substring(0, (values.Length - 1)) + " )";
            // Armo el comando SQL
            cmd.CommandText = select + campos + values;
            cmd.Connection = cnn;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Desconectar();
            return GetIdentity(nombreTabla);
        }
        catch (Exception ex)
        {
            Dispose();
            Desconectar();
            cLog log = new cLog(22, "INSERT", ex.ToString());
            return -1;
        }
    }


    // Update Generico, Devuelve TRUE si se ejecuta correctamente o FALSE si ocurre lo contrario
    public bool UpdateObject(string id, string nombreTabla, List<cAtributo> atributos)
    {
        try
        {
            Conectar();
            string update = "UPDATE " + nombreTabla + " SET ";
            string campos = "";
            string condicion = " WHERE id = " + id;
            SqlCommand cmd = new SqlCommand();
            foreach (cAtributo atributo in atributos)
            {
                campos += atributo.Nombre + " = @" + atributo.Nombre + ", ";
                cmd.Parameters.Add(new SqlParameter(atributo.Nombre, atributo.Valor));
            }
            // Reemplazamos la ultima coma por un parentesis
            campos = campos.Substring(0, (campos.Length - 2));
            // Armo la consulta SQL
            cmd.CommandText = update + campos + condicion;
            cmd.Connection = cnn;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Desconectar();
            return true;
        }
        catch (Exception ex)
        {
            Dispose();
            Desconectar();
            cLog log = new cLog(22, "UPDATE", ex.ToString());
            return false;
        }
    }


    // Delete Generico, Devuelve TRUE si se ejecuta correctamente o FALSE si ocurre lo contrario
    public bool DeleteObject(string id, string nombreTabla)
    {
        try
        {
            Conectar();
            string queryDelete = "DELETE FROM " + nombreTabla + " WHERE id = " + id;
            SqlCommand cmd = new SqlCommand(queryDelete, cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Desconectar();
            return true;
        }
        catch
        {
            Dispose();
            Desconectar();
            return false;
        }
    }


    // Devuelve el objeto correspondiente a "id" en la tabla "nombreTabla"
    public Hashtable LoadObject(int id, string nombreTabla)
    {
        try
        {
            Conectar();
            Hashtable tablaObjeto = new Hashtable();
            string query = "SELECT * FROM " + nombreTabla + " WHERE id = " + id;
            SqlCommand cmd = new SqlCommand(query, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            for (int i = 0; reader.FieldCount > i; i++)
            {
                tablaObjeto.Add(reader.GetName(i), reader.GetValue(i));
            }
            cmd.Dispose();
            Desconectar();
            return tablaObjeto;
        }
        catch
        {
            Dispose();
            Desconectar();
            return null;
        }
    }


    // Devuelve todos los ID's correspondientes a "nombreTabla" ordenados por "orderBy"
    public ArrayList LoadTable(string nombreTabla, string orderBy)
    {
        try
        {
            Conectar();
            ArrayList idList = new ArrayList();
            string query = "SELECT id FROM " + nombreTabla + " ORDER BY " + orderBy;
            SqlCommand cmd = new SqlCommand(query, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idList.Add(reader.GetValue(0));
            }
            cmd.Dispose();
            Desconectar();
            return idList;
        }
        catch
        {
            Dispose();
            Desconectar();
            return null;
        }
    }


    // Ejecuta la consulta "com" y devuelve el resultado en formato STRING  
    public string ExecuteScalar(SqlCommand com)
    {
        try
        {
            Conectar();
            com.Connection = cnn;
            string r = com.ExecuteScalar().ToString();
            com.Dispose();
            Desconectar();
            return r;
        }
        catch
        {
            Desconectar();
            return null;
        }
    }


    // Ejecuta la consulta "com" y devuelve un array con los Id's de las filas afectadas
    public ArrayList ExecuteReader(SqlCommand com)
    {
        try
        {
            Conectar();
            ArrayList idList = new ArrayList();
            com.Connection = cnn;
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                idList.Add(reader.GetValue(0));
            }
            com.Dispose();
            Desconectar();
            return idList;
        }
        catch
        {
            Desconectar();
            return null;
        }
    }


    // Devuelve el ultimo id insertado
    public int GetIdentity(string nombreTabla)
    {
        string query = "SELECT MAX(id) FROM " + nombreTabla;
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToInt32(ExecuteScalar(cmd));
    }

    // Devuelve un DataSet de las "Querys" pasadas por parametro
    public DataSet GetDataSet(ArrayList querys, ArrayList nombreTablas)
    {
        try
        {
            Conectar();
            DataSet ds = new DataSet();
            for (int i = 0; querys.Count > i; i++)
            {
                SqlDataAdapter da = new SqlDataAdapter(querys[i].ToString(), cnn);
                da.Fill(ds, nombreTablas[i].ToString());
                da.Dispose();
            }

            Desconectar();
            return ds;
        }
        catch
        {
            Desconectar();
            return null;
        }
    }

    // Sobreescribo el metodo Dispose para que tb cierre la coneccion
    public void Dispose()
    {
        Desconectar();
    }

}
