using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cDomicilioDAO
    {
        public string GetTable
        { get { return "tDomicilio"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cDomicilio domicilio)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("calle", domicilio.Calle));
            lista.Add(new cAtributo("direccion", domicilio.Direccion));
            lista.Add(new cAtributo("codPostal", domicilio.CodPostal));
            lista.Add(new cAtributo("idProvincia", domicilio.IdProvincia));
            lista.Add(new cAtributo("ciudad", domicilio.Ciudad));
            return lista;
        }

        public int Save(cDomicilio domicilio)
        {
            if (string.IsNullOrEmpty(domicilio.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(domicilio));
            else
                return cDataBase.GetInstance().UpdateObject(domicilio.Id, GetTable, AttributesClass(domicilio));
        }

        public cDomicilio Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cDomicilio domicilio = new cDomicilio();
            domicilio.Id = Convert.ToString(atributos["id"]);
            domicilio.Calle = Convert.ToString(atributos["calle"]);
            domicilio.Direccion = Convert.ToString(atributos["direccion"]);
            domicilio.CodPostal = Convert.ToString(atributos["codPostal"]);
            domicilio.IdProvincia = Convert.ToString(atributos["idProvincia"]);
            domicilio.Ciudad = Convert.ToString(atributos["ciudad"]);
            return domicilio;
        }
    }
}
