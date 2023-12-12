using BusinessLogic.Contracts;
using BusinessLogic.Models;
using Contracts.Request;
using DataSource;
using DataSource.Contracts;
using DataSource.Entities;
using DataSource.Expressions;
using DataSource.Interfaces;
using DataSource.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Drawing.Printing;

namespace BusinessLogic.Services
{
    internal class PropertiesService : BaseService, IPropertiesService
    {
        private readonly ILogger<PropertiesService> _logger;
        protected readonly IAzureBlobService _azureSasTokenService;
        private readonly IUserPropertiesRepository _userPropertiesRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IParametersRepository _parametersRepository;
        private readonly IIsiService _isiService;
        private readonly IConfiguration _configuration;
        private readonly IPropertiesRespository _propertiesRespository;
        private PropertiesExpression _whereExpression;
        public PropertiesService(AppDbContext context, 
            IUserPropertiesRepository userPropertiesRepository, 
            IPropertyRepository propertyRepository, 
            IUserRepository userRepository, IAzureBlobService azureSasTokenService, 
            IAzureBlobService azureBlobService, IParametersRepository parametersRepository,
            IIsiService isiService, IConfiguration configuration,
            IPropertiesRespository propertiesRespository,
            ILogger<PropertiesService> logger) : base(context)
        {
            this._azureSasTokenService = azureSasTokenService;
            this._userPropertiesRepository = userPropertiesRepository;
            this._propertyRepository = propertyRepository;
            this._userRepository = userRepository;
            this._azureBlobService = azureSasTokenService;
            this._parametersRepository = parametersRepository;
            this._isiService = isiService;
            this._configuration = configuration;
            _propertiesRespository = propertiesRespository;
            _logger = logger;
            _whereExpression = new PropertiesExpression();
        }

        /// <summary>
        /// Get all properties records 
        /// </summary>
        /// <returns>object list with propesties</returns>
        public async Task<IEnumerable> Properties()
        {
            var parameters = this._parametersRepository.GetParameters("BLOBSTORAGE");

            var CONTAINERIMAGES = parameters["CONTAINERNAMEIMAGES"].ToString();
            var HOUREXPIRED = Convert.ToInt32(parameters["SASTOKENEXPIRETIME"]);

            string sasToken = this._azureSasTokenService.GetToken(CONTAINERIMAGES, HOUREXPIRED);

            return _context.TPROPERTIES.Select(x => new
            {
                title = x.IDPROCEDURALSTAGENavigation.TCTITLES.DESCRIPTION,
                creditnumber = x.ID,
                rooms = x.IDBEDROOMNavigation.DESCRIPTION,
                bathrooms = x.IDBATHROOMNavigation.DESCRIPTION,
                constructionsize = x.CONSTRUCTIONSIZE,
                lotsize = x.LOTSIZE,
                price = x.SALEPRICE,
                parkingspaces = x.IDPARKINGSPACENavigation.DESCRIPTION,
                settlement = x.TADDRESSES.SETTLEMENT,
                city = x.TADDRESSES.IDCITYv2Navigation.DESCRIPTION,
                state = x.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation.DESCRIPTION,
                thumbnail = "data:image/jpg;base64," + this._azureBlobService.ImageByPathToBase64(x.TFILES.Where(x => x.PREVIEW == 1).FirstOrDefault().URI, CONTAINERIMAGES).Result,
            }).ToList();
        }
        public async Task<ResponseBase<PropertyPaged[]>> PropertiesRange(PropertiesFilterDto request)
        {
            ResponseBase<PropertyPaged[]> response = new ResponseBase<PropertyPaged[]>();

            try
            {
                if (request.pageNumber < 0)
                    request.pageNumber = 1;
                if (request.pageSize < 0)
                    request.pageNumber = 9;

                List<SP_Get_Properties_Filter> listProperties;
                int totalRecords = 0;

                var propertiesList = !request.oportunity
                    ? await _propertiesRespository.Get(request)
                    : request.isCarrusel
                    ? await _propertiesRespository.GetFirstOpportunities()
                    : await _propertiesRespository.Getpportunities(request);

                listProperties = propertiesList.properties;
                totalRecords = propertiesList.totalRecords;

                _logger.LogInformation($"Se obtiene Lista de propiedades");
                _logger.LogInformation($"Número de Propiedades obtenidas {totalRecords}");

                _logger.LogInformation($"Inicia mapeo de propiedades {DateTime.Now}");

                string containerImage = _parametersRepository.GetParameter<string>("BLOBSTORAGE", "CONTAINERNAMEIMAGES");

                var propertyTasks = listProperties.Select(async x => new PropertyPaged()
                {
                    Title = x.Title,
                    CreditNumber = x.CreditNumber,
                    Id = x.Id,
                    Rooms = x.Rooms,
                    Bathrooms = x.Bathrooms,
                    ConstructionSize = x.ConstructionSize,
                    LotSize = x.LotSize,
                    Price = x.Price,
                    ParkingSpaces = x.ParkingSpaces,
                    Settlement = x.Settlement,
                    City = x.City,
                    State = x.State,
                    Thumbnail = "data:image/jpg;base64," + await _azureBlobService.ImageByPathToBase64(x.pathFile, containerImage),
                    Favorite = false,
                }).ToArray();

                PropertyPaged[] properties = await Task.WhenAll(propertyTasks);

                _logger.LogInformation($"Finaliza mapeo de propiedades");

                _logger.LogInformation($"Se inicia obtencion de token servicio isi");

                string token = await _isiService.GenerateToken();

                _logger.LogInformation($"Se obtiene de token servicio isi");

                _logger.LogInformation($"Inicia obtención de status de credito");

                List<string> soldstatus = _parametersRepository.GetParameters<string>("ISISERVICESTATUSVENDIDO", "ESTATUSVENDIDO");

                var statusTasks = properties.Select(async item =>
                {
                    item.Sold = await _isiService.StatusSoldByCredit(item.Id, token, soldstatus);
                }).ToArray();

                await Task.WhenAll(statusTasks);

                _logger.LogInformation($"Finaliza obtención de status de credito {DateTime.Now}");

                _logger.LogInformation($"****OK {DateTime.Now}");

                response.data = properties;
                response.pageSize = request.pageSize;
                response.pageNumber = request.pageNumber;
                response.totalRecords = totalRecords;
                response.totalPage = (int)Math.Ceiling((double)totalRecords / request.pageSize);
                response.Succes = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _logger.LogError(ex.Message);

                response.Succes = false;
                response.Error = new ErrorBase()
                {
                    messageClient = "Hubo un error al obtner los datos trate de nuevo",
                    messageError = ex.Message,
                    messageInnerError = ex.InnerException != null ? ex.InnerException.ToString() : "Sin error a mostrar"
                };

            }
            return response;
        }

        private void CreatePredicate(PropertiesExpression predicate, int? propertytype, int? state, int? city, decimal? price, int? rooms, int? bathrooms, int? proceduralStage)
        {
            PropertiesExpression whereExpression = predicate;

            if (propertytype.HasValue)
            {
                whereExpression.AddPropertyType(propertytype);
            }

            if (state.HasValue)
            {
                whereExpression.AddState(state);
            }

            if (city.HasValue)
            {
                whereExpression.AddCity(city);
            }

            if (price.HasValue)
            {
                whereExpression.AddPrice(price);
            }

            if (rooms.HasValue)
            {
                whereExpression.AddRooms(rooms);
            }

            if (bathrooms.HasValue)
            {
                whereExpression.AddBathrooms(bathrooms);
            }

            if (proceduralStage.HasValue)
            {
                whereExpression.AddProceduralStage(proceduralStage);
            }
        }

        private static string AddSasToken(string urlImage, string sasToken)
        {
            if (!string.IsNullOrEmpty(urlImage))
            {
                string urlImageWithSasToken = string.Concat(urlImage, sasToken);

                return urlImageWithSasToken;
            }

            return null;
        }

        /// <summary>
        /// Get the all relevant info from property according  a credit number
        /// </summary>
        /// <param name="id">credit number of property</param>
        /// <returns>object with details info of property</returns>
        public async Task<object> PropertyDetails(string id)
        {
            var property = await _context.TPROPERTIES
                .Include(x => x.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation)
                .Include(x => x.IDPROCEDURALSTAGENavigation)
                .Include(x => x.TFILES)
                .Include(x => x.IDTYPENavigation).Where(x => x.ID.ToString() == id && x.IDSTATUS == 1).FirstOrDefaultAsync();

            if (property == null)
            {
                return null;
            }
            var parameters = this._parametersRepository.GetParameters("BLOBSTORAGE");

            var CONTAINERIMAGES = parameters["CONTAINERNAMEIMAGES"].ToString();
            var HOUREXPIRED = Convert.ToInt32(parameters["SASTOKENEXPIRETIME"]);

            string sasToken = this._azureSasTokenService.GetToken(CONTAINERIMAGES, HOUREXPIRED);

            var result = new
            {
                creditnumber = property.ID,
                title = property.IDPROCEDURALSTAGENavigation.TCTITLES.DESCRIPTION,
                rooms = property.IDBEDROOMNavigation.DESCRIPTION,
                halfbathrooms = property.IDHALFBATHROOMNavigation.DESCRIPTION,
                bathrooms = property.IDBATHROOMNavigation.DESCRIPTION,
                description = property.DESCRIPTION,
                lotsize = property.LOTSIZE,
                constructionsize = property.CONSTRUCTIONSIZE,
                parkingspaces = property.IDPARKINGSPACENavigation.DESCRIPTION,
                price = property.SALEPRICE,
                //proceduralstage = this.ToTitle(property.IDPROCEDURALSTAGENavigation.DESCRIPTION),
                type = this.ToTitle(property.IDTYPENavigation.DESCRIPTION),
                favorite = false,

                postcode = property.TADDRESSES.POSTCODE,
                street = this.ToTitle(property.TADDRESSES.STREETNAME),
                extnumber = property.TADDRESSES.EXTERIORNUMBER,
                intnumber = property.TADDRESSES.INTERIORNUMBER,
                settlement = this.ToTitle(property.TADDRESSES.SETTLEMENT),
                address2 = this.ToTitle($"{property.TADDRESSES.STREETNAME} {property.TADDRESSES.EXTERIORNUMBER} {property.TADDRESSES.INTERIORNUMBER}"),
                address = this.ToTitle($"{property.TADDRESSES.SETTLEMENT}, {property.TADDRESSES.POSTCODE} {property.TADDRESSES.IDCITYv2Navigation.DESCRIPTION}, {property.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation.DESCRIPTION}."),
                latitude = property.TADDRESSES.LATITUDE,
                longitude = property.TADDRESSES.LONGITUDE,

                state = property.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation.DESCRIPTION,
                city = property.TADDRESSES.IDCITYv2Navigation.DESCRIPTION,
                photos = property.TFILES.Where(x => x.PREVIEW == 0 && x.IDSTATUS == 1).Select(x => new { photo = "data:image/jpg;base64," + this._azureBlobService.ImageByPathToBase64(x.PATH, CONTAINERIMAGES).Result, title = x.TITLE, description = x.DESCRIPTION }).ToList(),
                sold = property.IDSTAGE == 1 ? false : true
            };

            return result;
        }

        public async Task<object> LegalDetails(Dictionary<string, object> data)
        {
            var property = await _context.TPROPERTIES
                .Include(x => x.IDPROCEDURALSTAGENavigation)
                .Where(x => x.ID.ToString() == (string)data["CreditNumber"] && x.IDSTATUS == 1).FirstOrDefaultAsync();

            if (property == null)
            {
                return new Response(false, "No se encontraron datos legales de la propeidad");
            }

            var result = new { totaldebt = property.TOTALDEBT, guaranteevalue = property.GUARANTEEVALUE, acquisition = property.ACQUISITIONDEADLINE, proceduralstage = property.IDPROCEDURALSTAGENavigation.DESCRIPTION, creditnumber = property.CREDITNUMBER };

            return new Response(true, result);
        }

        public async Task<Response> Favorites(Dictionary<string, object> user)
        {
            var records = await _context.TRUSERPROPERTIES
                .Include(x => x.IDPROPERTYNavigation)
                .Include(x => x.IDUSERNavigation)
                .Include(x => x.IDPROPERTYNavigation.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation)
                .Include(x => x.IDPROPERTYNavigation.TFILES)
                .Include(x => x.IDPROPERTYNavigation.IDTYPENavigation)
                .Where(x => x.IDSTATUS == 1 && x.IDUSERNavigation.CELLPHONE == (string)user["Cellphone"]).Select(x => x.IDPROPERTYNavigation).ToListAsync();//.Skip((page - 1) * rowsTotal).Take(items ?? rowsTotal);

            var parameters = this._parametersRepository.GetParameters("BLOBSTORAGE");
            var CONTAINERIMAGES = parameters["CONTAINERNAMEIMAGES"].ToString();
            var HOUREXPIRED = Convert.ToInt32(parameters["SASTOKENEXPIRETIME"]);

            //string sasToken = this._azureSasTokenService.GetToken(CONTAINERIMAGES, HOUREXPIRED);

            var result = records.Select(x => new
            {
                title = x.IDPROCEDURALSTAGENavigation.TCTITLES.DESCRIPTION,
                creditnumber = x.ID,
                rooms = x.IDBEDROOMNavigation.DESCRIPTION,
                bathrooms = x.IDBATHROOMNavigation.DESCRIPTION,
                constructionsize = x.CONSTRUCTIONSIZE,
                lotsize = x.LOTSIZE,
                price = x.SALEPRICE,
                parkingspaces = x.IDPARKINGSPACENavigation.DESCRIPTION,
                settlement = this.ToTitle(x.TADDRESSES.SETTLEMENT),
                city = x.TADDRESSES.IDCITYv2Navigation.DESCRIPTION,
                state = x.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation.DESCRIPTION,
                thumbnail = "data:image/jpg;base64," + this._azureBlobService.ImageByPathToBase64(x.TFILES.Where(x => x.PREVIEW == 1).FirstOrDefault().PATH, CONTAINERIMAGES).Result,
                favorite = true,
            });

            return new Response(true, result);
        }

        public async Task<Response> AddFavorite(Dictionary<string, object> prop)
        {
            string email = (string)prop["Cellphone"];
            int id = Convert.ToInt32(prop["Id"]);

            try
            {
                var instanceUser = this._userRepository.InstanceObject();
                instanceUser.CELLPHONE = email;

                var instanceProp = this._propertyRepository.InstanceObject();
                instanceProp.ID = id;

                var user = this._userRepository.FindByCellphone(instanceUser);
                var property = this._propertyRepository.FindById(instanceProp);

                if (user == null || property == null)
                {
                    return new Response(false, "No se encontraron los valores recibidos.");
                }

                var newFavorite = this._userPropertiesRepository.InstanceObject();
                newFavorite.IDUSER = user.ID;
                newFavorite.IDPROPERTY = property.ID;
                newFavorite.IDSTATUS = 1;

                this._userPropertiesRepository.Insert(newFavorite);
                this._userPropertiesRepository.Save();

                return new Response(true, "Se agrego exitosamente a la lista.");
            }
            catch (Exception ex)
            {
                return new Response(false, "No se pudo guardar la información.");
            }
        }

        public async Task<Response> RemoveFavorite(Dictionary<string, object> prop)
        {
            string email = (string)prop["Cellphone"];
            int id = Convert.ToInt32(prop["Id"]);

            try
            {
                var instanceUser = this._userRepository.InstanceObject();
                instanceUser.CELLPHONE = email;

                var instanceProp = this._propertyRepository.InstanceObject();
                instanceProp.ID = id;

                var user = this._userRepository.FindByCellphone(instanceUser);
                var property = this._propertyRepository.FindById(instanceProp);

                if (user == null || property == null)
                {
                    return new Response(false, "No se encontraron los valores recibidos.");
                }

                var item = this._userPropertiesRepository.FindByUserAndProperty(user.ID, property.ID);

                this._userPropertiesRepository.Delete(item);

                this._userPropertiesRepository.Save();

                return new Response(true, "Se elimino exitosamente de la lista.");
            }
            catch (Exception ex)
            {
                return new Response(false, "No se pudo eliminar la información.");
            }
        }

        public async Task<Response> RemoveAllFavorite(Dictionary<string, object> prop)
        {
            string email = (string)prop["Cellphone"];

            var instanceUser = this._userRepository.InstanceObject();
            instanceUser.CELLPHONE = email;

            var user = this._userRepository.FindByCellphone(instanceUser);

            if (user == null)
            {
                return new Response(false, "No se encontraron los valores recibidos.");
            }

            var favorites = this._userPropertiesRepository.GetAllByUser(user.ID);
            this._userPropertiesRepository.DeleteRange(favorites);
            var result = this._userPropertiesRepository.Save();

            if (result.Success)
            {
                return new Response(true, "se han eliminado todos los favoritos.");
            }

            return new Response(false, "No se logro eliminar los favoritos."); ;
        }
    }
}
