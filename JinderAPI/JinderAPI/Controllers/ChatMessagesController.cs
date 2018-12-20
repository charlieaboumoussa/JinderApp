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
    public class ChatMessagesController : ApiController
    {
        private JinderDBEntities1 db = new JinderDBEntities1();

        // GET: api/ChatMessages
        public IQueryable<ChatMessage> GetChatMessages()
        {
            return db.ChatMessages;
        }

        // GET: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public IHttpActionResult GetChatMessage(int id)
        {
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            return Ok(chatMessage);
        }

        // PUT: api/ChatMessages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChatMessage(int id, ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chatMessage.ChatMessageId)
            {
                return BadRequest();
            }

            db.Entry(chatMessage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(id))
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

        // POST: api/ChatMessages
        [ResponseType(typeof(ChatMessage))]
        public IHttpActionResult PostChatMessage(ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ChatMessages.Add(chatMessage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = chatMessage.ChatMessageId }, chatMessage);
        }

        // DELETE: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public IHttpActionResult DeleteChatMessage(int id)
        {
            ChatMessage chatMessage = db.ChatMessages.Find(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            db.ChatMessages.Remove(chatMessage);
            db.SaveChanges();

            return Ok(chatMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatMessageExists(int id)
        {
            return db.ChatMessages.Count(e => e.ChatMessageId == id) > 0;
        }
    }
}