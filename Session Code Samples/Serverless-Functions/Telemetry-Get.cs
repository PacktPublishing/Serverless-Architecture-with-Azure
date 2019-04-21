using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessFunctions
{
    public static class TelemetryGet
    {
        [FunctionName("Telemetry-Get")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string name = null;

            // Get Conntection String
            var connectionString = Environment.GetEnvironmentVariable("SqlConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_SelectAllDeviceTelemetry", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                var dataReader = await cmd.ExecuteReaderAsync();

            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
