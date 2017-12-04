using DLL.Base_de_Datos;
using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public class cProyecto
{
    private string id;        
    private string descripcion;
    private decimal supTotal;
    private Int16 papelera;

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
    public decimal SupTotal
    {
        get { return supTotal; }
        set { supTotal = value; }
    }
    public string GetSupTotal
    {
        get { return String.Format("{0:#,#0.00}", cUnidad.GetTotalSupByProyecto(-1, Id)); }
    }

    public string SupTotalDisponible
    {
        get
        {
            return String.Format("{0:#,#0.00}", cUnidad.GetTotalSupByProyecto((Int16)estadoUnidad.Disponible, Id));
        }
    }

    public string SupTotalReservado
    {
        get
        {
            return String.Format("{0:#,#0.00}", cUnidad.GetTotalSupByProyecto((Int16)estadoUnidad.Reservado, Id));
        }
    }

    public string SupTotalVendido
    {
        get
        {
            return String.Format("{0:#,#0.00}", cUnidad.GetTotalSupByProyecto((Int16)estadoUnidad.Vendido, Id) + cUnidad.GetTotalSupByProyecto((Int16)estadoUnidad.Vendido_sin_boleto, Id));
        }
    }

    public string CantUnidadesDisponibles
    {
        get
        {
            return cUnidad.GetCantUnidadesDisponibles(Id).ToString();
        }
    }

    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }

    public string GetProyecto
    {
        get{
            cProyectoDAO DAO = new cProyectoDAO();
            return DAO.GetProyecto();
        }
    }

    public string ValorM2PorObra
    {
        get
        {
            decimal valor = 0;

            if (Convert.ToDecimal(SupTotalDisponible) != 0 || Convert.ToDecimal(SupTotalReservado) != 0)
            {
                if (cUnidad.GetMonedaByProyecto(Id) == Convert.ToString((Int16)tipoMoneda.Dolar))
                    valor = cUnidad.GetTotalValorAVenta(Id) / (Convert.ToDecimal(SupTotalDisponible) + Convert.ToDecimal(SupTotalReservado));
                else
                    valor = (cUnidad.GetTotalValorAVenta(Id) / (Convert.ToDecimal(SupTotalDisponible) + Convert.ToDecimal(SupTotalReservado))) / cValorDolar.LoadActualValue();
            }
            else
                valor = 0;

            return String.Format("{0:#,#0}", valor);
        }
    }
    #endregion

    #region Acceso a Datos
    public int Save()
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.Save(this);
    }

    public static cProyecto Load(string id)
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.Load(id);
    }
    public static ArrayList LoadTable()
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.LoadTable();
    }

    public static DataTable GetDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
        ArrayList valores = LoadTable();
        valores.Reverse();
        foreach (cProyecto cg in valores)
            tbl.Rows.Add(cg.Id, cg.Descripcion);
        return tbl;
    }

    public static List<cProyecto> GetProyectos()
    {
        cProyectoDAO Dao = new cProyectoDAO();
        return Dao.GetProyectos();
    }

    public static ArrayList GetProyectoByIdOperacionVenta(string _idOperacionVenta)
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.GetProyectoByIdOperacionVenta(_idOperacionVenta);
    }

    public static List<cProyecto> GetUnidadesGroupByIdProyecto(string _idOpv)
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.GetUnidadesGroupByIdProyecto(_idOpv);
    }

    public static List<cProyecto> GetProyectosSaldos()
    {
        cProyectoDAO DAO = new cProyectoDAO();
        return DAO.GetProyectosSaldos();
    }
    #endregion

    public static List<cProyecto> GetEmpresas()
    {
        cProyectoDAO Dao = new cProyectoDAO();
        return Dao.GetProyectos();
    }
}

