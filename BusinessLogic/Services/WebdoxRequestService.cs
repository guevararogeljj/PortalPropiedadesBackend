using BusinessLogic.Contracts;
using BusinessLogic.Models;
using DataSource.Contracts;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    internal class WebdoxRequestService : IWebdoxRequestService
    {
        private readonly IWebdoxRequestRepository _webdoxRequestRepository;
        private readonly ILogger<WebdoxRequestService> _logger;

        public WebdoxRequestService(IWebdoxRequestRepository webdoxRequestRepository, ILogger<WebdoxRequestService> logger)
        {
            this._webdoxRequestRepository = webdoxRequestRepository;
            this._logger = logger;
        }
        public async Task<Response> AddNewRequestNDA(int order, int user, string formwebdoxid, string jsonresponse, string description, bool status = false)
        {
            var newrequest = this._webdoxRequestRepository.InstanceObject();

            newrequest.ORDER = order;
            newrequest.IDUSER = user;
            newrequest.FORMWEBDOXID = formwebdoxid;
            newrequest.DESCRIPTION = description;
            newrequest.JSON = jsonresponse;
            newrequest.STATUS = status;

            this._webdoxRequestRepository.Insert(newrequest);
            var result = this._webdoxRequestRepository.Save();

            if (result.Success)
            {
                return new Response(true, result.Message);
            }

            return new Response(false, result.Message);
        }

        public async Task<Response> WebdoxReponse(string idwebodx, string jsonresponse)
        {
            try
            {
                this._logger.LogInformation($"from service webodx: {idwebodx}");
                var instancewebdox = this._webdoxRequestRepository.InstanceObject();
                instancewebdox.FORMWEBDOXID = idwebodx;

                var entities = this._webdoxRequestRepository.FindAllRequestByIdWebdox(idwebodx);
                this._logger.LogInformation($"instancewebdox: {instancewebdox.FORMWEBDOXID}");
                this._logger.LogInformation($"count: {entities.Count}");

                if (entities.Count == 0)
                {
                    return new Response(false, "webdox request no encontrado");
                }

                var first = entities.Where(x => x.ORDER == 1).FirstOrDefault();

                if (first == null)
                {
                    return new Response(false, "registro webdox no encontrado");
                }

                var entity = this._webdoxRequestRepository.FindById(first);

                entity.STATUS = true;
                entity.IDUSERNavigation.CONTRACT_SIGN_AT = DateTime.Now;

                _webdoxRequestRepository.Update(entity);
                _webdoxRequestRepository.Save();

                return new Response(true, entity.IDUSER);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
    }
}
