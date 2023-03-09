using SApInterface.API.Model.Domain;
using Sofcom.CoreServices;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Principal;
using Microsoft.Azure.Cosmos.Linq;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Azure;

namespace SApInterface.API.Repositry
{


    public class SectionRepositry : ISectionRepositry
    {
        #region Cosmos Db
        private readonly string CosmosDBAccountUri = "https://spectrumsqlaccount.documents.azure.com:443/";
        private readonly string CosmosDBAccountPrimaryKey = "tlE6oXWGvoPLwG7zoFVjrbZLPXCq4GTwABenqRuxIc9KeUJxWL12hRm62dLDgkdU1ZqXqV0MUmvcACDbVdsIaw==";
        private readonly string CosmosDbName = "spectrumacpl";
        private readonly string CosmosDbContainerName = "section";

        #endregion
        private Sofcom.CoreServices.IDbManager _dbManager;
        public List<Section> sectionResult = new List<Section>();
       public List<SectionDetail> sectiondet = new List<SectionDetail>(); 
        public Section sectionresponse = new Section();
        System.DateTime MDateTime;
        protected Sofcom.CoreServices.IDbManager dbManager
        {
            get
            {
                return _dbManager;
            }
            set
            {
                _dbManager = value;
            }
        }
        public SectionRepositry()
        {
            SF0001G.LocalDBCredentialsProvider.MIntCompID = 1011;
            SF0001G.LocalDBCredentialsProvider.GstrEnv = "Live";
            SF0001G.GClsGeneral.GStrDBType = "SQL";

            SF0001G.GClsConnection_Sql.LAppType = "Web";
            SF0001G.GClsConnection_Sql.MIntCompID = 1011;
            SF0001G.GClsConnection_Sql.GstrEnv = "Live";
            SF0001G.GClsConnection_Sql.GstrServer = "SofsrvDB01\\Client2014";

            this.dbManager = SF0001G.Factory.GetDBManager();

        }

        public async Task<List<Section>> GetAsync()
        {
            try
            {
                StringBuilder selectCommand = new StringBuilder();
                DataTable dt = new DataTable();

                selectCommand.Append("Select * from SPBSSectionPRM ");
                //selectCommand.Append(" where ProdCode = @ProdCode");

                //List<DbParameter> dbParameters = new List<DbParameter>()
                //{
                //    new SqlParameter() {ParameterName = "ProdCode", DbType = DbType.String, Value = ""}
                //};
                dt = this.dbManager.FetchData(selectCommand.ToString());

                if (dt.Rows.Count > 0)
                {
                    //{
                    //    new Section()
                    //    {
                    //        sectionCode = dt.Rows[0]["SectionCode"].ToString().Trim(),
                    //        sectionName = dt.Rows[0]["SectionDesc"].ToString().Trim()
                    //    };
                    //};
                    Section section = new Section()
                    {
                        sectionCode = dt.Rows[0]["SectionCode"].ToString().Trim(),
                        sectionName = dt.Rows[0]["SectionDesc"].ToString().Trim()
                    };
                    sectionResult.Add(section);
                }
            }
            catch (Exception ex)
            {

                //throw ex.Message;
            }
            return sectionResult.ToList();
            //throw new NotImplementedException();
        }

        public async Task<Section> GetSectionAsync(string code)
        {
            try
            {
                StringBuilder selectCommand = new StringBuilder();
                DataTable dt = new DataTable();

                selectCommand.Append("Select * from SPBSCOUNTRYPRM ");
                selectCommand.Append(" where CountryCode = @CountryCode");

                List<DbParameter> dbParameters = new List<DbParameter>()
                {
                    new SqlParameter() {ParameterName = "CountryCode", DbType = DbType.String, Value = code}
                };
                dt = this.dbManager.FetchData(selectCommand.ToString(), dbParameters.ToArray());

                if (dt.Rows.Count > 0)
                {
                  
                    await Task.Run(() => sectionresponse = new Section()
                    {
                        sectionCode = dt.Rows[0]["CountryCode"].ToString().Trim(),
                        sectionName = dt.Rows[0]["CountryName"].ToString().Trim()
                    });
                   
                }
            }
            catch (Exception)
            {

                //throw ex.Message;
            }
            return sectionresponse;
        }

        public async Task<Section> AddAsync(Section section)
        {
            string LStrErr;
                SF0001G.GClsGeneral LClsGeneral = new SF0001G.GClsGeneral();
                MDateTime =DateTime.Now;
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {



                    StringBuilder insertCommand = new StringBuilder();
                    insertCommand.Append("Insert into SPBSCOUNTRYPRM (CountryCode,CountryName,");
                    insertCommand.Append(" LASTUSER, LASTUPDATE,CREATEDBY,CREATEDON) Values(");
                    insertCommand.Append(" @CountryCode,@CountryName,@LASTUSER, @LASTUPDATE,@CREATEDBY,@CREATEDON)");

                    List<DbParameter> parameters = new List<DbParameter>()
                            {
                                new SqlParameter() {ParameterName = "CountryCode", DbType = DbType.String, Value = section.sectionCode},
                                new SqlParameter() {ParameterName = "CountryName", DbType = DbType.String, Value = section.sectionName},
                                new SqlParameter() {ParameterName = "LASTUSER", DbType = DbType.String, Value = "DEMOUSER"},
                                new SqlParameter() {ParameterName = "LASTUPDATE", DbType = DbType.DateTime, Value = MDateTime},
                                new SqlParameter() {ParameterName = "CREATEDBY", DbType = DbType.String, Value = "DEMOUSER"},
                                new SqlParameter() {ParameterName = "CREATEDON", DbType = DbType.DateTime, Value = MDateTime}
                            };
                    this.dbManager.ExecuteCommand(insertCommand.ToString(), parameters.ToArray());

                    transaction.Complete();
                    }
                    catch (Exception)
                    {
                        transaction.Dispose();
                        
                    }
                }
         
           
            return section;
        }

        public async Task<List<SectionDetail>> GetSectionDetails()
        {
           
            try
            {
              
                var container = ContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<SectionDetail> queryResultSetIterator = container.GetItemQueryIterator<SectionDetail>(queryDefinition);
               
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<SectionDetail> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (SectionDetail sec in currentResultSet)
                    {

                        sectiondet.Add(sec);
                    }
                   
                }
              
            }
            catch (Exception)
            {
                
            }
            return sectiondet.ToList();
        }

        public async Task<SectionDetail> AddSectionDetail(SectionDetail section)
        {
            
                var container = ContainerClient();
                var response = await container.CreateItemAsync(section, new PartitionKey(section.sectioncode));
         
            return response;

            
            
        }

        private Container ContainerClient()
          {
                CosmosClient cosmosDbClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
                Container containerClient = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
                return containerClient;
          }
        
    }
}
