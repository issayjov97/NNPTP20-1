using System;

namespace Mathematics
{
    public class ComplexNumber
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public override bool Equals(object obj)
        {
            var double epsilon = 0.0001;
            if (obj is ComplexNumber)
            {
                ComplexNumber complexNumber = obj as ComplexNumber;
                return (Math.abs(complexNumber.Re - Re) < epsilon) 
                && (Math.abs(complexNumber.Im - Im) < epsilon);
            }
            return base.Equals(obj);
        }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            Re = 0.0,
            Im = 0.0
        };

        public ComplexNumber Multiply(ComplexNumber multiplicand)
        {
            return new ComplexNumber()
            {
                Re = this.Re * multiplicand.Re - this.Im * multiplicand.Im,
                Im = this.Re * multiplicand.Im + this.Im * multiplicand.Re
            };
        }
        public double GetAbS()
        {
            return Math.Sqrt(Re * Re + Im * Im);
        }

        public ComplexNumber Add(ComplexNumber summand)
        {
            return new ComplexNumber()
            {
                Re = this.Re + summand.Re,
                Im = this.Im + summand.Im
            };
        }
        public double GetAngleInDegrees()
        {

            return Math.Atan(Im / Re) * (180/Math.PI);
        }
        public ComplexNumber Subtract(ComplexNumber subtrahend)
        {
            return new ComplexNumber()
            {
                Re = this.Re - subtrahend.Re,
                Im = this.Im - subtrahend.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        internal ComplexNumber Divide(ComplexNumber divisor)
        {
            var numerator = this.Multiply(new ComplexNumber() { Re = divisor.Re, Im = -divisor.Im });
            var denominator = divisor.Re * divisor.Re + divisor.Im * divisor.Im;

            return new ComplexNumber()
            {
                Re = numerator.Re / denominator,
                Im = numerator.Im / denominator
            };
        }
    }
}