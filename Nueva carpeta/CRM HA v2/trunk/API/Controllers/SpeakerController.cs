using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MobileServices.Models;

namespace MobileServices.Controllers
{
    public class SpeakerController : ApiController
    {
        private ArgentinaConvocaEntities db = new ArgentinaConvocaEntities();

        // GET api/Speaker
        public IEnumerable<Speaker> GetSpeakers()
        {
            return db.Speaker.AsEnumerable();
        }

        // GET api/Speaker/5
        public Speaker GetSpeaker(long id)
        {
            Speaker speaker = db.Speaker.Single(s => s.IdSpeaker == id);
            if (speaker == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return speaker;
        }

        // PUT api/Speaker/5
        public HttpResponseMessage PutSpeaker(long id, Speaker speaker)
        {
            if (ModelState.IsValid && id == speaker.IdSpeaker)
            {
                db.Speaker.Attach(speaker);
                db.ObjectStateManager.ChangeObjectState(speaker, EntityState.Modified);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Speaker
        public HttpResponseMessage PostSpeaker(Speaker speaker)
        {
            if (ModelState.IsValid)
            {
                db.Speaker.AddObject(speaker);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, speaker);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = speaker.IdSpeaker }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Speaker/5
        public HttpResponseMessage DeleteSpeaker(long id)
        {
            Speaker speaker = db.Speaker.Single(s => s.IdSpeaker == id);
            if (speaker == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Speaker.DeleteObject(speaker);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, speaker);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}