using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using Mathematics.NewtonFractal;
using INPTPZ1.CommandLineInputHandler;



namespace Mathematics
{
    class FractalViewer
    {
        private Color[] colors;
        private Bitmap bitmap;
        private NewtonFractal newtonFactorial;
        private static readonly String defaultOutupPath = "../../../out.png";
        public FractalViewer()
        {
            bitmap = new Bitmap(
                CommandLineInputHandler.GetGeometricMeasurements(0), 
                CommandLineInputHandler.GetGeometricMeasurements(1)
                );
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
                    color = Color.FromArgb(Math.Min(Math.Max(0, color.R - it * 2), 255), Math.Min(Math.Max(0, color.G - it * 2), 255), Math.Min(Math.Max(0, color.B - it * 2), 255));
                    bitmap.SetPixel(j, i, color);
                }
            }
            bitmap.Save(CommandLineInputHandler.GetOutputPath() ?? defaultOutupPath);
        }


    }
}