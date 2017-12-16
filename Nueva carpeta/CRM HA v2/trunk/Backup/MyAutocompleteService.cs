using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Drawing.Design;

//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "crm.naex.com.ar")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]

public class MyAutocompleteService : WebService
{
    [WebMethod]
    public string[] GetSuggestions(string prefixText, int count)
    {
        return cCliente.SearchByText(prefixText);
    }

    [WebMethod]
    public string[] GetEmpresasAutoCompletar(string prefixText, int count)
    {
        return cEmpresa.GetEmpresasAutoCompletar(prefixText);
    }

    [WebMethod]
    public string[] GetEmpresasPosibles(string prefixText, int count)
    {
        return cEmpresa.GetEmpresasPosibles(prefixText);
    }

    [WebMethod]
    public string[] GetManualesPosibles(string prefixText, int count)
    {
        return cManual.GetManualesPosibles(prefixText);
    }
}

