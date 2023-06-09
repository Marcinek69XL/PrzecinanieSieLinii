using FluentAssertions;
using WinForms;

namespace Test
{
    public class CheckInsersectionTest
    {
        [Theory]
        [InlineData(1, 1, 4, 4, 1, 4, 4, 1, true)]   // Przecinaj¹ce siê proste
        [InlineData(1, 1, 3, 3, 4, 4, 6, 6, false)]  // Nieprzecinaj¹ce siê proste
        [InlineData(1, 1, 2, 2, 2, 2, 3, 3, false)]  // Wspó³liniowe proste
        void CheckIntersection_ShouldReturnCorrectResult(
            double x1, double y1, double x2, double y2,
            double x3, double y3, double x4, double y4,
            bool expectedResult)
        {
            //act
            var a = new MyPoint(x1, y1);
            var b = new MyPoint(x2, y2);
            var c = new MyPoint(x3, y3);
            var d = new MyPoint(x4, y4);

            //arrange
            var res = new InsertionContoller().CzySiePrzecinaja(a, b, c, d);

            //assert
            res.Should().Be(expectedResult);
        }
    }
}