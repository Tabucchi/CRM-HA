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
        return lista;
    }

    public bool Update(cUsuario usuario)
    {
        return cDataBase.GetInstance().UpdateObject(usuario.Id.ToString(), GetTable, AttributesClass(usuario));
    }

    public cUsuario Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cUsuario usuario = new cUsuario();
        usuario.Id = Convert.ToInt32(atributos["id"]);
        usuario.Nombre = Convert.ToString(atributos["Nombre"]);
        usuario.Usuario = Convert.ToString(atributos["Usuario"]);
        usuario.Clave = Convert.ToString(atributos["Clave"]);
        usuario.Mail = Convert.ToString(atributos["Mail"]);
        usuario.IdCategoria = Convert.ToInt32(atributos["idCategoria"]);
        return usuario;
    }

    public ArrayList LoadTable()
    {
        ArrayList usuarios = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            usuarios.Add(Load(Convert.ToInt32(idList[i])));
        }
        return usuarios;
    }

    public int LoginUser(string nameUser, string password)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        // Si la clave y el usuario son correctos devolverá 1, sino 0.
        query.Append("SELECT id FROM " + this.GetTable);
        query.Append(" WHERE Usuario = @Usuario AND Clave = @Clave");
        SqlCommand cmd = new SqlCommand(query.ToString());
        // Creamos los parámetros
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 63);
        // Asignamos los valores recibidos como parámetro
        cmd.Parameters["@Usuario"].Value = nameUser;
        cmd.Parameters["@Clave"].Value = password;
        // Ejecutamos la consulta
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    /* registra los accesos al sistema */
    public void RegisterAccess(string idUsuario)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("INSERT INTO tRegistroAcceso (idUsuario,Fecha) VALUES(" + idUsuario + ", getdate())");
        SqlCommand cmd = new SqlCommand(query.ToString());
        cDataBase.GetInstance().ExecuteScalar(cmd);
    }

    public string GetName(int id)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("SELECT Nombre FROM " + this.GetTable);
        query.Append(" WHERE id = @id");
        SqlCommand cmd = new SqlCommand(query.ToString());
        cmd.Parameters.Add("@id", SqlDbType.Int);
        cmd.Parameters["@id"].Value = id;
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public int GetId_By_IdAsignacionResponsable(int idAsignacion)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("SELECT idResponsable FROM tAsignacionResponsable");
        query.Append(" WHERE id = @id");
        SqlCommand cmd = new SqlCommand(query.ToString());
        cmd.Parameters.Add("@id", SqlDbType.Int);
        cmd.Parameters["@id"].Value = idAsignacion;
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));        
    }
}
