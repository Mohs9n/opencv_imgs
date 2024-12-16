using System;
using System.Windows.Forms;
using OpenCvSharp;
 
namespace img
{
    public partial class Form1 : Form
    {
        private Mat _originalImage;
        private Mat _processedImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                try
                {
                    _originalImage = Ocv.LoadImage(openFileDialog.FileName);

                    _processedImage = _originalImage.Clone();
                    DisplayImage(_processedImage);
                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void DisplayImage(Mat image)
        {
            if (image == null)
            {
                MessageBox.Show("No image to display!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var scaledImage = Ocv.ScaleImage(image, pictureBox);

            pictureBox.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(scaledImage);
        }
        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ApplyGaussianBlur(image, gbKernelSize.Value), "Gaussian Blur");
        }

        private void cannyEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ApplyCannyEdgeDetection(image, 50, 100), "Canny Edge Detection");
        }

        private void laplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(Ocv.ApplyLaplacian, "Laplacian");
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ConvertToColorSpace(image, ColorSpace.Red), "Red", _originalImage);
        }
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ConvertToColorSpace(image, ColorSpace.Blue), "Blue", _originalImage);
        }
        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ConvertToColorSpace(image, ColorSpace.Green), "Green", _originalImage);
        }
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ConvertToColorSpace(image, ColorSpace.Gray), "Gray", _originalImage);
        }
        
        private void ApplyEffect(Func<Mat, Mat> effect, string effectName, Mat sourceImage = null)
        {
            // Use the processed image if sourceImage is null
            var imageToProcess = sourceImage ?? _processedImage;

            if (imageToProcess == null)
            {
                MessageBox.Show("Please load an image first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _processedImage = effect(imageToProcess);
                DisplayImage(_processedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying {effectName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetEffectsBTN_Click(object sender, EventArgs e)
        {
            if (_processedImage is null)
            {
                MessageBox.Show("Please load an image first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _processedImage = _originalImage.Clone();
            DisplayImage(_processedImage);
        }
        
        private void gbKernelSize_Scroll(object sender, EventArgs e)
        {
            if (gbKernelSize.Value % 2 == 0)
            {
                gbKernelSize.Value += 1;
            }
            
            gbKernilSizeLabel.Text = gbKernelSize.Value.ToString();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_processedImage is null)
            {
                MessageBox.Show("Please load an image first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Ocv.SaveImage(_processedImage);
        }

        private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(Ocv.EqualizeHistogram, "Histogram Equalization");
        }

        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyEffect(image => Ocv.ApplySmoothing(image, gbKernelSize.Value), "Smoothing");
        }

        private void showHistBtn_Click(object sender, EventArgs e)
        {
            if (_processedImage == null)
            {
                MessageBox.Show("Please load an image first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Ocv.ShowHistogramUsingScottPlot(_processedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying histogram: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}