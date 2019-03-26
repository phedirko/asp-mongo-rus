using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspMongo.Models.Persistance;
using AspMongo.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AspMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public ValuesController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<object> Get()
        {
            return Ok(_userRepository.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<object> Get(Guid id)
        {
            return _userRepository.GetById(id);
        }

        [HttpGet("post")]
        public ActionResult<object> Post()
        {
            return _userRepository
                .Insert(
                    new User
                    {
                        UserName = "user" + DateTime.Now.Millisecond.ToString(),
                        Password = "pass"
                    });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
