using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cCategoriaInventario
{
    private string id;   
    private string descripcion;    
    private string numero;    
    private string contador;

    #region Acceso a Datos
    public static cCategoriaInventario Load(string id)
    {
        cCategoriaInventarioDAO DAO = new cCategoriaInventarioDAO();
        return DAO.Load(id);
    }

    public int Save()
    {
        cCategoriaInventarioDAO DAO = new cCategoriaInventarioDAO();
        return DAO.Save(this);
    }
    #endregion

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public string Numero
    {
        get { return numero; }
        set { numero = value; }
    }

    public string Contador
    {
        get { return contador; }
        set { contador = value; }
    }
    #endregion

    public static List<cCategoriaInventario> GetCategorias()
    {
        cCategoriaInventarioDAO campoGenericoDao = new cCategoriaInventarioDAO();
        return campoGenericoDao.GetCategorias();
    }
}

