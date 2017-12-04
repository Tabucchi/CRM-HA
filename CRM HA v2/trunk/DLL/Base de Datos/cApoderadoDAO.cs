using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public  class cApoderadoDAO
{
    public string GetTable
    { get { return "tApoderado"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cApoderado apoderado)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("razonSocial", apoderado.RazonSocial));
        lista.Add(new cAtributo("cuit", apoderado.Cuit));
        lista.Add(new cAtributo("tipoDoc", apoderado.TipoDoc));
        lista.Add(new cAtributo("documento", apoderado.Documento));
        lista.Add(new cAtributo("telefono", apoderado.Telefono));
        lista.Add(new cAtributo("mail", apoderado.Mail));
        lista.Add(new cAtributo("idDomicilio", apoderado.IdDomicilio));
        lista.Add(new cAtributo("papelera", apoderado.Papelera));
        return lista;
    }

    public int Save(cApoderado apoderado)
    {
        if (string.IsNullOrEmpty(apoderado.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(apoderado));
        else
            return cDataBase.GetInstance().UpdateObject(apoderado.Id, GetTable, AttributesClass(apoderado));
    }

    public cApoderado Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cApoderado apoderado = new cApoderado();
            apoderado.Id = Convert.ToString(atributos["id"]);
            apoderado.RazonSocial = Convert.ToString(atributos["razonSocial"]);
            apoderado.Cuit = Convert.ToString(atributos["cuit"]);
            apoderado.TipoDoc = Convert.ToString(atributos["tipoDoc"]);
            apoderado.Documento = Convert.ToString(atributos["documento"]);
            apoderado.Telefono = Convert.ToString(atributos["telefono"]);
            apoderado.Mail = Convert.ToString(atributos["mail"]);
            apoderado.IdDomicilio = Convert.ToString(atributos["idDomicilio"]);
            apoderado.Papelera = Convert.ToInt16(atributos["papelera"]);
            return apoderado;
        }
        catch
        {
            return null;
        }
    }
}

