using DLL.Base_de_Datos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public enum estadoUnidad { Disponible = 1, Reservado = 2, Vendido = 3, Porteria = 4, Modificado = 5, Socios = 6, Reservado_con_seña = 7, Vendido_sin_boleto = 8 };
public enum tipoMoneda { Dolar = 0, Pesos = 1 };

public enum tipoUnidad { Casa = 1, Dpto = 2, Cochera = 3, Baulera = 4, Terreno = 5 }

namespace DLL.Negocio
{
    public class cUnidad
    {
        private string id;
        private string idProyecto;
        private string codigoUF;
        private string unidadFuncional;
        private string nroUnidad;
        private string nivel;
        private string ambiente;
        private string supCubierta;
        private string supSemiDescubierta;
        private string supDescubierta;
        private string supTotal;
        private decimal porcentaje;
        private Decimal precioBaseOriginal;
        private Decimal precioBase;
        private string idEstado;
        private string moneda;
        private Int16 papelera;
        private string idUsuario;

        private Decimal precioAcordado;
        private string monedaAcordada;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
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
        public string CodigoUF
        {
            get { return codigoUF; }
            set { codigoUF = value; }
        }
        public string UnidadFuncional
        {
            get { return unidadFuncional; }
            set { unidadFuncional = value; }
        }

        public Int16 GetUnidadFuncional
        {
            get
            {
                Int16 _tipoUnidad = 0;
                switch (UnidadFuncional)
                {
                    case "Casa":
                        _tipoUnidad = (Int16)tipoUnidad.Casa;
                        break;
                    case "Dpto":
                        _tipoUnidad = (Int16)tipoUnidad.Dpto;
                        break;
                    case "Cochera":
                        _tipoUnidad = (Int16)tipoUnidad.Cochera;
                        break;
                    case "Baulera":
                        _tipoUnidad = (Int16)tipoUnidad.Baulera;
                        break;
                    case "Terreno":
                        _tipoUnidad = (Int16)tipoUnidad.Terreno;
                        break;
                }
                return _tipoUnidad;
            }
        }

        public string NroUnidad
        {
            get { return nroUnidad; }
            set { nroUnidad = value; }
        }
        public string Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }
        public string Ambiente
        {
            get { return ambiente; }
            set { ambiente = value; }
        }
        public string SupCubierta
        {
            get { return supCubierta; }
            set { supCubierta = value; }
        }
        public string GetSupCubierta
        {
            get
            {
                if (cAuxiliar.IsNumeric(supCubierta) && !string.IsNullOrEmpty(supCubierta))
                    return String.Format("{0:#,#0.00}", Convert.ToDecimal(supCubierta));
                else
                    return supCubierta;
            }
        }

        public string SupSemiDescubierta
        {
            get { return supSemiDescubierta; }
            set { supSemiDescubierta = value; }
        }
        public string GetSupSemiDescubierta
        {
            get
            {
                if (cAuxiliar.IsNumeric(SupSemiDescubierta) && !string.IsNullOrEmpty(SupSemiDescubierta))
                    return String.Format("{0:#,#0.00}", Convert.ToDecimal(SupSemiDescubierta));
                else
                    return SupSemiDescubierta;
            }
        }
        public string SupDescubierta
        {
            get { return supDescubierta; }
            set { supDescubierta = value; }
        }
        public string GetSupDescubierta
        {
            get
            {
                if (cAuxiliar.IsNumeric(SupDescubierta) && !string.IsNullOrEmpty(SupDescubierta))
                    return String.Format("{0:#,#0.00}", Convert.ToDecimal(SupDescubierta));
                else
                    return SupDescubierta;
            }
        }
        public string SupTotal
        {
            get { return supTotal; }
            set { supTotal = value; }
        }
        public string GetSupTotal
        {
            get
            {
                //if (cAuxiliar.IsNumeric(SupTotal) && !string.IsNullOrEmpty(SupTotal))
                return String.Format("{0:#,#0.00}", SupTotal);
                /*else
                    return SupTotal;*/
            }
        }
        public decimal Porcentaje
        {
            get { return porcentaje; }
            set { porcentaje = value; }
        }
        public Decimal PrecioBase
        {
            get { return precioBase; }
            set { precioBase = value; }
        }
        public string GetPrecioBase
        {
            get
            {
                if (IdEstado != Convert.ToString((Int16)estadoUnidad.Vendido) && IdEstado != Convert.ToString((Int16)estadoUnidad.Vendido_sin_boleto))
                    return String.Format("{0:#,#}", precioBase);
                else
                    return "-";
            }
        }

        public Decimal PrecioBaseOriginal
        {
            get { return precioBaseOriginal; }
            set { precioBaseOriginal = value; }
        }
        public string GetPrecioBaseOriginal
        {
            get { return String.Format("{0:#,#}", precioBaseOriginal); }
        }
        public string IdEstado
        {
            get { return idEstado; }
            set { idEstado = value; }
        }
        public string GetEstado
        {
            get
            {
                string estado = null;
                switch (IdEstado)
                {
                    case "1": estado = estadoUnidad.Disponible.ToString();
                        break;
                    case "2": estado = estadoUnidad.Reservado.ToString();
                        break;
                    case "3": estado = estadoUnidad.Vendido.ToString();
                        break;
                    case "4": estado = estadoUnidad.Porteria.ToString();
                        break;
                    case "5": estado = estadoUnidad.Modificado.ToString();
                        break;
                    case "6": estado = estadoUnidad.Socios.ToString();
                        break;
                    case "7": estado = estadoUnidad.Reservado_con_seña.ToString().Replace("_", " ");
                        break;
                    case "8": estado = estadoUnidad.Vendido_sin_boleto.ToString().Replace("_", " ");
                        break;
                }
                return estado;
            }
        }
        public string Moneda
        {
            get { return moneda; }
            set { moneda = value; }
        }
        public string GetMoneda
        {
            get
            {
                string moneda = null;
                switch (Moneda)
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
        public static string MonedaById(string _moneda)
        {
            string moneda = null;
            switch (_moneda)
            {
                case "0": moneda = tipoMoneda.Dolar.ToString();
                    break;
                case "1": moneda = tipoMoneda.Pesos.ToString();
                    break;
            }
            return moneda;
        }

        public static string Estado(string _idEstado)
        {
            string estado = null;
            switch (_idEstado)
            {
                case "1": estado = estadoUnidad.Disponible.ToString();
                    break;
                case "2": estado = estadoUnidad.Reservado.ToString();
                    break;
                case "3": estado = estadoUnidad.Vendido.ToString();
                    break;
                case "4": estado = estadoUnidad.Porteria.ToString();
                    break;
                case "5": estado = estadoUnidad.Modificado.ToString();
                    break;
                case "6": estado = estadoUnidad.Socios.ToString();
                    break;
                case "7": estado = estadoUnidad.Reservado_con_seña.ToString();
                    break;
                case "8": estado = estadoUnidad.Vendido_sin_boleto.ToString();
                    break;
            }
            return estado;
        }

        public string idEmpresa
        {
            get
            {
                string idEmpresa = "-";
                cEmpresa empresa = cEmpresaUnidad.GetEmpresaByUnidad(CodigoUF, IdProyecto);
                if (empresa != null)
                    idEmpresa = cEmpresaUnidad.GetEmpresaByUnidad(CodigoUF, IdProyecto).Id;
                return idEmpresa;
            }
        }
        public string GetEmpresa
        {
            get
            {
                string nombre = "-";
                cEmpresa empresa = cEmpresaUnidad.GetEmpresaByUnidad(CodigoUF, IdProyecto);
                if (empresa != null)
                    nombre = empresa.Apellido + ", " + empresa.Nombre;
                return nombre;
            }
        }

        public string FechaAdquision
        {
            get
            {
                cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(CodigoUF, IdProyecto);
                cCuentaCorriente cc = cCuentaCorriente.GetCuentaCorrienteByIdEmpresaUnidad(eu.Id);
                if (cc == null) return "-";
                return cCuota.GetFirst(cc.Id).Fecha.ToString();
            }
        }
        public Int16 Papelera
        {
            get { return papelera; }
            set { papelera = value; }
        }
        public string IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        public string GetIdUsuario
        {
            get
            {
                if (!string.IsNullOrEmpty(IdUsuario) && IdUsuario != "-1")
                    return cUsuario.Load(IdUsuario).Nombre;
                else
                    return "-";
            }
        }

        public Decimal PrecioAcordado
        {
            get { return precioAcordado; }
            set { precioAcordado = value; }
        }
        public string GetPrecioAcordado
        {
            get { return String.Format("{0:#,#}", precioAcordado); }
        }

        public string ValorM2
        {
            get
            {
                string valor = null;
                if (SupTotal != "0" && SupTotal != "0,00")
                    valor = String.Format("{0:#,#}", PrecioBase / Convert.ToDecimal(SupTotal));
                else
                    valor = "0";
                return valor;
            }
        }

        public string GetImporteReserva
        {
            get
            {
                return String.Format("{0:#,#0.00}", cReserva.GetImporteReservaByIdUnidad(Id));
            }
        }

        public string GetFechaBoleto
        {
            get
            {
                return GetFechaBoletoByUnidad(Id).ToString("dd/MM/yyyy");
            }
        }

        public string GetFechaPosesion
        {
            get
            {
                return GetFechaPosesionByUnidad(Id);
            }
        }

        public string GetFechaEscritura
        {
            get
            {
                return GetFechaEscrituraByUnidad(Id);
            }
        }


        //public string ValorM2PorObra
        //{
        //    get
        //    {
        //        decimal valor = 0;

        //        if (Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
        //            valor = (GetTotalValorAVenta(IdProyecto) / Convert.ToDecimal(GetSupTotal)) * cValorDolar.LoadActualValue();
        //        else
        //            valor = GetTotalValorAVenta(IdProyecto) / Convert.ToDecimal(GetSupTotal);

        //        return String.Format("{0:#,#}", valor);
        //    }
        //}

        #endregion

        public int Save()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.Save(this);
        }

        public static cUnidad Load(string id)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.Load(id);
        }

        public static ArrayList LoadTable(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.LoadTable(_idProyecto);
        }

        public static cUnidad LoadByCodUF(string _codUF, string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.LoadByCodUF(_codUF, _idProyecto);
        }

        public static cUnidad LoadByIdEmpresaUnidad(string _idEmpresaUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.LoadByIdEmpresaUnidad(_idEmpresaUnidad);
        }

        public static DataTable GetAmbientes(string _idProyecto)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("ambiente", typeof(string)));
            ArrayList valores = LoadTable(_idProyecto);
            foreach (cUnidad u in valores)
            {
                if (!string.IsNullOrEmpty(u.ambiente) || u.Ambiente == "-")
                    tbl.Rows.Add(u.Id, u.Ambiente);
            }
            return tbl;
        }

        public static List<cUnidad> GetUnidadesByIdProyecto(string idProyecto)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.GetUnidadesByIdProyecto(idProyecto);
        }

        public static List<cUnidad> GetUnidadesByIdProyectoSinUnidadesModificadas(string idProyecto)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.GetUnidadesByIdProyectoSinUnidadesModificadas(idProyecto);
        }

        public static List<cUnidad> GetUnidadesByIdEmpresa(string _idEmpresa)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.GetUnidadesByIdEmpresa(_idEmpresa);
        }

        public static List<cUnidad> Search(string _filtro, string _idProyecto, string _idEstado, string _idUnidad, string _idAmbiente, string _superficie, string _supMin, string _supMax, string _precioMin, string _precioMax)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.Search(_filtro, _idProyecto, _idEstado, _idUnidad, _idAmbiente, _superficie, _supMin, _supMax, _precioMin, _precioMax);
        }

        public static List<cUnidad> GetUnidadesByRango(string nivel, string nivelHasta, string idProyecto)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.GetUnidadesByRango(nivel, nivelHasta, idProyecto);
        }

        public static DataTable SearchExport(string _filtro, string _idProyecto, string _idEstado, string _superficie, string _supMin, string _supMax, string _precioMin, string _precioMax)
        {
            cUnidadDAO Dao = new cUnidadDAO();
            return Dao.SearchExport(_filtro, _idProyecto, _idEstado, _superficie, _supMin, _supMax, _precioMin, _precioMax);
        }

        public static string GetClienteByUnidad(string _idCliente)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetClienteByUnidad(_idCliente);
        }

        public static string GetTotalSupTotal(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                    sum += Convert.ToDecimal(u.supTotal);
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalSupSemiDescubierta(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                {
                    if (cAuxiliar.IsNumeric(u.supSemiDescubierta) && !string.IsNullOrEmpty(u.supSemiDescubierta))
                        sum += Convert.ToDecimal(u.supSemiDescubierta);
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalSupDescubierta(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                {
                    if (cAuxiliar.IsNumeric(u.SupDescubierta) && !string.IsNullOrEmpty(u.SupDescubierta))
                        sum += Convert.ToDecimal(u.SupDescubierta);
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalSupCubierta(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                {
                    if (cAuxiliar.IsNumeric(u.SupCubierta) && !string.IsNullOrEmpty(u.SupCubierta))
                        sum += Convert.ToDecimal(u.SupCubierta);
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static decimal GetTotalPorcentaje(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                    sum += u.Porcentaje;
            }
            return Math.Round(sum);
        }

        public static string GetTotalPrecioBase(List<cUnidad> unidades)
        {
            decimal sum = 0;
            if (unidades != null)
            {
                foreach (cUnidad u in unidades)
                    sum += u.precioBase;
            }

            return String.Format("{0:#,#}", sum);
        }

        public static ArrayList LoadNivelByIdProyecto(string _idProyecto, string _unidadFuncional)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return GetDistinctArrayList(DAO.LoadNivelByIdProyecto(_idProyecto, _unidadFuncional));
        }

        public static ArrayList LoadNroUnidadByIdProyectoCC(string _idProyecto, string _nivel) //BORRAR ES PARA EL CC
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.LoadNroUnidadByIdProyectoCC(_idProyecto, _nivel);
        }

        public static ArrayList GetNroUnidadByIdProyecto(string _idProyecto, string _nivel)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetNroUnidadByIdProyecto(_idProyecto, _nivel);
        }

        public static ArrayList GetNroUnidadMotivoByIdProyecto(string _idProyecto, string _nivel)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetNroUnidadMotivoByIdProyecto(_idProyecto, _nivel);
        }

        public static ArrayList GetNroUnidadReservadaByIdProyecto(string _idProyecto, string _nivel, string _idEmpresa)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetNroUnidadReservadaByIdProyecto(_idProyecto, _nivel, _idEmpresa);
        }

        public static List<cUnidad> GetNroUnidadByIdProyecto1(string _idProyecto, string _nivel)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetNroUnidadByIdProyecto1(_idProyecto, _nivel);
        }

        public static ArrayList LoadNroUnidadByIdProyecto(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetTotalesByMoneda(_idProyecto);
        }

        public static ArrayList GroupByUnidadFuncional(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GroupByUnidadFuncional(_idProyecto);
        }

        public static ArrayList GroupByNivel(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GroupByNivel(_idProyecto);
        }

        public static ArrayList GetDistinctArrayList(ArrayList arr)
        {
            ArrayList myStringList = new ArrayList();
            foreach (cUnidad s in arr)
            {
                if (!myStringList.Contains(s.Nivel))
                {
                    myStringList.Add(s.Nivel);
                }
            }
            return myStringList;
        }

        public static string GetMonedaByProyecto(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetMonedaByProyecto(_idProyecto);
        }

        public static cUnidad GetUnidadByProyecto(string _idProyecto, string _nivel, string _nroUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadByProyecto(_idProyecto, _nivel, _nroUnidad);
        }

        public static cUnidad GetUnidadIguales(string _idProyecto, string _nivel, string _nroUnidad, string _codUF)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadIguales(_idProyecto, _nivel, _nroUnidad, _codUF);
        }

        public static List<cUnidad> GetListUnidadByProyecto(string _idProyecto, string _nivel, string _nroUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetListUnidadByProyecto(_idProyecto, _nivel, _nroUnidad);
        }

        public static cUnidad GetUnidadByProyectoAndUF(string _idProyecto, string _nroUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadByProyectoAndUF(_idProyecto, _nroUnidad);
        }

        public static Dictionary<string, string> GetEstadoUnidadesPorProyecto()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            Dictionary<string, string> d = new Dictionary<string, string>();
            List<cProyecto> proyectos = cProyecto.GetProyectos();

            foreach (cProyecto p in proyectos)
            {
                d.Add(p.Descripcion, DAO.GetEstadoUnidadesPorProyecto(p.Id));
            }

            return d;
        }

        public static List<cUnidad> GetUnidadByIdOV(string _idOperacionVenta)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadByIdOV(_idOperacionVenta);
        }

        public static ArrayList LoadTableByIdEmpresa(string _idEmpresa)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.LoadTableByIdEmpresa(_idEmpresa);
        }
        
        public static DataTable List<cUnidad>(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadesVendidas(_idProyecto);
        }

        public static List<cUnidad> GetListUnidadesVendidas(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetListUnidadesVendidas(_idProyecto);
        }

        public static DataTable GetUnidadesVendidas(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadesVendidas(_idProyecto);
        }

        public static DataTable GetDataTableProyectoByIdEmpresa(string _idEmpresa)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
            ArrayList valores = LoadTableByIdEmpresa(_idEmpresa);
            valores.Reverse();
            foreach (cUnidad cg in valores)
                tbl.Rows.Add(cg.Id, cg.Nivel);
            return tbl;
        }

        public static decimal GetUnidadesByEstado(Int16 _idEstado, string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadesByEstado(_idEstado, _idProyecto);
        }

        public static DataTable GetMinCantidadEstado(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetMinCantidadEstado(_idProyecto);
        }

        public static List<cUnidad> GetUnidadByOV(string _idOperacionVenta)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetUnidadByOV(_idOperacionVenta);
        }

        public static ArrayList GetCantProyectosByOV(string _idOperacionVenta)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetCantProyectosByOV(_idOperacionVenta);
        }

        public static Int16 GetCantidadUnidadesVendidas(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetCantidadUnidadesVendidas(_idProyecto);
        }

        #region Combos
        public static ListItemCollection CargarComboTipoUnidad()
        {
            ListItemCollection collection = new ListItemCollection();
            string[] unidades = Enum.GetNames(typeof(tipoUnidad));

            collection.Add(new ListItem("Todas", "0"));

            foreach (string item in unidades)
            {
                int value = (int)Enum.Parse(typeof(tipoUnidad), item);
                collection.Add(new ListItem(item.Replace("_", " "), item.Replace("_", " ")));
            }

            return collection;
        }

        public static ListItemCollection CargarComboEstadoUnidad()
        {
            ListItemCollection collection = new ListItemCollection();
            string[] unidades = Enum.GetNames(typeof(estadoUnidad));

            foreach (string item in unidades)
            {
                int value = (int)Enum.Parse(typeof(estadoUnidad), item);
                collection.Add(new ListItem(item.Replace("_", " "), value.ToString()));
            }

            return collection;
        }

        public static ListItemCollection CargarComboAmbiente()
        {
            ListItemCollection collection = new ListItemCollection();

            collection.Add(new ListItem("Todos", "0"));
            collection.Add(new ListItem("1", "1"));
            collection.Add(new ListItem("2", "2"));
            collection.Add(new ListItem("2 1/2", "2 1/2"));
            collection.Add(new ListItem("3", "3"));
            collection.Add(new ListItem("3 1/2", "3 1/2"));
            collection.Add(new ListItem("4", "4"));
            collection.Add(new ListItem("5", "5"));

            return collection;
        }

        /*public static string GetTotalSupTotalDisponibles()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            decimal sum = 0;
            foreach(var a in DAO.GetTotalSupTotalDisponibles())
            {
                sum = sum + Convert.ToDecimal(a);
            }
            return String.Format("{0:#,#.00}", sum);
        }
        public static string GetTotalSupTotalVendidos()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            decimal sum = 0;
            foreach (var a in DAO.GetTotalSupTotalVendidos())
            {
                sum = sum + Convert.ToDecimal(a);
            }
            return String.Format("{0:#,#.00}", sum);
        }*/

        public static string GetUnidadesDisponiblesPesos()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            decimal sum = 0;
            foreach (cUnidad u in DAO.GetUnidadesDisponibles())
            {
                if (u.GetMoneda == tipoMoneda.Dolar.ToString())
                    sum = sum + cValorDolar.ConvertToPeso(u.PrecioBase);
                else
                    sum = sum + u.PrecioBase;
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetUnidadesDisponiblesDolar()
        {
            cUnidadDAO DAO = new cUnidadDAO();
            decimal sum = 0;
            foreach (cUnidad u in DAO.GetUnidadesDisponibles())
            {
                if (u.GetMoneda == tipoMoneda.Pesos.ToString())
                    sum = sum + cValorDolar.ConvertToDolar(u.PrecioBase);
                else
                    sum = sum + u.PrecioBase;
            }
            return String.Format("{0:#,#0.00}", sum);
        }


        public static DateTime GetFechaBoletoByUnidad(string _idUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetFechaBoletoByUnidad(_idUnidad);
        }
        public static string GetFechaPosesionByUnidad(string _idUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetFechaPosesionByUnidad(_idUnidad);
        }

        public static string GetFechaEscrituraByUnidad(string _idUnidad)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetFechaEscrituraByUnidad(_idUnidad);
        }

        public static decimal GetTotalSupTotal(Int16 _idEstado)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetTotalSupTotal(_idEstado);
        }

        public static decimal GetTotalSupByProyecto(Int16 _idEstado, string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetTotalSupByProyecto(_idEstado, _idProyecto);
        }

        public static decimal GetTotalValorAVenta(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetTotalValorAVenta(_idProyecto);
        }

        public static Int16 GetCantUnidadesDisponibles(string _idProyecto)
        {
            cUnidadDAO DAO = new cUnidadDAO();
            return DAO.GetCantUnidadesDisponibles(_idProyecto);
        }
        #endregion
    }
}

