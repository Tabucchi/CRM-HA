using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileServices.Models
{
    public class ComentarioModel
    {
        public Int64 Id {get;set;}
        public Int64 IdPedido {get;set;}
        public Int64 IdUsuario {get;set;}
        public string Usuario { get; set; }
        public string Fecha {get;set;}
        public string Descripcion {get;set;}
        public bool VisibilidadCliente {get;set;}
        public string Tipo {get;set;}

        public static ComentarioModel comentarioModel(tComentario tComentario)
        {
            NaexCRMEntities db = new NaexCRMEntities();
            ComentarioModel comentarioModel = new ComentarioModel();
            comentarioModel.Id = Convert.ToInt64(tComentario.id);
            comentarioModel.IdPedido = Convert.ToInt64(tComentario.idPedido);
            comentarioModel.IdUsuario = Convert.ToInt64(tComentario.idUsuario);
            comentarioModel.Usuario = (from u in db.tUsuario where u.id == comentarioModel.IdUsuario select u.Nombre).FirstOrDefault();
            comentarioModel.Fecha = String.Format("{0:dd/MM/yyyy}", tComentario.Fecha);
            comentarioModel.Descripcion = tComentario.Descripcion;
            comentarioModel.VisibilidadCliente = Convert.ToBoolean(tComentario.VisibilidadCliente);
            comentarioModel.Tipo = tComentario.Tipo;
            return comentarioModel;
        }
    }
}