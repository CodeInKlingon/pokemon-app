
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{   
    [ApiController]
    [Route("api/trainers")]

    public class TrainerController : ControllerBase
    {
        // GET: TrainersController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (var db = new DatabaseContext()) { 
                var trainers = await db.Trainers.ToListAsync();
                return Ok(trainers);
            }
        }

        // POST: TrainerController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Trainer trainer)
        {
            if (trainer == null || string.IsNullOrEmpty(trainer.Name) || string.IsNullOrEmpty(trainer.Region))
            {
                return BadRequest("Name and Region are required.");
            }

            var newTrainer = new Trainer() { Name = trainer.Name, Region = trainer.Region };

            using (var db = new DatabaseContext())
            { 
                await db.Trainers.AddAsync(newTrainer);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = newTrainer.TrainerId }, newTrainer);
            }
        }
    }
}
