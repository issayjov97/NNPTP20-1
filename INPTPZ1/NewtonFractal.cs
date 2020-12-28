using System;
using System.Collections.Generic;
using Mathematics.ComplexNumber;
using Mathematics.Polynomial;



namespace Mathematics
{
    class NewtonFractal
    {

        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private double xstep;
        private double ystep;
        private Polynomial polynomial;
        private Polynomial derivativePolynomial;
        private List<ComplexNumber> roots;
        private ComplexNumber complexNumber;
        private const int ITERATIONSNUMBER = 30;

        public NewtonFractal()
        {
            roots = new List<ComplexNumber>();
            xmin = CommandLineInputHandler.GetAxesCoordinates(0);
            xmax = CommandLineInputHandler.GetAxesCoordinates(1);
            ymin = CommandLineInputHandler.GetAxesCoordinates(2);
            ymin = CommandLineInputHandler.GetAxesCoordinates(3);
            xstep = (xmax - xmin) / CommandLineInputHandler.GetGeometricMeasurements(0);
            ystep = (ymax - ymin) / CommandLineInputHandler.GetGeometricMeasurements(1);
            polynomial = GetPolynomial();
            derivativePolynomial = polynomial.Derive();

        }


        private Polynomial GetPolynomial()
        {
            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            return polynomial;

        }
        public ComplexNumber SetComplexNumber(int y, int x)
        {
             complexNumber = new ComplexNumber()
            {
                Re = xmin + x * xstep,
                Im = ymin + y * ystep
            };

            if (complexNumber.Re == 0)
                complexNumber.Re = 0.0001;
            if (complexNumber.Im == 0)
                complexNumber.Im = 0.0001;

            return complexNumber;

        }
        public int FindRootNumber()
        {
            var known = false;
            var id = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(complexNumber.Re - roots[i].Re, 2) + Math.Pow(complexNumber.Im - roots[i].Im, 2) <= 0.01)
                {
                    known = true;
                    id = i;
                }
            }
            if (!known)
            {
                roots.Add(complexNumber);
                id = roots.Count;
            }

            return id;
        }

        public int SolveEquationUsingNewtonsIteration()
        {
            int iterationCounter = 0;
            for (int i = 0; i < ITERATIONSNUMBER; i++)
            {
                var difference = polynomial.Evaluate(complexNumber).Divide(derivativePolynomial.Evaluate(complexNumber));
                complexNumber = complexNumber.Subtract(difference);

                if (Math.Pow(difference.Re, 2) + Math.Pow(difference.Im, 2) >= 0.5)
                {
                    i--;
                }
                iterationCounter++;
            }

            return iterationCounter;
        }


    }

}