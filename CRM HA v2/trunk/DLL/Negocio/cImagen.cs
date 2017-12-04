using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cImagen
{
    private string id;   
    private string descripcion;    
    private byte[] imagen;

    public cImagen()
    {
    }

    #region Acceso a Datos
    public static cImagen Load(string id)
    {
        cImagenDAO DAO = new cImagenDAO();
        return DAO.Load(id);
    }

    public int Save()
    {
        cImagenDAO DAO = new cImagenDAO();
        return DAO.Save(this);
    }
    #endregion

    public static string Existe(string descripcion)
    {
        cImagenDAO DAO = new cImagenDAO();
        return DAO.Existe(descripcion);
    }
    public bool Delete(string id)
    {
        cImagenDAO imagenDAO = new cImagenDAO();
        return imagenDAO.Delete(id);
    }

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

    public byte[] Imagen
    {
        get { return imagen; }
        set { imagen = value; }
    }
    #endregion

}

