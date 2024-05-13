using ebooks_dotnet7_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ebooks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var ebooks = app.MapGroup("api/ebook");

// TODO: Add more routes
ebooks.MapPost("/", async (CreateEBookDTO dto, DataContext context) => {
    var ExistingEbook = await context.EBooks.FirstOrDefaultAsync(book => book.Title == dto.Title && book.Author == dto.Author);
    if(ExistingEbook != null){
        Results.BadRequest("el libro ya existe");
    }

    var newBook = new EBook
    {
        Title = dto.Title,
        Author = dto.Author,
        Genre = dto.Genre,
        Price = dto.Price,
        Format = dto.Format,
        IsAvailable = true,
        Stock = 0
    };
    context.EBooks.Add(newBook);
    await context.SaveChangesAsync();

    return Results.Created($"/api/ebook{newBook.Id}", newBook);
});

ebooks.MapGet("/", async (DataContext context) =>{
    var ebooks = await context.EBooks.ToListAsync();
    return Results.Ok(ebooks);
});

ebooks.MapPut("/{id}", async (int id, UpdateEbookDTO dto, DataContext context) => {
    var Existingbook = await context.EBooks.FindAsync(id);
    if (Existingbook == null){
        return Results.NotFound("libro no econtrado");
    }
    Existingbook.Title = dto.Title;
    Existingbook.Author = dto.Author;
    Existingbook.Format = dto.Format;
    Existingbook.Genre = dto.Genre;
    Existingbook.Price = dto.Price;      

    await context.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

// TODO: Add more methods
async Task<IResult> CreateEBookAsync(DataContext context)
{
    return Results.Ok();
}
