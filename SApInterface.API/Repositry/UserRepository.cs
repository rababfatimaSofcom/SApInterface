using SApInterface.API.Model;
using SApInterface.API.Model.Domain;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SApInterface.API.Repositry
{
    public class UserRepository : IUserRepository
    {
        public User userresponse = new User();
        private Sofcom.CoreServices.IDbManager _dbManager;
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
        public UserRepository()
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


        public async Task<User> AuthenticateAsync(string username, string password)
        {
            try
            {
                StringBuilder selectCommand = new StringBuilder();
                DataTable dt = new DataTable();

                selectCommand.Append("Select * from SPSUUSERPRM ");
                selectCommand.Append(" where Userid = @Userid");

                List<DbParameter> dbParameters = new List<DbParameter>()
                {
                    new SqlParameter() {ParameterName = "Userid", DbType = DbType.String, Value = username}
                };
                dt = this.dbManager.FetchData(selectCommand.ToString(), dbParameters.ToArray());

                if (dt.Rows.Count > 0)
                {

                    userresponse = new User()
                    {
                         Username= dt.Rows[0]["Userid"].ToString().Trim(),
                         Password = dt.Rows[0]["UserPassword"].ToString(),
                        FirstName = dt.Rows[0]["FirstName"].ToString().Trim(),
                        LastName = dt.Rows[0]["LastName"].ToString().Trim(),
                        EmailAddress = dt.Rows[0]["Email"].ToString().Trim()
                    
                    };
                    userresponse.Roles = new List<string> { "reader", "writer" };

                }
            }
            catch (Exception)
            {

                //throw ex.Message;
            }

            if (userresponse == null)
            {
                return null;
            }

            //var userRoles = await nZWalksDbContext.Users_Roles.Where(x => x.UserId == user.Id).ToListAsync();

            //if (userRoles.Any())
            //{
            //    user.Roles = new List<string>();
            //    foreach (var userRole in userRoles)
            //    {
            //        var role = await nZWalksDbContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            //        if (role != null)
            //        {
            //            user.Roles.Add(role.Name);
            //        }
            //    }
            //}
          
           

            userresponse.Password = null;
            return userresponse;
        }
    }
}
