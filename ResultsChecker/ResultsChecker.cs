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
            Console.WriteLine("SiSW 2016 students reults checker!\r\n Author: Michal Fularz" + System.Environment.NewLine);
            Console.WriteLine("More info and code can be found on github: https://github.com/Michal-Fularz/ProgramsExecutor" + System.Environment.NewLine);

            Console.WriteLine("For proper operation this program requires file wyniki.txt with ground truth data and folder wyniki with files Nazwisko_Imie.txt with data to be compared." + System.Environment.NewLine);

            //Main2015();

            Main_general();

            Console.WriteLine("If the program does not throw any exception it means provided files were correctly formatted." + System.Environment.NewLine);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        #region general

        static void Main_general()
        {
            // prepare ground truth data
            // use appropraite specialized type eg. Student2015Score_SE
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

                // use appropraite specialized type eg. Student2015Score_SE
                StudentScore newStudent = new StudentScoreJellyBean(partsOfFilename[0], partsOfFilename[1]);
                newStudent.LoadResultsFromFile(file);

                studentsScores.Add(newStudent);
            }

            foreach (var studentScore in studentsScores)
            {
                studentScore.CompareWithGroundTruth(groundTruth);
            }

            // save results to a file
            using (TextWriter writer = File.CreateText(System.IO.Directory.GetCurrentDirectory() + @"\summary.txt"))
            {
                // use appropraite specialized type eg. Student2015Score_SE
                StringBuilder sb = StudentScoreJellyBean.GetTitleRow();

                foreach (var studentScore in studentsScores)
                {
                    sb.Append(studentScore.GetResults());
                }
                writer.WriteLine(sb);
            }
        }

        #endregion

        #region 2015 - old code!!!

        static private void Main2015()
        {
            List<StudentScore2015> studentsScores = new List<StudentScore2015>();
            List<StudentScore2015> groundTruth = new List<StudentScore2015>();

            LoadDataFromFilesToStudentsScores(System.IO.Directory.GetCurrentDirectory() + @"\wyniki\", studentsScores);
            LoadDataFromFilesToStudentsScores(System.IO.Directory.GetCurrentDirectory() + @"\gt\", groundTruth);

            foreach (var item in studentsScores)
            {
                item.CompareWithGroundTruth(groundTruth[0].licensePlateNumbers);
            }

            SaveResults(System.IO.Directory.GetCurrentDirectory() + @"\podsumowanie.txt", studentsScores, groundTruth[0].licensePlateNumbers.Count);

            Console.WriteLine(studentsScores[0].score.ToString());
        }

        static private void LoadDataFromFilesToStudentsScores(string pathToFiles, List<StudentScore2015> studentsScores)
        {
            string[] filesInDirectory = System.IO.Directory.GetFiles(pathToFiles, "*.txt");

            foreach (var file in filesInDirectory)
            {
                string filename = file.Replace(pathToFiles, string.Empty);

                string[] partsOfFilename = System.Text.RegularExpressions.Regex.Split(filename.TrimEnd('.', 't', 'x', 't'), "_");

                StudentScore2015 newStudent = new StudentScore2015(partsOfFilename[0], partsOfFilename[1]);
                newStudent.licensePlateNumbers = LoadLicensePlateNumbersFromFile(file);

                studentsScores.Add(newStudent);
            }
        }

        static private List<string> LoadLicensePlateNumbersFromFile(string fileNameWithPath)
        {
            List<string> licensePlatesNumbers = new List<string>();

            FileStream fs = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string licensePlateNumber = line.TrimEnd('\r', '\n');
                if (line != "")
                {
                    licensePlatesNumbers.Add(licensePlateNumber.ToUpper());
                }
                else
                {
                    licensePlatesNumbers.Add("XXXXXXX");
                }
            }

            return licensePlatesNumbers;
        }

        static private void SaveResults(string fileNameWithPath, List<StudentScore2015> studentsScores, int numberOfScores)
        {
            using (TextWriter writer = File.CreateText(fileNameWithPath))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("$Forename$, $Surname$, $finalScore$, $Other:$, partial scores:, ");
                for (int i = 0; i < numberOfScores; i++)
                {
                    sb.Append("$" + i + "$, ");
                }
                sb.Append("\r\n");

                foreach (var studentScore in studentsScores)
                {

                    sb.Append(studentScore.forename).Append(", ").Append(studentScore.surname).Append(", ");
                    sb.Append(studentScore.score).Append(", ").Append(studentScore.others).Append(", partial scores:, ");
                    foreach (var partialScore in studentScore.scoreForEachLicensePlate)
                    {
                        sb.Append(partialScore).Append(", ");
                    }
                    sb.Append(studentScore.others);
                    sb.Append("\r\n");
                }

                writer.WriteLine(sb);
            }
        }

        #endregion
    }
}
