using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;

public class cCategoriaDAO
{
    public string GetTable
    { get { return "tCategoria"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cCategoria categoria)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Tipo", categoria.Tipo));
        return lista;
    }

    public cCategoria Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cCategoria categoria = new cCategoria();
        categoria.Id = Convert.ToInt32(atributos["id"]);
        categoria.Tipo = Convert.ToString(atributos["Tipo"]);
        return categoria;
    }

    public ArrayList LoadTable()
    {
        ArrayList categorias = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            categorias.Add(Load(Convert.ToInt32(idList[i])));
        }
        return categorias;
    }
}