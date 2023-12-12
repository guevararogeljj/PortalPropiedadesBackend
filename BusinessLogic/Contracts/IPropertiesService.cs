using BusinessLogic.Models;
using Contracts.Request;
using System.Collections;

namespace BusinessLogic.Contracts
{
    public interface IPropertiesService
    {
        Task<ResponseBase<PropertyPaged[]>> PropertiesRange(PropertiesFilterDto request);

        Task<IEnumerable> Properties();

        Task<object> PropertyDetails(string id);

        Task<object> LegalDetails(Dictionary<string, object> prop);

        Task<Response> Favorites(Dictionary<string, object> user);

        Task<Response> AddFavorite(Dictionary<string, object> user);

        Task<Response> RemoveFavorite(Dictionary<string, object> user);

        Task<Response> RemoveAllFavorite(Dictionary<string, object> user);
    }
}
