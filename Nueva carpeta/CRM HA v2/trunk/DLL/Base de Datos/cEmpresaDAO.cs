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
using System.Data.SqlClient;


public class cEmpresaDAO
{
    public string GetTable
    { get { return "tEmpresa"; } }

    public string GetOrderBy
    { get { return "Nombre ASC"; } }

    public string GetOrderApellidoNombreBy
    { get { return "Apellido, Nombre ASC"; } }

    public List<cAtributo> AttributesClass(cEmpresa empresa)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Nombre", empresa.Nombre));
        lista.Add(new cAtributo("Apellido", empresa.Apellido));
        lista.Add(new cAtributo("idEstadoCivil", empresa.IdEstadoCivil));
        lista.Add(new cAtributo("domicilio", empresa.Direccion));
        lista.Add(new cAtributo("Documento", empresa.Documento));
        lista.Add(new cAtributo("Telefono", empresa.Telefono));
        lista.Add(new cAtributo("Mail", empresa.Mail));
        lista.Add(new cAtributo("Cuit", empresa.Cuit));
        lista.Add(new cAtributo("idCondicionIva", empresa.CondicionIva));
        lista.Add(new cAtributo("idEstado", empresa.IdEstado));
        lista.Add(new cAtributo("Datos", empresa.Datos));
        lista.Add(new cAtributo("Clave", empresa.Clave));
        lista.Add(new cAtributo("idEmpresa", empresa.IdEmpresa));
        lista.Add(new cAtributo("Papelera", empresa.Papelera));
        lista.Add(new cAtributo("caracter", empresa.Caracter));
        lista.Add(new cAtributo("apoderado", empresa.Apoderado));
        lista.Add(new cAtributo("tipoDoc", empresa.TipoDoc));
        lista.Add(new cAtributo("idDomicilio", empresa.IdDomicilio));
        lista.Add(new cAtributo("tipoCliente", empresa.TipoCliente));
        lista.Add(new cAtributo("comentarios", empresa.Comentarios));

        return lista;
    }

    public int Save(cEmpresa empresa)
    {
        if (string.IsNullOrEmpty(empresa.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(empresa));
        else
            return cDataBase.GetInstance().UpdateObject(empresa.Id, GetTable, AttributesClass(empresa));
    }

    public cEmpresa Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cEmpresa empresa = new cEmpresa();
            empresa.Id = Convert.ToString(atributos["id"]);
            empresa.Nombre = Convert.ToString(atributos["nombre"]);
            empresa.Apellido = Convert.ToString(atributos["apellido"]);
            empresa.IdEstado = Convert.ToInt16(atributos["idEstado"]);
            empresa.IdEstadoCivil = Convert.ToInt16(atributos["idEstadoCivil"]);
            empresa.Documento = Convert.ToString(atributos["documento"]);
            empresa.Direccion = Convert.ToString(atributos["domicilio"]);
            empresa.Telefono = Convert.ToString(atributos["telefono"]);
            empresa.Mail = Convert.ToString(atributos["mail"]);
            empresa.Cuit = Convert.ToString(atributos["cuit"]);
            empresa.CondicionIva = Convert.ToInt16(atributos["idCondicionIva"]);
            empresa.Datos = Convert.ToString(atributos["datos"]);
            empresa.Clave = Convert.ToString(atributos["clave"]);
            empresa.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            empresa.Papelera = Convert.ToInt16(atributos["papelera"]);
            empresa.Caracter = Convert.ToString(atributos["caracter"]);
            empresa.Apoderado = Convert.ToString(atributos["apoderado"]);
            empresa.TipoDoc = Convert.ToString(atributos["tipoDoc"]);
            empresa.IdDomicilio = Convert.ToString(atributos["idDomicilio"]);
            empresa.TipoCliente = Convert.ToString(atributos["tipoCliente"]);
            empresa.Comentarios = Convert.ToString(atributos["comentarios"]);
            return empresa;
        }
        catch
        {
            return null;
        }
    }

    public int LoginEmpresaCliente(string nameUser, string password)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        // Si la clave y el usuario son correctos devolverá 1, sino 0.
        query.Append("SELECT id FROM " + this.GetTable);
        query.Append(" WHERE DominioMail = @Mail AND Clave = @Clave AND Papelera = @Papelera");
        SqlCommand cmd = new SqlCommand(query.ToString());
        // Creamos los parámetros.
        cmd.Parameters.Add("@Mail", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Papelera", SqlDbType.SmallInt);
        // Asignamos los valores recibidos como parámetro.
        cmd.Parameters["@Mail"].Value = nameUser;
        cmd.Parameters["@Clave"].Value = password;
        cmd.Parameters["@Papelera"].Value = 1; // Usuario activo.
        // Ejecutamos la consulta
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public ArrayList LoadTable()
    {
        ArrayList empresas = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderApellidoNombreBy);
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public string GetFullPhone(string id)
    {
        string query = "SELECT Telefono FROM " + GetTable + " WHERE id = " + id;
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string DeleteEmpresa(string id) //Cambia estado del campo papelera
    {
        string query = "UPDATE " + GetTable + " SET Papelera=0 " + "WHERE id= " + "'" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cEmpresa> GetEmpresas()
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1" + " Order by Apellido, Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public List<cEmpresa> GetEmpresasSaldos()
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        string query = "SELECT e.id, e.Apellido, e.nombre, p.descripcion FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv  INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta ";
        query += " INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto WHERE eu.papelera = '1' AND eu.idOv <> '-1' GROUP BY e.id, e.Apellido, e.nombre, p.descripcion ORDER BY e.Apellido, e.nombre,  p.descripcion";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public List<cEmpresa> GetEmpresasPersonaFisica()
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1 AND tipoCliente='" + (Int16)tipoCliente.Persona_física + "' Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public List<cEmpresa> GetEmpresasAutoCompletar(string texto)
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1" + " Order by Nombre ASC";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public List<cEmpresa> GetEmpresasPosibles(string texto)
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 2" + " Order by Nombre ASC";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public string GetNombreEmpresaByPedido(string id)
    {
        string query = "SELECT idEmpresa FROM tPedido WHERE id= '" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string GetNombreEmpresa(string id)
    {
        string query = "SELECT Nombre FROM " + GetTable + " WHERE Papelera= 1 AND id= '" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string GetDatosEmpresa(string id)
    {
        string query = "SELECT Datos FROM " + GetTable + " WHERE Papelera= 1 AND id= '" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string GetIdByNombre(string empresa)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE (nombre like '%" + empresa + "%')";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string GetDominioMail(string dominio)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE DominioMail = '" + dominio + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cCliente> GetClientes(string idEmpresa)
    {
        cClienteDAO DAO = new cClienteDAO();
        List<cCliente> clientes = new List<cCliente>();
        string query = "SELECT id FROM tCliente WHERE idEmpresa = " + idEmpresa + " AND Papelera = 1" + " Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;

        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(DAO.Load(Convert.ToString(idList[i])));
            string claveDeco = cUsuario.Decode(clientes[i].ClaveSistema);
            clientes[i].ClaveSistema = claveDeco;
        }
        return clientes;
    }
    
    public cEmpresa GetClientesByItemCCU(string _idItemCCU)
    {
        try
        {
            string query = "SELECT e.id FROM tRecibo r INNER JOIN tItemCCU i on r.idItemCCU=i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id ";
            query += " INNER JOIN tEmpresa e ON e.id=ccu.idEmpresa WHERE i.id='" + _idItemCCU  + "'";

            SqlCommand cmd = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            if (res == null)
                return null;
            
            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public cEmpresa GetClientesReciboByItemCCU(string _idItemCCU)
    {
        try
        {
            string query = "SELECT e.id FROM tRecibo nc INNER JOIN tItemCCU i ON nc.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario=ccu.id ";
            query += "INNER JOIN tEmpresa e ON ccu.idEmpresa=e.id WHERE i.id='" + _idItemCCU + "'";

            SqlCommand cmd = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            if (res == null)
                return null;

            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public cEmpresa GetClientesCondonacionByItemCCU(string _idItemCCU)
    {
        try
        {
            string query = "SELECT e.id FROM tCondonacion c INNER JOIN tItemCCU i ON c.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario=ccu.id ";
            query += "INNER JOIN tEmpresa e ON ccu.idEmpresa=e.id WHERE i.id='" + _idItemCCU + "'";

            SqlCommand cmd = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            if (res == null)
                return null;

            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public cEmpresa GetClientesNotaDebitoByItemCCU(string _idItemCCU)
    {
        try
        {
            string query = "SELECT e.id FROM tNotaDebito nc INNER JOIN tItemCCU i ON nc.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario=ccu.id ";
            query += "INNER JOIN tEmpresa e ON ccu.idEmpresa=e.id WHERE i.id='" + _idItemCCU + "'";

            SqlCommand cmd = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            if (res == null)
                return null;

            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public cEmpresa GetClientesNotaCreditoByItemCCU(string _idItemCCU)
    {
        try
        {
            string query = "SELECT e.id FROM tNotaCredito nc INNER JOIN tItemCCU i ON nc.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario=ccu.id ";
            query += "INNER JOIN tEmpresa e ON ccu.idEmpresa=e.id WHERE i.id='" + _idItemCCU + "'";

            SqlCommand cmd = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            if (res == null)
                return null;
            
            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public List<cEmpresa> Search(string idEmpresa)
    {
        List<cEmpresa> empresas = new List<cEmpresa>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT * FROM " + GetTable + " WHERE ");

        if (Convert.ToInt32(idEmpresa) > 0)
            query.Append(" id = @id");
        else
            query.Append(" id <> @id");

        cmd.Parameters.Add("@id", SqlDbType.Int);
        cmd.Parameters["@id"].Value = idEmpresa;

        query.Append(" ORDER  BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }
}

