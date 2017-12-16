using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLL.Negocio;
using log4net;
using System.Data;
using DLL.Auxiliares;

namespace crm
{
    public partial class ResponsableEmpresa : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {}

        protected void ListPager_PreRender(object sender, EventArgs e)
        {
            if (Request["idEmpresa"] == null || Request["idUsuario"] == null)
                lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
            else
                lvEmpresas.DataSource = cResponsableEmpresa.Search(Request["idEmpresa"].ToString(), Request["idUsuario"].ToString());

            lvEmpresas.DataBind();
        }

        #region Metodo Insertar en ListView
        protected void lvEmpresas_ItemInserting(object sender, ListViewInsertEventArgs e) 
        {
            try
            {
                cResponsableEmpresa re = new cResponsableEmpresa();

                DropDownList ddl = (e.Item.FindControl("cbIngresarEmpresa")) as DropDownList;
                if (ddl.SelectedValue != null)
                    re.IdEmpresa = ddl.SelectedValue;

                ddl = (e.Item.FindControl("cbIngresarUsuario")) as DropDownList;
                if (ddl.SelectedValue != null)
                    re.IdUsuario = ddl.SelectedValue;

                re.Save();
             
                lvEmpresas.EditIndex = -1;
                lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
                lvEmpresas.DataBind();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ResponsableEmpresa - " + DateTime.Now + "- " + ex.Message + " - lvEmpresas_ItemInserting" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Edición
        protected void lvEmpresas_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvEmpresas.EditIndex = e.NewEditIndex;
            lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
            lvEmpresas.DataBind();
        }

        protected void lvEmpresas_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            cResponsableEmpresa re = new cResponsableEmpresa();

            TextBox txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
            if (txt != null)
                re = cResponsableEmpresa.Load(txt.Text);

            DropDownList ddl = (lvEmpresas.Items[e.ItemIndex].FindControl("cbEditEmpresa")) as DropDownList;
            if (ddl.SelectedValue != null)
                re.IdEmpresa = ddl.SelectedValue;

            ddl = (lvEmpresas.Items[e.ItemIndex].FindControl("cbEditUsuario")) as DropDownList;
            if (ddl.SelectedValue != null)
                re.IdUsuario = ddl.SelectedValue;

            re.Save();

            lvEmpresas.EditIndex = -1;
            lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
            lvEmpresas.DataBind();
        }

        protected void lvEmpresas_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvEmpresas.EditIndex = -1;
            lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
            lvEmpresas.DataBind();
        }

        protected void lvEmpresas_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            cResponsableEmpresa re = new cResponsableEmpresa();

            TextBox txt = (lvEmpresas.Items[e.ItemIndex].FindControl("txtEditId")) as TextBox;
            if (txt != null)
                re = cResponsableEmpresa.Load(txt.Text);

            re.Eliminar(txt.Text);

            lvEmpresas.EditIndex = -1;
            lvEmpresas.DataSource = cResponsableEmpresa.Search(null, null);
            lvEmpresas.DataBind();
        }
        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResponsableEmpresa.aspx?idEmpresa=" + cbEmpresa.SelectedValue + "&idUsuario=" + cbUsuario.SelectedValue);
        }

        protected void lkbExcelReporte_Click(object sender, EventArgs e)
        {
            DataTable tabla = new DataTable();
            tabla = cResponsableEmpresa.reporteXLS();
            List<DataTable> list = new List<DataTable>();
            list.Add(tabla);
            string filename = "Reporte Reponsables-" + DateTime.Today.ToShortDateString().Replace("/", "-") + ".xls";
            cExcel.DataTableToExcel(list, filename);
        }

    }
}