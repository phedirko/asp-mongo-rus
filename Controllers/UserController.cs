using AspMongo.Models.Api;
using AspMongo.Models.Persistance;
using AspMongo.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AspMongo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Post(UserApiModel model)
        {
            var dbUser = _userRepository
                .Insert(new User
                {
                    UserName = model.UserName,
                    Password = model.Password
                });

            return Ok(new UserResponseModel { Id = dbUser.Id, UserName = dbUser.UserName });
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository
                .GetAll()
                .Select(
                    x => new UserResponseModel
                    {
                        Id = x.Id,
                        UserName = x.UserName
                    });

            return Ok(users);
        }

    }
}
