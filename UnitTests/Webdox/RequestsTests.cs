using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using WebDox;

namespace UnitTests.Webdox
{
    [TestClass]
    public class RequestsTests
    {
        private IConfigurationRoot configuration;
        private NdaContract webdox;

        [TestInitialize]
        public void Setup()
        {
            var rootpath = Environment.CurrentDirectory;
            var root = Directory.GetParent(rootpath).Parent.Parent.Parent;
            this.configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(root.FullName, "backend_marketplace\\appsettings.QA.json")).Build();
        }


        [TestMethod]
        public async Task AuthenticateTest()
        {

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "QA");

            var credentials = this.configuration.GetSection("WebdoxServices:Credentials");

            //this.webdox = new NdaContract(credentials["UsernameService"], credentials["PasswordService"], credentials["GranttypeService"]);

            //var result = await this.webdox.Authentication();

            //var data = this.webdox.GetFormRequest("Juan perez");

            //var formid = this.configuration.GetValue<string>("WebdoxServices:FormIdNDA");
            //var description = this.configuration.GetValue<string>("WebdoxServices:DefaultWorkflowDescription");
            //var workflowname = this.configuration.GetValue<string>("WebdoxServices:DefaultWorkflowName");

            //var formattributes = this.configuration.GetSection("WebdoxServices:WebdoxFormNDAFields");

            //data.workflow_request = new WorkflowRequest();
            //data.workflow_request.description = description;
            //data.workflow_request.title = String.Format(workflowname, "JUAN PEREZ");
            //data.workflow_request.request_form_id = formid;

            var contract = this.webdox.GetContractDataIntance();

            contract.Fullname = "JUAN PEREZ";//fullname
            contract.EntityBirth = "MONTERREY, NUEVO LEON";//fullname
            contract.Birthday = "09 DE OCTUBRE DE 1989";//fullname
            contract.Occupation = "DESARROLLADOR";//fullname
            contract.ExpeditionPlace = "REYNOSA, TAMAULIPAS";//fullname
            contract.Rfc = "CCCM899910CL2";//fullname
            contract.Address1 = "CALLE ESTAMPIDA 555";//fullname
            contract.Address2 = "COL TAMPS, CP 88989";//fullname
            contract.Cellphone = "8129665544";//fullname
            contract.Email = "CARLOS@GMAIL.COM";//
            contract.CurrentDate = DateTime.Now.ToShortDateString();//fullname

            //var contractkeys = this.webdox.GetContractDataFormated(contract);
            //data.request_form_attribute = contractkeys;

            //var newrequest = await this.webdox.NewRequest(data);

            //var clientsigner = new Signer();
            //clientsigner.email = "ccicler@gmail.com";
            //clientsigner.name = "Mario Morales Martinez";
            //clientsigner.national_identification_number = "kj4l2kj34m2nkn3lk42l3k4j2l3kj4";

            //var signeradded = await this.webdox.AddSigner(clientsigner);



            //var comments = new InitiateSigners();

            //var start = await this.webdox.InitiateSignatures(comments);

            //Assert.IsTrue(result);

            //Assert.IsTrue(newrequest);

            //Assert.IsTrue(signeradded);

            //Assert.IsTrue(start);


        }

    }
}
