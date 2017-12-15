using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public enum estadoOperacionVenta { A_confirmar = 0, Activo = 1, Cancelado = 2, Anulado = 3, Todas = 4 }

namespace DLL.Negocio
{
    public class cOperacionVenta
    {
        private string id;
        private string idEmpresaUnidad;
        private string monedaAcordada;
        private decimal anticipo;
        private decimal precioAcordado;
        private string idIndiceCAC;
        private decimal totalComision;
        private bool iva;
        private decimal valorDolar;
        private Int16 idEstado;
        private DateTime fecha;
        private bool cac;
        private bool uva;
        private decimal valorBaseUVA;
        private DateTime? fechaPosesion;               
        private DateTime? fechaEscritura;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string IdEmpresaUnidad
        {
            get { return idEmpresaUnidad; }
            set { idEmpresaUnidad = value; }
        }

        public string GetEmpresa
        {
            get { return cEmpresaUnidad.Load(idEmpresaUnidad).GetEmpresa; }
        }

        public string GetProyecto
        {
            get { return cEmpresaUnidad.Load(idEmpresaUnidad).GetProyecto; }
        }

        public string MonedaAcordada
        {
            get { return monedaAcordada; }
            set { monedaAcordada = value; }
        }

        public string GetMoneda
        {
            get
            {
                string moneda = null;
                switch (MonedaAcordada)
                {
                    case "0":
                        moneda = tipoMoneda.Dolar.ToString();
                        break;
                    case "1":
                        moneda = tipoMoneda.Pesos.ToString();
                        break;
                }
                return moneda;
            }
        }
        public decimal Anticipo
        {
            get { return anticipo; }
            set { anticipo = value; }
        }
        public string GetAnticipo
        {
            get { return String.Format("{0:#,#0.00}", Anticipo); }
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
        public string GetPrecioAcordadoAPesos
        {
            get {
                decimal precio = 0;

                if (GetMoneda == tipoMoneda.Dolar.ToString())
                    precio = PrecioAcordado * ValorDolar;
                else
                    precio = PrecioAcordado;

                return String.Format("{0:#,#0.00}", precio);
            }
        }

        public string IdIndiceCAC
        {
            get { return idIndiceCAC; }
            set { idIndiceCAC = value; }
        }
        public string GetIdIndiceCAC
        {
            get
            {
                if (IdIndiceCAC != "0" && IdIndiceCAC != "1" && IdIndiceCAC != "-1")
                    return String.Format("{0:#,#0.00}", cIndiceCAC.Load(IdIndiceCAC).Valor);
                else
                    return "-";
            }
        }
        public decimal TotalComision
        {
            get { return totalComision; }
            set { totalComision = value; }
        }
        public string GetTotalComision
        {
            get { return String.Format("{0:#,#0.00}", TotalComision); }
        }

        public bool Iva
        {
            get { return iva; }
            set { iva = value; }
        }
        public decimal ValorDolar
        {
            get { return valorDolar; }
            set { valorDolar = value; }
        }
        public string GetValorDolar
        {
            get { return String.Format("{0:#,#0.00}", ValorDolar); }
        }

        public Int16 IdEstado
        {
            get { return idEstado; }
            set { idEstado = value; }
        }

        public string GetEstado
        {
            get
            {
                string _estado = null;
                switch (IdEstado)
                {
                    case 0:
                        _estado = estadoOperacionVenta.A_confirmar.ToString().Replace("_", " ");
                        break;
                    case 1:
                        _estado = estadoOperacionVenta.Activo.ToString();
                        break;
                    case 2:
                        _estado = estadoOperacionVenta.Cancelado.ToString();
                        break;
                    case 3:
                        _estado = estadoOperacionVenta.Anulado.ToString();
                        break;
                }
                return _estado;
            }
        }

        public string GetPrecioBase
        {
            get { return cEmpresaUnidad.Load(IdEmpresaUnidad).GetPrecioBase; }
        }

        public Int16 GetCantCuotas
        {
            get { return cFormaPagoOV.LoadByIdOV(Id).CantCuotas; }
        }

        public DateTime? FechaVencimiento
        {
            get { return cFormaPagoOV.LoadByIdOV(Id).FechaVencimiento; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string GetFecha
        {
            get
            { return Fecha.ToString("dd/MM/yyyy"); }
        }

        public bool Cac
        {
            get { return cac; }
            set { cac = value; }
        }

        public bool Uva
        {
            get { return uva; }
            set { uva = value; }
        }

        public decimal ValorBaseUVA
        {
            get { return valorBaseUVA; }
            set { valorBaseUVA = value; }
        }

        public DateTime? FechaPosesion
        {
            get { return fechaPosesion; }
            set { fechaPosesion = value; }
        }
        public string GetFechaPosesion
        {
            get
            {
                string fecha = null;
                if (FechaPosesion != null)
                    fecha = Convert.ToDateTime(FechaPosesion).ToString("dd/MM/yyyy");
                else
                    fecha = "-";

                return fecha;
            }
        }
        public DateTime? FechaEscritura
        {
            get { return fechaEscritura; }
            set { fechaEscritura = value; }
        }
        public string GetFechaEscritura
        {
            get
            {
                string fecha = null;
                if (FechaEscritura != null)
                    fecha = Convert.ToDateTime(FechaEscritura).ToString("dd/MM/yyyy");
                else
                    fecha = "-";

                return fecha;
            }
        }
        #endregion

        public int Save()
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.Save(this);
        }

        public static cOperacionVenta Load(string id)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.Load(id);
        }

        public static List<cOperacionVenta> GetOperacionesVenta()
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetOperacionesVenta();
        }

        public static List<cOperacionVenta> Search(string _idEmpresa, string _idProyecto, Int16 _idEstado, Int16 _monedaIndice, string _desde, string _hasta)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.Search(_idEmpresa, _idProyecto, _idEstado, _monedaIndice, _desde, _hasta);
        }

        public static ListItemCollection CargarComboEstadoOV()
        {
            ListItemCollection collection = new ListItemCollection();
            string[] unidades = Enum.GetNames(typeof(estadoOperacionVenta));

            foreach (string item in unidades)
            {
                int value = (int)Enum.Parse(typeof(estadoOperacionVenta), item);
                collection.Add(new ListItem(item.Replace("_", " "), value.ToString()));
            }

            return collection;
        }

        public static ListItemCollection CargarComboMonedaIndiceOV()
        {
            ListItemCollection collection = new ListItemCollection();
            string[] unidades = Enum.GetNames(typeof(eMonedaIndice));

            foreach (string item in unidades)
            {
                int value = (int)Enum.Parse(typeof(eMonedaIndice), item);

                switch(value){
                    case (Int16)eMonedaIndice.CAC:
                        collection.Add(new ListItem(item.Replace(eMonedaIndice.CAC.ToString(), "Pesos/CAC"), value.ToString()));
                        break;
                    case (Int16)eMonedaIndice.UVA:
                        collection.Add(new ListItem(item.Replace(eMonedaIndice.UVA.ToString(), "Pesos/UVA"), value.ToString()));
                        break;
                    default:
                        collection.Add(new ListItem(item, value.ToString()));
                        break;
                }                
            }

            return collection;
        }

        public static List<cOperacionVenta> GetOV_AConfirmar()
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetOV_AConfirmar();
        }

        public static List<cOperacionVenta> GetOVByIdEmpresa(string _idEmpresa)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetOVByIdEmpresa(_idEmpresa);
        }

        public static string GetEmpresaByIdOv(string _idOv)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetEmpresaByIdOv(_idOv);
        }

        public static string GetTotalPrecioBase(List<cOperacionVenta> ovs)
        {
            decimal sum = 0;
            if (ovs != null)
            {
                foreach (cOperacionVenta o in ovs)
                {
                    decimal precioBase = Convert.ToDecimal(o.GetPrecioBase);
                    if (cAuxiliar.IsNumeric(precioBase.ToString()) && !string.IsNullOrEmpty(precioBase.ToString()))
                        sum += Convert.ToDecimal(o.GetPrecioBase);
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalPrecioAcordado(List<cOperacionVenta> ovs)
        {
            decimal sum = 0;
            if (ovs != null)
            {
                foreach (cOperacionVenta o in ovs)
                {
                    if (cAuxiliar.IsNumeric(o.PrecioAcordado.ToString()) && !string.IsNullOrEmpty(o.PrecioAcordado.ToString()))
                        sum += Convert.ToDecimal(o.PrecioAcordado.ToString());
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public Dictionary<string, int> GetAsignacionesPendientesPorUsuario(DateTime dateDesde, DateTime dateHasta)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetAsignacionesPendientesPorUsuario(dateDesde, dateHasta);
        }

        public static cOperacionVenta GetOperacionByFormaPago(string _idFormaPago)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetOperacionByFormaPago(_idFormaPago);
        }

        public static cOperacionVenta GetIdOperacionVentaByUnidad(string _idUnidad)
        {
            cOperacionVentaDAO DAO = new cOperacionVentaDAO();
            return DAO.GetIdOperacionVentaByUnidad(_idUnidad);
        }
    }
}
