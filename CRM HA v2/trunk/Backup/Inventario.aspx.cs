using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace crm
{
    public partial class Inventario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbResponsable.DataSource = cCampoGenerico.GetResponsableInventario("1");
                cbResponsable.DataValueField = "id";
                cbResponsable.DataTextField = "descripcion";
                cbResponsable.DataBind();
                ListItem it = new ListItem("Ninguno", "-1");
                cbResponsable.Items.Insert(0, it);

                cbCategoria.DataSource = cCategoriaInventario.GetCategorias();
                cbCategoria.DataValueField = "id";
                cbCategoria.DataTextField = "descripcion";
                cbCategoria.DataBind();

                lvInventario.DataSource = cInventario.GetInventarios("1");
                lvInventario.DataBind();

                lbTotal.Text = cInventario.GetValor("1").ToString();
            }
        }
                
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            lbMensaje.Visible = false;

            if (!string.IsNullOrEmpty(fileArchivo.FileName))
            {
                #region Carga imagen
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\" + fileArchivo.FileName;
                fileArchivo.SaveAs(path);

                string descripcion = fileArchivo.FileName;

                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                byte[] file = reader.ReadBytes((int)stream.Length);

                cImagen img = new cImagen();
                img.Descripcion = descripcion;
                img.Imagen = file;
                

                stream.Close();
                #endregion

                cInventario inv = new cInventario();
                inv.Descripcion = txtDescripcion.Text;
                inv.IdCategoria = cbCategoria.SelectedValue;
                inv.Empresa = cbEmpresa.SelectedValue;
                        
                int j = 0;
                string prefijoContador = null;
                int length = cCategoriaInventario.Load(cbCategoria.SelectedValue).Contador.ToString().Length;
                if (length <= 4)
                {
                    while (j != (4 - length))
                    {
                        prefijoContador += "0";
                        j++;
                    }
                }

                inv.Numero = cCategoriaInventario.Load(cbCategoria.SelectedValue).Numero + "-" + prefijoContador + cCategoriaInventario.Load(cbCategoria.SelectedValue).Contador;

                if (!string.IsNullOrEmpty(txtValor.Text))
                    inv.Valor = Convert.ToDecimal(txtValor.Text);
                else
                    inv.Valor = 0;

                if (!string.IsNullOrEmpty(txtCantUnidades.Text))
                    inv.CantUnidades = Convert.ToInt16(txtCantUnidades.Text);
                else
                    inv.CantUnidades = 1;

                inv.IdResponsable = Convert.ToInt16(cbResponsable.SelectedValue);

                cCategoriaInventario categoria = cCategoriaInventario.Load(cbCategoria.SelectedValue);
                int cont = Convert.ToInt16(categoria.Contador) + 1;
                categoria.Contador = Convert.ToString(Convert.ToInt16(categoria.Contador) + 1);

                if (string.IsNullOrEmpty(cImagen.Existe(fileArchivo.FileName)))
                {
                    int _idImagen = img.Save();
                    inv.IdImagen = _idImagen.ToString();

                    categoria.Save();
                    inv.Save();
                }
                else
                {
                    lbMensaje.Visible = true;
                    lbMensaje.Text = "Ya existe una imagen con ese nombre";
                }
            }else{
                lbMensaje.Visible = true;
                lbMensaje.Text = "Falta seleccionar una imagen";
            }
        }
              
        protected void cbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbResponsable.DataSource = cCampoGenerico.GetResponsableInventario(cbEmpresa.SelectedValue);
            cbResponsable.DataValueField = "id";
            cbResponsable.DataTextField = "descripcion";
            cbResponsable.DataBind();
            ListItem it = new ListItem("Ninguno", "-1");
            cbResponsable.Items.Insert(0, it);
        }

        protected void cbBuscarEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvInventario.DataSource = cInventario.GetInventarios(cbBuscarEmpresa.SelectedValue);
            lvInventario.DataBind();

            lbTotal.Text = cInventario.GetValor(cbBuscarEmpresa.SelectedValue).ToString();
        }

        protected void lnkImagen_Click(object sender, EventArgs e)
        {
            LinkButton boton = (LinkButton)sender;
            cImagen archivo = cImagen.Load(boton.CommandArgument.ToString());

            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", archivo.Id));
            switch ("jpg")
            {
                case "jpg":
                    Response.ContentType = "image/jpeg";
                    break;
            }

            string fileName = archivo.Descripcion + ".jpg";
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\" + fileName;
            fileArchivo.SaveAs(path);


            // Create a new stream to write to the file
            BinaryWriter Writer = new BinaryWriter(File.OpenWrite(path));

            // Writer raw data                
            Writer.Write(archivo.Imagen);
            Writer.Flush();
            Writer.Close();

            Response.Redirect("~\\Archivos\\" + fileName);
        }        
    }
}