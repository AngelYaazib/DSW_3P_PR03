using DSW_3P_PR03.Models;
using DSW_3P_PR03.Repositories;

namespace DSW_3P_PR03.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    public List<Personaje> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Personaje? ObtenerPorId(int id)
    {
        if (id <= 0)
            return null;

        return _repository.ObtenerPorId(id);
    }

    public List<Personaje> ObtenerPorVideojuego(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del videojuego es obligatorio.");

        return _repository.ObtenerPorVideojuego(nombre);
    }

    public List<Personaje> ObtenerPorNivel(int nivel)
    {
        if (nivel < 1 || nivel > 100)
            throw new ArgumentException("El nivel debe estar entre 1 y 100.");

        return _repository.ObtenerPorNivel(nivel);
    }

    public Personaje Agregar(Personaje personaje)
    {
        ValidarPersonaje(personaje);

        personaje.Id = 0;
        personaje.Disponible = true;

        return _repository.Agregar(personaje);
    }

    public bool Actualizar(int id, Personaje personaje)
    {
        if (id <= 0)
            return false;

        ValidarPersonaje(personaje);

        var existente = _repository.ObtenerPorId(id);

        if (existente == null)
            return false;

        personaje.Id = id;

        return _repository.Actualizar(id, personaje);
    }

    public bool Eliminar(int id)
    {
        if (id <= 0)
            return false;

        return _repository.Eliminar(id);
    }

    private static void ValidarPersonaje(Personaje personaje)
    {
        if (personaje == null)
            throw new ArgumentNullException(nameof(personaje));

        if (string.IsNullOrWhiteSpace(personaje.Nombre))
            throw new ArgumentException("El nombre del personaje es obligatorio.");

        if (string.IsNullOrWhiteSpace(personaje.Videojuego))
            throw new ArgumentException("El videojuego del personaje es obligatorio.");

        if (string.IsNullOrWhiteSpace(personaje.Clase))
            throw new ArgumentException("La clase del personaje es obligatoria.");

        if (personaje.Nivel < 1 || personaje.Nivel > 100)
            throw new ArgumentException("El nivel debe estar entre 1 y 100.");

        if (personaje.Poder <= 0)
            throw new ArgumentException("El poder debe ser mayor que cero.");

        if (!Uri.TryCreate(
                personaje.ImagenUrl,
                UriKind.Absolute,
                out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp &&
             uriResult.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "La URL de la imagen debe ser absoluta y utilizar HTTP o HTTPS."
            );
        }
    }
}
