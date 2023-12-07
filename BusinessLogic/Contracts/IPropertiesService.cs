using BusinessLogic.Models;
using System.Collections;

namespace BusinessLogic.Contracts
{
    public interface IPropertiesService
    {
        Task<Response> PropertiesRange(int? index, int? items, int? order, int? propertytype, int? state, int? city, decimal? price, int? rooms, int? bathrooms, int? proceduralStage);

        Task<IEnumerable> Properties();

        Task<object> PropertyDetails(string id);

        Task<object> LegalDetails(Dictionary<string, object> prop);

        Task<Response> Favorites(Dictionary<string, object> user);

        Task<Response> AddFavorite(Dictionary<string, object> user);

        Task<Response> RemoveFavorite(Dictionary<string, object> user);

        Task<Response> RemoveAllFavorite(Dictionary<string, object> user);
    }
}
