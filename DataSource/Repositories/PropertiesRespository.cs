using Contracts.Request;
using DataSource.Entities;
using DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataSource.Repositories;

public class PropertiesRespository: IPropertiesRespository
{
    private readonly AppDbContext _context;
    public PropertiesRespository(AppDbContext context) { _context = context; }

    public async Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> Get(PropertiesFilterDto request)
    {
        try {

            int totalRecords = 0;

            List<SP_Get_Properties_Filter> listproperties = await _context.Set<SP_Get_Properties_Filter>()
                .FromSqlInterpolated($"SP_Get_Properties_Filter {request.propertyType}, {request.state},{request.municipality},{request.price},{request.rooms},{request.bathrooms},{request.proceduralStage},{request.pageSize},{request.pageNumber}").ToListAsync();

            totalRecords = listproperties.Select(x => x.TotalRecords).FirstOrDefault();

            return (listproperties, totalRecords);
        }
        catch (Exception) 
        {
            throw new Exception("Error al obtener las propiedades");
        }
    }
    public async Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> GetFirstOpportunities()
    {
        try
        {

            int totalRecords = 0;

            List<SP_Get_Properties_Filter> listproperties = await _context.Set<SP_Get_Properties_Filter>()
                .FromSqlInterpolated($"SP_Get_Properties_Oportunity").ToListAsync();
            totalRecords = listproperties.Select(x => x.TotalRecords).FirstOrDefault();
            return (listproperties, totalRecords);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener las propiedades");
        }
    }
    public async Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> Getpportunities(PropertiesFilterDto request)
    {
        try
        {

            int totalRecords = 0;

            List<SP_Get_Properties_Filter> listproperties = await _context.Set<SP_Get_Properties_Filter>()
                .FromSqlInterpolated($"SP_Get_Properties_Filter_Opportunities {request.propertyType}, {request.state},{request.municipality},{request.price},{request.rooms},{request.bathrooms},{request.proceduralStage},{request.pageSize},{request.pageNumber}").ToListAsync();
            totalRecords = listproperties.Select(x => x.TotalRecords).FirstOrDefault();
            return (listproperties, totalRecords);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener las propiedades");
        }
    }

}
