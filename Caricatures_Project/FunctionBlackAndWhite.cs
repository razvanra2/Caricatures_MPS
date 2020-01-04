using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Caricatures_Project
{
    public static class FunctionBlackAndWhite
    {
        [FunctionName("FunctionBlackAndWhite")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body


            MultipartMemoryStreamProvider stream = await req.Content.ReadAsMultipartAsync();
            var st = stream.Contents[0];
            var fileBytes = await st.ReadAsByteArrayAsync();

            using (var ms = new MemoryStream(fileBytes))
            {
                using (var img = new Bitmap(Image.FromStream(ms)))
                {
                    using (var blackAndWhiteImg = MakeBlackAndWhite(img))
                    {
                        using (var res = new MemoryStream())
                        {
                            blackAndWhiteImg.Save(res, ImageFormat.Png);
                            var imagAsArr = res.ToArray();
                            return req.CreateResponse(HttpStatusCode.OK, Convert.ToBase64String(imagAsArr));
                        }
                    }
                }
            }
        }

        public static Bitmap MakeBlackAndWhite(Bitmap original)
        {

            Bitmap output = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {

                for (int j = 0; j < original.Height; j++)
                {
                    var c = original.GetPixel(i, j);
                    var rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    output.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                }
            }

            return output;
        }
    }
}
