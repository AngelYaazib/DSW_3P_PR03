using Microsoft.AspNetCore.Mvc;
using DSW_3P_PR03.Models;
using DSW_3P_PR03.Services;
using Microsoft.AspNetCore.Authorization;

namespace DSW_3P_PR03.Controllers;

[ApiController]
[Route("api/personajes")]
[Authorize]
public class PersonajeController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajeController(IPersonajeService service)
    {
        _service = service;
    }

    // GET: api/personajes
    [HttpGet]
    public ActionResult<List<Personaje>> ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
    }

    // GET: api/personajes/1
    [HttpGet("{id:int}")]
    public ActionResult<Personaje> ObtenerPorId(int id)
    {
        var personaje = _service.ObtenerPorId(id);

        if (personaje == null)
            return NotFound(new { mensaje = "El personaje no existe." });

        return Ok(personaje);
    }

    // GET: api/personajes/videojuego/Skyrim
    [HttpGet("videojuego/{nombre}")]
    public ActionResult<List<Personaje>> ObtenerPorVideojuego(string nombre)
    {
        var personajes = _service.ObtenerPorVideojuego(nombre);

        return Ok(personajes);
    }

    // GET: api/personajes/nivel/50
    [HttpGet("nivel/{nivel:int}")]
    public ActionResult<List<Personaje>> ObtenerPorNivel(int nivel)
    {
        try
        {
            var personajes = _service.ObtenerPorNivel(nivel);
            return Ok(personajes);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // POST: api/personajes
    [HttpPost]
    public ActionResult<Personaje> Agregar([FromBody] Personaje personaje)
    {
        try
        {
            var nuevoPersonaje = _service.Agregar(personaje);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = nuevoPersonaje.Id },
                nuevoPersonaje
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // PUT: api/personajes/1
    [HttpPut("{id:int}")]
    public ActionResult Actualizar(int id, [FromBody] Personaje personaje)
    {
        try
        {
            bool actualizado = _service.Actualizar(id, personaje);

            if (!actualizado)
                return NotFound(new { mensaje = "El personaje no existe." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // DELETE: api/personajes/1
    [HttpDelete("{id:int}")]
    public ActionResult Eliminar(int id)
    {
        bool eliminado = _service.Eliminar(id);

        if (!eliminado)
            return NotFound(new { mensaje = "El personaje no existe." });

        return NoContent();
    }
}
