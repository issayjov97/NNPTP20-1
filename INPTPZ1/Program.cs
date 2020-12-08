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
using INPTPZ1.CommandLineParameters;
using Mathematics.FractalViewer;

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

  
}
