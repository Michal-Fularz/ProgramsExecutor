using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace ProgramsExecutor
{
    class ProgramsExecutor
    {
        #region time calculations functions (different for eachtype of program)
        static int CalculateTimeForWholeProject_2013version(int _numberOfScenes, int _numberOfSecondsPerShot, int _numberOfShotsPerScene)
        {
            int timeForWholeProjectInMSeconds = _numberOfScenes * _numberOfShotsPerScene * _numberOfSecondsPerShot * 1000;

            return timeForWholeProjectInMSeconds;
        }

        static int CalculateTimeForWholeProject_2014version(int _numberOfSecondsInVideoFile)
        {
            const int gracePeriod = 30;
            const int fps = 25;
            int timeForWholeProjectInMSeconds = ((_numberOfSecondsInVideoFile / 2 * fps) + gracePeriod) * 1000;

            return timeForWholeProjectInMSeconds;
        }

        static int CalculateTimeForWholeProject_2015version(int _numberOfLicensePlateImages)
        {
            const int gracePeriod = 30;
            const int timeInSecondsForEachPlateImage = 2;
            int timeForWholeProjectInMSeconds = ((_numberOfLicensePlateImages * timeInSecondsForEachPlateImage) + gracePeriod) * 1000;

            return timeForWholeProjectInMSeconds;
        }

        static int CalculateTimeForWholeProject_2015version_secondEdition(int _numberOfRoadSignImages)
        {
            const int gracePeriod = 30;
            const int timeInSecondsForEachRoadSignImage = 2;
            int timeForWholeProjectInMSeconds = ((_numberOfRoadSignImages * timeInSecondsForEachRoadSignImage) + gracePeriod) * 1000;

            return timeForWholeProjectInMSeconds;
        }
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("SiSW 2014 students program executor!\r\n Author: Michal Fularz" + System.Environment.NewLine);

            // get *.exe files from directory
            string[] filesInDirectory = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + @"\", "*.exe");

            //int timeForWholeProjectInMSeconds = CalculateTimeForWholeProject_2013version(10, 30, 3);
            //int timeForWholeProjectInMSeconds = CalculateTimeForWholeProject_2014version(126);
            //int timeForWholeProjectInMSeconds = CalculateTimeForWholeProject_2015version(57);
            int timeForWholeProjectInMSeconds = CalculateTimeForWholeProject_2015version_secondEdition(24);

            Console.WriteLine("Time provided for each program (in seconds): " + (timeForWholeProjectInMSeconds / 1000).ToString() + System.Environment.NewLine);

            foreach (var file in filesInDirectory)
            {
                try
                {
                    if (file != System.IO.Directory.GetCurrentDirectory() + @"\" + "ProgramsExecutor.exe")
                    {
                        Console.WriteLine("Executing " + file);

                        // Prepare the process to run
                        ProcessStartInfo start = new ProcessStartInfo();
                        // Enter in the command line arguments, everything you would enter after the executable name itself
                        start.Arguments = "";
                        // Enter the executable to run, including the complete path
                        start.FileName = file;
                        // Do you want to show a console window?
                        start.WindowStyle = ProcessWindowStyle.Hidden;
                        start.CreateNoWindow = true;

                        // Run the external process & wait for it to finish
                        using (Process proc = Process.Start(start))
                        {
                            proc.WaitForExit(timeForWholeProjectInMSeconds);
                            if (!proc.HasExited)
                            {
                                Console.WriteLine("Timeout! Killing the program");
                                proc.Kill();
                            }

                            // Retrieve the app's exit code
                            int exitCode = proc.ExitCode;
                            Console.WriteLine("Exit code: " + exitCode);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
