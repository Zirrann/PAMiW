using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Shop.DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CrudController<TEntity, TDto, TKey> : ControllerBase
        where TEntity : class
        where TDto : class
    {
        protected readonly ICrudService<TEntity, TKey> _service;
        protected readonly IMapper _mapper;

        protected CrudController(ICrudService<TEntity, TKey> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/[controller]
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            if (response.Success)
            {
                var dtos = _mapper.Map<IEnumerable<TDto>>(response.Data);
                return Ok(dtos);
            }
            return StatusCode(500, response.Message);
        }

        // GET: api/[controller]/{id}
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(TKey id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response.Success)
            {
                var dto = _mapper.Map<TDto>(response.Data);
                return Ok(dto);
            }
            if (response.Data == null)
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // POST: api/[controller]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TDto dto)
        {
            if (dto == null)
                return BadRequest($"{typeof(TDto).Name} cannot be null.");

            var entity = _mapper.Map<TEntity>(dto);
            var response = await _service.CreateAsync(entity);
            if (response.Success)
            {
                var createdDto = _mapper.Map<TDto>(response.Data);
                return CreatedAtAction(nameof(GetById), new { id = createdDto }, createdDto);
            }
            return StatusCode(500, response.Message);
        }

        // PUT: api/[controller]/{id}
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update([FromRoute] TKey id, [FromBody] TDto dto)
        {
            if (dto == null)
                return BadRequest($"{typeof(TDto).Name} cannot be null.");

            var entity = _mapper.Map<TEntity>(dto);
            var response = await _service.UpdateAsync(id, entity);
            if (response.Success)
            {
                var updatedDto = _mapper.Map<TDto>(response.Data);
                return Ok(updatedDto);
            }
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }



        // DELETE: api/[controller]/{id}
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            var response = await _service.DeleteAsync(id);
            if (response.Success)
                return Ok(response.Message);
            if (response.Message == "Entity not found.")
                return NotFound(response.Message);
            return StatusCode(500, response.Message);
        }

        // DELETE: api/[controller]/all
        [HttpDelete("all")]
        public virtual async Task<IActionResult> DeleteAll()
        {
            var response = await _service.DeleteAllAsync(); 
            if (response.Success)
                return Ok(response.Message);
            return StatusCode(500, response.Message);
        }

    }
}
