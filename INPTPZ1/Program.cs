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

        public static int[] GetIntArgs(string[] args)
        {
            int[] intargs = new int[2];
            for (int i = 0; i < intargs.Length; i++)
            {
                intargs[i] = int.Parse(args[i]);
            }

            return intargs;
        }

        public static double[] SetDoubleArgs(string[] args)
        {
             double[] doubleargs = new double[4];
            for (int i = 0; i < doubleargs.Length; i++)
            {   
                doubleargs[i] = double.Parse(args[i + 2]);
            }
            return doubleargs;
        }

        public static void DisplayPolynomials(Polynomial polynomial, Polynomial derivativePolynomial)
        {
            Console.WriteLine(polynomial);
            Console.WriteLine(derivativePolynomial);
        }

        private static Polynomial getPolynomial()
        {
            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            return polynomial;

        }
        static void Main(string[] args)
        {
            int[] intargs = GetIntArgs(args);
            double[] doubleargs = GetDoubleArgs(args);
            string output = args[6];
            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(intargs[0], intargs[1]);
            double xmin = doubleargs[0];
            double xmax = doubleargs[1];
            double ymin = doubleargs[2];
            double ymax = doubleargs[3];

            double xstep = (xmax - xmin) / intargs[0];
            double ystep = (ymax - ymin) / intargs[1];

          
            Polynomial polynomial = getPolynomial();
            Polynomial derivativePolynomial = polynomial.Derive();

            DisplayPolynomials(polynomial, derivativePolynomial);

            var clrs = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var maxid = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < intargs[0]; i++)
            {
                for (int j = 0; j < intargs[1]; j++)
                {
                    // find "world" coordinates of pixel
                    double y = ymin + i * ystep;
                    double x = xmin + j * xstep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Re = x,
                        Imaginari = (float)(y)
                    };

                    if (ox.Re == 0)
                        ox.Re = 0.0001;
                    if (ox.Imaginari == 0)
                        ox.Imaginari = 0.0001;

                    //Console.WriteLine(ox);

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q< 30; q++)
                    {
                        var diff = polynomial.Evaluate(ox).Divide(derivativePolynomial.Evaluate(ox));
                        ox = ox.Subtract(diff);

                        //Console.WriteLine($"{q} {ox} -({diff})");
                        if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Imaginari, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    //Console.ReadKey();

                    // find solution root number
                    var known = false;
                    var id = 0;
                    for (int w = 0; w <koreny.Count;w++)
                    {
                        if (Math.Pow(ox.Re- koreny[w].Re, 2) + Math.Pow(ox.Imaginari - koreny[w].Imaginari, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(ox);
                        id = koreny.Count;
                        maxid = id + 1; 
                    }

                    // colorize pixel according to root number
                    //int vv = id;
                    //int vv = id * 50 + (int)it*5;
                    var vv = clrs[id % clrs.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R-(int)it*2), 255), Math.Min(Math.Max(0, vv.G - (int)it*2), 255), Math.Min(Math.Max(0, vv.B - (int)it*2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    bmp.SetPixel(j, i, vv);
                    //bmp.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }

                    bmp.Save(output ?? "../../../out.png");
            Console.ReadKey();
        }
    }

    namespace Mathematics
    {
        class NewtonFactorial
        {

            List<ComplexNumber> koreny;
            ComplexNumber ox;

            public NewtonFactorial()
            {
                koreny = new List<ComplexNumber>();
                ox = new ComplexNumber()
                {
                    Re = x,
                    Imaginari = y
                };

            }

            private void SetValues()
            {
                double[] doubleargs = Program.GetDoubleArgs();
            //    xmin = Program doubleargs[0];
                xmax = doubleargs[1];
                ymin = doubleargs[2];
                ymax = doubleargs[3];

            }

            public void SolveRoot()
            {
                var known = false;
                var id = 0;
                for (int w = 0; w < koreny.Count; w++)
                {
                    if (Math.Pow(ox.Re - koreny[w].Re, 2) + Math.Pow(ox.Imaginari - koreny[w].Imaginari, 2) <= 0.01)
                    {
                        known = true;
                        id = w;
                    }
                }
                if (!known)
                {
                    koreny.Add(ox);
                    id = koreny.Count;
                    maxid = id + 1;
                }

            }

            private void SolveEquationUsingNewtonsIteration()
            {
                float it = 0;
                for (int q = 0; q < 30; q++)
                {
                    var diff = polynomial.Evaluate(ox).Divide(derivativePolynomial.Evaluate(ox));
                    ox = ox.Subtract(diff);

                    //Console.WriteLine($"{q} {ox} -({diff})");
                    if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Imaginari, 2) >= 0.5)
                    {
                        q--;
                    }
                    it++;
                }
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
                return Math.Sqrt( Re * Re + Imaginari * Imaginari);
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
