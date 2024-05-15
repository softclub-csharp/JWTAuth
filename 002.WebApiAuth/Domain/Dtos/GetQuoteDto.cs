namespace Domain.Dtos;

public class GetQuoteDto : QuoteDto
{

    public string CategoryName { get; set; }
    
    public GetQuoteDto(int id, string? author, string text, string? imageName)
    {
        Id = id;
        AuthorName = author;
        Title = text;
        ImageName = imageName;
    }

    public GetQuoteDto()
    {
        
    }

    public DateTime CreatedAt { get; set; }
}