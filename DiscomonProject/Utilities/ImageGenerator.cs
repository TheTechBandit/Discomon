using System;
using System.Drawing;
using System.IO;
using Discord.Commands;

namespace DiscomonProject
{
    public static class ImageGenerator
    {
        static ImageGenerator()
        {
            
        }

        public static void ImageTest()
        {

        }

        public static Bitmap MergeTwoImages(/*Image firstImage, Image secondImage*/ SocketCommandContext Context)
        {
            Image firstImage = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Assets\\background.jpg");
            Image secondImage = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Assets\\suki.png");

            secondImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;

            //int outputImageHeight = firstImage.Height + secondImage.Height + 1;
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height - secondImage.Height), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            using (MemoryStream stream = new MemoryStream())
            {                                                   
                outputImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Seek(0, SeekOrigin.Begin);
                outputImage.Dispose();                                
                Context.Channel.SendFileAsync(stream, "Text.jpg");
            }

            return outputImage;
        }
        
    }
}