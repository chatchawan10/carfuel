using CarFuel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carfuel.Models
{
    public class FillUp
    {
        public int Odometer { get; set; }

        public bool IsFull { get; set; }

        public double Liters { get; set; }

        public DateTime Date { get; set; }

        private FillUp _NextFillUp;

        public FillUp NextFillUp
        {
            get { return _NextFillUp; }
            set
            {
                if (value.Odometer < this.Odometer)
                {
                    throw new ArgumentException("Invalid odometer value", "value");
                }
                _NextFillUp = value;
            }
        }
        

        public FillUp()
        {
            this.Date = SystemTime.Now();
        }       

        public double? KilometerPerLiter
        {
            get
            {
                if (this.NextFillUp == null)
                {
                    return null;
                }

                return (this.NextFillUp.Odometer - this.Odometer)
                       / this.NextFillUp.Liters;
            }
        }

    }
}
