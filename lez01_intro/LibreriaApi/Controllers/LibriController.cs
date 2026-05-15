using LibreriaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibriController : ControllerBase
{
    private static readonly List<Libro> Libri =
    [
        new Libro
        {
            Id = 1,
            Titolo = "Il nome della rosa",
            Autore = "Umberto Eco",
            AnnoPubblicazione = 1980,
            Genere = "Romanzo storico"
        },
        new Libro
        {
            Id = 2,
            Titolo = "Le citta invisibili",
            Autore = "Italo Calvino",
            AnnoPubblicazione = 1972,
            Genere = "Narrativa"
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Libro>> GetAll()
    {
        return Ok(Libri);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Libro> GetById(int id)
    {
        var libro = Libri.FirstOrDefault(l => l.Id == id);

        if (libro is null)
        {
            return NotFound();
        }

        return Ok(libro);
    }

    [HttpPost]
    public ActionResult<Libro> Create(Libro nuovoLibro)
    {
        nuovoLibro.Id = Libri.Count == 0 ? 1 : Libri.Max(l => l.Id) + 1;
        Libri.Add(nuovoLibro);

        return CreatedAtAction(nameof(GetById), new { id = nuovoLibro.Id }, nuovoLibro);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Libro libroAggiornato)
    {
        var libro = Libri.FirstOrDefault(l => l.Id == id);

        if (libro is null)
        {
            return NotFound();
        }

        libro.Titolo = libroAggiornato.Titolo;
        libro.Autore = libroAggiornato.Autore;
        libro.AnnoPubblicazione = libroAggiornato.AnnoPubblicazione;
        libro.Genere = libroAggiornato.Genere;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var libro = Libri.FirstOrDefault(l => l.Id == id);

        if (libro is null)
        {
            return NotFound();
        }

        Libri.Remove(libro);

        return NoContent();
    }
}
