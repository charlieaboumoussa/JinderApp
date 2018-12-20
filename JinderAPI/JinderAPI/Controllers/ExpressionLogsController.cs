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
using JinderAPI.Models;

namespace JinderAPI.Controllers
{
    public class ExpressionLogsController : ApiController
    {
        private JinderDBEntities1 db = new JinderDBEntities1();

        // GET: api/ExpressionLogs
        public IQueryable<ExpressionLog> GetExpressionLogs()
        {
            return db.ExpressionLogs;
        }

        // GET: api/ExpressionLogs/5
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult GetExpressionLog(int id)
        {
            ExpressionLog expressionLog = db.ExpressionLogs.Find(id);
            if (expressionLog == null)
            {
                return NotFound();
            }

            return Ok(expressionLog);
        }

        // PUT: api/ExpressionLogs/5
        [HttpPost]
        [Route(" api/expressions")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExpressionLog([FromBody] ExpressionLogDTO expressionLogDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ExpressionLog expression = new ExpressionLog();
            expression.SourceUserId = expressionLogDTO.SourceUserId;
            expression.TargetUserId = expressionLogDTO.TargetUserId;
            expression.IsInterested = expressionLogDTO.IsInterested;
            expression.ExpressionDate = DateTime.Now;
            expression.JinderUser = db.JinderUsers.Find(expressionLogDTO.SourceUserId);
            expression.JinderUser1 = db.JinderUsers.Find(expressionLogDTO.TargetUserId);
            db.ExpressionLogs.Add(expression);

            foreach(ExpressionLog e in db.ExpressionLogs)
            {               
                    if(expression.SourceUserId == e.TargetUserId && expression.TargetUserId == e.SourceUserId)
                    {
                       //JinderUser matchedUser =  db.JinderUsers.Find(e.SourceUserId);
                        return Ok(true);
                    }                
            }

            return Ok();
        }

        // POST: api/ExpressionLogs
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult PostExpressionLog(ExpressionLog expressionLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExpressionLogs.Add(expressionLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expressionLog.ExpressionLogId }, expressionLog);
        }

        // DELETE: api/ExpressionLogs/5
        [ResponseType(typeof(ExpressionLog))]
        public IHttpActionResult DeleteExpressionLog(int id)
        {
            ExpressionLog expressionLog = db.ExpressionLogs.Find(id);
            if (expressionLog == null)
            {
                return NotFound();
            }

            db.ExpressionLogs.Remove(expressionLog);
            db.SaveChanges();

            return Ok(expressionLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpressionLogExists(int id)
        {
            return db.ExpressionLogs.Count(e => e.ExpressionLogId == id) > 0;
        }
    }
}