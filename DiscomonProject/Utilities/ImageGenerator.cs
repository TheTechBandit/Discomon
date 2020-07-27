using System;
using System.Collections.Generic;
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
            Image firstImage = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\background.jpg");
            Image secondImage = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\Mon Art\\suki.png");

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
            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using(Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height - secondImage.Height), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        public static Bitmap PartyMenu(List<BasicMon> mons)
        {
            Console.WriteLine("A");
            Image back = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\background.png");

            if(back == null)
                throw new ArgumentNullException("background");

            List<Bitmap> pBoxes = new List<Bitmap>();

            foreach(BasicMon m in mons)
            {
                Image monImage = Image.FromFile($"C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\Mon Art\\{m.Species.ToLower()}.png");
                Image box = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\bluebox.png");
                Image border = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\healthborder.png");

                Image bar;
                if(m.HealthPercentage() > 0.5)
                    bar = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\healthbarG.png");
                else if(m.HealthPercentage() > 0.2)
                    bar = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\healthbarY.png");
                else
                    bar = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\healthbarR.png");

                if(monImage == null)
                    throw new ArgumentNullException("monImage");
                if(box == null)
                    throw new ArgumentNullException("partyBox");
                if(border == null)
                    throw new ArgumentNullException("HPBorder");
                if(bar == null)
                    throw new ArgumentNullException("HPBar");

                int outputImageWidth = box.Width;
                int outputImageHeight = box.Height;
                Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using(Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(box, new Rectangle(new Point(), box.Size), new Rectangle(new Point(), box.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(monImage, new Rectangle(50, 40, 120, 120), new Rectangle(new Point(), monImage.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(bar, new Rectangle(265, 140, (int)(390*m.HealthPercentage()), 42), new Rectangle(new Point(), bar.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(border, new Rectangle(261, 136, 400, 48), new Rectangle(new Point(), border.Size), GraphicsUnit.Pixel);
                    
                    Font electrolizeSmall = new Font("Electrolize", 26);
                    Font electrolizeLarge = new Font("Electrolize", 36);
                    graphics.DrawString($"Lv. {m.Level}", electrolizeSmall, Brushes.Black, new PointF(55, 165));
                    graphics.DrawString($"{m.Species} {m.GenderSymbol}", electrolizeLarge, Brushes.Black, new PointF(365, 65));
                    graphics.DrawString($"HP", electrolizeSmall, Brushes.Black, new PointF(195, 140));
                    graphics.DrawString($"{m.CurrentHP} / {m.TotalHP}", electrolizeSmall, Brushes.Black, new PointF(390, 190));
                }

                pBoxes.Add(outputImage);
            }

            while(pBoxes.Count != 6)
            {
                Image box = Image.FromFile("C:\\Users\\lifei_jljypee\\Desktop\\Discomon Project\\DiscomonProject\\Assets\\UI Assets\\bluebox.png");
                int extraBoxWidth = box.Width;
                int extraBoxHeight = box.Height;
                Bitmap extraBox = new Bitmap(extraBoxWidth, extraBoxHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using(Graphics graphics = Graphics.FromImage(extraBox))
                {
                    graphics.DrawImage(box, new Rectangle(new Point(), box.Size),
                        new Rectangle(new Point(), box.Size), GraphicsUnit.Pixel);
                }
                pBoxes.Add(extraBox);
            }

            int finalImageWidth = back.Width;
            int finalImageHeight = back.Height;
            Bitmap finalImage = new Bitmap(finalImageWidth, finalImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using(Graphics graphics = Graphics.FromImage(finalImage))
            {
                graphics.DrawImage(back, new Rectangle(new Point(), back.Size),
                        new Rectangle(new Point(), back.Size), GraphicsUnit.Pixel);
                bool right = false;
                int level = 1;
                foreach(Bitmap b in pBoxes)
                {
                    int leftCoord = 71*level+(b.Height*(level-1));
                    int rightCoord;
                    if(right)
                    {
                        rightCoord = 310+b.Width;
                        right = false;
                        level++;
                    }
                    else
                    {
                        rightCoord =  155;
                        right = true;
                    }
                    graphics.DrawImage(b, new Rectangle(new Point(rightCoord, leftCoord), b.Size),
                        new Rectangle(new Point(), b.Size), GraphicsUnit.Pixel);
                }
            }

            return finalImage;
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }
        
    }
}