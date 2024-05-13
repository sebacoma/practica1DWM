public class CreateEBookDTO
    {
    public int Id { get; set; }

    public required string Title { get; set; }


    public required string Author { get; set; }

    public required string Genre { get; set; }

    public required string Format { get; set; }

    public required int Price { get; set; }

    public required int Stock { get; set; }
}

