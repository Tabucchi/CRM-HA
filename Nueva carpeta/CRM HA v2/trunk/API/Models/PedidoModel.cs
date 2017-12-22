using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileServices.Models
{
    public class PedidoModel
    {
        public Int64 Id {get;set;}
        public Int64 IdEmpresa {get;set;}
        public string Empresa {get; set;}
        public Int64 IdCliente {get; set;}
        public string Cliente {get; set;}
        public Int64 IdUsuario {get;set;}
        public string Usuario { get; set; }
        public string Titulo {get;set;}
        public string Descripcion {get;set;}
        public string Fecha {get;set;}
        public string FechaARealizar {get;set;}
        public int IdEstado {get;set;}
        public string Estado {get; set;}
        public Int64 IdCategoria {get;set;}
        public string Categoria {get; set;}
        public Int64 IdPrioridad {get; set;}
        public string Prioridad {get; set;}
        public Int64 IdAsignacionResponsable {get;set;}
        public string Responsable {get; set;}
        public Int64 IdModoResolucion {get;set;}
        public string ModoResolucion {get; set;}

        public static PedidoModel pedidoModel(tPedido tPedido)
        {
            NaexCRMEntities db = new NaexCRMEntities();
            PedidoModel pedidoModel = new PedidoModel();
            
            pedidoModel.Id = Convert.ToInt64(tPedido.id);
            pedidoModel.IdEmpresa = Convert.ToInt32(tPedido.idEmpresa);
            pedidoModel.Empresa = (from e in db.tEmpresa where e.id == tPedido.idEmpresa select e.Nombre).FirstOrDefault();
            pedidoModel.IdUsuario = Convert.ToInt32(tPedido.idUsuario);
            pedidoModel.Usuario = (from u in db.tUsuario where u.id == tPedido.idUsuario select u.Nombre).FirstOrDefault();
            pedidoModel.Cliente = (from c in db.tCliente where c.id == tPedido.idCliente select c.Nombre).FirstOrDefault();
            pedidoModel.Titulo = tPedido.Titulo;
            pedidoModel.Descripcion = tPedido.Descripcion;
            pedidoModel.Fecha = String.Format("{0:dd/MM/yyyy}", tPedido.Fecha);
            pedidoModel.FechaARealizar = String.Format("{0:dd/MM/yyyy}", tPedido.FechaARealizar);
            pedidoModel.IdEstado = tPedido.Estado.Value;
            pedidoModel.Estado = (from e in db.tEstado where e.id == tPedido.Estado select e.Tipo).FirstOrDefault();
            pedidoModel.IdCategoria = Convert.ToInt32(tPedido.idCategoria.Value);
            pedidoModel.Categoria = (from c in db.tCategoria where c.id == tPedido.idCategoria select c.Tipo).FirstOrDefault();
            pedidoModel.IdPrioridad = tPedido.idPrioridad.Value;
            pedidoModel.Prioridad = (from p in db.tPrioridad where p.id == tPedido.idPrioridad select p.Tipo).FirstOrDefault();
            pedidoModel.IdAsignacionResponsable = Convert.ToInt32(tPedido.idAsignacionResponsable);
            pedidoModel.Responsable = (from c in db.tUsuario
                                       join ar in db.tAsignacionResponsable on c.id equals ar.idResponsable
                                       where ar.id == tPedido.idAsignacionResponsable
                                       select c.Nombre).FirstOrDefault();
            pedidoModel.IdModoResolucion = Convert.ToInt32(tPedido.idModoResolucion);
            pedidoModel.ModoResolucion = (from m in db.tModoResolucion where m.id == tPedido.idModoResolucion select m.Tipo).FirstOrDefault();

            return pedidoModel;
        }
    }
}