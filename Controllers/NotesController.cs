using AspMongo.Models.Api;
using AspMongo.Models.Persistance;
using AspMongo.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace AspMongo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;

        public NotesController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }


        /// <summary>
        /// Returns all notes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var dbNotes = _noteRepository.GetAll();
            return Ok(dbNotes);
        }

        /// <summary>
        /// Get notes by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var dbNote = _noteRepository.GetById(id);

            return Ok(dbNote);
        }


        /// <summary>
        /// Get all user notes by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("userId/{userId}")]
        public IActionResult GetByUserId(Guid userId)
        {
            var dbNotes = _noteRepository.GetByUserId(userId);

            return Ok(dbNotes);
        }


        /// <summary>
        /// Post new note for user
        /// </summary>
        /// <param name="noteModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] NoteApiModel noteModel)
        {
            var useId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var dbNote = _noteRepository
                .Insert(new Note
                {
                    Text = noteModel.Text,
                    Title = noteModel.Title,
                    UserId = useId
                });

            return Ok(dbNote);
        }
    }
}
