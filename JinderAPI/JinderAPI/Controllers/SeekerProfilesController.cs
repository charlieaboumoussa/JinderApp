using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using JinderAPI;

namespace JinderAPI.Controllers
{
    public class SeekerProfilesController : ApiController
    {
        private JinderDBEntities1 db = new JinderDBEntities1();

        // GET: api/SeekerProfiles
        public IQueryable<SeekerProfile> GetSeekerProfiles()
        {
            return db.SeekerProfiles;
        }

        // GET: api/SeekerProfiles/5
        [ResponseType(typeof(SeekerProfile))]
        public IHttpActionResult GetSeekerProfile(int id)
        {
            SeekerProfile seekerProfile = db.SeekerProfiles.Find(id);
            if (seekerProfile == null)
            {
                return NotFound();
            }

            return Ok(seekerProfile);
        }

        // PUT: api/SeekerProfiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSeekerProfile(int id, SeekerProfile seekerProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seekerProfile.SeekerProfileId)
            {
                return BadRequest();
            }

            db.Entry(seekerProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeekerProfileExists(id))
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

        // POST: api/SeekerProfiles
        [ResponseType(typeof(SeekerProfile))]
        public IHttpActionResult PostSeekerProfile(SeekerProfile seekerProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SeekerProfiles.Add(seekerProfile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = seekerProfile.SeekerProfileId }, seekerProfile);
        }

        // DELETE: api/SeekerProfiles/5
        [ResponseType(typeof(SeekerProfile))]
        public IHttpActionResult DeleteSeekerProfile(int id)
        {
            SeekerProfile seekerProfile = db.SeekerProfiles.Find(id);
            if (seekerProfile == null)
            {
                return NotFound();
            }

            db.SeekerProfiles.Remove(seekerProfile);
            db.SaveChanges();

            return Ok(seekerProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeekerProfileExists(int id)
        {
            return db.SeekerProfiles.Count(e => e.SeekerProfileId == id) > 0;
        }
    }
}