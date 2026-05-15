# Book

Rappresenta un libro nel catalogo della libreria. È l'entità principale dell'API,
con operazioni CRUD complete esposte tramite endpoint REST.

---

## Model

**File**: `Models/Book.cs`

| Proprietà | Tipo     | Note                          |
|-----------|----------|-------------------------------|
| `Id`      | `int`    | Identificatore univoco        |
| `Title`   | `string` | Titolo del libro              |
| `Author`  | `string` | Autore del libro              |
| `Year`    | `int`    | Anno di pubblicazione         |

---

## Repository

### Interfaccia — `IBookRepository`

**File**: `Repositories/IBookRepository.cs`

| Metodo                          | Ritorna      | Descrizione                                      |
|---------------------------------|--------------|--------------------------------------------------|
| `GetAll()`                      | `List<Book>` | Restituisce tutti i libri                        |
| `GetById(int id)`               | `Book?`      | Restituisce il libro con quell'id, o `null`      |
| `Add(Book book)`                | `Book`       | Aggiunge il libro e restituisce l'istanza creata |
| `Update(int id, Book updated)`  | `bool`       | Aggiorna il libro; ritorna `false` se non esiste |
| `Delete(int id)`                | `bool`       | Elimina il libro; ritorna `false` se non esiste  |

### Implementazione — `BookRepository`

**File**: `Repositories/BookRepository.cs`  
**DI**: `Singleton`

Storage: lista in-memory (`List<Book>`) inizializzata con 3 libri di esempio:

| Id | Title                     | Author            | Year |
|----|---------------------------|-------------------|------|
| 1  | Il nome della rosa        | Umberto Eco       | 1980 |
| 2  | 1984                      | George Orwell     | 1949 |
| 3  | Il Signore degli Anelli   | J.R.R. Tolkien    | 1954 |

Strategia `Id`: `max(id esistenti) + 1`; se la lista è vuota, parte da `1`.

---

## Service

### Interfaccia — `IBookService`

**File**: `Services/IBookService.cs`

Espone gli stessi metodi di `IBookRepository`, fungendo da layer di business logic
tra gli endpoint e il repository.

| Metodo                        | Ritorna      |
|-------------------------------|--------------|
| `GetAll()`                    | `List<Book>` |
| `GetById(int id)`             | `Book?`      |
| `Add(Book book)`              | `Book`       |
| `Update(int id, Book book)`   | `bool`       |
| `Delete(int id)`              | `bool`       |

### Implementazione — `BookService`

**File**: `Services/BookService.cs`  
**DI**: `Scoped`

Riceve `IBookRepository` via constructor injection e delega tutte le operazioni
direttamente al repository, senza logica aggiuntiva.

---

## Endpoint REST

Base path: `/books`

| Metodo   | Route          | Body richiesta           | Risposta       | Status codes |
|----------|----------------|--------------------------|----------------|--------------|
| `GET`    | `/books`       | —                        | `List<Book>`   | 200          |
| `GET`    | `/books/{id}`  | —                        | `Book`         | 200 / 404    |
| `POST`   | `/books`       | `Book` (senza `Id`)      | `Book`         | 201          |
| `PUT`    | `/books/{id}`  | `Book` (campi da aggiornare) | `Book`     | 200 / 404    |
| `DELETE` | `/books/{id}`  | —                        | —              | 204 / 404    |

### Dettaglio endpoint

#### `GET /books`
Restituisce la lista completa di tutti i libri. Nessun parametro.

#### `GET /books/{id}`
Restituisce il libro con l'id specificato. Se non esiste risponde `404` con messaggio
`"Libro con ID {id} non trovato."`.

#### `POST /books`
Inserisce un nuovo libro. L'`Id` viene assegnato automaticamente.

Body di esempio:
```json
{
  "title": "Il Gattopardo",
  "author": "Giuseppe Tomasi di Lampedusa",
  "year": 1958
}
```

Risponde `201 Created` con la risorsa creata e l'header `Location: /books/{id}`.

#### `PUT /books/{id}`
Aggiorna `Title`, `Author` e `Year` del libro esistente. Risponde `200` con il libro
aggiornato oppure `404` se non esiste.

Body di esempio:
```json
{
  "title": "Il Gattopardo (edizione rivista)",
  "author": "Giuseppe Tomasi di Lampedusa",
  "year": 1960
}
```

#### `DELETE /books/{id}`
Elimina il libro con l'id specificato. Risponde `204 No Content` oppure `404` se
non esiste.

---

## Dependency Injection

Registrazioni in `Program.cs`:

```csharp
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
```
