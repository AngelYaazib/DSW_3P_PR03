using DSW_3P_PR03.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace DSW_3P_PR03.Repositories;

public class PersonajeJsonRepository : IPersonajeRepository
{
    private readonly string _ruta;

    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };


public PersonajeJsonRepository(IWebHostEnvironment environment)
{
    _ruta = Path.Combine(
        environment.ContentRootPath,
        "Data",
        "Personajes.json"
    );

    Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);

    // Crear el archivo si no existe o está vacío
    if (!File.Exists(_ruta) ||
        new FileInfo(_ruta).Length == 0)
    {
        File.WriteAllText(_ruta, "[]");
    }
}



private List<Personaje> Leer()
{
    if (!File.Exists(_ruta))
    {
        File.WriteAllText(_ruta, "[]");
        return new List<Personaje>();
    }

    string json = File.ReadAllText(_ruta);

    if (string.IsNullOrWhiteSpace(json))
    {
        File.WriteAllText(_ruta, "[]");
        return new List<Personaje>();
    }

    return JsonSerializer.Deserialize<List<Personaje>>(
        json,
        _opciones
    ) ?? new List<Personaje>();
}


    private void Guardar(List<Personaje> datos)
    {
        string json = JsonSerializer.Serialize(datos, _opciones);
        File.WriteAllText(_ruta, json);
    }

    public List<Personaje> ObtenerTodas()
    {
        return Leer();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return Leer().FirstOrDefault(p => p.Id == id);
    }

    public List<Personaje> ObtenerPorVideojuego(string nombre)
    {
        return Leer()
            .Where(p => p.Videojuego.Equals(
                nombre,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Personaje> ObtenerPorNivel(int nivel)
    {
        return Leer()
            .Where(p => p.Nivel >= nivel)
            .ToList();
    }

    public Personaje Agregar(Personaje nuevoPersonaje)
    {
        var datos = Leer();

        nuevoPersonaje.Id = datos.Count > 0
            ? datos.Max(p => p.Id) + 1
            : 1;

        datos.Add(nuevoPersonaje);

        Guardar(datos);

        return nuevoPersonaje;
    }

    public bool Actualizar(int id, Personaje personajeActualizado)
    {
        var datos = Leer();

        var personajeExistente = datos.FirstOrDefault(p => p.Id == id);

        if (personajeExistente == null)
            return false;

        personajeExistente.Nombre = personajeActualizado.Nombre;
        personajeExistente.Videojuego = personajeActualizado.Videojuego;
        personajeExistente.Clase = personajeActualizado.Clase;
        personajeExistente.Nivel = personajeActualizado.Nivel;
        personajeExistente.Poder = personajeActualizado.Poder;
        personajeExistente.Disponible = personajeActualizado.Disponible;
        personajeExistente.ImagenUrl = personajeActualizado.ImagenUrl;

        Guardar(datos);

        return true;
    }

    public bool Eliminar(int id)
    {
        var datos = Leer();

        var personajeExistente = datos.FirstOrDefault(p => p.Id == id);

        if (personajeExistente == null)
            return false;

        datos.Remove(personajeExistente);

        Guardar(datos);

        return true;
    }
}
