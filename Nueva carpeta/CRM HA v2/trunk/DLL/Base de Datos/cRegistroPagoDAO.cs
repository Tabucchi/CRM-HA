using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cRegistroPagoDAO
    {
        public string GetTable
        { get { return "tRegistroPago"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cRegistroPago pago)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("fechaPago", pago.FechaPago));
            lista.Add(new cAtributo("monto", pago.Monto));
            lista.Add(new cAtributo("sucursal", pago.Sucursal));
            lista.Add(new cAtributo("transaccion", pago.Transaccion));
            lista.Add(new cAtributo("idImagen", pago.IdImagen));
            lista.Add(new cAtributo("idEmpresa", pago.IdEmpresa));
            lista.Add(new cAtributo("idEstado", pago.IdEstado));
            lista.Add(new cAtributo("idCC", pago.IdCC));
            lista.Add(new cAtributo("nro", pago.Nro));
            lista.Add(new cAtributo("formaPago", pago.FormaPago));
            return lista;
        }

        public int Save(cRegistroPago pago)
        {
            if (string.IsNullOrEmpty(pago.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(pago));
            else
                return cDataBase.GetInstance().UpdateObject(pago.Id, GetTable, AttributesClass(pago));
        }

        public cRegistroPago Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cRegistroPago pago = new cRegistroPago();
            pago.Id = Convert.ToString(atributos["id"]);
            pago.FechaPago = Convert.ToDateTime(atributos["fechaPago"]);
            pago.Monto = Convert.ToDecimal(atributos["monto"]);
            pago.Sucursal = Convert.ToString(atributos["sucursal"]);
            pago.Transaccion = Convert.ToString(atributos["transaccion"]);
            pago.IdImagen = Convert.ToInt32(atributos["idImagen"]);
            pago.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            pago.IdEstado = Convert.ToInt16(atributos["idEstado"]);
            pago.IdCC = Convert.ToInt16(atributos["idCC"]);
            pago.Nro = Convert.ToInt16(atributos["nro"]);
            pago.FormaPago = Convert.ToInt16(atributos["formaPago"]);
            return pago;
        }

        public string GetCancelarRegistro(string _id)
        {
            string query = "DELETE FROM " + GetTable + " WHERE id=" + _id;
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cRegistroPago> GetAllRegistrosByCC(string _idCC)
        {
            List<cRegistroPago> registroPago = new List<cRegistroPago>();
            string query = "SELECT id FROM tRegistroPago WHERE idCC = '" + _idCC + "' ORDER BY id ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
            {
                registroPago.Add(Load(Convert.ToString(idList[i])));
            }
            return registroPago;
        }

        public List<cRegistroPago> GetRegistros()
        {
            List<cRegistroPago> registroPago = new List<cRegistroPago>();
            string query = "SELECT id FROM tRegistroPago WHERE idEstado = '" + Convert.ToInt16(estadoCuenta_Cuota.Validar) + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
            {
                registroPago.Add(Load(Convert.ToString(idList[i])));
            }
            return registroPago;
        }

        public List<cRegistroPago> GetRegistrosByIdCC(string _idCC)
        {
            List<cRegistroPago> registroPago = new List<cRegistroPago>();
            string query = "SELECT id FROM tRegistroPago WHERE idEstado = '" + Convert.ToInt16(estadoCuenta_Cuota.Validar) + "' AND idCC='" + _idCC + "' ORDER BY id ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
            {
                registroPago.Add(Load(Convert.ToString(idList[i])));
            }
            return registroPago;
        }

        public List<cRegistroPago> GetRegistrosByIds(string _idCC, string _idRegistros)
        {
            string[] ids = _idRegistros.Split(',');
            int count = 0;
            List<cRegistroPago> registroPago = new List<cRegistroPago>();
            string query = "SELECT id FROM tRegistroPago WHERE idEstado = '" + Convert.ToInt16(estadoCuenta_Cuota.Validar) + "'";

            foreach (string txt in ids)
            {
                if (txt != "")
                {
                    if (count < 1)
                    {
                        query += " AND id='" + txt + "'";
                        count++;
                    }
                    else
                        query += " OR id='" + txt + "'";
                }
            }
            
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
            {
                registroPago.Add(Load(Convert.ToString(idList[i])));
            }
            return registroPago;
        }
    }
}
