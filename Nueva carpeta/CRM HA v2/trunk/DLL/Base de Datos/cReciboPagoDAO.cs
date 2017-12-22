using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cReciboPagoDAO
    {
        public string GetTable
        { get { return "tReciboPago"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cReciboPago recibo)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("gastoAdtvo", recibo.GastoAdtvo));
            lista.Add(new cAtributo("total", recibo.Total));
            lista.Add(new cAtributo("importePagado", recibo.ImportePagado));
            lista.Add(new cAtributo("diferencia", recibo.Diferencia));
            lista.Add(new cAtributo("punitorio", recibo.Punitorio));
            return lista;
        }

        public int Save(cReciboPago recibo)
        {
            if (string.IsNullOrEmpty(recibo.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(recibo));
            else
                return cDataBase.GetInstance().UpdateObject(recibo.Id, GetTable, AttributesClass(recibo));
        }

        public cReciboPago Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cReciboPago recibo = new cReciboPago();
            recibo.Id = Convert.ToString(atributos["id"]);
            recibo.GastoAdtvo = Convert.ToDecimal(atributos["gastoAdtvo"]);
            recibo.Total = Convert.ToDecimal(atributos["total"]);
            recibo.ImportePagado = Convert.ToDecimal(atributos["importePagado"]);
            recibo.Diferencia = Convert.ToDecimal(atributos["diferencia"]);
            recibo.Punitorio = Convert.ToDecimal(atributos["punitorio"]);
            return recibo;
        }
    }
}
