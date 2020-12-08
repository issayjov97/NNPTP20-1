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
        private ComplexNumber ox;
        private const int ITERATIONSNUMBER = 30;

        public NewtonFractal()
        {
            roots = new List<ComplexNumber>();
            xmin = CommandLineParameters.GetAxesCoordinates(0);
            xmax = CommandLineParameters.GetAxesCoordinates(1);
            ymin = CommandLineParameters.GetAxesCoordinates(2);
            ymin = CommandLineParameters.GetAxesCoordinates(3);
            xstep = (xmax - xmin) / CommandLineParameters.GetGeometricMeasurements(0);
            ystep = (ymax - ymin) / CommandLineParameters.GetGeometricMeasurements(1);
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
            ComplexNumber ox = new ComplexNumber()
            {
                Re = xmin + x * xstep,
                Im = ymin + y * ystep
            };

            if (ox.Re == 0)
                ox.Re = 0.0001;
            if (ox.Im == 0)
                ox.Im = 0.0001;

            return ox;

        }
        public int FindRootNumber()
        {
            var known = false;
            var id = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(ox.Re - roots[i].Re, 2) + Math.Pow(ox.Im - roots[i].Im, 2) <= 0.01)
                {
                    known = true;
                    id = i;
                }
            }
            if (!known)
            {
                roots.Add(ox);
                id = roots.Count;
            }

            return id;
        }

        public int SolveEquationUsingNewtonsIteration()
        {
            int iterationCounter = 0;
            for (int i = 0; i < ITERATIONSNUMBER; i++)
            {
                var diff = polynomial.Evaluate(ox).Divide(derivativePolynomial.Evaluate(ox));
                ox = ox.Subtract(diff);

                if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                {
                    i--;
                }
                iterationCounter++;
            }

            return iterationCounter;
        }


    }

}