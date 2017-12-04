using MobileServices;
using MobileServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MobileServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class tComentariosController : ApiController
    {
        private NaexCRMEntities db = new NaexCRMEntities();
        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();
        }

        // GET: api/tComentarios
        public List<ComentarioModel> GettComentario()
        {
            IQueryable<tPedido> query = db.tPedido.OrderBy(n => n.Estado).ThenByDescending(x => x.id).Take(50);
            
            /*IQueryable<tPedido> query = db.tPedido.Where(x => x.Estado == 0).OrderByDescending(x => x.id).Take(50);
            if (query.Count() != 50)
            {
                query = db.tPedido.OrderByDescending(x => x.id).Take(50);
            }*/

            List<ComentarioModel> comentarioModelList = new List<ComentarioModel>();
            foreach (var s in query)
            {
                List<tComentario> comentarios = (from c in db.tComentario where c.idPedido == s.id select c).ToList();
                foreach (var c in comentarios)
                {
                    ComentarioModel comentarioModel = ComentarioModel.comentarioModel(c);
                    comentarioModelList.Add(comentarioModel);                    
                }
            }

            return comentarioModelList;
        }
        
        // GET: api/tComentarios/5
        [ResponseType(typeof(tComentario))]
        public IHttpActionResult GettComentario(decimal id)
        {
            tComentario tComentario = db.tComentario.Find(id);
            if (tComentario == null)
            {
                return NotFound();
            }

            return Ok(tComentario);
        }

        // PUT: api/tComentarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttComentario(decimal id, tComentario tComentario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tComentario.id)
            {
                return BadRequest();
            }

            db.Entry(tComentario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tComentarioExists(id))
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

        // POST: api/tComentarios
        [ResponseType(typeof(tComentario))]
        public IHttpActionResult PosttComentario(tComentario tComentario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tComentario.Add(tComentario);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tComentario.id }, tComentario);
        }

        // DELETE: api/tComentarios/5
        [ResponseType(typeof(tComentario))]
        public IHttpActionResult DeletetComentario(decimal id)
        {
            tComentario tComentario = db.tComentario.Find(id);
            if (tComentario == null)
            {
                return NotFound();
            }

            db.tComentario.Remove(tComentario);
            db.SaveChanges();

            return Ok(tComentario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tComentarioExists(decimal id)
        {
            return db.tComentario.Count(e => e.id == id) > 0;
        }
    }
}