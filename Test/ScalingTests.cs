using FluentAssertions;
using System.Xml.Linq;
using WinForms;
using WinForms.Controllers;

namespace Test
{
    public class ScalingTests
    {
        [Theory]
        [InlineData(-4, -5, 5, 4, 6, 5, 7, 5)]
        [InlineData(-4, -5, 5, 4, 6, -5, -7, 5)]
        [InlineData(4, 5, 5, 4, 6, 55, 7, 5)]
        [InlineData(-4, -5, 5, 4, -786, 5, 7, 5)]
        [InlineData(-4, -5, 5, 54, 6, 85, 7, 5)]
        [InlineData(-4, -5, -5, -4, -6, -5, -7, -5)]
        void ScaleReduceMinusNumbers_ForInputPoints_ReduceMinusValues(
            double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy
            )
        {
            //act
            var a = new MyPoint(ax, ay);
            var b = new MyPoint(bx, by);
            var c = new MyPoint(cx, cy);
            var d = new MyPoint(dx, dy);

            //arrange
            var points = new ScaleController(0.98f, 30).ScaleReduceMinusNumbers(a, b, c, d);

            //assert
            foreach (var point in points)
            {
                point.X.Should().BeGreaterOrEqualTo(0);
                point.Y.Should().BeGreaterOrEqualTo(0);
            }
        }

        [Theory]
        //[InlineData(4, 5, 5, 4, 6, 5, 7, 5, 2, 4)]
        //[InlineData(4, 5, 5, 4, 6, 5, 7, 5, 4, 2)]
        //[InlineData(4, 5, 5, 4, 6, 55, 7, 5, 111, 222)]
        [InlineData(4, 5, 5, 4, 786,555, 7, 5, 255, 22)]
        //[InlineData(4, 5, 5, 54, 6, 85, 7, 5, 111, 666)]
        //[InlineData(4, 5, 5, 4, 6, 5, 7, 5, 66, 2)]
        void ScaleDicreasePoints_ForInputData_MakeXandYLowerThanWandH(double ax, double ay, double bx, double by,
            double cx, double cy, double dx, double dy, double h, double w)
        {
            //act
            var a = new MyPoint(ax, ay);
            var b = new MyPoint(bx, by);
            var c = new MyPoint(cx, cy);
            var d = new MyPoint(dx, dy);

            //arrange
            var points = new ScaleController(0.98f, 30).ScaleDicreasePoints(a, b, c, d, w, h);

            //assert

            foreach (var point in points)
            {
                point.X.Should().BeLessThan(w);
                point.Y.Should().BeLessThan(h);
            }
        }

        [Theory]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 2, 4)]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 4, 2)]
        [InlineData(4, 5, 5, 4, 6, 55, 7, 5, 111, 222)]
        [InlineData(4, 5, 5, 4, 786, 5, 7, 5, 255, 22)]
        [InlineData(4, 5, 5, 54, 6, 85, 7, 5, 111, 666)]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 66, 2)]
        void ScaleIncreasePoints_ForInputData_MakeMaxXandYCloserToWandHIfAreMuchBelow(double ax, double ay, double bx, double by,
            double cx, double cy, double dx, double dy, double h, double w)
        {
            //act
            var a = new MyPoint(ax, ay);
            var b = new MyPoint(bx, by);
            var c = new MyPoint(cx, cy);
            var d = new MyPoint(dx, dy);

            //arrange
            var points = new ScaleController(0.98f, 30).ScaleIncreasePoints(a, b, c, d, w, h);

            var maxX = 0.0;
            var maxY = 0.0;
            foreach (var point in points)
            {
                maxX = point.X > maxX ? point.X : maxX;
                maxY = point.Y > maxY ? point.Y : maxY;
            }

            //assert
            if ((maxX > w * 0.98) || (maxY > h * 0.98))
                Assert.True(true);
            else
                Assert.True(false);
        }


        /// <summary>
        /// Chodzi o to żeby te nasze A,B,C,D nie byly przekazywane przez referencje, nie mozna zmieniac ich wartosci...
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="h"></param>
        /// <param name="w"></param>
        [Theory]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 2, 4)]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 4, 2)]
        [InlineData(4, 5, 5, 4, 6, 55, 7, 5, 111, 222)]
        [InlineData(4, 5, 5, 4, 786, 5, 7, 5, 255, 22)]
        [InlineData(4, 5, 5, 54, 6, 85, 7, 5, 111, 666)]
        [InlineData(4, 5, 5, 4, 6, 5, 7, 5, 66, 2)]
        void ComplexScale_ForInputData_UseCopyNotReference(double ax, double ay, double bx, double by,
            double cx, double cy, double dx, double dy, double h, double w)
        {
            //act
            var a = new MyPoint(ax, ay);
            var b = new MyPoint(bx, by);
            var c = new MyPoint(cx, cy);
            var d = new MyPoint(dx, dy);

            //assert
            var res = new ScaleController(0.98f, 30).ComplexScale(a, b, c, d, w, h);

            //arrange
            a.X.Should().Be(ax);
            a.Y.Should().Be(ay);
            b.X.Should().Be(bx);
            b.Y.Should().Be(by);
            c.X.Should().Be(cx);
            c.Y.Should().Be(cy);
            d.X.Should().Be(dx);
            d.Y.Should().Be(dy);
        }
    }
}
