﻿using DemoSvelte.Models;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAppUserService appUserService;
        public AuthentificationController(IAppUserService appUserService)
        {
            this.appUserService = appUserService;
        }

        [HttpGet("GetAppUsers")]
        public ActionResult<List<AppUser>> GetAppUsers()
        {
            return appUserService.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetAppUserbyId(string id)
        {
            var appUser = appUserService.Get(id);

            if (appUser == null)
            {
                return NotFound($"AppUser with Id = {id} not found");
            }

            return appUser;
        }

        [HttpPost("AddAppUser")]
        public ActionResult<AppUser> Post([FromBody] AppUser appUser)
        {
            appUserService.Create(appUser);

            return CreatedAtAction(nameof(GetAppUsers), new { id = appUser.Id }, appUser);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] AppUser appUser)
        {
            var existingProperty = appUserService.Get(id);

            if (existingProperty == null)
            {
                return NotFound($"AppUser with Id = {id} not found");
            }

            appUserService.Update(id, appUser);

            return NoContent();
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteAppUser(string id)
        {
            var appUser = appUserService.Get(id);

            if (appUser == null)
            {
                return NotFound($"AppUser with Id = {id} not found");
            }

            appUserService.Delete(appUser.Id);

            return Ok($"AppUser with Id = {id} deleted");
        }
    }
}