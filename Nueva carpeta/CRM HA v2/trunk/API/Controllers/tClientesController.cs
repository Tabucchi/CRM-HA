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
    public class tClientesController : ApiController
    {
        private NaexCRMEntities db = new NaexCRMEntities();
        
        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();
        }

        // GET: api/tClientes
        public List<ClienteModel> GettCliente()
        {
            IQueryable<tCliente> query = db.tCliente.Where(x => x.Papelera == 1).OrderByDescending(x => x.Nombre);

            List<ClienteModel> clienteModelList = new List<ClienteModel>();
            foreach (var c in query)
            {
                /*ClienteModel clienteModel = new ClienteModel();
                clienteModel.Id = Convert.ToInt64(c.id);
                clienteModel.Nombre = c.Nombre;
                clienteModel.Interno = c.Interno;
                clienteModel.Mail = c.Mail;
                clienteModel.ClaveSistema = ClienteModel.Decode(c.ClaveSistema);
                clienteModelList.Add(clienteModel);*/
                ClienteModel clienteModel = ClienteModel.clienteModel(c);
                clienteModelList.Add(clienteModel);
            }

            return clienteModelList;
        }

        // GET: api/tClientes/5
        [ResponseType(typeof(tCliente))]
        public IHttpActionResult GettCliente(decimal id)
        {
            tCliente tCliente = db.tCliente.Find(id);
            if (tCliente == null)
            {
                return NotFound();
            }

            return Ok(tCliente);
        }

        // PUT: api/tClientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttCliente(decimal id, tCliente tCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tCliente.id)
            {
                return BadRequest();
            }

            db.Entry(tCliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tClienteExists(id))
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

        // POST: api/tClientes
        [ResponseType(typeof(tCliente))]
        public IHttpActionResult PosttCliente(tCliente tCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tCliente.Add(tCliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tCliente.id }, tCliente);
        }

        // DELETE: api/tClientes/5
        [ResponseType(typeof(tCliente))]
        public IHttpActionResult DeletetCliente(decimal id)
        {
            tCliente tCliente = db.tCliente.Find(id);
            if (tCliente == null)
            {
                return NotFound();
            }

            db.tCliente.Remove(tCliente);
            db.SaveChanges();

            return Ok(tCliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tClienteExists(decimal id)
        {
            return db.tCliente.Count(e => e.id == id) > 0;
        }
    }
}