using System.Net;
using System.Net.Http.Json;
using LibreriaApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibreriaApi.Tests;

public class UtentiFeatureTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_ReturnsSeededUsers()
    {
        var utenti = await _client.GetFromJsonAsync<List<Utente>>("/api/utenti");

        Assert.NotNull(utenti);
        Assert.Contains(utenti, utente =>
            utente.Id == 1 &&
            utente.Nome == "Mario" &&
            utente.Email == "mario.rossi@example.com");
        Assert.Contains(utenti, utente =>
            utente.Id == 2 &&
            utente.Nome == "Giulia" &&
            utente.Email == "giulia.bianchi@example.com");
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/utenti/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UserCrudLifecycle_WorksThroughHttpApi()
    {
        var nuovoUtente = new Utente
        {
            Nome = "Ada",
            Cognome = "Lovelace",
            Email = "ada.lovelace@example.com"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/utenti", nuovoUtente);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createResponse.Headers.Location);

        var utenteCreato = await createResponse.Content.ReadFromJsonAsync<Utente>();
        Assert.NotNull(utenteCreato);
        Assert.True(utenteCreato.Id > 0);
        Assert.Equal(nuovoUtente.Email, utenteCreato.Email);

        var getResponse = await _client.GetAsync($"/api/utenti/{utenteCreato.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var utenteLetto = await getResponse.Content.ReadFromJsonAsync<Utente>();
        Assert.NotNull(utenteLetto);
        Assert.Equal("Ada", utenteLetto.Nome);

        var utenteAggiornato = new Utente
        {
            Id = utenteCreato.Id,
            Nome = "Grace",
            Cognome = "Hopper",
            Email = "grace.hopper@example.com"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/utenti/{utenteCreato.Id}", utenteAggiornato);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var utenteDopoUpdate = await _client.GetFromJsonAsync<Utente>($"/api/utenti/{utenteCreato.Id}");
        Assert.NotNull(utenteDopoUpdate);
        Assert.Equal("Grace", utenteDopoUpdate.Nome);
        Assert.Equal("grace.hopper@example.com", utenteDopoUpdate.Email);

        var deleteResponse = await _client.DeleteAsync($"/api/utenti/{utenteCreato.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await _client.GetAsync($"/api/utenti/{utenteCreato.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
}
