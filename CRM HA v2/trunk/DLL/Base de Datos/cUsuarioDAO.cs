using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

public class cUsuarioDAO
{
    public string GetTable
    { get { return "tUsuario"; } }

    public string GetOrderBy
    { get { return "Nombre ASC"; } }

    public List<cAtributo> AttributesClass(cUsuario usuario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Nombre", usuario.Nombre));
        lista.Add(new cAtributo("Usuario", usuario.Usuario));
        lista.Add(new cAtributo("Clave", usuario.Clave));
        lista.Add(new cAtributo("Mail", usuario.Mail));
        lista.Add(new cAtributo("idCategoria", usuario.IdCategoria));
        lista.Add(new cAtributo("Papelera", usuario.Papelera));
        return lista;
    }

    public int Save(cUsuario user)
    {
        if (string.IsNullOrEmpty(user.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(user));
        else
            return cDataBase.GetInstance().UpdateObject(user.Id, GetTable, AttributesClass(user));
    }

    public cUsuario Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cUsuario usuario = new cUsuario();
            usuario.Id = Convert.ToString(atributos["id"]);
            usuario.Nombre = Convert.ToString(atributos["Nombre"]);
            usuario.Usuario = Convert.ToString(atributos["Usuario"]);
            usuario.Clave = Convert.ToString(atributos["Clave"]);
            usuario.Mail = Convert.ToString(atributos["Mail"]);
            usuario.IdCategoria = Convert.ToInt16(atributos["idCategoria"]);
            usuario.Papelera = Convert.ToInt16(atributos["Papelera"]);
            return usuario;
        }
        catch
        {
            return null;
        }
    }

    //Devuelve la lista de Usuarios Activos (Papelera = 1).
    public ArrayList LoadTable()
    {
        ArrayList usuarios = new ArrayList();
        SqlCommand cmd = new SqlCommand("SELECT id FROM " + GetTable + " WHERE Papelera = 1 ORDER BY Nombre");
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            usuarios.Add(Load(Convert.ToString(idList[i])));
        }
        return usuarios;
    }

    public int LoginUser(string nameUser, string password)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        // Si la clave y el usuario son correctos devolverá 1, sino 0.
        query.Append("SELECT id FROM " + this.GetTable);
        query.Append(" WHERE Usuario = @Usuario AND Clave = @Clave AND Papelera = @Papelera");
        SqlCommand cmd = new SqlCommand(query.ToString());
        // Creamos los parámetros.
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Papelera", SqlDbType.SmallInt);
        // Asignamos los valores recibidos como parámetro.
        cmd.Parameters["@Usuario"].Value = nameUser;
        cmd.Parameters["@Clave"].Value = password;
        cmd.Parameters["@Papelera"].Value = 1; // Usuario activo.
        // Ejecutamos la consulta
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    /* registra los accesos al sistema */
    public void RegisterAccess(string idUsuario, string ip)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("INSERT INTO tRegistroAcceso (idUsuario,Fecha, descripcion, ip) VALUES(" + idUsuario + ", getdate(), '" + "Acceso al sistema" + "', '" + ip + "')");
        SqlCommand cmd = new SqlCommand(query.ToString());
        cDataBase.GetInstance().ExecuteScalar(cmd);
    }

     /* registra los accesos a la informacion de cada empresa */
    public void InformationAccess(string idUsuario, string empresa, string ip)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("INSERT INTO tRegistroAcceso (idUsuario,Fecha, descripcion, ip) VALUES(" + idUsuario + ", getdate(), '" + "Acceso a la informacón de " + empresa + "', '" + ip + "')");
        SqlCommand cmd = new SqlCommand(query.ToString());
        cDataBase.GetInstance().ExecuteScalar(cmd);
    }

    public List<cUsuario> GetUsuarios()
    {
        List<cUsuario> usuarios = new List<cUsuario>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1" + " Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;

        for (int i = 0; idList.Count > i; i++)
        {
            usuarios.Add(Load(Convert.ToString(idList[i])));
            string query2 = "SELECT tipo FROM tCategoria c WHERE " + usuarios[i].IdCategoria + "=c.id";
            SqlCommand cmd = new SqlCommand(query2);
            string claveDeco = cUsuario.Decode(usuarios[i].Clave);
            usuarios[i].Clave = claveDeco;
        }
        return usuarios;
    }

    public string GetUsuarioByName(string nombre)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE Nombre LIKE '%" + nombre + "%'";
        SqlCommand com = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
    }

    public string GetLastRegistry(DateTime fecha)
    {
        string query = "SELECT TOP (1)id FROM tRegistroAcceso WHERE Fecha >= '" + fecha.Year + "/" + fecha.Month + " / " + fecha.Day + "'";
        SqlCommand com = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
    }
}
