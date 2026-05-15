using System.Net;
using System.Net.Http.Json;
using LibreriaApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibreriaApi.Tests;

public class LibriFeatureTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_ReturnsSeededBooks()
    {
        var libri = await _client.GetFromJsonAsync<List<Libro>>("/api/libri");

        Assert.NotNull(libri);
        Assert.Contains(libri, libro =>
            libro.Id == 1 &&
            libro.Titolo == "Il nome della rosa" &&
            libro.Autore == "Umberto Eco");
        Assert.Contains(libri, libro =>
            libro.Id == 2 &&
            libro.Titolo == "Le citta invisibili" &&
            libro.Autore == "Italo Calvino");
    }

    [Fact]
    public async Task GetById_WhenBookDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/libri/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task BookCrudLifecycle_WorksThroughHttpApi()
    {
        var nuovoLibro = new Libro
        {
            Titolo = "Dune",
            Autore = "Frank Herbert",
            AnnoPubblicazione = 1965,
            Genere = "Fantascienza"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/libri", nuovoLibro);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createResponse.Headers.Location);

        var libroCreato = await createResponse.Content.ReadFromJsonAsync<Libro>();
        Assert.NotNull(libroCreato);
        Assert.True(libroCreato.Id > 0);
        Assert.Equal(nuovoLibro.Titolo, libroCreato.Titolo);

        var getResponse = await _client.GetAsync($"/api/libri/{libroCreato.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var libroLetto = await getResponse.Content.ReadFromJsonAsync<Libro>();
        Assert.NotNull(libroLetto);
        Assert.Equal("Frank Herbert", libroLetto.Autore);

        var libroAggiornato = new Libro
        {
            Id = libroCreato.Id,
            Titolo = "Dune Messiah",
            Autore = "Frank Herbert",
            AnnoPubblicazione = 1969,
            Genere = "Fantascienza"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/libri/{libroCreato.Id}", libroAggiornato);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var libroDopoUpdate = await _client.GetFromJsonAsync<Libro>($"/api/libri/{libroCreato.Id}");
        Assert.NotNull(libroDopoUpdate);
        Assert.Equal("Dune Messiah", libroDopoUpdate.Titolo);
        Assert.Equal(1969, libroDopoUpdate.AnnoPubblicazione);

        var deleteResponse = await _client.DeleteAsync($"/api/libri/{libroCreato.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getDeletedResponse = await _client.GetAsync($"/api/libri/{libroCreato.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
}
