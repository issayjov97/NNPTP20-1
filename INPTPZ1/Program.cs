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

            ArgsAdapter.initArgs(args);
            FractalViewer fractalViewer = new FractalViewer();
            fractalViewer.DrawImage();
            Console.ReadKey();
        }
    }

    namespace Mathematics
    {

        class ArgsAdapter
        {
            private static int[] intargs;
            private static double[] doubleargs;
            private static String outputPath;

            public static void initArgs(string[] args)
            {
                intargs = SetIntArgs(args);
                doubleargs = SetDoubleArgs(args);
                outputPath = SetOutputPath(args);
            }


            private static String SetOutputPath(string[] args)
            {
                return args[6];
            }

            private static int[] SetIntArgs(string[] args)
            {
                intargs = new int[2];
                for (int i = 0; i < intargs.Length; i++)
                {
                    intargs[i] = int.Parse(args[i]);
                }

                return intargs;
            }

            private static double[] SetDoubleArgs(string[] args)
            {
                doubleargs = new double[4];
                for (int i = 0; i < doubleargs.Length; i++)
                {
                    doubleargs[i] = double.Parse(args[i + 2]);
                }
                return doubleargs;
            }


            public static String GetOutputPath()
            {
                return outputPath;
            }
            public static int GetIntArgs(int index)
            {
                return intargs[index];
            }
            public static double GetDoubleArgs(int index)
            {
                return doubleargs[index];
            }

        }

        class FractalViewer
        {
            private Color[] colors;
            private Bitmap bitmap;
            private NewtonFractal newtonFactorial;

            public FractalViewer()
            {
                bitmap = new Bitmap(ArgsAdapter.GetIntArgs(0), ArgsAdapter.GetIntArgs(1));
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
                bitmap.Save(ArgsAdapter.GetOutputPath() ?? "../../../out.png");
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
                xmin = ArgsAdapter.GetDoubleArgs(0);
                xmax = ArgsAdapter.GetDoubleArgs(1);
                ymin = ArgsAdapter.GetDoubleArgs(2);
                ymax = ArgsAdapter.GetDoubleArgs(3);
                xstep = (xmax - xmin) / ArgsAdapter.GetIntArgs(0);
                ystep = (ymax - ymin) / ArgsAdapter.GetIntArgs(1);
                polynomial = getPolynomial();
                derivativePolynomial = polynomial.Derive();

            }


            private Polynomial getPolynomial()
            {
                Polynomial polynomial = new Polynomial();
                polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
                polynomial.Coefficients.Add(ComplexNumber.Zero);
                polynomial.Coefficients.Add(ComplexNumber.Zero);
                polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
                return polynomial;

            }
            public ComplexNumber SetComplexNumber(int i, int j)
            {
                ComplexNumber ox = new ComplexNumber()
                {
                    Re = xmin + j * xstep,
                    Imaginari = ymin + i * ystep
                };

                if (ox.Re == 0)
                    ox.Re = 0.0001;
                if (ox.Imaginari == 0)
                    ox.Imaginari = 0.0001;

                return ox;

            }
            public int FindRootNumber()
            {
                var known = false;
                var id = 0;
                for (int i = 0; i < roots.Count; i++)
                {
                    if (Math.Pow(ox.Re - roots[i].Re, 2) + Math.Pow(ox.Imaginari - roots[i].Imaginari, 2) <= 0.01)
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

                    if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Imaginari, 2) >= 0.5)
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
            public double Imaginari { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is ComplexNumber)
                {
                    ComplexNumber complexNumber = obj as ComplexNumber;
                    return complexNumber.Re == Re && complexNumber.Imaginari == Imaginari;
                }
                return base.Equals(obj);
            }

            public readonly static ComplexNumber Zero = new ComplexNumber()
            {
                Re = 0,
                Imaginari = 0
            };

            public ComplexNumber Multiply(ComplexNumber multiplicand)
            {
                return new ComplexNumber()
                {
                    Re = this.Re * multiplicand.Re - this.Imaginari * multiplicand.Imaginari,
                    Imaginari = this.Re * multiplicand.Imaginari + this.Imaginari * multiplicand.Re
                };
            }
            public double GetAbS()
            {
                return Math.Sqrt(Re * Re + Imaginari * Imaginari);
            }

            public ComplexNumber Add(ComplexNumber summand)
            {
                return new ComplexNumber()
                {
                    Re = this.Re + summand.Re,
                    Imaginari = this.Imaginari + summand.Imaginari
                };
            }
            public double GetAngleInDegrees()
            {
                return Math.Atan(Imaginari / Re);
            }
            public ComplexNumber Subtract(ComplexNumber subtrahend)
            {
                return new ComplexNumber()
                {
                    Re = this.Re - subtrahend.Re,
                    Imaginari = this.Imaginari - subtrahend.Imaginari
                };
            }

            public override string ToString()
            {
                return $"({Re} + {Imaginari}i)";
            }

            internal ComplexNumber Divide(ComplexNumber divisor)
            {
                var tmp = this.Multiply(new ComplexNumber() { Re = divisor.Re, Imaginari = -divisor.Imaginari });
                var tmp2 = divisor.Re * divisor.Re + divisor.Imaginari * divisor.Imaginari;

                return new ComplexNumber()
                {
                    Re = tmp.Re / tmp2,
                    Imaginari = tmp.Imaginari / tmp2
                };
            }
        }
    }
}
