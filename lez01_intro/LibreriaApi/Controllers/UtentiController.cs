using LibreriaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtentiController : ControllerBase
{
    private static readonly List<Utente> Utenti =
    [
        new Utente
        {
            Id = 1,
            Nome = "Mario",
            Cognome = "Rossi",
            Email = "mario.rossi@example.com"
        },
        new Utente
        {
            Id = 2,
            Nome = "Giulia",
            Cognome = "Bianchi",
            Email = "giulia.bianchi@example.com"
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Utente>> GetAll()
    {
        return Ok(Utenti);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Utente> GetById(int id)
    {
        var utente = Utenti.FirstOrDefault(u => u.Id == id);

        if (utente is null)
        {
            return NotFound();
        }

        return Ok(utente);
    }

    [HttpPost]
    public ActionResult<Utente> Create(Utente nuovoUtente)
    {
        nuovoUtente.Id = Utenti.Count == 0 ? 1 : Utenti.Max(u => u.Id) + 1;
        Utenti.Add(nuovoUtente);

        return CreatedAtAction(nameof(GetById), new { id = nuovoUtente.Id }, nuovoUtente);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Utente utenteAggiornato)
    {
        var utente = Utenti.FirstOrDefault(u => u.Id == id);

        if (utente is null)
        {
            return NotFound();
        }

        utente.Nome = utenteAggiornato.Nome;
        utente.Cognome = utenteAggiornato.Cognome;
        utente.Email = utenteAggiornato.Email;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var utente = Utenti.FirstOrDefault(u => u.Id == id);

        if (utente is null)
        {
            return NotFound();
        }

        Utenti.Remove(utente);

        return NoContent();
    }
}
