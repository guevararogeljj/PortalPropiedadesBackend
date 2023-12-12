using Contracts.Request;
using DataSource.Entities;

namespace DataSource.Interfaces;

public interface IPropertiesRespository
{
    Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> Get(PropertiesFilterDto request);
    Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> GetFirstOpportunities();
    Task<(List<SP_Get_Properties_Filter> properties, int totalRecords)> Getpportunities(PropertiesFilterDto request);
}
