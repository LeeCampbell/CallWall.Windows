using System.IO.Packaging;
using System.Windows;
using CallWall.Shell.Images;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//TODO: Incomplete because these test are just proving too low value and too high cost. -LC

// ReSharper disable InconsistentNaming
namespace CallWall.Core.IntegrationTests
{
    public abstract class Given_a_GrayscaleConverter
    {
        private Given_a_GrayscaleConverter()
        { }

        private GrayscaleConverter _sut;

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new GrayscaleConverter();
        }

        [TestFixture]
        public sealed class When_converting_a_Bgra32_BitmapSource : Given_a_GrayscaleConverter
        {
            const int Depth = 4; //ie 4 byte values per pixel r,g,b & a

            [Test]
            public void Full_color_spectrum_test([Range(0, 1024)]int loop)
            {
                var width = 1024;
                var height = 1024;
                var dpi = 96;

                uint indexOffset = (uint)loop * ((uint)width * (uint)height);

                BitmapSource source, actual;
                source = CreateBitmap(width, height, dpi, dpi, indexOffset);
                actual = (BitmapSource)_sut.Convert(source, typeof(BitmapSource), null, CultureInfo.InvariantCulture);

                Assert.AreEqual(width, actual.Width);
                Assert.AreEqual(height, actual.Height);
                Assert.AreEqual(dpi, actual.DpiX);
                Assert.AreEqual(dpi, actual.DpiY);
            }

            private BitmapSource CreateBitmap(int width, int height, int dpiX, int dpiY, uint indexOffset = 0)
            {
                //now map to each pixel from 0,0 to width,height
                uint maxlength;
                checked
                {
                    maxlength = (uint)width * (uint)height;
                }

                var imageData = CreatePixelData(maxlength, indexOffset);

                var stride = width * Depth;
                return BitmapSource.Create(width, height, dpiX, dpiY, PixelFormats.Bgra32, null, imageData, stride);
            }

            //Unsafe for raw speed. If you can see a faster way make it so. -LC
            private static unsafe byte[] CreatePixelData(uint maxlength, uint indexOffset)
            {
                var arraySize = maxlength * Depth;
                byte[] imageData = new byte[arraySize];
                fixed (byte* pImageData = imageData)
                {
                    byte* ptr = pImageData + 0;

                    var limit = maxlength + indexOffset;
                    for (var pixelIdx = indexOffset; pixelIdx < limit; pixelIdx++)
                    {
                        *ptr = (byte)(pixelIdx / 16581375);//Red
                        ptr++;
                        *ptr = (byte)((pixelIdx / 65025) % 255);//Green
                        ptr++;
                        *ptr = (byte)((pixelIdx / 255) % 255);//Blue;
                        ptr++;
                        *ptr = (byte)(pixelIdx % 255);//Alpha;
                        ptr++;
                    }
                }
                return imageData;
            }
        }

        //[TestFixture]
        //[RequiresSTA] 
        //public sealed class When_converting_known_images : Given_a_GrayscaleConverter
        //{
        //    public override void SetUp()
        //    {
        //        base.SetUp();
        //        //Magic to get PackUris resolved for the convert tests.
        //        PackUriHelper.Create(new Uri("reliable://0"));
        //        new FrameworkElement();
        //        Application.ResourceAssembly = typeof(App).Assembly;
        //    }

        //    [Test]
        //    public void Should_convert_bluetooth_icon_to_expected_grayscale()
        //    {
        //        Uri uri = BluetoothImages.BluetoothIconUri;
        //        var actual = _sut.Convert(uri, typeof(BitmapSource), null, CultureInfo.InvariantCulture);
        //        Assert.IsInstanceOf<BitmapSource>(actual);
        //    }
        //}

        //TODO: Test various dimensions. Should really test for CallWall values up to 512x512 (the biggest Icon)

        //TODO: Test various dpi. Probably should not just hard code 96x96
        //TODO: Test various pixel depths e.g. 7bit (127color) 8bit (256color) 24bit(RBG) 32bit(aRGB)... Most are covered by PixelFormats below.
        /*TODO: Test each of the PixelFormats
            Default: for situations when the pixel format may not be important
            Indexed1: Paletted image with 2 colors. 
            Indexed2: Paletted image with 4 colors. 
            Indexed4: Paletted image with 16 colors.
            Indexed8: Paletted image with 256 colors.
            BlackWhite: Monochrome, 2-color image, black and white only.
            Gray2: Image with 4 shades of gray
            Gray4: Image with 16 shades of gray 
            Gray8: Image with 256 shades of gray 
            Bgr555: 16 bpp SRGB format
            Bgr565: 16 bpp SRGB format 
            Rgb128Float: 128 bpp extended format; Gamma is 1.0 
            Bgr24: 24 bpp SRGB format
            Rgb24: 24 bpp SRGB format
            Bgr101010: 32 bpp SRGB format
            Bgr32: 32 bpp SRGB format
            Bgra32: 32 bpp SRGB format 
            Pbgra32: 32 bpp SRGB format 
            Rgb48: 48 bpp extended format; Gamma is 1.0
            Rgba64: 64 bpp extended format; Gamma is 1.0 
            Prgba64: 64 bpp extended format; Gamma is 1.0 
            Gray16: 16 bpp Gray-scale format; Gamma is 1.0
            Gray32Float: 32 bpp Gray-scale format; Gamma is 1.0
            Rgba128Float: 128 bpp extended format; Gamma is 1.0
            Prgba128Float: 128 bpp extended format; Gamma is 1.0
            Cmyk32: 32 bpp format 
         */

        //TODO: Test each of the BitmapPalettes
        /*  BlackAndWhite
            BlackAndWhiteTransparent
            Halftone8
            Halftone8Transparent
            Halftone27
            Halftone27Transparent
            Halftone64
            Halftone64Transparent
            Halftone125
            Halftone125Transparent
            Halftone216
            Halftone216Transparent
            Halftone252
            Halftone252Transparent
            Halftone256
            Halftone256Transparent
            Gray4
            Gray4Transparent
            Gray16
            Gray16Transparent
            Gray256
            Gray256Transparent
            WebPalette
            WebPaletteTransparent
         */
    }
}
// ReSharper restore InconsistentNaming