var students = new List<Student>();
var nextId = 1;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/students", () => Results.Ok(students));

app.MapGet("/api/students/count", () => Results.Ok(students.Count));

app.MapGet("/api/students/search", (string? name) =>
{
    var results = string.IsNullOrWhiteSpace(name)
        ? students
        : students.Where(s =>
            s.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
            s.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

    return Results.Ok(results);
});

app.MapGet("/api/students/{id}", (int id) =>
{
    var student = students.FirstOrDefault(s => s.Id == id);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

app.MapPost("/api/students", (CreateStudentRequest request) =>
{
    var error = Validate(request.FirstName, request.LastName, request.Email, request.Age);
    if (error is not null)
        return Results.BadRequest(new { error });

    var student = new Student(nextId++, request.FirstName!, request.LastName!, request.Email!, request.Age);
    students.Add(student);
    return Results.Created($"/api/students/{student.Id}", student);
});

app.MapPut("/api/students/{id}", (int id, UpdateStudentRequest request) =>
{
    var index = students.FindIndex(s => s.Id == id);
    if (index == -1)
        return Results.NotFound();

    var error = Validate(request.FirstName, request.LastName, request.Email, request.Age);
    if (error is not null)
        return Results.BadRequest(new { error });

    students[index] = new Student(id, request.FirstName!, request.LastName!, request.Email!, request.Age);
    return Results.Ok(students[index]);
});

app.MapDelete("/api/students/{id}", (int id) =>
{
    var student = students.FirstOrDefault(s => s.Id == id);
    if (student is null)
        return Results.NotFound();

    students.Remove(student);
    return Results.NoContent();
});

app.Run();

static string? Validate(string? firstName, string? lastName, string? email, int age)
{
    if (string.IsNullOrWhiteSpace(firstName)) return "FirstName is required.";
    if (string.IsNullOrWhiteSpace(lastName)) return "LastName is required.";
    if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) return "Email is required and must be valid.";
    if (age <= 0) return "Age must be greater than 0.";
    return null;
}

record Student(int Id, string FirstName, string LastName, string Email, int Age);
record CreateStudentRequest(string? FirstName, string? LastName, string? Email, int Age);
record UpdateStudentRequest(string? FirstName, string? LastName, string? Email, int Age);
