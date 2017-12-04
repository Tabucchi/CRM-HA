using MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace MobileServices.Models
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class tPedidoController : ApiController
    {
        private NaexCRMEntities db = new NaexCRMEntities();

        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();
        }

        // GET: api/tPedido
        // [Route("api/getLast50Tickets")]
        // [HttpGet]
        public List<PedidoModel> GettPedido()
        {
            IQueryable<tPedido> query = db.tPedido.OrderBy(n => n.Estado).ThenByDescending(x => x.id).Take(50);
                        
            List<PedidoModel> pedidoModelList = new List<PedidoModel>();
            foreach (var s in query)
            {
                PedidoModel pedidoModel = PedidoModel.pedidoModel(s);
                pedidoModelList.Add(pedidoModel);             
            }
           
            return pedidoModelList;
        }
        
        // GET: api/tPedido/5
        public PedidoModel GettPedido(decimal id)
        {
            tPedido tPedido = db.tPedido.Find(id);
            return PedidoModel.pedidoModel(tPedido);
        }

       /* [ResponseType(typeof(tPedido))]
        public IHttpActionResult GettPedido(decimal id)
        {
            tPedido tPedido = db.tPedido.Find(id);
            if (tPedido == null)
            {
                return NotFound();
            }

            return Ok(tPedido);
        }*/
        
        // PUT: api/tPedido/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttPedido(decimal id, tPedido tPedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tPedido.id)
            {
                return BadRequest();
            }

            db.Entry(tPedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tPedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tPedido
        [ResponseType(typeof(tPedido))]
        public IHttpActionResult PosttPedido(tPedido tPedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tPedido.Add(tPedido);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tPedido.id }, tPedido);
        }

        // DELETE: api/tPedido/5
        [ResponseType(typeof(tPedido))]
        public IHttpActionResult DeletetPedido(decimal id)
        {
            tPedido tPedido = db.tPedido.Find(id);
            if (tPedido == null)
            {
                return NotFound();
            }

            db.tPedido.Remove(tPedido);
            db.SaveChanges();

            return Ok(tPedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tPedidoExists(decimal id)
        {
            return db.tPedido.Count(e => e.id == id) > 0;
        }
    }
}