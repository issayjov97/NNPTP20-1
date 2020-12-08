using System.Collections.Generic;
using Mathematics.ComplexNumber;


namespace Mathematics
{
    class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }

        public Polynomial() => Coefficients = new List<ComplexNumber>();

        public Polynomial Derive()
        {
            Polynomial polynom = new Polynomial();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                polynom.Coefficients.Add(Coefficients[i].Multiply(new ComplexNumber() { Re = i }));
            }

            return polynom;
        }

        public ComplexNumber Evaluate(ComplexNumber value)
        {
            ComplexNumber result = ComplexNumber.Zero;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                ComplexNumber coef = Coefficients[i];
                ComplexNumber bx = value;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(value);

                    coef = coef.Multiply(bx);
                }

                result = result.Add(coef);
            }

            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                result += Coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        result += "x";
                    }
                }
                result += " + ";
            }
            return result;
        }
    }
}