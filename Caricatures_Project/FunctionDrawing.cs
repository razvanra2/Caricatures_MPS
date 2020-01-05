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
    public static class FunctionDrawing
    {
        [FunctionName("FunctionDrawing")]
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
                    using (var blackAndWhiteImg = ApplyDrawingEffect(img))
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

        private static Bitmap ApplyDrawingEffect(Bitmap img)
        {
            var manipulation = new AdvancedManipulation();

            Bitmap sobel_applied_img = manipulation.apply_sobel(img);

            // apply binarization
            Bitmap binarization_applied_img = manipulation.apply_binarization(sobel_applied_img);

            // apply dilation
            Bitmap dilation_applied_img = manipulation.apply_dilation(binarization_applied_img);

            // combine dilation result with original image
            Bitmap combined_img = manipulation.combine_images(img, dilation_applied_img);

            sobel_applied_img.Dispose();
            binarization_applied_img.Dispose();
            dilation_applied_img.Dispose();

            return combined_img;
        }
    }
}