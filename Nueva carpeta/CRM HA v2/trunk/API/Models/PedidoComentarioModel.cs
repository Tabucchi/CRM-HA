using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileServices.Models
{
    public class PedidoComentarioModel
    {
        //public Int64 IdPedido { get; set; }

        public List<ComentarioModel> comentarios { get; set; }

        //public ComentarioModel comentario { get; set; }
    }
}