using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Moq;
using NUnit.Framework;

namespace CallWall.Core.UnitTests
{
    public abstract class Given_a_GrayscaleConverter
    {
        private Given_a_GrayscaleConverter()
        {}

        private GrayscaleConverter _sut;

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new GrayscaleConverter();
        }


        [TestFixture]
        public sealed class When_converting_a_BitmapSource : Given_a_GrayscaleConverter
        {
            private Mock<BitmapSource> _bitmapSourceMock;

            public override void SetUp()
            {
                base.SetUp();
                _bitmapSourceMock = new Mock<BitmapSource>();
            }


        }
        //TODO: Test various dimensions. Should really test for CallWall values up to 512x512 (the biggest Icon)

        //TODO: Test various dpi. Probably should not just hard code 96x96

        /*TODO: Test each of the c
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
    }
}
