using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Net;
using Microsoft.Xrm.Tooling.Connector;
//using ConexaoAlternativaExtending;

namespace TreinamentoExtending 
{
    class Program 
    {
        static void Main(string[] args)
        {
            
             Conexao conexao = new Conexao();
             var serviceproxy = conexao.Obter();

            //Descoberta();
            //MeuCreate(serviceproxy);
            //MeuUpdate(serviceproxy);
            //MeuDelete(serviceproxy);

            /*----------------- RETORNAR MULTIPLO ------------------------------*/

            /* var retornoLista = RetornarMultiplo(serviceproxy);
             foreach (var item in retornoLista.Entities)
             {
                 if (item.Attributes.Contains("websiteurl"))
                     Console.WriteLine(item["websiteurl"]);
                 else
                     Console.WriteLine("Campo não Encontrado!");
             }*/
            //RetornaMultiploComLink(serviceproxy);
            /*----------------- RETORNAR MULTIPLO ------------------------------*/

            /*----------------- CRUD LINQ ------------------------------*/
            //ConsultasLinq(serviceproxy);
            //CriacaoLinq(serviceproxy);
            //UpdateLinq(serviceproxy);
            //DeleteLinq(serviceproxy);
            /*----------------- CRUD LINQ ------------------------------*/

            /*----------------- FETCH XML ------------------------------*/
            //FetchXML(serviceproxy);
            //FetchXMLAggregade(serviceproxy);
            /*----------------- FETCH XML ------------------------------*/

            /*----------------- EXERCÍCIOS 1 ------------------------------*/
            //MeuCreateExercicio1(serviceproxy);
            //exercicioBuscar1(serviceproxy);
            //FetchXMLAggregadeExercicio1Buscar(serviceproxy);
            /*----------------- EXERCÍCIOS 1 ------------------------------*/


            Console.WriteLine("FIM!");
            Console.ReadKey();

        }
        #region Descoberta
        static void Descoberta()
        {
            Uri local = new Uri("https://disco.crm2.dynamics.com/XRMServices/2011/Discovery.svc");
                
            ClientCredentials clientcred = new ClientCredentials();
            clientcred.UserName.UserName = "flavio@academia2021crm.onmicrosoft.com";
            clientcred.UserName.Password = "R3s1d3nt2";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            DiscoveryServiceProxy dsp = new DiscoveryServiceProxy(local, null, clientcred, null);
            dsp.Authenticate();

            RetrieveOrganizationsRequest rosreq = new RetrieveOrganizationsRequest();
            rosreq.AccessType = EndpointAccessType.Default;
            rosreq.Release = OrganizationRelease.Current;

            RetrieveOrganizationsResponse response = (RetrieveOrganizationsResponse)dsp.Execute(rosreq);

            foreach (var item in response.Details)
            {
                Console.WriteLine("Unique " + item.UniqueName);
                Console.WriteLine("Friendly " + item.FriendlyName);

                foreach (var endpoint in item.Endpoints)
                {
                    Console.WriteLine(endpoint.Key);
                    Console.WriteLine(endpoint.Value);
                }
            }

            Console.ReadKey();
        }
        #endregion

        #region create
        static void MeuCreate(CrmServiceClient serviceproxy)
        {
            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Guid registro = new Guid();

                entidade.Attributes.Add("name", "Treinamento " + i.ToString());
                registro = serviceproxy.Create(entidade);
            }
        }
        #endregion

        #region Update
        static void MeuUpdate(CrmServiceClient serviceproxy)
        {
            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Guid idRegistro = new Guid();
                entidade.Attributes.Add("name", "Treinamento " + i.ToString());
                idRegistro = serviceproxy.Create(entidade);

                var registroDynamics = serviceproxy.Retrieve("account", idRegistro, new ColumnSet("name"));
                if (registroDynamics.Attributes.Contains("name"))
                {
                    registroDynamics.Attributes["name"] = "Novo Valor " + i.ToString();
                }
                else
                {
                    registroDynamics.Attributes.Add("name", "Novo Valor " + i.ToString());
                }

                serviceproxy.Update(registroDynamics);

            }
        }
        #endregion

        #region Delete
        static void MeuDelete(CrmServiceClient serviceproxy)
        {
            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Guid idRegistro = new Guid();

                entidade.Attributes.Add("name", "Treinamento " + i.ToString());
                idRegistro = serviceproxy.Create(entidade);
                serviceproxy.Delete("account", idRegistro);
            }
        }
        #endregion

        #region QueryExpression
        static EntityCollection RetornarMultiplo(CrmServiceClient serviceproxy)
        {
            QueryExpression queryExpression = new QueryExpression("account");

            queryExpression.Criteria.AddCondition("websiteurl", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("websiteurl");

            EntityCollection colecaoEntidades = serviceproxy.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidades.Entities)
            {
                if (item.Attributes.Contains("websiteburl"))
                    Console.Write(item["websiteburl"]);
                else
                    Console.WriteLine("Campo não Encontrado!");
            }

            return colecaoEntidades;
        }
        #endregion

        #region QueryExpression2
        static void RetornaMultiploComLink(CrmServiceClient serviceproxy)
        {
            QueryExpression queryExpression = new QueryExpression("account");
            queryExpression.ColumnSet = new ColumnSet(true);

            ConditionExpression condicao = new ConditionExpression("address1_city", ConditionOperator.Equal, "Natal");
            queryExpression.Criteria.AddCondition(condicao);

            LinkEntity link = new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner);
            link.Columns = new ColumnSet("firstname", "lastname");
            link.EntityAlias = "Contato";
            queryExpression.LinkEntities.Add(link);

            EntityCollection colecaoEntidades = serviceproxy.RetrieveMultiple(queryExpression);
            foreach (var entidade in colecaoEntidades.Entities)
            {
                Console.WriteLine("Id " + entidade.Id);
                Console.WriteLine("Nome Conta " + entidade["name"]);
                Console.WriteLine("Nome Contato " + ((AliasedValue)entidade["Contato.firstname"]).Value);
                Console.WriteLine("Sobrenome Contato " + ((AliasedValue)entidade["Contato.lastname"]).Value);
            }
        }
        #endregion

        #region Linq
        static void ConsultasLinq(CrmServiceClient serviceproxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceproxy);
            var resultados = from a in context.CreateQuery("contact")
                             join b in context.CreateQuery("account")
                                    on a["contactid"] equals b["primarycontactid"]

                             select new
                             {
                                 retorno = new
                                 {
                                     FirstName = a["firstname"],
                                     LastName = a["lastname"],
                                     NomeConta = b["name"]
                                 }
                             };
            foreach (var entidade in resultados)
            {
                Console.WriteLine("Nome " + entidade.retorno.FirstName);
                Console.WriteLine("Sobrenom " + entidade.retorno.LastName);
                Console.WriteLine("NomeConta " + entidade.retorno.NomeConta);
            }
        }
        #endregion

        #region CRUD Linq
        static void CriacaoLinq(CrmServiceClient serviceproxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceproxy);
            for (int i = 0; i < 10; i++)
            {
                Entity account = new Entity("account");
                account["name"] = "Conta Linq" + i;
                context.AddObject(account);
            }
            context.SaveChanges();
        }

        static void UpdateLinq(CrmServiceClient serviceproxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceproxy);
            var resultados = from a in context.CreateQuery("contact")
                             where ((string)a["firstname"]) == "Daniel"
                             select a;
            foreach (var item in resultados)
            {
                item.Attributes["firstname"] = "Daniel";
                item.Attributes["lastname"] = "Geraldeli";
                context.UpdateObject(item);
            }
            context.SaveChanges();
        }
       
        static void DeleteLinq(CrmServiceClient serviceproxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceproxy);
            var resultados = from a in context.CreateQuery("account")
                             where (string)a["name"] == "Treinamento Extending 2"
                             select a;
            foreach (var item in resultados)
            {
                item.Attributes["firstname"] = "Daniel";
                item.Attributes["lastname"] = " Geraldeli";
                context.DeleteObject(item);
            }
            context.SaveChanges();
        }
        #endregion

        #region FethXML1
        static void FetchXML(CrmServiceClient serviceproxy)
        {
            string query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='account'>
                                    <attribute name='name'/>
                                    <attribute name='primarycontactid'/>
                                    <attribute name='telephone1'/>
                                    <attribute name='accountid'/>
                                    <attribute name='createdon'/>
                                    <order attribute='name' descending='false'/>
                                        <filter type='and'>
                                             <condition attribute='name' operator='eq' value='flavio'/>
                                             <condition attribute='accountnumber' operator='not-null'/>
                                        </filter>
                                </entity>
                            </fetch>";
            EntityCollection colecao = serviceproxy.RetrieveMultiple(new FetchExpression(query));
            foreach (var item in colecao.Entities)
            {
                Console.WriteLine(item["name"]);
                Console.WriteLine(item["telephone1"]);
            }
        }
        #endregion

        #region FetchXML2
        static void FetchXMLAggregade(CrmServiceClient serviceproxy)
        {
            string query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                <entity name='account'>
                                      <attribute name='creditlimit' alias='creditlimit_soma' aggregate='avg'/>
                                </entity>
                           </fetch>";

            EntityCollection colecao = serviceproxy.RetrieveMultiple(new FetchExpression(query));
            foreach (var item in colecao.Entities)
            {
                Console.WriteLine($"Média: " + ((Money)((AliasedValue)item["creditlimit_soma"]).Value).Value.ToString());
            }
        }
        #endregion

        #region exercicios
        static void MeuCreateExercicio1(CrmServiceClient serviceproxy)
        {
            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("contact");

                entidade.Attributes.Add("firstname", "Nome " + i.ToString());
                entidade.Attributes.Add("lastname", "Sobrenome" + i.ToString());
                entidade.Attributes.Add("fsr_anonascimento", 1980+ i);

                Console.WriteLine(serviceproxy.Create(entidade));
            }
        }

        static void exercicioBuscar1(CrmServiceClient serviceproxy)
        {
            var query = new QueryExpression("contact");
            query.Criteria.AddCondition("fsr_anonascimento", ConditionOperator.NotNull);
            query.ColumnSet = new ColumnSet("fsr_anonascimento");

            var retorno = serviceproxy.RetrieveMultiple(query);
            foreach (var item in retorno.Entities)
            {
                item.Attributes.Add("fsr_idade", (DateTime.Now.Year - Convert.ToInt32(item["fsr_anonascimento"])));
                serviceproxy.Update(item);
            }
        }

        static void FetchXMLAggregadeExercicio1Buscar(CrmServiceClient serviceproxy)
        {
            string query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                <entity name='contact'>
                                      <attribute name='fsr_idade' alias='idade_soma' aggregate='avg'/>
                                </entity>
                           </fetch>";

            EntityCollection colecao = serviceproxy.RetrieveMultiple(new FetchExpression(query));
            foreach (var item in colecao.Entities)
            {
                Console.WriteLine($"Média Idade: " + ((AliasedValue)item["idade_soma"]).Value);
            }
        }
        #endregion

    }
}