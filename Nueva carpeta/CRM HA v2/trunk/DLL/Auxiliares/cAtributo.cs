using System;
using System.Web;

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
