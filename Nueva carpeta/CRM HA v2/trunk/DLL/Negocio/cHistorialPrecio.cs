using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public enum historial { Evolución_de_precios = 3, Unidad_modificada = 4 };

public class cHistorial
{
    private string id;
    private DateTime fecha;
    private string motivo;
    private decimal valorViejo;
    private decimal valorNuevo;
    private string codUF;
    private string nroUnidad;
    private string idUsuario;
    private string idEstadoViejo;
    private string idEstadoNuevo;
    private string idProyecto;

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string Motivo
    {
        get { return motivo; }
        set { motivo = value; }
    }

    public decimal ValorViejo
    {
        get { return valorViejo; }
        set { valorViejo = value; }
    }
    public string GetValorViejo
    {
        get
        { return String.Format("{0:#,#}", ValorViejo); }
    }

    public decimal ValorNuevo
    {
        get { return valorNuevo; }
        set { valorNuevo = value; }
    }
    public string GetValorNuevo
    {
        get
        { return String.Format("{0:#,#}", ValorNuevo); }
    }

    public string PorcentajePrecio
    {
        get
        {
            if (valorNuevo != 0)
                return String.Format("{0:#,#0.00}", 100 - ((valorViejo * 100) / valorNuevo));
            else
                return "0";
        }
    }

    public string CodUF
    {
        get { return codUF; }
        set { codUF = value; }
    }

    public decimal PrecioOriginal
    {
        get
        {
            cUnidad unidad = cUnidad.LoadByCodUF(codUF, idProyecto);
            if (unidad != null)
                return unidad.PrecioBaseOriginal;
            else
                return 0;
        }
    }
    public string GetPrecioOriginal
    {
        get
        {
            string g = String.Format("{0:#,#}", PrecioOriginal);
            return String.Format("{0:#,#}", PrecioOriginal);
        }
    }

    public string NroUnidad
    {
        get { return nroUnidad; }
        set { nroUnidad = value; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string GetUsuario
    {
        get { return cUsuario.Load(IdUsuario).Nombre; }
    }

    public string IdEstadoViejo
    {
        get { return idEstadoViejo; }
        set { idEstadoViejo = value; }
    }
    public string IdEstadoNuevo
    {
        get { return idEstadoNuevo; }
        set { idEstadoNuevo = value; }
    }
    public string GetEstadoViejo
    {
        get
        {
            string estado = null;
            switch (IdEstadoViejo)
            {
                case "1": estado = estadoUnidad.Disponible.ToString();
                    break;
                case "2": estado = estadoUnidad.Reservado.ToString();
                    break;
                case "3": estado = estadoUnidad.Vendido.ToString();
                    break;
                case "4": estado = estadoUnidad.Porteria.ToString();
                    break;
                case "5": estado = estadoUnidad.Modificado.ToString();
                    break;
                case "6": estado = estadoUnidad.Socios.ToString();
                    break;
                case "7": estado = estadoUnidad.Reservado_con_seña.ToString();
                    break;
                case "8": estado = estadoUnidad.Vendido_sin_boleto.ToString();
                    break;
            }
            return estado;
        }
    }
    public string GetEstadoNuevo
    {
        get
        {
            string estado = null;
            switch (IdEstadoNuevo)
            {
                case "1": estado = estadoUnidad.Disponible.ToString();
                    break;
                case "2": estado = estadoUnidad.Reservado.ToString();
                    break;
                case "3": estado = estadoUnidad.Vendido.ToString();
                    break;
                case "4": estado = estadoUnidad.Porteria.ToString();
                    break;
                case "5": estado = estadoUnidad.Modificado.ToString();
                    break;
                case "6": estado = estadoUnidad.Socios.ToString();
                    break;
                case "7": estado = estadoUnidad.Reservado_con_seña.ToString();
                    break;
                case "8": estado = estadoUnidad.Vendido_sin_boleto.ToString();
                    break;
            }
            return estado;
        }
    }
    public string IdProyecto
    {
        get { return idProyecto; }
        set { idProyecto = value; }
    }

    public string GetProyecto
    {
        get { return cProyecto.Load(IdProyecto).Descripcion; }
    }
    #endregion

    public cHistorial() { }

    public cHistorial(DateTime _fecha, string _motivo, decimal _valorViejo, decimal _valorNuevo, string _codUF, string _nroUnidad, string _idEstadoViejo, string _idEstadoNuevo, string _idUsuario, string _idProyecto)
    {
        fecha = _fecha;
        motivo = _motivo;
        valorViejo = _valorViejo;
        valorNuevo = _valorNuevo;
        codUF = _codUF;
        nroUnidad = _nroUnidad;
        idEstadoViejo = _idEstadoViejo;
        idEstadoNuevo = _idEstadoNuevo;
        idUsuario = _idUsuario;
        idProyecto = _idProyecto;
    }

    public int Save()
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.Save(this);
    }

    public static cHistorial Load(string id)
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.Load(id);
    }

    public static List<cHistorial> GetHistorialByIdProyecto(string _idProyecto, string _motivo, bool todos)
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.GetHistorial(_idProyecto, _motivo, todos);
    }

    public static List<cHistorial> GetHistorialByUnidad(string _idProyecto, string _nroUnidad, string _nivel, string _motivo)
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.GetHistorialByUnidad(_idProyecto, _nroUnidad, _nivel, _motivo);
    }

    public static List<cHistorial> GetHistorialByCodUF_NroUnidad(string _idProyecto, string _motivo, string _codUf)
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.GetHistorialByCodUF_NroUnidad(_idProyecto, _motivo, _codUf);
    }

    public static ArrayList CargaCombo()
    {
        ArrayList list = new ArrayList();
        ListItem _item = new ListItem();
        _item.Value = "0";
        _item.Text = "Seleccione el motivo...";
        list.Add(_item);

        foreach (string countrie in Enum.GetNames(typeof(historial)))
        {
            ListItem item = new ListItem();
            item.Value = countrie.Replace("_", " ");
            item.Text = countrie.Replace("_", " ");
            list.Add(item);
        }

        return list;
    }

    public static cHistorial LoadByIdProyecto(string _idProyecto)
    {
        cHistorialDAO DAO = new cHistorialDAO();
        return DAO.LoadByIdProyecto(_idProyecto);
    }
}

