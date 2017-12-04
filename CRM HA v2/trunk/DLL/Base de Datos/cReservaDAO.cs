using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cReservaDAO
    {
        public string GetTable
        { get { return "tReserva"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cReserva reserva)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idEmpresa", reserva.IdEmpresa));
            lista.Add(new cAtributo("idUnidad", reserva.IdUnidad));
            lista.Add(new cAtributo("fechaVencimiento", reserva.FechaVencimiento));
            lista.Add(new cAtributo("idEmpresaUnidad", reserva.IdEmpresaUnidad));
            lista.Add(new cAtributo("importe", reserva.Importe));
            lista.Add(new cAtributo("papelera", reserva.Papelera));
            lista.Add(new cAtributo("idItemCCU", reserva.IdItemCCU));
            return lista;
        }

        public int Save(cReserva reserva)
        {
            if (string.IsNullOrEmpty(reserva.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(reserva));
            else
                return cDataBase.GetInstance().UpdateObject(reserva.Id, GetTable, AttributesClass(reserva));
        }

        public cReserva Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cReserva reserva = new cReserva();
            reserva.Id = Convert.ToString(atributos["id"]);
            reserva.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            reserva.IdUnidad = Convert.ToString(atributos["idUnidad"]);
            reserva.FechaVencimiento = Convert.ToDateTime(atributos["fechaVencimiento"]);
            reserva.IdEmpresaUnidad = Convert.ToString(atributos["idEmpresaUnidad"]);
            reserva.Importe = Convert.ToDecimal(atributos["importe"]);
            reserva.Papelera = Convert.ToInt16(atributos["papelera"]);
            reserva.IdItemCCU = Convert.ToString(atributos["idItemCCU"]);
            return reserva;
        }

        public string GetReservaByIdUnidad(string _idUnidad)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera = '" + (Int16)papelera.Activo + "' AND idUnidad= '" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public decimal GetImporteReservaByIdUnidad(string _idUnidad)
        {
            string query = "SELECT importe FROM " + GetTable + " WHERE Papelera = '" + (Int16)papelera.Activo + "' AND idUnidad= '" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(cmd));
        }
        
        public string GetIdUnidadByIdEmpresa1(string _idEmpresa)
        {
            string query = "SELECT idUnidad FROM " + GetTable + " WHERE Papelera = '" + (Int16)papelera.Activo + "' AND idEmpresa= '" + _idEmpresa + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cReserva> GetIdUnidadByIdEmpresa(string _idEmpresa)
        {
            List<cReserva> reservas = new List<cReserva>();
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera = '" + (Int16)papelera.Activo + "' AND idEmpresa= '" + _idEmpresa + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                reservas.Add(Load(Convert.ToString(idList[i])));
            }
            return reservas;
        }

        public cReserva GetReservaByIdItemCCU(string _idItemCUU)
        {
            string query = "SELECT id FROM tReserva WHERE idItemCCU='" + _idItemCUU + "' AND papelera = " + (Int16)papelera.Activo;

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public List<cReserva> GetReservasToday()
        {
            List<cReserva> reservas = new List<cReserva>();
            string query = "SELECT id FROM " + GetTable + " WHERE papelera = " + (Int16)papelera.Activo;

            SqlCommand com = new SqlCommand(query);
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = DateTime.Now;

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                reservas.Add(Load(Convert.ToString(idList[i])));
            }
            return reservas;
        }

        public List<cReserva> GetReservasByIdCCU(string _idCCU)
        {
            List<cReserva> reservas = new List<cReserva>();
            string query = "SELECT r.id FROM tItemCCU i INNER JOIN tReserva r ON i.id=r.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND i.idEstado='" + (Int16)eEstadoItem.Reserva + "' AND i.tipoOperacion='" + (Int16)eTipoOperacion.Reserva + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                reservas.Add(Load(Convert.ToString(idList[i])));
            }
            return reservas;
        }
    }
}
