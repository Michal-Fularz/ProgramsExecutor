using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ResultsChecker
{
    class ResultsChecker
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SiSW 2017 students reults checker!\r\n Author: Michal Fularz" + System.Environment.NewLine);
            Console.WriteLine("More info and code can be found on github: https://github.com/Michal-Fularz/ProgramsExecutor" + System.Environment.NewLine);

            Console.WriteLine("For proper operation this program requires file wyniki.txt with ground truth data and folder wyniki with files Nazwisko_Imie.txt with data to be compared." + System.Environment.NewLine);

            // prepare ground truth data
            // use appropraite specialized type eg. StudentScore2015, Student2015Score_SE, StudentScoreJellyBean
            StudentScore groundTruth = new StudentScoreJellyBean("ground", "truth");
            groundTruth.LoadResultsFromFile(@"wyniki.txt");

            // load students from file
            List<StudentScore> studentsScores = new List<StudentScore>();

            string pathToFiles = System.IO.Directory.GetCurrentDirectory() + @"\wyniki\";
            string[] filesInDirectory = System.IO.Directory.GetFiles(pathToFiles, "*.txt");
            foreach (var file in filesInDirectory)
            {
                string filename = file.Replace(pathToFiles, string.Empty);

                string[] partsOfFilename = System.Text.RegularExpressions.Regex.Split(filename.TrimEnd('.', 't', 'x', 't'), "_");

                // use appropraite specialized type eg. StudentScore2015, Student2015Score_SE, StudentScoreJellyBean
                StudentScore newStudent = new StudentScoreJellyBean(partsOfFilename[0], partsOfFilename[1]);
                newStudent.LoadResultsFromFile(file);

                studentsScores.Add(newStudent);
            }

            // calc each student score
            foreach (var studentScore in studentsScores)
            {
                Console.WriteLine(studentScore.forename + " " + studentScore.surname);
                studentScore.CompareWithGroundTruth(groundTruth);
            }

            // save results to a file
            using (TextWriter writer = File.CreateText(System.IO.Directory.GetCurrentDirectory() + @"\summary.txt"))
            {
                // use appropraite specialized type eg. StudentScore2015, Student2015Score_SE, StudentScoreJellyBean 
                StringBuilder sb = StudentScoreJellyBean.GetTitleRow();

                foreach (var studentScore in studentsScores)
                {
                    sb.Append(studentScore.GetResults());
                }
                writer.WriteLine(sb);
            }

            Console.WriteLine("If the program does not throw any exception it means provided files were correctly formatted." + System.Environment.NewLine);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
