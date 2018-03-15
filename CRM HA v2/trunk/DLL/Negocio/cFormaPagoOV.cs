using DLL.Base_de_Datos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cFormaPagoOV
    {
        private string id;
        private string idOperacionVenta;
        private string moneda;
        private decimal monto;
        private decimal saldo;
        private Int16 cantCuotas;
        private string rangoCuotaCAC;
        private string gastosAdtvo;
        private decimal interesAnual;
        private decimal valor;
        private DateTime? fechaVencimiento;
        private Int16 papelera;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdOperacionVenta
        {
            get { return idOperacionVenta; }
            set { idOperacionVenta = value; }
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
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }
        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }
        public string GetSaldo
        {
            get { return String.Format("{0:#,#0.00}", Saldo); }
        }
        public string GetAnticipo
        {
            get { return String.Format("{0:#,#0.00}", Monto); }
        }

        public Int16 CantCuotas
        {
            get { return cantCuotas; }
            set { cantCuotas = value; }
        }
        public string RangoCuotaCAC
        {
            get { return rangoCuotaCAC; }
            set { rangoCuotaCAC = value; }
        }
        public string GetRangoCuotaCAC
        {
            get
            {
                string rango = null;
                switch (rangoCuotaCAC)
                {
                    case "-1":
                        rango = "Si";
                        break;
                    case "0":
                        rango = "No";
                        break;
                    default:
                        rango = rangoCuotaCAC;
                        break;
                }
                return rango;
            }
        }
        public string GastosAdtvo
        {
            get { return gastosAdtvo; }
            set { gastosAdtvo = value; }
        }
        public string GetGastosAdtvo
        {
            get
            {
                string gasto = null;
                switch (gastosAdtvo)
                {
                    case "0":
                        gasto = eGastosAdtvo.No.ToString();
                        break;
                    case "1":
                        gasto = eGastosAdtvo.Si.ToString();
                        break;
                }
                return gasto;
            }
        }
        public decimal InteresAnual
        {
            get { return interesAnual; }
            set { interesAnual = value; }
        }
        public string GetInteresAnual
        {
            get { return String.Format("{0:#,#0.00}", InteresAnual); }
        }
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        public string GetValor
        {
            get { return String.Format("{0:#,#0.00}", Valor); }
        }

        public DateTime? FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }
        public Int16 Papelera
        {
            get { return papelera; }
            set { papelera = value; }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.Save(this);
        }

        public static cFormaPagoOV Load(string id)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.Load(id);
        }

        public static cFormaPagoOV LoadByIdOV(string _idOV)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.LoadByIdOV(_idOV);
        }

        public static List<cFormaPagoOV> GetFormaPagoOVByIdOV(string _idOperacionVenta)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.GetFormaPagoOVByIdOV(_idOperacionVenta);
        }

        public static List<cFormaPagoOV> GetFormaPagoOVByIdOVActivas(string _idOperacionVenta)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.GetFormaPagoOVByIdOVActivas(_idOperacionVenta);
        }

        public static ArrayList LoadTableFormaPago(string _idEmpresa, string _idUnidad)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.LoadTableFormaPago(_idEmpresa, _idUnidad);
        }

        public static Int16 GetCantCuotas(string _idOperacionVenta, Int16 moneda, bool cac, bool uva)
        {
            cFormaPagoOVDAO DAO = new cFormaPagoOVDAO();
            return DAO.GetCantCuotas(_idOperacionVenta, moneda, cac, uva);
        }

        public static DataTable GetDataTableFormaPago(string _idEmpresa, string _idUnidad)
        {
            int count = 1;

            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
            ArrayList valores = LoadTableFormaPago(_idEmpresa, _idUnidad);
            //valores.Reverse();
            foreach (cFormaPagoOV cg in valores)
            {
                tbl.Rows.Add(cg.Id, count);
                count++;
            }
            return tbl;
        }

        public static string GetTotalSaldo(string _idOV, string _monedaAcordada, decimal _dolar)
        {
            List<cFormaPagoOV> saldos = GetFormaPagoOVByIdOV(_idOV);
            decimal saldo = 0;
            foreach (cFormaPagoOV fp in saldos)
            {
                if (_monedaAcordada == Convert.ToString((Int16)tipoMoneda.Dolar))
                {
                    if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
                        saldo += fp.Saldo;
                    else
                        saldo += cValorDolar.ConvertToDolar(fp.Saldo, _dolar);
                }

                if (_monedaAcordada == Convert.ToString((Int16)tipoMoneda.Pesos))
                {
                    if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
                        saldo += cValorDolar.ConvertToPeso(fp.Saldo, _dolar);
                    else
                        saldo += fp.Saldo;
                }
            }

            return String.Format("{0:#,#0.00}", saldo);
        }
        #endregion
    }
}
