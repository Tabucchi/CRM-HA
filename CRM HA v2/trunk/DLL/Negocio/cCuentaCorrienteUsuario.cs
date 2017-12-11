using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cCuentaCorrienteUsuario
    {
        private string id;
        private string idEmpresa;
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
        public string GetSaldo
        {
            get
            {
                decimal saldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Id));
                return String.Format("{0:#,#0.00}", saldo);
            }
        }
        public string GetSaldoPositivo
        {
            get
            {
                decimal saldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Id));
                return String.Format("{0:#,#0.00}", saldo * -1);
            }
        }
        public string GetTotalDeuda
        {
            get
            {
                List<cCuota> cuotas = cCuota.GetCuotasActivasByEmpresa(IdEmpresa);
                decimal total = 0;

                if (cuotas != null)
                {
                    foreach (cCuota c in cuotas)
                    {
                        if (c.GetMoneda == tipoMoneda.Dolar.ToString())
                            total += c.Monto * cValorDolar.LoadActualValue();
                        else
                            total += c.Monto;
                    }
                }
                
                return String.Format("{0:#,#0.00}", total);

            }
        }

        public string GetTotal
        {
            get
            {
                decimal total = Convert.ToDecimal(GetSaldoPositivo) + Convert.ToDecimal(GetTotalDeuda);
                return String.Format("{0:#,#0.00}", total);
            }
        }

        public Int16 Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }
        #endregion

        public cCuentaCorrienteUsuario() { }

        public cCuentaCorrienteUsuario(string _idEmpresa)
        {
            idEmpresa = _idEmpresa;
            _papelera = (Int16)papelera.Activo;
            this.Save();
        }

        public int Save()
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.Save(this);
        }

        public static cCuentaCorrienteUsuario Load(string id)
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.Load(id);
        }

        public static List<cCuentaCorrienteUsuario> GetCuentaCorriente()
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetCuentaCorriente();
        }

        public static List<cCuentaCorrienteUsuario> GetCuentaCorrienteConSaldo()
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetCuentaCorrienteConSaldo();
        }

        public static decimal GetTotalCtaCte()
        {
            decimal total = 0;
            List<cCuentaCorrienteUsuario> ccus = GetCuentaCorriente();

            foreach (cCuentaCorrienteUsuario c in ccus)
            {
                total += Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(c.Id));
            }

            return total;
        }

        public static List<cCuentaCorrienteUsuario> GetCuentaCorrienteConOperacionVenta()
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetCuentaCorrienteConOperacionVenta();
        }

        public static List<cCuentaCorrienteUsuario> GetListCuentaCorrienteByIdEmpresa(string _idEmpresa)
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetListCuentaCorrienteByIdEmpresa(_idEmpresa);
        }

        public static List<cCuentaCorrienteUsuario> GetListCuentaCorrienteByEmpresa(string nombre, string apellido)
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetListCuentaCorrienteByEmpresa(nombre, apellido);
        }

        public static string GetCuentaCorrienteByIdEmpresa(string _idEmpresa)
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetCuentaCorrienteByIdEmpresa(_idEmpresa);
        }

        public static Dictionary<string, string> GetSaldoProyecto()
        {
            cCuentaCorrienteUsuarioDAO DAO = new cCuentaCorrienteUsuarioDAO();
            return DAO.GetSaldoProyecto();
        }
    }
}

