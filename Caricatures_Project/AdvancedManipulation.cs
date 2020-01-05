using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caricatures_Project
{
    class AdvancedManipulation
    {

        public Bitmap apply_sobel(Bitmap img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var gx = 0;
                    var gy = 0;

                    /////////////////////////////////
                    var p = img.GetPixel(x - 1, y - 1);

                    var r = p.R;
                    var g = p.G;
                    var b = p.B;

                    var intensity = r + g + b;

                    gx += -intensity;
                    gy += -intensity;


                    /////////////////////////

                    p = img.GetPixel(x - 1, y);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gx += -2 * (r + g + b);

                    p = img.GetPixel(x - 1, y + 1);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gx += -(r + g + b);
                    gy += (r + g + b);

                    ////////////////////////

                    p = img.GetPixel(x, y - 1);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gy += -2 * (r + g + b);

                    p = img.GetPixel(x, y + 1);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gy += 2 * (r + g + b);

                    ////////////////////////
                    p = img.GetPixel(x + 1, y - 1);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gx += (r + g + b);
                    gy += -(r + g + b);

                    p = img.GetPixel(x + 1, y);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gx += 2 * (r + g + b);

                    p = img.GetPixel(x + 1, y + 1);

                    r = p.R;
                    g = p.G;
                    b = p.B;

                    gx += (r + g + b);
                    gy += -(r + g + b);

                    //////////////////////////

                    var length = Math.Sqrt(gx * gx + gy * gy);

                    length = Math.Round(length / 4328 * 255);

                    output.SetPixel(x, y, Color.FromArgb((int)length, (int)length, (int)length));
                }
            }
            return output;
        }

        public Bitmap apply_binarization(Bitmap img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            var threshold = 35;

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var p = img.GetPixel(x, y);

                    var r = 1.0 * (p.R > threshold ? 1 : 0);
                    var g = 1.0 * (p.G > threshold ? 1 : 0);
                    var b = 1.0 * (p.B > threshold ? 1 : 0);


                    if (r * g * b == 1.0)
                    {
                        output.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        output.SetPixel(x, y, Color.White);
                    }
                }
            }

            return output;
        }

        public Bitmap apply_dilation(Bitmap img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            var threshold = 100;

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (img.GetPixel(x - 1, y).R == 255 && img.GetPixel(x - 1, y).G == 255 && img.GetPixel(x - 1, y).B == 255)
                    {
                        output.SetPixel(x, y, Color.White);
                    }

                    if (img.GetPixel(x + 1, y).R == 255 && img.GetPixel(x + 1, y).G == 255 && img.GetPixel(x + 1, y).B == 255)
                    {
                        output.SetPixel(x, y, Color.White);
                    }

                    if (img.GetPixel(x, y - 1).R == 255 && img.GetPixel(x, y - 1).G == 255 && img.GetPixel(x, y - 1).B == 255)
                    {
                        output.SetPixel(x, y, Color.White);
                    }

                    if (img.GetPixel(x, y + 1).R == 255 && img.GetPixel(x, y + 1).G == 255 && img.GetPixel(x, y + 1).B == 255)
                    {
                        output.SetPixel(x, y, Color.White);
                    }
                }
            }
            return output;
        }

        public Bitmap combine_images(Bitmap img, Bitmap edge_img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var p = edge_img.GetPixel(x, y);
                    var ref_p = img.GetPixel(x, y);

                    if (p.R == 255 && p.G == 255 && p.B == 255)
                    {
                        output.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        output.SetPixel(x, y, ref_p);
                    }
                }
            }

            return output;
        }

        List<Color> colors = new List<Color>()
{
    Color.FromArgb(128, 0, 0),
    Color.FromArgb(170, 110, 40),
    Color.FromArgb(128, 128, 0),
    Color.FromArgb(0, 128, 128),
    Color.FromArgb(0, 0, 128),
    Color.FromArgb(0, 0, 0),
    Color.FromArgb(230, 25, 75),
    Color.FromArgb(245, 130, 48),
    Color.FromArgb(255, 255, 25),
    Color.FromArgb(210, 245, 60),
    Color.FromArgb(60, 180, 75),
    Color.FromArgb(70, 240, 240),
    Color.FromArgb(0, 130, 200),
    Color.FromArgb(145, 30, 180),
    Color.FromArgb(240, 50, 230),
    Color.FromArgb(128, 128, 128),
    Color.FromArgb(250, 190, 190),
    Color.FromArgb(255, 215, 180),
    Color.FromArgb(255, 250, 200),
    Color.FromArgb(170, 255, 195),
    Color.FromArgb(230, 190, 255),
    Color.FromArgb(255, 255, 255)
};
        Color get_reduced(Color p)
        {
            var min = 999999.0;

            Color output_color = Color.Transparent;

            foreach (var color in colors)
            {
                var dist = Math.Sqrt(
                    (color.R - p.R) * (color.R - p.R) +
                    (color.G - p.G) * (color.G - p.G) +
                    (color.B - p.B) * (color.B - p.B)
                    );

                if (dist < min)
                {
                    min = dist;
                    output_color = color;
                }
            }

            return output_color;
        }

        public Bitmap color_reduction_m1(Bitmap img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var p = img.GetPixel(x, y);

                    var reduced = get_reduced(p);

                    output.SetPixel(x, y, reduced);
                }
            }
            return output;
        }

        int[][] overlay;
        Dictionary<int, Color> averageDict = new Dictionary<int, Color>();

        List<Color> region_growing(int x, int y, Bitmap img, int crtArea, int threshold = 5)
        {
            if (overlay[x][y] == 0)
            {
                var width = img.Width;
                var height = img.Height;

                var pixel = img.GetPixel(x, y);

                var avgList = new List<Color>();

                avgList.Add(pixel);

                overlay[x][y] = crtArea;

                var neighbouringPixels = new List<Tuple<int, int>>();

                neighbouringPixels.Add(Tuple.Create(x - 1, y));
                neighbouringPixels.Add(Tuple.Create(x + 1, y));
                neighbouringPixels.Add(Tuple.Create(x, y - 1));
                neighbouringPixels.Add(Tuple.Create(x, y + 1));
                neighbouringPixels.Add(Tuple.Create(x - 1, y - 1));
                neighbouringPixels.Add(Tuple.Create(x - 1, y + 1));
                neighbouringPixels.Add(Tuple.Create(x + 1, y - 1));
                neighbouringPixels.Add(Tuple.Create(x + 1, y + 1));

                var validNeighbours = neighbouringPixels
                    .Where(p => p.Item1 > 0 && p.Item1 < width && p.Item2 > 0 && p.Item2 < height);

                var neighboursToVisit = new List<Tuple<int, int>>();
                foreach (var n in validNeighbours)
                {
                    var xn = n.Item1;
                    var yn = n.Item2;

                    var px_r = img.GetPixel(x, y).R;
                    var px_g = img.GetPixel(x, y).G;
                    var px_b = img.GetPixel(x, y).B;

                    var pxn_r = img.GetPixel(xn, yn).R;
                    var pxn_g = img.GetPixel(xn, yn).G;
                    var pxn_b = img.GetPixel(xn, yn).B;

                    var euclid_dist = Math.Sqrt((pxn_r - px_r) * (pxn_r - px_r) + (pxn_g - px_g) * (pxn_g - px_g) + (pxn_b - px_b) * (pxn_b - px_b));

                    if (euclid_dist < threshold)
                    {
                        if (overlay[xn][yn] == 0)
                        {
                            neighboursToVisit.Add(n);
                            avgList.Add(img.GetPixel(xn, yn));
                        }
                    }
                }

                while (neighboursToVisit.Count() > 0)
                {
                    var toScan = neighboursToVisit[0];
                    neighboursToVisit.RemoveAt(0);

                    var xn = toScan.Item1;
                    var yn = toScan.Item2;

                    overlay[xn][yn] = crtArea;

                    neighbouringPixels = new List<Tuple<int, int>>();

                    neighbouringPixels.Add(Tuple.Create(xn - 1, yn));
                    neighbouringPixels.Add(Tuple.Create(xn + 1, yn));
                    neighbouringPixels.Add(Tuple.Create(xn, yn - 1));
                    neighbouringPixels.Add(Tuple.Create(xn, yn + 1));
                    neighbouringPixels.Add(Tuple.Create(xn - 1, yn - 1));
                    neighbouringPixels.Add(Tuple.Create(xn - 1, yn + 1));
                    neighbouringPixels.Add(Tuple.Create(xn + 1, yn - 1));
                    neighbouringPixels.Add(Tuple.Create(xn + 1, yn + 1));

                    validNeighbours = neighbouringPixels
                        .Where(p => p.Item1 > 0 && p.Item1 < width && p.Item2 > 0 && p.Item2 < height);

                    foreach (var n in validNeighbours)
                    {
                        var xnew = n.Item1;
                        var ynew = n.Item2;

                        var px_r = img.GetPixel(xnew, ynew).R;
                        var px_g = img.GetPixel(xnew, ynew).G;
                        var px_b = img.GetPixel(xnew, ynew).B;

                        var pxn_r = img.GetPixel(xn, yn).R;
                        var pxn_g = img.GetPixel(xn, yn).G;
                        var pxn_b = img.GetPixel(xn, yn).B;

                        if ((pxn_r - px_r) * (pxn_r - px_r) + (pxn_g - px_g) * (pxn_g - px_g) + (pxn_b - px_b) * (pxn_b - px_b) < threshold)
                        {
                            if (overlay[xnew][ynew] == 0)
                            {
                                if (!neighboursToVisit.Contains(n))
                                {
                                    neighboursToVisit.Add(n);
                                }

                                var new_p = img.GetPixel(xnew, ynew);

                                if (!avgList.Contains(new_p))
                                {
                                    avgList.Add(new_p);
                                }
                            }
                        }
                    }
                }

                return avgList;
            }

            return null;
        }

        Color get_average_pixel(List<Color> colors)
        {
            var sum_r = 0.0;
            var sum_g = 0.0;
            var sum_b = 0.0;

            foreach (var color in colors)
            {
                sum_r += color.R;
                sum_g += color.G;
                sum_b += color.B;
            }

            return Color.FromArgb(
                (int)sum_r / colors.Count(),
                (int)sum_g / colors.Count(),
                (int)sum_b / colors.Count()
            );
        }

        Bitmap color_reduction_m2(Bitmap img)
        {
            var width = img.Width;
            var height = img.Height;

            var output = new Bitmap(width, height);

            overlay = new int[width][];

            for (int i = 0; i < width; i++)
            {
                overlay[i] = new int[height];
            }

            var currentArea = 1;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var regionGrowingRes = region_growing(x, y, img, currentArea, 15);

                    if (regionGrowingRes != null)
                    {
                        var avgPixel = get_average_pixel(regionGrowingRes);
                        averageDict[currentArea] = avgPixel;

                        currentArea += 1;
                    }
                }
            }

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var zone = overlay[x][y];
                    output.SetPixel(x, y, averageDict[zone]);
                }
            }

            return output;
        }
    }
}
