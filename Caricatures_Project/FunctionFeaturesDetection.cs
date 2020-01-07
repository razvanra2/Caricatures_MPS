using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Caricatures_Project
{
    public static class FunctionFeaturesDetection
    {
        [FunctionName("FunctionFeaturesDetection")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string key = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "key", true) == 0)
                .Value;

            // Get request body
            MultipartMemoryStreamProvider stream = await req.Content.ReadAsMultipartAsync();
            var st = stream.Contents[0];
            var fileBytes = await st.ReadAsByteArrayAsync();

            using (var ms = new MemoryStream(fileBytes))
            {
                var bytes = ms.ToArray();
                var resultAnalysis = await MakeRequest(bytes, key);

                return req.CreateResponse(HttpStatusCode.OK, resultAnalysis);
            }
        }

        static async Task<string> MakeRequest(byte[] imgBytes, string key)
        {
            var client = new HttpClient();
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "true";
            queryString["returnFaceAttributes"] = "age,gender,smile,facialHair,glasses,hair,headPose";
            queryString["recognitionModel"] = "recognition_01";
            queryString["returnRecognitionModel"] = "false";
            queryString["detectionModel"] = "detection_01";
            var uri = "https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;

            // Request body
            using (var content = new ByteArrayContent(imgBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }
    }
}
