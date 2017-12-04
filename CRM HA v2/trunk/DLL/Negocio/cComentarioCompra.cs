using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;

public class cComentarioCompra
{
    private string id;
    private string idUsuario;
    private string idCompra;
    private string tipo;
    private string descripcion;
    private DateTime fecha;
    
    public cComentarioCompra(string _idUsuario, string _tipo, string _idCompra, string _descripcion)
    {
        idUsuario = _idUsuario;
        tipo = _tipo;
        idCompra = _idCompra;
        descripcion = _descripcion;
        fecha = DateTime.Now;
    }

    public cComentarioCompra()
    { }

    public int Save()
    {
        if (Validar())
        {
            cComentarioCompraDAO DAO = new cComentarioCompraDAO();
            return DAO.Save(this);
        }
        return -1;
    }

    public bool Validar()
    {
        try
        {
            bool flag = false;
            flag = Convert.ToInt16(this.idUsuario) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idUsuario) ? false : true;

            flag = Convert.ToInt16(this.IdCompra) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.IdCompra) ? false : true;
            return flag;
        }
        catch
        {
            return false;
        }
    }

    public static ArrayList SearchByIdPedido(string idCompra)
    {
        cComentarioCompraDAO DAO = new cComentarioCompraDAO();
        return DAO.SearchByIdPedido(idCompra);
    }

    public static List<cComentarioCompra> GetList(string idCompra)
    {
        List<cComentarioCompra> comentarios = new List<cComentarioCompra>();
        ArrayList res = SearchByIdPedido(idCompra);

        if (res != null)
        {
            foreach (cComentarioCompra c in res)
            {
                comentarios.Add(c);
            }
        }
        return comentarios;
    }
    
    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string IdCompra
    {
        get { return idCompra; }
        set { idCompra = value; }
    }

    public string Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public iAutorComentario GetAutorComentario()
    {
        return (iAutorComentario)cUsuario.Load(idUsuario);
    }

    public string GetNombreAutor()
    {
        return cUsuario.Load(idUsuario).Nombre;
    }

    public string GetEstado()
    {
        return cCompra.Load(IdCompra,null).GetEstado;
    }
    #endregion
}
