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
    public class JobPostsController : ApiController
    {
        private JinderDBEntities1 db = new JinderDBEntities1();

        // GET: api/JobPosts
        public IQueryable<JobPost> GetJobPosts()
        {
            return db.JobPosts;
        }

        // GET: api/JobPosts/5
        [ResponseType(typeof(JobPost))]
        public IHttpActionResult GetJobPost(int id)
        {
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return Ok(jobPost);
        }

        // PUT: api/JobPosts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobPost(int id, JobPost jobPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobPost.JobPostId)
            {
                return BadRequest();
            }

            db.Entry(jobPost).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPostExists(id))
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

        // POST: api/JobPosts
        [ResponseType(typeof(JobPost))]
        public IHttpActionResult PostJobPost(JobPost jobPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobPosts.Add(jobPost);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = jobPost.JobPostId }, jobPost);
        }

        // DELETE: api/JobPosts/5
        [ResponseType(typeof(JobPost))]
        public IHttpActionResult DeleteJobPost(int id)
        {
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            db.JobPosts.Remove(jobPost);
            db.SaveChanges();

            return Ok(jobPost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobPostExists(int id)
        {
            return db.JobPosts.Count(e => e.JobPostId == id) > 0;
        }
    }
}