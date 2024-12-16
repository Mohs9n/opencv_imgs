using System;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using ScottPlot;
using ScottPlot.WinForms;


namespace img
{
    public static class Ocv
    {
        public static Mat LoadImage(string filePath)
        {
            try
            {
                return Cv2.ImRead(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        }
        
        public static Mat ConvertToColorSpace(Mat image, ColorSpace colorSpace)
        {
            if (image is null)
                throw new ArgumentNullException(nameof(image));

            var result = new Mat();
            switch (colorSpace)
            {
                case ColorSpace.Red:
                {
                    var channels = Cv2.Split(image);
                    Cv2.Merge(new Mat[] { Mat.Zeros(image.Size(), MatType.CV_8UC1), Mat.Zeros(image.Size(), MatType.CV_8UC1), channels[2] }, result);
                    break;
                }
                case ColorSpace.Green:
                {
                    var channels = Cv2.Split(image);
                    Cv2.Merge(new Mat[] { Mat.Zeros(image.Size(), MatType.CV_8UC1), channels[1], Mat.Zeros(image.Size(), MatType.CV_8UC1) }, result);
                    break;
                }
                case ColorSpace.Blue:
                {
                    var channels = Cv2.Split(image);
                    Cv2.Merge(new Mat[] { channels[0], Mat.Zeros(image.Size(), MatType.CV_8UC1), Mat.Zeros(image.Size(), MatType.CV_8UC1) }, result);
                    break;
                }
                case ColorSpace.Gray:
                    Cv2.CvtColor(image, result, ColorConversionCodes.BGR2GRAY);
                    break;
                default:
                    throw new ArgumentException($"Invalid color option: {colorSpace}");
            }

            return result;
        }
        
//         public static Mat ComputeHistogram(Mat image)
//         {
//             if (image is null)
//                 throw new ArgumentNullException(nameof(image));
//
//             var grayImage = new Mat();
//             Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
//
//             var hist = new Mat();
//             int[] histSize = { 256 };
//             Rangef[] ranges = { new Rangef(0, 256) };
//
//             Cv2.CalcHist(new Mat[] { grayImage }, new int[] { 0 }, null, hist, 1, histSize, ranges);
//     
//             int histW = 512, histH = 400;
//             var binW = histW / histSize[0];
//
//             var histImage = new Mat(histH, histW, MatType.CV_8UC3, new Scalar(0, 0, 0));
//
//             Cv2.Normalize(hist, hist, 0, histImage.Rows, NormTypes.MinMax);
//
//             for (var i = 1; i < histSize[0]; i++)
//             {
//                 Cv2.Line(histImage,
//                     new Point(binW * (i - 1), histH - Math.Round(hist.At<float>(i - 1))),
//                     new Point(binW * i, histH - Math.Round(hist.At<float>(i))),
//                     new Scalar(255, 255, 255), 2);
//             }
//
//             return histImage;
//         }
//         public static Mat DisplayColorHistogram(Mat image)
//     {
//     if (image.Channels() != 3)
//     {
//         throw new ArgumentException("The image must have three color channels (BGR).");
//     }
//
//     // Split the image into its Blue, Green, and Red channels
//     var channels = Cv2.Split(image);
//
//     // Initialize histogram parameters
//     var histSize = 256; // Number of bins
//     var range = new Rangef(0, 256); // Pixel intensity range
//     int histWidth = 512, histHeight = 400;
//     var binWidth = histWidth / histSize;
//
//     // Create an image to draw the histograms
//     var histImage = new Mat(new Size(histWidth, histHeight), MatType.CV_8UC3, Scalar.All(0));
//
//     // Colors for each channel
//     var blue = new Scalar(255, 0, 0); // Blue
//     var green = new Scalar(0, 255, 0); // Green
//     var red = new Scalar(0, 0, 255); // Red
//
//     // Calculate histograms for each channel
//     Mat blueHist = new Mat(), greenHist = new Mat(), redHist = new Mat();
//     Cv2.CalcHist(new Mat[] { channels[0] }, new int[] { 0 }, null, blueHist, 1, new int[] { histSize }, new Rangef[] { range });
//     Cv2.CalcHist(new Mat[] { channels[1] }, new int[] { 0 }, null, greenHist, 1, new int[] { histSize }, new Rangef[] { range });
//     Cv2.CalcHist(new Mat[] { channels[2] }, new int[] { 0 }, null, redHist, 1, new int[] { histSize }, new Rangef[] { range });
//
//     // Normalize histograms to fit in the display
//     Cv2.Normalize(blueHist, blueHist, 0, histHeight, NormTypes.MinMax);
//     Cv2.Normalize(greenHist, greenHist, 0, histHeight, NormTypes.MinMax);
//     Cv2.Normalize(redHist, redHist, 0, histHeight, NormTypes.MinMax);
//
//     // Draw the histograms
//     for (var i = 1; i < histSize; i++)
//     {
//         // Blue channel
//         Cv2.Line(histImage,
//             new Point(binWidth * (i - 1), histHeight - (int)blueHist.Get<float>(i - 1)),
//             new Point(binWidth * i, histHeight - (int)blueHist.Get<float>(i)),
//             blue, 2);
//
//         // Green channel
//         Cv2.Line(histImage,
//             new Point(binWidth * (i - 1), histHeight - (int)greenHist.Get<float>(i - 1)),
//             new Point(binWidth * i, histHeight - (int)greenHist.Get<float>(i)),
//             green, 2);
//
//         // Red channel
//         Cv2.Line(histImage,
//             new Point(binWidth * (i - 1), histHeight - (int)redHist.Get<float>(i - 1)),
//             new Point(binWidth * i, histHeight - (int)redHist.Get<float>(i)),
//             red, 2);
//     }
//
//     return histImage;
// }



        public static Mat EqualizeHistogram(Mat image)
        {
            // if (image == null)
            //     throw new ArgumentNullException(nameof(image));
            //
            // var grayImage = new Mat();
            // Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
            //
            // var equalizedImage = new Mat();
            // Cv2.EqualizeHist(grayImage, equalizedImage);
            //
            // return equalizedImage;
            
            // Split the image into individual color channels
            Mat[] channels = Cv2.Split(image);

            // Equalize each channel independently
            for (int i = 0; i < channels.Length; i++)
            {
                Cv2.EqualizeHist(channels[i], channels[i]);
            }

            // Merge the channels back together
            Mat result = new Mat();
            Cv2.Merge(channels, result);

            return result;
        }
        
        public static Mat ApplyCannyEdgeDetection(Mat image, double threshold1, double threshold2)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

            var edges = new Mat();
            Cv2.Canny(grayImage, edges, threshold1, threshold2);

            return edges;
        }

        public static Mat ApplyGaussianBlur(Mat image, int kernelSize)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var blurred = new Mat();
            Cv2.GaussianBlur(image, blurred, new Size(kernelSize, kernelSize), 0);

            return blurred;
        }

        public static Mat ApplySmoothing(Mat image, int kernelSize)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var smoothed = new Mat();
            Cv2.Blur(image, smoothed, new Size(kernelSize, kernelSize));

            return smoothed;
        }
        
        public static Mat ApplyLaplacian(Mat image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

            var laplacian = new Mat();
            Cv2.Laplacian(grayImage, laplacian, MatType.CV_8U);

            return laplacian;
        }

        public static Mat ScaleImage(Mat image, PictureBox pictureBox)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (pictureBox == null)
                throw new ArgumentNullException(nameof(pictureBox));

            // Get the dimensions of the PictureBox
            var pictureBoxWidth = pictureBox.Width;
            var pictureBoxHeight = pictureBox.Height;

            // Get the dimensions of the image
            var imageWidth = image.Width;
            var imageHeight = image.Height;

            // Calculate the scaling factor to fit the image within the PictureBox
            var scaleX = (double)pictureBoxWidth / imageWidth;
            var scaleY = (double)pictureBoxHeight / imageHeight;

            // Use the smaller scale to maintain the aspect ratio
            var scale = Math.Min(scaleX, scaleY);

            // Compute the new dimensions
            var newWidth = (int)(imageWidth * scale);
            var newHeight = (int)(imageHeight * scale);

            // Resize the image
            var resizedImage = new Mat();
            Cv2.Resize(image, resizedImage, new OpenCvSharp.Size(newWidth, newHeight));

            return resizedImage;
        }

        public static void ShowHistogramUsingScottPlot(Mat image)
        {
            if (image.Channels() != 3)
            {
                throw new ArgumentException("The image must have three color channels (BGR).");
            }

            // Split the image into its Blue, Green, and Red channels
            Mat[] channels = Cv2.Split(image);

            // Calculate histograms for each channel
            int histSize = 256;
            Rangef range = new Rangef(0, 256);

            Mat blueHist = new Mat(), greenHist = new Mat(), redHist = new Mat();
            Cv2.CalcHist(new Mat[] { channels[0] }, new int[] { 0 }, null, blueHist, 1, new int[] { histSize }, new Rangef[] { range });
            Cv2.CalcHist(new Mat[] { channels[1] }, new int[] { 0 }, null, greenHist, 1, new int[] { histSize }, new Rangef[] { range });
            Cv2.CalcHist(new Mat[] { channels[2] }, new int[] { 0 }, null, redHist, 1, new int[] { histSize }, new Rangef[] { range });

            // Convert histograms to arrays for plotting
            float[] blueArray = new float[histSize];
            float[] greenArray = new float[histSize];
            float[] redArray = new float[histSize];

            blueHist.GetArray(out blueArray);
            greenHist.GetArray(out greenArray);
            redHist.GetArray(out redArray);

            // Convert float arrays to double arrays for ScottPlot
            double[] blueData = Array.ConvertAll(blueArray, x => (double)x);
            double[] greenData = Array.ConvertAll(greenArray, x => (double)x);
            double[] redData = Array.ConvertAll(redArray, x => (double)x);

            ShowHistogram(blueData, greenData, redData, (image.Width * image.Height));
        }

        public static void ShowHistogram(double[] blueData, double[] greenData, double[] redData, double pixels)
        {
            // Create a new form
            var form = new Form
            {
                Text = "Color Histogram",
                Width = 800,
                Height = 600
            };

            // Add a FormsPlot control
            var scottPlotViewer = new FormsPlot { Dock = DockStyle.Fill };
            form.Controls.Add(scottPlotViewer);

            // X-axis values for the histogram
            double[] xValues = Enumerable.Range(0, blueData.Length).Select(i => (double)i).ToArray();
            
            if (blueData[0]  !=  pixels)
            {
                scottPlotViewer.Plot.Add.Scatter(xValues, blueData, color: ScottPlot.Color.FromColor(System.Drawing.Color.Blue));
            }
            if (greenData[0]  !=  pixels)
            {
                scottPlotViewer.Plot.Add.Scatter(xValues, greenData, color: ScottPlot.Color.FromColor(System.Drawing.Color.Green));
            }
            if (redData[0]  !=  pixels)
            {
                scottPlotViewer.Plot.Add.Scatter(xValues, redData, color: ScottPlot.Color.FromColor(System.Drawing.Color.Red));
            }

            // Customize the plot
            scottPlotViewer.Plot.Title("Color Histogram");
            scottPlotViewer.Plot.XLabel("Pixel Intensity");
            scottPlotViewer.Plot.YLabel("Frequency");
            // scottPlotViewer.Plot.ShowLegend();
            
            // Refresh the plot and display the form
            scottPlotViewer.Refresh();
            form.ShowDialog();
        }



        
        public static void SaveImage(Mat image)
        {
            if (image is null)
                throw new ArgumentNullException(nameof(image));

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save Processed Image";
                saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|All Files|*.*";
                saveFileDialog.DefaultExt = "png"; // Default to PNG

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    image.SaveImage(saveFileDialog.FileName);
                    MessageBox.Show($"Image saved successfully to {saveFileDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public enum ColorSpace
    {
        Red,
        Blue,
        Green,
        Gray
    }
}