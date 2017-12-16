using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

public class cManual
{
    private string id;
    private string titulo;
    private string descripcion;
    private string idUsuario;
    private DateTime fecha;
    private string usuario;
    private string idEmpresa;
    private Int16 papelera;
    
    public cManual(){}

    public cManual(string _titulo, string _idUsuario, string _idEmpresa)
    {
        titulo = _titulo;
        idUsuario = _idUsuario;
        fecha = DateTime.Today;
        idEmpresa = _idEmpresa;
        Papelera = 1;
    }
    
    public int Save()
    {
        cManualDAO manualDAO = new cManualDAO();
        return manualDAO.Save(this);
    }

    public static cManual Load(string id)
    {
        cManualDAO DAO = new cManualDAO();
        return DAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cManualDAO manualDao=new cManualDAO();
        return manualDao.LoadTable();
    }

    public static List<cManual> GetManualesTop5()
    {
        cManualDAO manualDao = new cManualDAO();
        return manualDao.GetManualesTop5();
    }

    public static List<cManual> GetManuales()
    {
        cManualDAO manualDao = new cManualDAO();
        return manualDao.GetManuales();
    }

    public static List<cManual> Search(string id)
    {
        cManualDAO manualDAO = new cManualDAO();
        return manualDAO.Search(id);
    }

    public static List<cManual> SearchByEmpresa(string idEmpresa)
    {
        cManualDAO DAO = new cManualDAO();
        return DAO.SearchByEmpresa(idEmpresa);
    }

    public static string GetIdByNombre(string manual)
    {
        cManualDAO manualDao = new cManualDAO();
        return manualDao.GetIdByNombre(manual);
    }

    public static string[] GetManualesPosibles(string texto)
    {
        int i = 0;
        cManualDAO DAO = new cManualDAO();
        List<cManual> c = DAO.GetManualesPosibles(texto);
        string[] items = new string[c.Count];
        foreach (cManual x in c)
        {
            items.SetValue(x.Titulo, i);
            i++;
        }
        return items;
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Titulo
    {
        get { return titulo; }
        set { titulo = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
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

    public string GetUsuario
    {
        get
        {
            try { return cUsuario.Load(idUsuario).Nombre; }
            catch { return "DATO PERDIDO"; }
        }
    }

    public string IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public string GetEmpresa
    {
        get
        {
            try{ return cEmpresa.Load(idEmpresa).Nombre;}
            catch
            {
                if (idEmpresa == "-1")
                    return "-";
                else
                    return "Dato Perdido";
            }
        }
    }

    public short Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }

    #endregion
}
