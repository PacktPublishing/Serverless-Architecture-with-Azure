using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace ServerlessFunctions
{
    public static class TelemetryInsert
    {
        [FunctionName("Telemetry-Insert")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Telemetry-Insert Fired!");

            TelemetryData[] telemetryData = await req.Content.ReadAsAsync<TelemetryData[]>();

            IList<TelemetryData> telemetryList = telemetryData;

            log.Info("Getting Connection String from App Settings.");

            // Get Conntection String
            var connectionString = Environment.GetEnvironmentVariable("SqlConnection");

            foreach (var data in telemetryList)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("usp_InsertDeviceTelemetry", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DeviceName", data.devicename));
                    cmd.Parameters.Add(new SqlParameter("@Name", data.name));
                    cmd.Parameters.Add(new SqlParameter("@Value", data.value));

                    var recordChange = cmd.ExecuteNonQuery();
                }
            }

            log.Info("Function Complete Return Error Logged!");

            return telemetryList == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, telemetryList);


        }
    }
}
