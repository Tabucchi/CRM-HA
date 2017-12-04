using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cEmpresaUnidad
{
    private string id;    
    private string idEmpresa;    
    private string idUnidad;
    private string codUF;    
    private string idProyecto;
    private decimal precioAcordado;
    private string idOv;
    private Int16 _papelera;    

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public string GetEmpresa
    {
        get { return cEmpresa.Load(IdEmpresa).GetNombreCompleto; }
    }

    public string IdUnidad
    {
        get { return idUnidad; }
        set { idUnidad = value; }
    }

    public string GetTipoUnidad
    {
        get { return cUnidad.Load(IdUnidad).UnidadFuncional; }
    }

    public string GetNivel
    {
        get { return cUnidad.Load(IdUnidad).Nivel; }
    }

    public string GetNroUnidad
    {
        get { return cUnidad.Load(IdUnidad).NroUnidad; }
    }
    
    public string CodUF
    {
        get { return codUF; }
        set { codUF = value; }
    }
    
    public string IdProyecto
    {
        get { return idProyecto; }
        set { idProyecto = value; }
    }

    public string GetProyecto
    {
        get { return cProyecto.Load(IdProyecto).Descripcion; }
    }

    public decimal PrecioAcordado
    {
        get { return precioAcordado; }
        set { precioAcordado = value; }
    }

    public string GetPrecioAcordado
    {
        get { return String.Format("{0:#,#0.00}", PrecioAcordado); }
    }

    public string GetPrecioBase
    {
        get { return String.Format("{0:#,#0.00}", cUnidad.Load(IdUnidad).PrecioBase); }
    }

    public string IdOv
    {
        get { return idOv; }
        set { idOv = value; }
    }

    public Int16 Papelera
    {
        get { return _papelera; }
        set { _papelera = value; }
    }
    #endregion

    public cEmpresaUnidad() { }

    public cEmpresaUnidad(string _idEmpresa, string _idUnidad, string _codUF, string _idProyecto) 
    {
        idEmpresa = _idEmpresa;
        idUnidad = _idUnidad;
        codUF = _codUF;
        idProyecto = _idProyecto;
        _papelera = (Int16)papelera.Activo;
    }

    public int Save()
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.Save(this);
    }

    public static cEmpresaUnidad Load(string id)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.Load(id);
    }

    public static string GetUnidadByIdProyecto(string _idProyecto)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetUnidadByIdProyecto(_idProyecto);
    }
    public static string GetUnidadByIdUnidad(string _idUnidad)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetUnidadByIdUnidad(_idUnidad);
    }

    public static string GetIdOperacionVentaByIdUnidad(string _idUnidad)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetIdOperacionVentaByIdUnidad(_idUnidad);
    }

    public static cEmpresaUnidad GetUnidad(string _codUF, string _idProyecto)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetUnidad(_codUF, _idProyecto);
    }

    public static cEmpresa GetEmpresaByUnidad(string _codUF, string _idProyecto)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetEmpresaByUnidad(_codUF, _idProyecto);
    }

    public static List<cEmpresaUnidad> GetEmpresaUnidadOV(string _idOperacionVenta)
    {
        cEmpresaUnidadDAO DAO = new cEmpresaUnidadDAO();
        return DAO.GetEmpresaUnidadOV(_idOperacionVenta);
    }
}

