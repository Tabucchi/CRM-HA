using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de cLog
/// </summary>
public class cLog
{
    public cLog(int _idUsuario, string _accion, string _Mensaje)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", _idUsuario));
        lista.Add(new cAtributo("Accion", _accion));
        lista.Add(new cAtributo("Mensaje", _Mensaje));
        cDataBase.GetInstance().InsertObject(GetTable, lista);
    }

    public string GetTable
    { get { return "tLog"; } }
}
