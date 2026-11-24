using DSW_3P_PR03.Data;
using DSW_3P_PR03.Models;


namespace DSW_3P_PR03.Repositories;

public class PersonajeSQLRepository : IPersonajeRepository
{
    private readonly AppDbContext _context;

    public PersonajeSQLRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Personaje> ObtenerTodas()
    {
        return _context.Personajes.ToList();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return _context.Personajes.Find(id);
    }

    public Personaje Agregar(Personaje personaje)
    {
        personaje.Id = 0;

        _context.Personajes.Add(personaje);
        _context.SaveChanges();

        return personaje;
    }

    public bool Actualizar(int id, Personaje personaje)
    {
        var actual = _context.Personajes.Find(id);

        if(actual == null)
            return false;
        actual.Nombre = personaje.Nombre;
        actual.Videojuego = personaje.Videojuego;
        actual.Clase = personaje.Clase;
        actual.Nivel = personaje.Nivel;
        actual.Poder = personaje.Poder;
        actual.Disponible = personaje.Disponible;
        actual.ImagenUrl = personaje.ImagenUrl;

        _context.SaveChanges();

        return true;        
    }

    public bool Eliminar(int id)
    {
        var personaje = _context.Personajes.Find(id);

        if(personaje == null)
            return false;
        _context.Personajes.Remove(personaje);
        _context.SaveChanges();

        return true;
    }

    //Obtener por videojuego
    public List<Personaje> ObtenerPorVideojuego(string videojuego)
    {
        return _context.Personajes.Where(p => p.Videojuego == videojuego).ToList();
    }

    //Obtener por nivel

    public List<Personaje> ObtenerPorNivel(int nivel)
    {
        return _context.Personajes
            .Where(p => p.Nivel >= nivel)
            .ToList();
    }


 


}   