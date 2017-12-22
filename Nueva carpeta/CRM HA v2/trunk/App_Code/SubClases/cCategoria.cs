using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

public class cCategoria
{
    private int id;
    private string tipo;

    public cCategoria()
    {
    }

    public static cCategoria Load(int id)
    {
        cCategoriaDAO categoriaDAO = new cCategoriaDAO();
        return categoriaDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cCategoriaDAO categoriaDAO = new cCategoriaDAO();
        return categoriaDAO.LoadTable();
    }

    #region Propiedades
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }
    #endregion
}
