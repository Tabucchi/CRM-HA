using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

public enum Tablas { tCategoria, tPrioridad, tEstado, tModoResolucion, tResponsableInventario, tEstadoCivil, tCondicionIva, tEstadoCliente, tEstadoUnidad, tTipoUnidad };
public enum papelera { Eliminado = 0, Activo = 1, ClientePosible = 2 };
public enum estadoComprobante { Anulado = 0, Activo = 1 };
public enum eCivil { Soltero = 1, Comprometido = 2, Casado = 3, Divorciado = 4, Viudo = 5 };
public enum condIva { Consumidor_final = 1, Exento = 2, Monotributista = 3, No_responsable = 4, Responsable_inscripto = 5 };
public enum estadoCliente { Persona_fisica = 1, Comision = 2, Apoderado = 3 };
public enum tipoCliente { Persona_física = 1, Persona_jurídica = 2 }
public enum tipoDocumento { DNI = 1, CI = 2, LC = 3, LE = 4, No = 5 }
public enum tipoCaracter { Título_personal = 1, En_comisión = 2 }
public enum tipoAnticipo { Saldo = 1, Cuotas = 2 }
public enum eApoderado { No = 1, Si = 2 }
public enum eGastosAdtvo { No = 0, Si = 1 }
public enum eInteresAnual { No = 0, Si = 1 }
public enum eComprobante { Recibo = 0, NotaDebito = 1, NotaCredito = 2, Condonacion = 3 }
public enum eEstadoItem { Pagado = 0, Cuota = 1, Adelanto = 2, PagoParcialAdelanto = 3, A_confirmar = 4, Eliminado = 5, CuotaSinCAC = 6, Anular = 7, Reserva = 8 }
public enum eTipoOperacion { Cuota = 0, PagoCuota = 1, NotaCredito = 2, NotaDebito = 3, OtrosPago = 4, Adelanto = 5, Saldo = 6, Anular = 7, Reserva = 8, Condonacion = 9 }

public enum eIndice { CAC = 0, UVA = 1}

public enum eMonedaIndice { Todos = 0, Dolar = 1, Pesos = 2, CAC = 3, UVA = 4}

public class cCampoGenerico
{
    private string id;
    private string descripcion;

    public cCampoGenerico() { }

    public List<cAtributo> AttributesClass(cCampoGenerico campo)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("id", campo.id));
        lista.Add(new cAtributo("descripcion", campo.descripcion));
        return lista;
    }

    #region Acceso a Datos
    public static cCampoGenerico Load(string id, Tablas tabla)
    {
        cCampoGenericoDAO DAO = new cCampoGenericoDAO();
        return DAO.Load(id, tabla.ToString());
    }

    public static ArrayList LoadTable(Tablas tabla)
    {
        cCampoGenericoDAO DAO = new cCampoGenericoDAO();
        return DAO.LoadTable(tabla.ToString());
    }

    public int Save(string tabla)
    {
        cCampoGenericoDAO DAO = new cCampoGenericoDAO();
        return DAO.Save(this, tabla);
    }
    #endregion

    public static DataTable GetDataTable(Tablas tabla)
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
        ArrayList valores = LoadTable(tabla);
        foreach (cCampoGenerico cg in valores)
        {
            tbl.Rows.Add(cg.id, cg.descripcion);
        }
        return tbl;
    }

    public static DataTable GetListaCategoria() //No muestra el estado de NO INSTALADAS
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
        ArrayList sistema = LoadTableCategoria();
        foreach (cCampoGenerico c in sistema)
        {
            if (c.id != "0")
                tbl.Rows.Add(c.id, c.Descripcion);
        }
        return tbl;
    }

    public static ArrayList LoadTableCategoria()
    {
        cCampoGenericoDAO campogenericoDAO = new cCampoGenericoDAO();
        return campogenericoDAO.LoadTable(Convert.ToString(Tablas.tCategoria));
    }

    public static DataTable GetListaEstadoUnidad()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
        ArrayList sistema = LoadTableEstadoUnidad();
        foreach (cCampoGenerico c in sistema)
        {
            if (c.id != "0")
                tbl.Rows.Add(c.id, c.Descripcion);
        }
        return tbl;
    }

    public static ArrayList LoadTableEstadoUnidad()
    {
        cCampoGenericoDAO campogenericoDAO = new cCampoGenericoDAO();
        return campogenericoDAO.LoadTable(Convert.ToString(Tablas.tEstadoUnidad));
    }

    public static DataTable GetListaTipoUnidad()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
        ArrayList sistema = LoadTableTipoUnidad();
        foreach (cCampoGenerico c in sistema)
        {
            if (c.id != "0")
                tbl.Rows.Add(c.id, c.Descripcion);
        }
        return tbl;
    }

    public static ArrayList LoadTableTipoUnidad()
    {
        cCampoGenericoDAO campogenericoDAO = new cCampoGenericoDAO();
        return campogenericoDAO.LoadTable(Convert.ToString(Tablas.tTipoUnidad));
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }
    #endregion

    public override string ToString()
    {
        return Descripcion;
    }

    public static List<cCampoGenerico> GetResponsableInventario(string responsable)
    {
        cCampoGenericoDAO campoGenericoDao = new cCampoGenericoDAO();
        return campoGenericoDao.GetResponsableInventario(responsable);
    }

    public string Codify(string _pass)
    {
        string newPass = "";
        char character;
        for (int i = 0; i < _pass.Length; i++)
        {
            character = _pass[i];
            Convert.ToInt16(character);
            newPass += Convert.ToString(character + 210);
        }
        return newPass;
    }

    public static string GetPassDatosTecnicos()
    {
        cCampoGenericoDAO campoGenericoDao = new cCampoGenericoDAO();
        return campoGenericoDao.GetPassDatosTecnicos();
    }

    public static string GetFechaVencimiento(Int16 _idPrioridad, DateTime _fechaCarga)
    {

        string fecha = null;
        switch (_idPrioridad)
        {
            case (Int16)Prioridad.SinUrgencia:
                fecha = String.Format("{0:dd/MM/yyyy}", cPedido.diasHabiles_SinUrgencia(_fechaCarga));
                break;
            case (Int16)Prioridad.Inmediato:
                if (_fechaCarga != null)
                    fecha = String.Format("{0:dd/MM/yyyy}", _fechaCarga);
                else
                    fecha = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                break;
            case (Int16)Prioridad._24hs:
                fecha = String.Format("{0:dd/MM/yyyy}", cPedido.diasHabiles_24hs(_fechaCarga));
                break;
            case (Int16)Prioridad._48hs:
                fecha = String.Format("{0:dd/MM/yyyy}", cPedido.diasHabiles_48hs(_fechaCarga));
                break;
            case (Int16)Prioridad.ProximaVisita:
                fecha = "";
                break;
        }
        return fecha;
    }

    public static Int16 GetIdEstado(string _estado)
    {
        Int16 IdEstado = 0;
        switch (_estado)
        {
            case "Nuevo":
                IdEstado = 0;
                break;
            case "Finalizado":
                IdEstado = 2;
                break;
            case "A cobrar":
                IdEstado = 4;
                break;
        }

        return IdEstado;
    }

    public static Int16 GetIdPrioridad(string _prioridad)
    {
        Int16 idPrioridad = 0;
        switch (_prioridad)
        {
            case "Sin Urgencia":
                idPrioridad = 0;
                break;
            case "Inmediato":
                idPrioridad = 1;
                break;
            case "24 hs.":
                idPrioridad = 2;
                break;
            case "48 hs.":
                idPrioridad = 3;
                break;
        }
        return idPrioridad;
    }

    public static Int16 GetIdModoResolucion(string _modo)
    {
        Int16 idModo = 0;
        switch (_modo)
        {
            case "Telefónico":
                idModo = 1;
                break;
            case "In situ":
                idModo = 2;
                break;
            case "Remoto":
                idModo = 3;
                break;
            case "E-Mail":
                idModo = 4;
                break;
            case "Oficina NAEX":
                idModo = 5;
                break;
        }
        return idModo;
    }

    public static ListItemCollection CargarComboMoneda()
    {
        ListItemCollection collection = new ListItemCollection();
        collection.Add(new ListItem("Seleccione una moneda...", "-1"));
        collection.Add(new ListItem("Dolar", "0"));
        collection.Add(new ListItem("Pesos", "1"));

        return collection;
    }

    public static ListItemCollection CargarComboTipoPersona()
    {
        ListItemCollection collection = new ListItemCollection();
        collection.Add(new ListItem("Seleccione un tipo de cliente...", "0"));
        collection.Add(new ListItem(tipoCliente.Persona_física.ToString().Replace("_", " "), "1"));
        collection.Add(new ListItem(tipoCliente.Persona_jurídica.ToString().Replace("_", " "), "2"));

        return collection;
    }

    public static ListItemCollection CargarComboEstadoComprobante()
    {
        ListItemCollection collection = new ListItemCollection();
        collection.Add(new ListItem("Seleccione un estado...", "-1"));
        collection.Add(new ListItem(estadoComprobante.Anulado.ToString(), "0"));
        collection.Add(new ListItem(estadoComprobante.Activo.ToString(), "1"));

        return collection;
    }

    public static string GetProvincia(string idProvincia)
    {
        string provincia = null;
        switch (idProvincia)
        {
            case "1":
                provincia = "Capital Federal";
                break;
            case "2":
                provincia = "Buenos Aires";
                break;
            case "3":
                provincia = "Catamarca";
                break;
            case "4":
                provincia = "Córdoba";
                break;
            case "5":
                provincia = "Corrientes";
                break;
            case "6":
                provincia = "Chaco";
                break;
            case "7":
                provincia = "Chubut";
                break;
            case "8":
                provincia = "Entre Ríos";
                break;
            case "9":
                provincia = "Formosa";
                break;
            case "10":
                provincia = "Jujuy";
                break;
            case "11":
                provincia = "La Pampa";
                break;
            case "12":
                provincia = "La Rioja";
                break;
            case "13":
                provincia = "Mendoza";
                break;
            case "14":
                provincia = "Misiones";
                break;
            case "15":
                provincia = "Neuquén";
                break;
            case "16":
                provincia = "Río Negro";
                break;
            case "17":
                provincia = "Salta";
                break;
            case "18":
                provincia = "San Juan";
                break;
            case "19":
                provincia = "San Luis";
                break;
            case "20":
                provincia = "Santa Cruz";
                break;
            case "21":
                provincia = "Santa Fe";
                break;
            case "22":
                provincia = "Santiago del Estero";
                break;
            case "23":
                provincia = "Tierra del Fuego";
                break;
            case "24":
                provincia = "Tucumán";
                break;
        }
        return provincia;
    }
}
