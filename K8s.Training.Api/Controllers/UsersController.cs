using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using K8s.Training.Data.Contexts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace K8s.Training.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UsersDbContext context, IMapper mapper, ILogger<UsersController> logger)
        {
            ArgumentNullException.ThrowIfNull(nameof(context));
            ArgumentNullException.ThrowIfNull(nameof(mapper));

            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogInformation($"Invoked {nameof(GetAsync)}");

            DTO.User[]? users = await _context.Users?.ProjectTo<DTO.User>(_mapper.ConfigurationProvider)?.ToArrayAsync();

            if (users?.Length == 0)
            {
                return NotFound();
            }

            DTO.User[] userDtos = _mapper.Map<DTO.User[]>(users);

            return Ok(userDtos);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Invoked {nameof(GetByIdAsync)}");

            if (id <= 0)
            {
                return BadRequest();
            }

            DTO.User? user = await _context?.Users?.ProjectTo<DTO.User>(_mapper.ConfigurationProvider)?.FirstOrDefaultAsync(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> PostAsync(DTO.User newUser)
        {
            Domain.Entities.User user = _mapper.Map<Domain.Entities.User>(newUser);
            _ = await _context.Users.AddAsync(user);
            _ = await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] DTO.User updatedUser)
        {
            if (id != updatedUser?.Id)
            {
                return BadRequest();
            }

            Domain.Entities.User user = _mapper.Map<Domain.Entities.User>(updatedUser);
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _ = _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users?.AnyAsync(u => u.Id == user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Domain.Entities.User? user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            _ = _context.Users.Remove(user);
            _ = await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
