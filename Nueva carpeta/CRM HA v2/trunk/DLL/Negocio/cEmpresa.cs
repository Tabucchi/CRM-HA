using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DLL.Negocio;

public enum condicionIva { Consumidor_final = 1, Exento = 2, Monotributista = 3, No_responsable = 4, Responsable_inscripto = 5 }

public class cEmpresa
{
    private string id;
    private string nombre;
    private string apellido;
    private Int16 idEstadoCivil;
    private string documento;
    private string direccion;
    private string telefono;
    private string mail;
    private string cuit;
    private Int16 condicionIva;
    private Int16 idEstado;
    private string datos;
    private string clave;
    private string idEmpresa; //para el caso si es apoderado de otro cliente
    private Int16 papelera;
    private string caracter;
    private string apoderado;
    private string tipoDoc;
    private string idDomicilio;
    private string tipoCliente;
    private string comentarios;

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    public string Apellido
    {
        get { return apellido; }
        set { apellido = value; }
    }
    public string GetNombreCompleto
    {
        get
        {
            if (string.IsNullOrEmpty(Nombre))
                return Apellido;
            else
                return Apellido + ", " + Nombre;
        }
    }

    public Int16 IdEstadoCivil
    {
        get { return idEstadoCivil; }
        set { idEstadoCivil = value; }
    }

    public string GetEstadoCivil
    {
        get
        {
            string estadoCivil = null;
            switch (IdEstadoCivil)
            {
                case 1:
                    estadoCivil = eCivil.Soltero.ToString();
                    break;
                case 2:
                    estadoCivil = eCivil.Comprometido.ToString();
                    break;
                case 3:
                    estadoCivil = eCivil.Casado.ToString();
                    break;
                case 4:
                    estadoCivil = eCivil.Divorciado.ToString();
                    break;
                case 5:
                    estadoCivil = eCivil.Viudo.ToString();
                    break;
            }
            return estadoCivil;
        }
    }

    public string Direccion
    {
        get { return direccion; }
        set { direccion = value; }
    }

    public string Documento
    {
        get { return documento; }
        set { documento = value; }
    }

    public string Telefono
    {
        get { return telefono; }
        set { telefono = value; }
    }

    public string Mail
    {
        get { return mail; }
        set { mail = value; }
    }

    public string Cuit
    {
        get { return cuit; }
        set { cuit = value; }
    }

    public Int16 CondicionIva
    {
        get { return condicionIva; }
        set { condicionIva = value; }
    }

    public string GetCondicionIva
    {
        get
        {
            string condicion = null;
            switch (CondicionIva)
            {
                case 1:
                    condicion = condIva.Consumidor_final.ToString().Replace("_", " ");
                    break;
                case 2:
                    condicion = condIva.Exento.ToString();
                    break;
                case 3:
                    condicion = condIva.Monotributista.ToString();
                    break;
                case 4:
                    condicion = condIva.No_responsable.ToString().Replace("_", " ");
                    break;
                case 5:
                    condicion = condIva.Responsable_inscripto.ToString().Replace("_", " ");
                    break;
            }
            return condicion;
        }
    }

    public Int16 IdEstado
    {
        get { return idEstado; }
        set { idEstado = value; }
    }

    public string GetEstado
    {
        get
        {
            string estado = null;
            switch (IdEstado)
            {
                case 1:
                    estado = estadoCliente.Persona_fisica.ToString().Replace("_", " ");
                    break;
                case 2:
                    estado = estadoCliente.Comision.ToString();
                    break;
                case 3:
                    estado = estadoCliente.Apoderado.ToString();
                    break;
            }
            return estado;
        }
    }

    public string Datos
    {
        get { return datos; }
        set { datos = value; }
    }

    public string Clave
    {
        get { return clave; }
        set { clave = value; }
    }

    public string ClaveDecode
    {
        get { return Decode(clave); }
    }

    public string IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public string GetEmpresa
    {
        get
        {
            if (idEmpresa != "-1" && idEmpresa != "")
                return " de " + cEmpresa.Load(idEmpresa).Nombre;
            else
                return "";
        }
    }
    
    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }

    public override string ToString()
    {
        return nombre.ToString();
    }

    public string Caracter
    {
        get { return caracter; }
        set { caracter = value; }
    }
    public string GetCaracter
    {
        get
        {
            string caracter = null;
            switch (Caracter)
            {
                case "1":
                    caracter = tipoCaracter.Título_personal.ToString().Replace("_", " ");
                    break;
                case "2":
                    caracter = tipoCaracter.En_comisión.ToString().Replace("_", " ");
                    break;
            }
            return caracter;
        }
    }
    public string Apoderado
    {
        get { return apoderado; }
        set { apoderado = value; }
    }

    public cApoderado apoderadoClass
    {
        get
        {
            if (Apoderado == Convert.ToString((Int16)eApoderado.Si))
                return cApoderado.Load(IdEmpresa);
            else
                return null;
        }
    }

    public cDomicilio domicilioApoderado
    {
        get
        {
            if (apoderadoClass != null)
                return cDomicilio.Load(apoderadoClass.IdDomicilio);
            else
                return null;
        }
    }
    public string GetProvinciaApoderado
    {
        get
        {
            if (apoderadoClass != null)
                return cCampoGenerico.GetProvincia(domicilioApoderado.IdProvincia);
            else
                return "-";
        }
    }

    public string TipoDoc
    {
        get { return tipoDoc; }
        set { tipoDoc = value; }
    }
    public string GetTipoDoc
    {
        get
        {
            string tipoDoc = null;
            switch (TipoDoc)
            {
                case "1":
                    tipoDoc = tipoDocumento.DNI.ToString();
                    break;
                case "2":
                    tipoDoc = tipoDocumento.CI.ToString();
                    break;
                case "3":
                    tipoDoc = tipoDocumento.LC.ToString();
                    break;
                case "4":
                    tipoDoc = tipoDocumento.LE.ToString();
                    break;
            }
            return tipoDoc;
        }
    }
    public string IdDomicilio
    {
        get { return idDomicilio; }
        set { idDomicilio = value; }
    }

    public cDomicilio domicilio
    {
        get
        {
            if (IdDomicilio != "")
                return cDomicilio.Load(IdDomicilio);
            else
                return null;
        }
    }
    public string GetProvincia
    {
        get
        {
            if (domicilio != null)
                return cCampoGenerico.GetProvincia(domicilio.IdProvincia);
            else
                return "-";
        }
    }

    public string TipoCliente
    {
        get { return tipoCliente; }
        set { tipoCliente = value; }
    }

    public string Comentarios
    {
        get { return comentarios; }
        set { comentarios = value; }
    }
    #endregion

    public cEmpresa(string _nombre, string _direccion, string _telefono, string _cuit)
    {
        nombre = _nombre;
        direccion = _direccion;
        telefono = _telefono;
        cuit = _cuit;
        datos = "";
    }

    public cEmpresa()
    { }

    public int Save()
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.Save(this);
    }

    public static cEmpresa Load(string id)
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.LoadTable();
    }

    public static int LoginEmpresaCliente(string nameCliente, string password)
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.LoginEmpresaCliente(nameCliente, password);
    }

    public static List<cEmpresa> GetEmpresas()
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetEmpresas();
    }
    public static List<cEmpresa> GetEmpresasSaldos()
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetEmpresasSaldos();
    }

    public static DataTable GetDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        ArrayList valores = LoadTable();
        foreach (cEmpresa u in valores)
        {
            if (u.id != "2")
                tbl.Rows.Add(u.id, u.GetNombreCompleto);
        }
        return tbl;
    }

    public static List<cEmpresa> GetEmpresasPersonaFisica()
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetEmpresasPersonaFisica();
    }

    public static string[] GetEmpresasAutoCompletar(string texto)
    {
        int i = 0;
        cEmpresaDAO DAO = new cEmpresaDAO();
        List<cEmpresa> c = DAO.GetEmpresasAutoCompletar(texto);
        string[] items = new string[c.Count];
        foreach (cEmpresa x in c)
        {
            items.SetValue(x.nombre, i);
            i++;
        }
        return items;
    }

    public static string[] GetEmpresasPosibles(string texto)
    {
        int i = 0;
        cEmpresaDAO DAO = new cEmpresaDAO();
        List<cEmpresa> c = DAO.GetEmpresasPosibles(texto);
        string[] items = new string[c.Count];
        foreach (cEmpresa x in c)
        {
            items.SetValue(x.nombre, i);
            i++;
        }
        return items;
    }

    public static List<cCliente> GetClientes(string idEmpresa)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetClientes(idEmpresa);
    }

    public static string GetEmpresaByPedido(string id)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetNombreEmpresaByPedido(id);
    }

    public static string GetNombreEmpresa(string id)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetNombreEmpresa(id);
    }

    public static string GetIdByNombre(string empresa)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetIdByNombre(empresa);
    }

    public static string DeleteEmpresa(string id)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.DeleteEmpresa(id);
    }

    public static string GetDominioMail(string dominio)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetDominioMail(dominio);
    }

    public static DataTable GetListaEmpresas()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        tbl.Columns.Add(new DataColumn("direccion", typeof(string)));
        tbl.Columns.Add(new DataColumn("telefono", typeof(string)));
        tbl.Columns.Add(new DataColumn("cuit", typeof(string)));
        ArrayList empresas = LoadTable();
        foreach (cEmpresa c in empresas)
        {
            if (c.Papelera == 1) //Empresa Activa.
                tbl.Rows.Add(c.id, c.nombre, c.Direccion, c.Telefono, c.Cuit);
        }
        return tbl;
    }

    public static List<cEmpresa> Search(string idEmpresa)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.Search(idEmpresa);
    }

    public static cEmpresa GetClientesByItemCCU(string _idItemCCU)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.GetClientesByItemCCU(_idItemCCU);
    }

    public static cEmpresa GetClientesNotaCreditoByItemCCU(string _idItemCCU)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.GetClientesNotaCreditoByItemCCU(_idItemCCU);
    }

    public static cEmpresa GetClientesReciboByItemCCU(string _idItemCCU)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.GetClientesReciboByItemCCU(_idItemCCU);
    }

    public static cEmpresa GetClientesCondonacionByItemCCU(string _idItemCCU)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.GetClientesCondonacionByItemCCU(_idItemCCU);
    }

    public static cEmpresa GetClientesNotaDebitoByItemCCU(string _idItemCCU)
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.GetClientesNotaDebitoByItemCCU(_idItemCCU);
    }

    public static string GetDatosEmpresa(string id)
    {
        cEmpresaDAO empresaDao = new cEmpresaDAO();
        return empresaDao.GetDatosEmpresa(id);
    }

    #region Codificar
    public static string Codify(string _pass)
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

    public static string Decode(string _pass)
    {
        string newPass = "";
        char character;

        int i = 0;

        if (i < _pass.Length && _pass.Length != 3) //si el tamaño de la clave es mayor a 3 
        {
            while (i < _pass.Length)
            {
                //se toman de a tres valores
                string _aux = Convert.ToString(_pass[i]);
                _aux += Convert.ToString(_pass[i + 1]);
                _aux += Convert.ToString(_pass[i + 2]);

                //se obtiene el caracter
                int valorCaracter = (Convert.ToInt16(_aux) - 210);
                character = Convert.ToChar(valorCaracter);

                Convert.ToString(character);
                newPass += character;
                i = i + 3;
            }
        }
        else
        {
            /* int aux = (Convert.ToInt16(_pass) - 210);
             character = Convert.ToChar(aux);
             Convert.ToString(character);
             newPass += character;*/
        }

        return newPass;
    }
    #endregion
}