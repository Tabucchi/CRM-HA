using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public enum estadoActualizarPrecio { A_confirmar = 0, Aceptar = 1, Cancelar = 2}
public enum tipoActualizacion { Porcentaje = 1, Valor = 0}

 public class cActualizarPrecio
{
     private string id;
     private string codigoUF;
     private string idProyecto;
     private decimal valorViejo;
     private decimal valorNuevo;
     private string _tipoActualizacion;
     private decimal valorActualizacion;
     private Int16 estado;

     #region Propiedades
     public string Id
     {
         get { return id; }
         set { id = value; }
     }
     public string CodigoUF
     {
         get { return codigoUF; }
         set { codigoUF = value; }
     }
     public string IdProyecto
     {
         get { return idProyecto; }
         set { idProyecto = value; }
     }
     public string GetProyecto
     {
         get { return cProyecto.Load(idProyecto).Descripcion; }
     }
     public string GetMoneda
     {
         get { return cUnidad.LoadByCodUF(CodigoUF, IdProyecto).GetMoneda; }
     }
     public decimal ValorViejo
     {
         get { return valorViejo; }
         set { valorViejo = value; }
     }
     public decimal ValorNuevo
     {
         get { return valorNuevo; }
         set { valorNuevo = value; }
     }
     public string TipoActualizacion
     {
         get { return _tipoActualizacion; }
         set { _tipoActualizacion = value; }
     }
     public decimal ValorActualizacion
     {
         get { return valorActualizacion; }
         set { valorActualizacion = value; }
     }
     public string GetValorActualizacion
     {
         get
         {
             if (TipoActualizacion == tipoActualizacion.Porcentaje.ToString())
                 return ValorActualizacion + "%";
             else
                 return ValorActualizacion.ToString();
         }
     }

     public Int16 Estado
     {
         get { return estado; }
         set { estado = value; }
     }
     public string GetEstado
     {
         get
         {
             string _estado = null;
             switch (Estado)
             {
                 case 0:
                     _estado = estadoActualizarPrecio.A_confirmar.ToString().Replace("_", " ");
                     break;
                 case 1:
                     _estado = estadoActualizarPrecio.Aceptar.ToString();
                     break;
                 case 2:
                     _estado = estadoActualizarPrecio.Cancelar.ToString();
                     break;
             }
             return _estado;
         }
     }

     public string GetTable
     { get { return "tActualizarPrecio"; } }
     #endregion

     public cActualizarPrecio() {}

     public cActualizarPrecio(string _codigoUF, string _idProyecto, decimal _valorViejo, decimal _valorNuevo, string tipoActualizacionAux, decimal _valorActualizacion)
     {
         codigoUF = _codigoUF;
         idProyecto = _idProyecto;
         valorViejo = _valorViejo;
         valorNuevo = _valorNuevo;
         _tipoActualizacion = tipoActualizacionAux;
         valorActualizacion = _valorActualizacion;
         estado = (Int16)estadoActualizarPrecio.A_confirmar;
         this.Save();
     }

     public int Save()
     {
         cActualizarPrecioDAO DAO = new cActualizarPrecioDAO();
         return DAO.Save(this);
     }

     public static cActualizarPrecio Load(string id)
     {
         cActualizarPrecioDAO DAO = new cActualizarPrecioDAO();
         return DAO.Load(id);
     }

     public static List<cActualizarPrecio> GetPrecios()
     {
         cActualizarPrecioDAO DAO = new cActualizarPrecioDAO();
         return DAO.GetPrecios();
     }

     public static cActualizarPrecio GetActualizacionByProyectoAndUF(string _codUF, int _idProyecto)
     {
         cActualizarPrecioDAO DAO = new cActualizarPrecioDAO();
         return DAO.GetActualizacionByProyectoAndUF(_codUF, _idProyecto);
     }

     public static List<cActualizarPrecio> GetActualizacionByIdProyecto(string _idProyecto)
     {
         cActualizarPrecioDAO DAO = new cActualizarPrecioDAO();
         return DAO.GetActualizacionByIdProyecto(_idProyecto);
     }
 }
