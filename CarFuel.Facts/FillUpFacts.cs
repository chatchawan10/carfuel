using Carfuel.Models;
using CarFuel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CarFuel.Facts
{
    public class FillUpFacts
    {
        public class GeneralUsage : IDisposable
        {
            [Fact]
            public void Basic()
            {
                // Arrange
                var f1 = new FillUp();
                f1.Odometer = 1000;
                f1.IsFull = true;
                f1.Liters = 50.0;
                // Act
                // Assert
                Assert.Equal(1000, f1.Odometer);
                Assert.Equal(true, f1.IsFull);
                Assert.Equal(50.0, f1.Liters);
                
            }
            
            [Fact]
            public void DefaultDateShouldBeNow()
            {                
                // Arrange
                var now = DateTime.Now;
                SystemTime.SetDateTime(now);

                var f = new FillUp();
                f.Odometer = 1000;
                f.IsFull = true;
                f.Liters = 50.0;

                // Act
                DateTime dt = f.Date;
                // Assert
                Assert.Equal(expected: now, actual: dt);
            }

            public void Dispose()
            {
                SystemTime.ResetDateTime();
            }
        }

        public class KilometerPerLiterProperty
        {
            private FillUp f1, f2, f3;

            // Test Setup (Constructor is test setup in xUnit.net)
            public KilometerPerLiterProperty()
            {
                f1 = new FillUp();
                f2 = new FillUp();
                f3 = new FillUp();             
            }
            [Fact]
            public void FirstFillUp_HasNoKmL()
            {
                // Arrange
                f1.Odometer = 1000;
                f1.IsFull = true;
                f1.Liters = 50.0;

                double? kml = f1.KilometerPerLiter;

                Assert.Null(kml);                             
            }

            [Fact]
            public void SecondFillUp_HasNoKmL()
            {               
                // Arrange
                f1.Odometer = 1000;
                f1.Liters = 50.0;
                f1.IsFull = true;

                f2.Odometer = 1500;
                f2.Liters = 40.0;
                f2.IsFull = true;

                f1.NextFillUp = f2;

                double? kml = f1.KilometerPerLiter;

                Assert.Equal(12.5, kml);

            }

            public void ThirdFillUp_HasNoKmL()
            {
                // Arrange
                f1.Odometer = 1000;
                f1.IsFull = true;
                f1.Liters = 50.0;

                f2.Odometer = 1500;
                f2.IsFull = true;
                f2.Liters = 40.0;

                f3.Odometer = 2100;
                f3.IsFull = true;
                f3.Liters = 50.0;

                f1.NextFillUp = f2;
                f2.NextFillUp = f3;

                double? kml = f2.KilometerPerLiter;

                Assert.Equal(12, kml);
            }
        }

        public class NextFillUpProperty
        {
            [Fact]
            public void NextFillUpOdometer_ShouldNolessThanPreviousFillUp()
            {
                var f1 = new FillUp();
                f1.Odometer = 1000;

                var f2 = new FillUp();
                f2.Odometer = 500;

                Assert.ThrowsAny<Exception>(() =>
                {                    
                    f1.NextFillUp = f2;
                });
            }
        }

        public class KilometerPerLiterProperty_Theory
        {
            private ITestOutputHelper output; //to write output in log of testing
            public KilometerPerLiterProperty_Theory(ITestOutputHelper output)
            {
                this.output = output;
            }

            // make 3 test cases input in one function
            [Theory]
            [InlineData(1000, 50, 1500, 40, 12.5)]
            [InlineData(1500, 40, 2100, 50, 12.0)]
            [InlineData(2100, 50, 2800, 70, 10.0)]
            public void GeneralCases(int odo1, double liters1, 
                                     int odo2, double liters2,
                                    double kml)
            {
                var f1 = new FillUp();
                f1.Odometer = 1000;
                f1.Liters = 50.0;

                var f2 = new FillUp();
                f2.Odometer = 1500;
                f2.Liters = 40.0;

                f1.NextFillUp = f2;

                output.WriteLine("1st: {0} {1:0.0} liters", odo1, liters1);
                output.WriteLine("2nd: {0} {1:0.0} liters", odo2, liters2);
                output.WriteLine("Kilometer/Liter = {0:n2}", f1.KilometerPerLiter);

                Assert.Equal(kml, f1.KilometerPerLiter);
            }
        }
    }
}
