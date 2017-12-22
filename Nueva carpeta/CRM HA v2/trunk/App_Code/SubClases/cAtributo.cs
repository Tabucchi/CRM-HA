using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class cAtributo
{
    protected string _Nombre;
    protected object _Valor;

    public cAtributo(string nombre, object valor)
    {
        _Nombre = nombre;
        _Valor = valor;
    }

    public string Nombre
    {
        get { return _Nombre; }
        set { _Nombre = value; }
    }

    public object Valor
    {
        get { return _Valor; }
        set { _Valor = value; }
    }
}
