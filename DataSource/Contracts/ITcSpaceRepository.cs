using DataSource.Entities;

namespace DataSource.Contracts;

public interface ITcSpaceRepository
{
    Task<List<TCSPACES>> Get();
}
