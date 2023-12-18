using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories;
public class TcSpaceRepository: ITcSpaceRepository
{
    private readonly AppDbContext _context;
    public TcSpaceRepository(AppDbContext context) { _context = context; }

    public async Task<List<TCSPACES>> Get()
    {
        try
        {
            return await _context.TCSPACES.ToListAsync();  
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener las propiedades");
        }
    }
}
