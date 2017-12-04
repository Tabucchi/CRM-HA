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
    public class tEmpresasController : ApiController
    {
        private NaexCRMEntities db = new NaexCRMEntities();

        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();
        }

        // GET: api/tEmpresas
        public List<EmpresaModel> GettEmpresa()
        {
            IQueryable<tEmpresa> query = db.tEmpresa.Where(x => x.Papelera == 1).OrderByDescending(x => x.id);
            
            List<EmpresaModel> empresaModelList = new List<EmpresaModel>();
            foreach (var e in query)
            {
                /*EmpresaModel empresaModel = new EmpresaModel();
                empresaModel.Id = Convert.ToInt64(e.id);
                empresaModel.Nombre = e.Nombre;
                empresaModel.Direccion = e.Direccion;
                empresaModel.Telefono = e.Telefono;
                empresaModel.Cuit = e.Cuit;
                empresaModelList.Add(empresaModel);*/
                EmpresaModel empresaModel = EmpresaModel.empresaModel(e);
                empresaModelList.Add(empresaModel);
            }

            return empresaModelList;
        }

        // GET: api/tEmpresas/5
        [ResponseType(typeof(tEmpresa))]
        public IHttpActionResult GettEmpresa(decimal id)
        {
            tEmpresa tEmpresa = db.tEmpresa.Find(id);
            if (tEmpresa == null)
            {
                return NotFound();
            }

            return Ok(tEmpresa);
        }

        // PUT: api/tEmpresas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttEmpresa(decimal id, tEmpresa tEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tEmpresa.id)
            {
                return BadRequest();
            }

            db.Entry(tEmpresa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tEmpresaExists(id))
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

        // POST: api/tEmpresas
        [ResponseType(typeof(tEmpresa))]
        public IHttpActionResult PosttEmpresa(tEmpresa tEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tEmpresa.Add(tEmpresa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tEmpresa.id }, tEmpresa);
        }

        // DELETE: api/tEmpresas/5
        [ResponseType(typeof(tEmpresa))]
        public IHttpActionResult DeletetEmpresa(decimal id)
        {
            tEmpresa tEmpresa = db.tEmpresa.Find(id);
            if (tEmpresa == null)
            {
                return NotFound();
            }

            db.tEmpresa.Remove(tEmpresa);
            db.SaveChanges();

            return Ok(tEmpresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tEmpresaExists(decimal id)
        {
            return db.tEmpresa.Count(e => e.id == id) > 0;
        }
    }
}