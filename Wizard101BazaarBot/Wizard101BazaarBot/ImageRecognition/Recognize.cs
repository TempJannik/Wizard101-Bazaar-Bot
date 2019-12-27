using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wizard101BazaarBot.ImageRecognition
{
    class Recognize
    {
        /*public static bool CanPressX()
        {
            if (Contains(Properties.Resources.xBtn))
                return true;
            return false;
        }

        public static bool IsFinished()
        {
            if (Contains(Properties.Resources.outofBattle))
                return true;
            return false;
        }

        public static bool IsInDungeon()
        {
            if (Contains(Properties.Resources.isInside))
                return true;
            return false;
        }*/

        public static bool Contains(Bitmap bitmapToSearchFor)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), bitmapToSearchFor.Size);
            Bitmap formattedImage = bitmapToSearchFor.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (FindBitmapsEntry(makeScreen(), formattedImage).Count > 0)
            {
                formattedImage.Dispose();
                return true;
            }
            else
            {
                formattedImage.Dispose();
                return false;
            }

        }

        public static List<Point> GetPositions(Bitmap bitmapToSearchFor)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), bitmapToSearchFor.Size);
            Bitmap formattedImage = bitmapToSearchFor.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            return FindBitmapsEntry(makeScreen(), formattedImage);
        }

        public static List<Point> FindBitmapsEntry(Bitmap sourceBitmap, Bitmap serchingBitmap)
        {
            #region Arguments check

            if (sourceBitmap == null || serchingBitmap == null)
                throw new ArgumentNullException();

            if (sourceBitmap.PixelFormat != serchingBitmap.PixelFormat)
                throw new ArgumentException("Pixel formats arn't equal");

            if (sourceBitmap.Width < serchingBitmap.Width || sourceBitmap.Height < serchingBitmap.Height)
                throw new ArgumentException("Size of serchingBitmap bigger then sourceBitmap");

            #endregion

            var pixelFormatSize = Image.GetPixelFormatSize(sourceBitmap.PixelFormat) / 8;


            // Copy sourceBitmap to byte array
            var sourceBitmapData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
            var sourceBitmapBytesLength = sourceBitmapData.Stride * sourceBitmap.Height;
            var sourceBytes = new byte[sourceBitmapBytesLength];
            Marshal.Copy(sourceBitmapData.Scan0, sourceBytes, 0, sourceBitmapBytesLength);
            sourceBitmap.UnlockBits(sourceBitmapData);

            // Copy serchingBitmap to byte array
            var serchingBitmapData =
                serchingBitmap.LockBits(new Rectangle(0, 0, serchingBitmap.Width, serchingBitmap.Height),
                    ImageLockMode.ReadOnly, serchingBitmap.PixelFormat);
            var serchingBitmapBytesLength = serchingBitmapData.Stride * serchingBitmap.Height;
            var serchingBytes = new byte[serchingBitmapBytesLength];
            Marshal.Copy(serchingBitmapData.Scan0, serchingBytes, 0, serchingBitmapBytesLength);
            serchingBitmap.UnlockBits(serchingBitmapData);

            var pointsList = new List<Point>();

            // Serching entries
            // minimazing serching zone
            // sourceBitmap.Height - serchingBitmap.Height + 1
            for (var mainY = 0; mainY < sourceBitmap.Height - serchingBitmap.Height + 1; mainY++)
            {
                var sourceY = mainY * sourceBitmapData.Stride;

                for (var mainX = 0; mainX < sourceBitmap.Width - serchingBitmap.Width + 1; mainX++)
                {// mainY & mainX - pixel coordinates of sourceBitmap
                 // sourceY + sourceX = pointer in array sourceBitmap bytes
                    var sourceX = mainX * pixelFormatSize;

                    var isEqual = true;
                    for (var c = 0; c < pixelFormatSize; c++)
                    {// through the bytes in pixel
                        if (sourceBytes[sourceX + sourceY + c] == serchingBytes[c])
                            continue;
                        isEqual = false;
                        break;
                    }

                    if (!isEqual) continue;

                    var isStop = false;

                    // find fist equalation and now we go deeper) 
                    for (var secY = 0; secY < serchingBitmap.Height; secY++)
                    {
                        var serchY = secY * serchingBitmapData.Stride;

                        var sourceSecY = (mainY + secY) * sourceBitmapData.Stride;

                        for (var secX = 0; secX < serchingBitmap.Width; secX++)
                        {// secX & secY - coordinates of serchingBitmap
                         // serchX + serchY = pointer in array serchingBitmap bytes

                            var serchX = secX * pixelFormatSize;

                            var sourceSecX = (mainX + secX) * pixelFormatSize;

                            for (var c = 0; c < pixelFormatSize; c++)
                            {// through the bytes in pixel
                                if (sourceBytes[sourceSecX + sourceSecY + c] == serchingBytes[serchX + serchY + c]) continue;

                                // not equal - abort iteration
                                isStop = true;
                                break;
                            }

                            if (isStop) break;
                        }

                        if (isStop) break;
                    }

                    if (!isStop)
                    {// serching bitmap is founded!!
                        pointsList.Add(new Point(mainX, mainY));
                    }
                }
            }

            return pointsList;
        }

        private static bool IsInnerImage(Bitmap searchBitmap, Bitmap withinBitmap, int left, int top) //Using x+=2 instead of x++ improves optimization at the cost of accuracy, this however shouldnt be problematic in our use case
        {
            for (int y = top; y < top + withinBitmap.Height; y++)
            {
                for (int x = left; x < left + withinBitmap.Width; x++)
                {
                    if (searchBitmap.GetPixel(x, y) != withinBitmap.GetPixel(x - left, y - top))
                        return false;
                }
            }

            return true;
        }

        public static bool FindBitmap(Bitmap searchBitmap, Bitmap withinBitmap, out Point point)
        {
            Color innerTopLeft = withinBitmap.GetPixel(0, 0);

            for (int y = 0; y < searchBitmap.Height - withinBitmap.Height; y++)
            {
                for (int x = 0; x < searchBitmap.Width - withinBitmap.Width; x++)
                {
                    Color clr = searchBitmap.GetPixel(x, y);
                    if (innerTopLeft == clr && IsInnerImage(searchBitmap, withinBitmap, x, y))
                    {
                        point = new Point(x, y); // Top left corner of the inner bitmap in searchBitmap - coordinates
                        return true;
                    }
                }
            }

            point = Point.Empty;
            searchBitmap.Dispose();
            withinBitmap.Dispose();
            return false;
        }

        public static Bitmap makeScreen2(bool limit = false)
        {
            if (!limit)
            {
                var bmpScreenshot = new Bitmap(105, 82,
                                           PixelFormat.Format24bppRgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(1200,
                                            740,
                                            0,
                                            0,
                                            new Size(new Point(105, 82)),
                                            CopyPixelOperation.SourceCopy);
                return bmpScreenshot;
            }
            else
            {
                var bmpScreenshot = new Bitmap(270,
                                           130,
                                           PixelFormat.Format24bppRgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(830,
                                            450,
                                            0,
                                            0,
                                            new Size(new Point(270, 130)),
                                            CopyPixelOperation.SourceCopy);
                return bmpScreenshot;
                //700, 400 , 1200, 650
            }

        }
        public static Bitmap makeScreen(bool limit = false)
        {
            if (!limit)
            {
                var bmpScreenshot = new Bitmap(1920,
                                           1080,
                                           PixelFormat.Format24bppRgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            new Size(new Point(1920, 1080)),
                                            CopyPixelOperation.SourceCopy);
                return bmpScreenshot;
            }
            else
            {
                var bmpScreenshot = new Bitmap(327,
                                           375,
                                           PixelFormat.Format24bppRgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(800,
                                            380,
                                            0,
                                            0,
                                            new Size(new Point(327, 375)),
                                            CopyPixelOperation.SourceCopy);
                return bmpScreenshot;
                //700, 400 , 1200, 650
            }

        }
    }
}
