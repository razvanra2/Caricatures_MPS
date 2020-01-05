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
    public static class FunctionSepia
    {
        [FunctionName("FunctionSepia")]
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
                    using (var blackAndWhiteImg = MakeSepia(img))
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


        public static Bitmap MakeSepia(Bitmap original)
        {
            Bitmap output = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    //get pixel value
                    var p = original.GetPixel(x, y);

                    //extract pixel component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //calculate temp value
                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    //set new RGB value
                    if (tr > 255)
                    {
                        r = 255;
                    }
                    else
                    {
                        r = tr;
                    }

                    if (tg > 255)
                    {
                        g = 255;
                    }
                    else
                    {
                        g = tg;
                    }

                    if (tb > 255)
                    {
                        b = 255;
                    }
                    else
                    {
                        b = tb;
                    }

                    //set the new RGB value in image pixel
                    output.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }

            return output;
        }
    }
}
