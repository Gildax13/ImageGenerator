using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace ApplicationPhotoV1
{
    public partial class Form1 : Form
    {
        private Bitmap[] images;
        private int currentImage;
        private int imageCount = 1;
        public Form1()
        {
            InitializeComponent();
            InitializeOpenFileDialog();
            this.setImages();
        }
        private void InitializeOpenFileDialog()
        {
            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "My Image Browser";
        }
        private void swapImageFromToOrigin(int from, int to)
        {

            if (from < 0 || from >= images.Length || to < 0 || to >= images.Length)
                return;
            if (from > to)
            {
                for (int i = to; i < from; i++)
                {
                    swapImageFromTo(i, i - 1);
                }
            }
            else
            {
                for (int i = to; i > from; i--)
                {
                    swapImageFromTo(i, i + 1);
                }
            }
        }

        private void swapImageFromTo(int from, int to)
        {

            if (from < 0 || from >= images.Length || to < 0 || to >= images.Length)
                return;
            Bitmap tmp = images[from];
            images[from] = images[to];
            images[to] = tmp;
        }
        private void addImage_Click(object sender, EventArgs e) //Add photos
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (String imageDir in openFileDialog1.FileNames)
                    {

                        Bitmap image = new Bitmap(imageDir);
                        image = ResizeBitmap(image,image.Width,image.Height);
                        Array.Resize(ref images, images.Length + 1);

                        images[currentImage++] = image;

                    }
                    updateListBox();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }
        private void updateListBox()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null)
                {
                    Bitmap image = images[i];
                    listBox1.Items.Add("Image " + i + " : " + image.Width + "x" + image.Height);
                }

            }
        }

        public Bitmap[] getImages()
        {
            return images;
        }

        public void setImages()
        {
            images = new Bitmap[imageCount];
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) //Largeur
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) //Hauteur
        {

        }

        private int getWidthOfImage(Bitmap image)
        {
            // Get the width of the image
            int width = image.Width;

            return width;
        }

        private int getHeightOfImage(Bitmap image)
        {
            // Get the height of the image
            int height = image.Height;
            return height;
        }

        private void setValueOnForm()
        {
            Bitmap image = new Bitmap(pictureBox1.ImageLocation);
            int width = getWidthOfImage(image);
            int height = getHeightOfImage(image);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = images[listBox1.SelectedIndex];

        }

        private void generateImage_Click(object sender, EventArgs e)
        {
            downloadImage();
        }
        private Bitmap getBitmapFromImage()
        {
            Bitmap image = images[0];
            if (image != null)
            {
                Bitmap bitmap = null;
                using (var stream = new MemoryStream())
                {
                    stream.Position = 0;
                    bitmap = new Bitmap(image.Width * (images.Length - 1), image.Height);
                }
                if (bitmap == null)
                    return null;
                Graphics g = Graphics.FromImage(bitmap);
                for (int i = 0; i < images.Length; i++)
                {
                    image = images[i];
                    if (image != null)
                    {
                        g.DrawImage(image, i * image.Width, 0);
                    }
                }
                g.Dispose();
                return bitmap;
            }
            return null;
        }

        private Bitmap appendImageOnRight(Bitmap image1, Bitmap image2)
        {
            if (image1 != null && image2 != null)
            {
                Bitmap bitmap = new Bitmap(image1.Width + image2.Width, image1.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawImage(image1, 0, 0);
                g.DrawImage(image2, image1.Width, 0);
                g.Dispose();
                return bitmap;
            }
            return null;
        }
        private void downloadImage()
        {
            Bitmap bitmap = getBitmapFromImage();
            if (bitmap != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        bitmap.Save(dialog.FileName);
                    }
                    catch(Exception e) {
                        textBox1.Text = e.Message;
                    }
                    
                }
            }
        }
    }
}