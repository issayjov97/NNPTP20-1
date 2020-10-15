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
            // TODO: add parameters from args?
            FractalViewer fractalViewer = new FractalViewer();
            fractalViewer.DrawImage();
            Console.ReadKey();
        }
    }

    namespace Mathematics
    {

        class ArgsController
        {
            public static readonly int[] intargs;
            public static readonly double[] doubleargs;
            public static readonly String outputPath;

            public ArgsController(string[] args)
            {
                intargs = GetIntArgs(args);
                doubleargs = GetDoubleArgs(args);
                outputPath = GetOutputPath(args);
            }

            private String GetOutputPath(string[] args){
                return args[6];
            }

            private int[] GetIntArgs(string[] args)
            {
                intargs = new int[2];
                for (int i = 0; i < intargs.Length; i++)
                {
                    intargs[i] = int.Parse(args[i]);
                }

            return intargs;
            }

            private  double[] GetDoubleArgs(string[] args)
            {
                doubleargs = new double[4];
                for (int i = 0; i < doubleargs.Length; i++)
                {   
                    doubleargs[i] = double.Parse(args[i + 2]);
                }
            return doubleargs;
            }

        }

        class FractalViewer
        {
            private readonly Color colors;
            private  Bitmap bitmap;
            private NewtonFractal newtonFactorial;
            public FractalViewer(string[] args){
                bitmap = new Bitmap(ArgsController.intargs[0],ArgsController.intargs[1]);
                newtonFactorial = new NewtonFactorial(ArgsController.intargs,ArgsController.doubleargs);
                colors = new Color[]{
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange,
                Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta 
                };
            }

            public void DrawImage(){

                for (int i = 0; i < bitmap.Width; i++) {

                    for (int j = 0; j < bitmap.Height; j++) {
                        newtonFactorial.SetComplexNumber(i,j);
                        var id = newtonFactorial.SolveRoot();
                        var it = newtonFactorial.SolveEquationUsingNewtonsIteration();
                        var vv = colors[id % colors.Length];
                        vv = Color.FromArgb(vv.R, vv.G, vv.B);
                        vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R-(int)it*2), 255), Math.Min(Math.Max(0, vv.G - (int)it*2), 255), Math.Min(Math.Max(0, vv.B - (int)it*2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    bmp.SetPixel(j, i, vv);
                    }
                }
                    bmp.Save(ArgsController.outputPath ?? "../../../out.png");
            }


    }
        class NewtonFractal
        {

            private  readonly double xmin;
            private  readonly double xmax;
            private  readonly double ymin;
            private  readonly double ymax;
            private  readonly double xstep;
            private  readonly double ystep;
            private  readonly Polynomial polynomial;
            private  readonly Polynomial derivativePolynomial;
            private List<ComplexNumber> roots;
            private ComplexNumber ox;

            public NewtonFractal()
            {
                roots = new List<ComplexNumber>();
                xmin = ArgsController.doubleargs[0];
                xmax = ArgsController.doubleargs[1];
                ymin = ArgsController.doubleargs[2];
                ymax = ArgsController.doubleargs[3];
                xstep = (xmax - xmin) / ArgsController.intargs[0];
                ystep = (ymax - ymin) / ArgsController.intargs[1];
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

        public void DisplayPolynomials()
        {
            Console.WriteLine(polynomial);
            Console.WriteLine(derivativePolynomial);
        }

        public ComplexNumber SetComplexNumber(int i ,int j)
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
            public int SolveRoot()
            {
                var known = false;
                var id = 0;
                for (int w = 0; w < roots.Count; w++)
                {
                    if (Math.Pow(ox.Re - roots[w].Re, 2) + Math.Pow(ox.Imaginari - roots[w].Imaginari, 2) <= 0.01)
                    {
                        known = true;
                        id = w;
                    }
                }
                if (!known)
                {
                    roots.Add(ox);
                    id = roots.Count;
                }

                return id;
            }

            public float SolveEquationUsingNewtonsIteration()
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

                return it;
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
