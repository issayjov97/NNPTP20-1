using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Threading;
using INPTPZ1.Mathematics;

namespace INPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {

            CommandLineParameters.SetCommandLineParameters(args);
            FractalViewer fractalViewer = new FractalViewer();
            fractalViewer.DrawImage();
            Console.ReadKey();
        }
    }

    namespace Mathematics
    {

        class CommandLineParameters
        {
            private static int[] geometricMeasurements;
            private static double[] axesCoordinates;
            private static String outputPath;
            private static string[] commandLineParameters;

            public static void SetCommandLineParameters(string[] inputArgs){
                commandLineParameters = inputArgs;
            }

            public static String GetOutputPath()
            {
                
                return outputPath == null ? commandLineParameters[6]:outputPath ;
            }
            public static int GetGeometricMeasurements(int index)
            {
                if(geometricMeasurements == null){
                    for (int i = 0; i < geometricMeasurements.Length; i++)
                    {
                    geometricMeasurements[i] = int.Parse(commandLineParameters[i]);
                 }
                }
                return geometricMeasurements[index];
            }
            public static double GetAxesCoordinates(int index)
            {
                if(axesCoordinates == null){
                axesCoordinates = new double[4];
                for (int i = 0; i < axesCoordinates.Length; i++)
                {
                    axesCoordinates[i] = double.Parse(commandLineParameters[i + 2]);
                }
                }
                return axesCoordinates[index];
            }

        }

        class FractalViewer
        {
            private Color[] colors;
            private Bitmap bitmap;
            private NewtonFractal newtonFactorial;

            public FractalViewer()
            {
                bitmap = new Bitmap(CommandLineParameters.GetGeometricMeasurements(0), CommandLineParameters.GetGeometricMeasurements(1));
                newtonFactorial = new NewtonFractal();
                colors = new Color[]{
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange,
                Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta };
            }

            public void DrawImage()
            {

                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        newtonFactorial.SetComplexNumber(i, j);
                        var id = newtonFactorial.FindRootNumber();
                        var it = newtonFactorial.SolveEquationUsingNewtonsIteration();
                        var color = colors[id % colors.Length];
                        color = Color.FromArgb(color.R, color.G, color.B);
                        color = Color.FromArgb(Math.Min(Math.Max(0, color.R - it * 2), 255), Math.Min(Math.Max(0, color.G - it * 2), 255), Math.Min(Math.Max(0, color.B - it * 2), 255));
                        bitmap.SetPixel(j, i, color);
                    }
                }
                bitmap.Save(CommandLineParameters.GetOutputPath() ?? "../../../out.png");
            }


        }
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

        public class ComplexNumber
        {
            public double Re { get; set; }
            public double Im { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is ComplexNumber)
                {
                    ComplexNumber complexNumber = obj as ComplexNumber;
                    return complexNumber.Re == Re && complexNumber.Im == Im;
                }
                return base.Equals(obj);
            }

            public readonly static ComplexNumber Zero = new ComplexNumber()
            {
                Re = 0,
                Im = 0
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
                return Math.Atan(Im / Re);
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
}
