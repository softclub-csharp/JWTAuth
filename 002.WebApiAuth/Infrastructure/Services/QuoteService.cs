using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuoteService : IQuoteService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public QuoteService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PagedResponse<List<GetQuoteDto>>> GetQuotes(GetQuoteFilter filter)
    {
        
        var quotes =  _context.Quotes.AsQueryable();

        if (string.IsNullOrEmpty(filter.Author) == false)
            quotes = quotes.Where(q => q.Author.ToLower().Contains(filter.Author.ToLower()));
        
        if (string.IsNullOrEmpty(filter.QuoteText) == false)
            quotes = quotes.Where(q => q.Category.Name.ToLower().Contains(filter.QuoteText.ToLower()));

        var totalRecords = quotes.Count();
        var mapped = _mapper.Map<List<GetQuoteDto>>(quotes);
        return new PagedResponse<List<GetQuoteDto>>(filter.PageNumber,filter.PageSize,totalRecords,mapped);
        
    }

    public async Task<Response<GetQuoteDto>> GetQuoteById(int id)
    {
    
        var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == id);
        var mapped = _mapper.Map<GetQuoteDto>(quote);
        
        return new Response<GetQuoteDto>(mapped) ;
    }

    public async Task<Response<GetQuoteDto>> AddQuote(AddQuoteDto quoteDto)
    {
        var quote = _mapper.Map<Quote>(quoteDto);

        await _context.Quotes.AddAsync(quote);
       await _context.SaveChangesAsync();

       var mapped = _mapper.Map<GetQuoteDto>(quote);
       return new Response<GetQuoteDto>(mapped);
    }

    public async Task<Response<GetQuoteDto>> UpdateQuote(AddQuoteDto quoteDto)
    {
        var quote = _mapper.Map<Quote>(quoteDto);
        _context.Quotes.Update(quote);
        await _context.SaveChangesAsync();
        
         var mapped = _mapper.Map<GetQuoteDto>(quote);
         return new Response<GetQuoteDto>(mapped);

    }

    public async Task<Response<bool>> DeleteQuote(int id)
    {
        var existing = await _context.Quotes.FindAsync(id);
        if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest,"Quote not found");
        
        _context.Quotes.Remove(existing);
        await _context.SaveChangesAsync();
        return new Response<bool>(true);

    }
}