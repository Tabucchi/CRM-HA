using DLL.Base_de_Datos;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cReserva
{
    private string id;
    private string idEmpresa;
    private string idUnidad;
    private DateTime fechaVencimiento;
    private string idEmpresaUnidad;
    private decimal importe;
    private Int16 _papelera;
    private string idItemCCU;

    public cReserva() { }

    public cReserva(string _idEmpresa, string _idUnidad, DateTime _fechaVencimiento, string _idEmpresaUnidad, decimal _importe, int _idItemCCU)
    {
        idEmpresa = _idEmpresa;
        idUnidad = _idUnidad;
        fechaVencimiento = _fechaVencimiento;
        idEmpresaUnidad = _idEmpresaUnidad;
        importe = _importe;
        idItemCCU = _idItemCCU.ToString();
        _papelera = (Int16)papelera.Activo;
        this.Save();
    }

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

    public string IdEmpresaUnidad
    {
        get { return idEmpresaUnidad; }
        set { idEmpresaUnidad = value; }
    }

    public cUnidad unidad
    {
        get { return cUnidad.Load(IdUnidad); }
    }

    public string GetProyecto
    {
        get { return cProyecto.Load(unidad.IdProyecto).Descripcion; }
    }
    public string GetCodigoUF
    {
        get { return unidad.CodigoUF; }
    }
    public string GetUnidadFuncional
    {
        get { return unidad.UnidadFuncional; }
    }
    public string GetNivel
    {
        get { return unidad.Nivel; }
    }
    public string GetNroUnidad
    {
        get { return unidad.NroUnidad; }
    }
    public DateTime FechaVencimiento
    {
        get { return fechaVencimiento; }
        set { fechaVencimiento = value; }
    }
    public decimal Importe
    {
        get { return importe; }
        set { importe = value; }
    }
    public string GetImporte
    {
        get { return String.Format("{0:#,#0.00}", Importe); }
    }
    public Int16 Papelera
    {
        get { return _papelera; }
        set { _papelera = value; }
    }
    public string IdItemCCU
    {
        get { return idItemCCU; }
        set { idItemCCU = value; }
    }
    #endregion

    #region Acceso a Datos
    public int Save()
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.Save(this);
    }

    public static cReserva Load(string id)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.Load(id);
    }

    public static string GetReservaByIdUnidad(string _idUnidad)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetReservaByIdUnidad(_idUnidad);
    }

    public static decimal GetImporteReservaByIdUnidad(string _idUnidad)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetImporteReservaByIdUnidad(_idUnidad);
    }

    public static string GetIdUnidadByIdEmpresa1(string _idEmpresa)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetIdUnidadByIdEmpresa1(_idEmpresa);
    }

    public static List<cReserva> GetIdUnidadByIdEmpresa(string _idEmpresa)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetIdUnidadByIdEmpresa(_idEmpresa);
    }

    public static cReserva GetReservaByIdItemCCU(string _idItemCUU)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetReservaByIdItemCCU(_idItemCUU);
    }

    public static List<cReserva> GetReservasToday()
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetReservasToday();
    }

    public static List<cReserva> GetReservasByIdCCU(string _idCCU)
    {
        cReservaDAO DAO = new cReservaDAO();
        return DAO.GetReservasByIdCCU(_idCCU);
    }
    #endregion
}
