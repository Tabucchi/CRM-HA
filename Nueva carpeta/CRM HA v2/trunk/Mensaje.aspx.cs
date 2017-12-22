using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mensaje : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        string id = Request["text"].ToString();

        if (id == "noMail")
            lbText.Text = "El cliente a quien desea cargar un Ticket no poseé un E-Mail registrado.";
        else {
            if(id == "noexists")
                lbText.Text = "El existe el cliente a quien desea cargar un Ticket.";
            else {
                if (string.IsNullOrEmpty(id))
                    lbText.Text = "Se produjo un error al cargar el ticket, por favor inténtelo nuevamente.";
                else
                {
                    lbText.Text = "El ticket Nro. " + id + " se ha cargado correctamente.";
                }
            }
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
