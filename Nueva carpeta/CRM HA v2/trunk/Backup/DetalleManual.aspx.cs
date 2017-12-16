using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;

public partial class DetalleManual : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DatosManual();
        }
    }

    public void DatosManual()
    {
        try
        {
            //Listado de empresas
            ddlEmpresas.DataSource = cEmpresa.GetEmpresas();
            ddlEmpresas.DataValueField = "id";
            ddlEmpresas.DataTextField = "nombre";
            ddlEmpresas.DataBind();
            ListItem i = new ListItem("Ninguna", "0");
            ddlEmpresas.Items.Insert(0, i);

            //Abre el archivo si existe
            if (System.IO.File.Exists("C:\\crm info\\Manuales\\" + Request["id"].ToString() + ".txt"))
            {
                FileStream fs = File.Open("C:\\crm info\\Manuales\\" + Request["id"].ToString() + ".txt", FileMode.Open);

                StreamReader sr = new StreamReader(fs);
                string cadena = "";
                cadena = sr.ReadToEnd();
                htmlEditor.Content = cadena.Trim();
                sr.Close();

                cManual manual = cManual.Load(Request["id"].ToString());
                txtTitulo.Text = manual.Titulo;
                lbCreado.Text = manual.GetUsuario;
                lbFecha.Text = manual.Fecha.ToString("dd/MM/yyyy");
                ddlEmpresas.SelectedValue = manual.IdEmpresa;
            }
            else
            {
                lbCreado.Text = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Nombre;
                lbFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - DatosManual" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void btnGuardarDatos_Click(object sender, EventArgs e)
    {
        try
        {
            string id = Request["id"].ToString();

            if (string.IsNullOrEmpty(id))
            {
                cManual manual = new cManual(txtTitulo.Text,
                                             cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id,
                                             ddlEmpresas.SelectedValue == "0" ? "-1" : ddlEmpresas.SelectedValue);
                manual.Id = Convert.ToString(manual.Save());

                string fileName = "C:\\crm info\\Manuales\\" + manual.Id + ".txt";
                FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter datos = new StreamWriter(stream);
                datos.WriteLine(htmlEditor.Content);
                datos.Close();
                lbMensajeGuardar.Visible = true;

                // Envio correo
                cSendMail mail = new cSendMail();
                mail.CrearNuevoManual(manual);
            }
            else //Actualizo
            {
                string fileName = "C:\\crm info\\Manuales\\" + id + ".txt";

                cManual manual = cManual.Load(id);
                manual.Titulo = txtTitulo.Text;
                manual.IdEmpresa = ddlEmpresas.SelectedValue == "0" ? "-1" : ddlEmpresas.SelectedValue;
                manual.Save();

                FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter datos = new StreamWriter(stream);
                datos.WriteLine(htmlEditor.Content);
                datos.Close();
                Response.Redirect("Manual.aspx");
            }
        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - btnGuardarDatos_Click" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        cManual manual = cManual.Load(Request["id"]);
        manual.Papelera = 0;
        manual.Save();
        Response.Redirect("Manual.aspx");
    }
}

