using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

public class cNovedad
{
    private string id;
    private string idUsuario;
    private string descripcion;
    private DateTime fecha;
    private string usuario;

    public cNovedad(string _idUsuario, string _descripcion)
    {
        idUsuario = _idUsuario;
        descripcion = _descripcion;
        fecha = DateTime.Now;
    }

    public cNovedad() { }

    #region Acceso a Datos
    public int Save()
    {
        if (Validar())
        {
            cNovedadDAO DAO = new cNovedadDAO();
            return DAO.Save(this);
        }
        return -1;
    }

    public static cNovedad Load(string id)
    {
        cNovedadDAO novedadDao = new cNovedadDAO();
        return novedadDao.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cNovedadDAO DAO = new cNovedadDAO();
        return DAO.LoadTable();
    }

    public static ArrayList SearchByIdUsuario(string idUsuario)
    {
        cNovedadDAO DAO = new cNovedadDAO();
        return DAO.SearchByIdUsuario(idUsuario);
    }
    #endregion

    public static ArrayList SearchLastFive()
    {
        cNovedadDAO DAO = new cNovedadDAO();
        return DAO.SearchLastFive();
    }

    #region Validaciones
    public bool Validar()
    {
        try
        {
            bool flag = false;
            flag = Convert.ToInt16(this.idUsuario) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idUsuario) ? false : true;

            return flag;
        }
        catch
        {
            return false;
        }
    }
    #endregion

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

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public string Usuario
    {
        get { return usuario; }
        set { usuario = value; }
    }

    public cUsuario GetUsuario()
    {
        try
        {
            return cUsuario.Load(idUsuario) == null ? null : cUsuario.Load(idUsuario);
        }catch{
            return null;
        }
    }

    public string GetNombrUsuario
    {
        get {
            try
            {
                return cUsuario.Load(idUsuario) == null ? "" : cUsuario.Load(idUsuario).Nombre;
            }
            catch {
                return "";
            }
        }
    }
    #endregion
      
    public static List<cNovedad> GetList()
    {
        List<cNovedad> novedades = new List<cNovedad>();
        ArrayList res = SearchLastFive();

        //if (res == null) return null;

        foreach (cNovedad n in res)
        {
            novedades.Add(n);
        }
        return novedades;
    }

    public static List<cNovedad> GetNovedades()
    {
        cNovedadDAO novedadDao = new cNovedadDAO();
        return novedadDao.GetNovedades();
    }

    public static string DeleteNovedad(string id)
    {
        cNovedadDAO novedadDao = new cNovedadDAO();
        return novedadDao.DeleteNovedad(id);
    }
}
