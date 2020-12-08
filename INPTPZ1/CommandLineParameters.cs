using System;

namespace INPTPZ1
{
    class CommandLineParameters
    {
        private static int[] geometricMeasurements;
        private static double[] axesCoordinates;
        private static String outputPath;
        private static string[] commandLineParameters;

        public static void SetCommandLineParameters(string[] inputArgs)
        {
            commandLineParameters = inputArgs;
        }

        public static String GetOutputPath()
        {

            return outputPath == null ? commandLineParameters[6] : outputPath;
        }
        public static int GetGeometricMeasurements(int index)
        {
            if (geometricMeasurements == null)
            {
                for (int i = 0; i < geometricMeasurements.Length; i++)
                {
                    geometricMeasurements[i] = int.Parse(commandLineParameters[i]);
                }
            }
            return geometricMeasurements[index];
        }
        public static double GetAxesCoordinates(int index)
        {
            if (axesCoordinates == null)
            {
                axesCoordinates = new double[4];
                for (int i = 0; i < axesCoordinates.Length; i++)
                {
                    axesCoordinates[i] = double.Parse(commandLineParameters[i + 2]);
                }
            }
            return axesCoordinates[index];
        }

    }
}