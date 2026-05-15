---
name: doc-extractor
description: >
  Estrae automaticamente la documentazione tecnica in formato Markdown dal pattern
  architetturale Model/Service/Repository di un progetto ASP.NET Core (o similare) e
  la scrive nella cartella `docs/`. Usa questa skill ogni volta che l'utente chiede di
  "documentare il codice", "generare la documentazione", "estrarre la doc", "creare i
  file .md per il progetto", oppure vuole un riferimento scritto delle classi, interfacce,
  servizi o repository del progetto. Attivala anche se l'utente menziona docs/, SKILL,
  architettura, Swagger alternativo, o vuole capire cosa fanno i layer dell'applicazione.
---

# doc-extractor

Genera documentazione `.md` leggibile e strutturata analizzando il codice sorgente
del progetto seguendo il pattern **Model → Repository → Service → Endpoints**.

---

## Obiettivo

Per ogni entità del dominio (es. `Book`, `Movie`) produrre un file Markdown in `docs/`
che descriva:

- Il **Model** (proprietà, tipi)
- L'**interfaccia Repository** e la sua implementazione (metodi, logica in-memory)
- L'**interfaccia Service** e la sua implementazione (orchestrazione, chiamate al repo)
- Gli **Endpoint REST** collegati (route, verbi HTTP, payload, risposta, status codes)
- Le **registrazioni DI** in `Program.cs`

Produrre inoltre un file `docs/index.md` come indice generale del progetto.

---

## Workflow passo per passo

### 1. Esplora la struttura del progetto

```bash
find . -name "*.cs" | sort
```

Identifica:
- Tutti i file in `Models/`, `Repositories/`, `Services/`
- `Program.cs` per DI e mapping degli endpoint

### 2. Leggi i file rilevanti

Per ogni entità individua e leggi (con `Read` / `cat`):

| Layer       | File da leggere                             |
|-------------|---------------------------------------------|
| Model       | `Models/<Entity>.cs`                        |
| Repository  | `Repositories/I<Entity>Repository.cs` + `Repositories/<Entity>Repository.cs` |
| Service     | `Services/I<Entity>Service.cs` + `Services/<Entity>Service.cs`               |
| Endpoints   | `Program.cs` (sezione relativa all'entità)  |

### 3. Crea la cartella `docs/` se non esiste

```bash
mkdir -p docs
```

### 4. Genera un file `.md` per ogni entità

Salva in `docs/<entity>.md` (tutto in minuscolo, es. `docs/book.md`).

Usa il template qui sotto — vedi sezione **Template**.

### 5. Genera `docs/index.md`

File di indice con:
- Titolo e descrizione sintetica del progetto
- Tabella delle entità con link ai rispettivi file
- Schema architetturale testuale (ASCII o lista)
- Riepilogo registrazioni DI

### 6. Conferma all'utente

Elenca i file creati, es.:
```
✅ docs/book.md
✅ docs/movie.md
✅ docs/index.md
```

---

## Template: `docs/<entity>.md`

```markdown
# <Entity>

Breve descrizione dell'entità e del suo ruolo nel dominio.

---

## Model

**File**: `Models/<Entity>.cs`

| Proprietà | Tipo     | Note              |
|-----------|----------|-------------------|
| Id        | int      | Identificatore    |
| ...       | ...      | ...               |

---

## Repository

### Interfaccia — `I<Entity>Repository`

**File**: `Repositories/I<Entity>Repository.cs`

| Metodo            | Ritorna        | Descrizione                        |
|-------------------|----------------|------------------------------------|
| `GetAll()`        | `List<Entity>` | Restituisce tutte le entità        |
| `GetById(int id)` | `Entity?`      | Restituisce l'entità o null        |
| `Add(Entity e)`   | `void`         | Aggiunge una nuova entità          |
| `Update(Entity e)`| `bool`         | Aggiorna; ritorna false se assente |
| `Delete(int id)`  | `bool`         | Elimina; ritorna false se assente  |

### Implementazione — `<Entity>Repository`

**File**: `Repositories/<Entity>Repository.cs`  
**DI**: `Singleton`

Storage: lista in-memory (`List<<Entity>>`).  
Strategia `Id`: `max(id esistenti) + 1`.

---

## Service

### Interfaccia — `I<Entity>Service`

**File**: `Services/I<Entity>Service.cs`

Espone gli stessi metodi del repository (o un sottoinsieme), fungendo da layer
di business logic tra endpoint e repository.

### Implementazione — `<Entity>Service`

**File**: `Services/<Entity>Service.cs`  
**DI**: `Scoped`

Delega le operazioni a `I<Entity>Repository` ricevuto via constructor injection.

---

## Endpoints REST

Base path: `/<entities>` (plurale, minuscolo)

| Metodo | Route              | Body richiesta      | Risposta          | Status codes  |
|--------|--------------------|---------------------|-------------------|---------------|
| GET    | `/<entities>`      | —                   | `List<<Entity>>`  | 200           |
| GET    | `/<entities>/{id}` | —                   | `<Entity>`        | 200 / 404     |
| POST   | `/<entities>`      | `<Entity>` (senza Id)| `<Entity>`       | 201           |
| PUT    | `/<entities>/{id}` | Campi aggiornabili  | `<Entity>`        | 200 / 404     |
| DELETE | `/<entities>/{id}` | —                   | —                 | 204 / 404     |

### Dettaglio endpoint

#### `GET /<entities>`
Restituisce la lista completa. Nessun parametro.

#### `GET /<entities>/{id}`
...

#### `POST /<entities>`
Body di esempio:
```json
{
  "title": "...",
  ...
}
```

#### `PUT /<entities>/{id}`
...

#### `DELETE /<entities>/{id}`
...

---

## Dependency Injection

Registrazioni in `Program.cs`:

```csharp
builder.Services.AddSingleton<I<Entity>Repository, <Entity>Repository>();
builder.Services.AddScoped<I<Entity>Service, <Entity>Service>();
```
```

---

## Regole qualitative

- **Sii preciso**: usa i nomi esatti di classi, metodi e proprietà trovati nel codice.
- **Non inventare**: se un metodo non esiste nel sorgente, non documentarlo.
- **Adatta il template**: se il progetto ha variazioni (es. metodi extra, tipi diversi), aggiornali.
- **Linguaggio**: usa la lingua del progetto o dell'utente (default: italiano se il CLAUDE.md è in italiano, inglese altrimenti).
- **Un file per entità**: non accorpare più entità nello stesso `.md`.

---

## Riferimenti

- Vedi `references/aspnet-patterns.md` per convenzioni ASP.NET Core Minimal API.

---

## Note per progetti diversi da BookStoreApi

Questa skill funziona su qualsiasi progetto con struttura layered simile.  
Adatta i percorsi delle cartelle (`Models/`, `Services/`, `Repositories/`) in base a quanto
trovato esplorando il filesystem con `find`.  
Se il progetto usa controller invece di Minimal API, cerca i file in `Controllers/`.
