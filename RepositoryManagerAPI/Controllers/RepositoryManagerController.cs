using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryManagerAPI.DTO;
using RepositoryManagerLib;

namespace RepositoryManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryManagerController : ControllerBase
    {
        private readonly IRepositoryManager _repoManager;

        public RepositoryManagerController(IRepositoryManager repoManager)
        {
            _repoManager = repoManager;
            _repoManager.Initialize();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _repoManager.Register(request.ItemName, request.ItemContent, request.ItemType);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("retrieve")]
        public ActionResult<string> Retrieve(string itemName)
        {
            try
            {
                var content = _repoManager.Retrieve(itemName);
                return Ok(content);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("type")]
        public ActionResult<int> GetType(string itemName)
        {
            try
            {
                var type = _repoManager.GetType(itemName);
                return Ok(type);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("deregister")]
        public IActionResult Deregister(string itemName) {
            try
            {
                _repoManager.Deregister(itemName);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}