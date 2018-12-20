using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using JinderAPI;
using JinderAPI.Models;

namespace JinderAPI.Controllers
{
    [RoutePrefix("api/users")]
    public class JinderUsersController : ApiController
    {
        private JinderDBEntities1 db = new JinderDBEntities1();

        [Route("")]
        public IHttpActionResult PostJinderUser([FromBody]JinderUserDTO jinderUserSignUpDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            JinderUser jinderUser = new JinderUser();
            fromDTO(jinderUser, jinderUserSignUpDTO);
            db.JinderUsers.Add(jinderUser);
            db.SaveChanges();            
            return Ok();
        }

        [HttpGet]
        [Route("~/api/login")]
        public IHttpActionResult SignIn(SignInInfoDTO dto)
        {
            var user = (from ju in db.JinderUsers
                           where ju.Username == dto.Username && ju.Password == dto.Password
                           select ju).FirstOrDefault<JinderUser>();

            if (user == null)
            {
                return NotFound();
            }
            else
            {      
                
                    JinderUserDTO jinderUserLoginDTO = new JinderUserDTO();
                    jinderUserLoginDTO.FullName = user.FullName;
                    jinderUserLoginDTO.DateOfBirth = user.DateOfBirth;
                    jinderUserLoginDTO.ProfilePicture = user.ProfilePicture;
                    jinderUserLoginDTO.Gender = user.Gender;
                    jinderUserLoginDTO.Address = user.Address;
                    jinderUserLoginDTO.UserType = user.UserType;
                    jinderUserLoginDTO.Username = user.Username;
                    jinderUserLoginDTO.Location = user.Location;
                    return Ok(jinderUserLoginDTO);
                
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{id}/candidates")]
        public IHttpActionResult GetJinderUsers(String id, [FromUri] int page = 1, [FromUri] int results_per_page = 5)
        {
            if (page < 1)
                return BadRequest("parameter 'page' cannot be less than 1.");
            if (results_per_page < 1)
                return BadRequest("parameter 'results_per_page' cannot be less than 1.");

            //Authorize
            var identity = User.Identity as ClaimsIdentity;
            string authorizedUsername = (from claim in identity.Claims
                                         where claim.Type == "id"
                                         select claim.Value).FirstOrDefault<string>();
            if (authorizedUsername != id)
            {
                return Unauthorized();
            }

            JinderUser jinderUser = (from user in db.JinderUsers
                                     where user.JinderUserId.ToString() == id
                                     select user).FirstOrDefault<JinderUser>();
            if (jinderUser == null)
            {
                return NotFound();
            }
            if (jinderUser.UserType == "seeker")
                return Ok(HandleSeekerCandidates(id, page, results_per_page));
            else
                return Ok(HandleJobPostCandidates(id, page, results_per_page));
        }
        private List<JobPost> HandleSeekerCandidates(String id, int page, int results_per_page)
        {
            JinderUser currentUser = (from user in db.JinderUsers
                                      where user.JinderUserId.ToString() == id
                                      select user).FirstOrDefault<JinderUser>();
            var seenPosts = from el in db.ExpressionLogs
                            where el.SourceUserId.ToString() == currentUser.JinderUserId.ToString()
                            select el.TargetUserId;
            var unseenPosts = (from jp in db.JobPosts
                               where !seenPosts.Contains(jp.PosterId)
                               select jp)
                              .Skip<JobPost>((page - 1) * results_per_page)
                              .Take<JobPost>(results_per_page);
            return (List<JobPost>)unseenPosts;
        }
        private List<SeekerProfile> HandleJobPostCandidates(String id, int page, int results_per_page)
        {
            JinderUser currentUser = (from user in db.JinderUsers
                                      where user.JinderUserId.ToString() == id
                                      select user).FirstOrDefault<JinderUser>();
            var seenPosts = from el in db.ExpressionLogs
                            where el.SourceUserId.ToString() == currentUser.JinderUserId.ToString()
                            select el.TargetUserId;
            var unseenPosts = (from sp in db.SeekerProfiles
                               where !seenPosts.Contains(sp.JinderUserId)
                               select sp)
                              .Skip<SeekerProfile>((page - 1) * results_per_page)
                              .Take<SeekerProfile>(results_per_page);
            return (List<SeekerProfile>)unseenPosts;
        }
        [HttpGet]
        [Route("/{id}/matches")]
        public IHttpActionResult GetMatches(String id)
        {
            //   List<CandidateDTO> matches = new List<CandidateDTO>();
            if (db.JinderUsers.Find(id) == null)
            {
                return NotFound();
            }
            List<JinderUser> matches = new List<JinderUser>();
            var userLogs = from el in db.ExpressionLogs
                            where el.SourceUserId.ToString() == id
                            select el;

            foreach (ExpressionLog e in db.ExpressionLogs)
            {
                foreach(ExpressionLog userLog in userLogs){
                    if (userLog.SourceUserId == e.TargetUserId && userLog.TargetUserId == e.SourceUserId)
                    {
                        JinderUser match = db.JinderUsers.Find(e.SourceUserId);
                        matches.Add(match);                        
                    }
                }
            }
            if (matches.Count == 0)
            {
                return Ok();
            }
            else
            {
                return Ok(matches);
            }
        }

        [Route("{id}")]
        [ResponseType(typeof(JinderUser))]
        public IHttpActionResult GetJinderUser(int id)
        {
            JinderUser jinderUser = db.JinderUsers.Find(id);
            if (jinderUser == null)
            {
                return NotFound();
            }

            return Ok(jinderUser);
        }

        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJinderUser(int id, JinderUser jinderUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jinderUser.JinderUserId)
            {
                return BadRequest();
            }

            db.Entry(jinderUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JinderUserExists(id))
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

    

        [Route("{id}")]
        [ResponseType(typeof(JinderUser))]
        public IHttpActionResult DeleteJinderUser(int id)
        {
            JinderUser jinderUser = db.JinderUsers.Find(id);
            if (jinderUser == null)
            {
                return NotFound();
            }

            db.JinderUsers.Remove(jinderUser);
            db.SaveChanges();

            return Ok(jinderUser);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JinderUserExists(int id)
        {
            return db.JinderUsers.Count(e => e.JinderUserId == id) > 0;
        }
        private void fromDTO(JinderUser user,JinderUserDTO dto) {
            user.FullName = dto.FullName;
            user.DateOfBirth = dto.DateOfBirth;
            user.ProfilePicture = dto.ProfilePicture;
            user.Gender = dto.Gender;
            user.Address = dto.Address;
            user.UserType = dto.UserType;
            user.Username = dto.Username;
            user.Password = dto.Password;
            user.Location = dto.Location;
        }
    } 
}