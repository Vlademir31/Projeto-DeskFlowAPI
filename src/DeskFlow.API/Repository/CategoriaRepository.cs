using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repository;
public class CategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository (AppDbContext context)
    {
        _context = context;
    }
}