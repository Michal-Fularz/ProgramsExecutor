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
            List<Student2015Score> studentsScores = new List<Student2015Score>();
            List<Student2015Score> groundTruth = new List<Student2015Score>();

            LoadDataFromFilesToStudentsScores(System.IO.Directory.GetCurrentDirectory() + @"\wyniki\", studentsScores);
            LoadDataFromFilesToStudentsScores(System.IO.Directory.GetCurrentDirectory() + @"\gt\", groundTruth);

            foreach (var item in studentsScores)
            {
                item.CompareWithGroundTruth(groundTruth[0].licensePlateNumbers);
            }

            SaveResults(System.IO.Directory.GetCurrentDirectory() + @"\podsumowanie.txt", studentsScores, groundTruth[0].licensePlateNumbers.Count);

            Console.WriteLine(studentsScores[0].score.ToString());
        }

        static private void LoadDataFromFilesToStudentsScores(string pathToFiles, List<Student2015Score> studentsScores)
        {
            string[] filesInDirectory = System.IO.Directory.GetFiles(pathToFiles, "*.txt");

            foreach (var file in filesInDirectory)
            {
                string filename = file.Replace(pathToFiles, string.Empty);

                string[] partsOfFilename = System.Text.RegularExpressions.Regex.Split(filename.TrimEnd('.', 't', 'x', 't'), "_");

                Student2015Score newStudent = new Student2015Score(partsOfFilename[0], partsOfFilename[1]);
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

        static private void SaveResults(string fileNameWithPath, List<Student2015Score> studentsScores, int numberOfScores)
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
    }

    class Student2015Score
    {
        public string forename;
        public string surname;
        public int score;
        public List<int> scoreForEachLicensePlate;
        public List<string> licensePlateNumbers;
        public string others;
        
        public Student2015Score()
        {
            this.forename = "John";
            this.surname = "Doe";
            this.score = 0;
            this.scoreForEachLicensePlate = new List<int>();
            this.licensePlateNumbers = new List<string>();
            this.others = "";
        }

        public Student2015Score(string _firstName, string _lastName)
        {
            this.forename = _firstName;
            this.surname = _lastName;
            this.score = 0;
            this.scoreForEachLicensePlate = new List<int>();
            this.licensePlateNumbers = new List<string>();
            this.others = "";
        }

        public void CompareWithGroundTruth(List<string> groundTruthLicensePlateNumbers)
        {
            // prepare all the members
            this.score = 0;
            this.others = "";
            this.scoreForEachLicensePlate.Clear();
            for (int i = 0; i < groundTruthLicensePlateNumbers.Count; i++)
            {
                this.scoreForEachLicensePlate.Add(0);
            }

            int numberOfCompares = 0;

            if (this.licensePlateNumbers.Count < groundTruthLicensePlateNumbers.Count)
            {
                this.others += "Number of results is lower (" + this.licensePlateNumbers.Count + ") than ground truth!";
                numberOfCompares = this.licensePlateNumbers.Count;
            }
            else if (this.licensePlateNumbers.Count > groundTruthLicensePlateNumbers.Count)
            {
                this.others += "Number of results is higher (" + this.licensePlateNumbers.Count + ") than ground truth!";
                numberOfCompares = groundTruthLicensePlateNumbers.Count;
            }
            else
            {
                numberOfCompares = groundTruthLicensePlateNumbers.Count;
            }
            

            {
                for (int i = 0; i < numberOfCompares; i++)
			    {
                    string current = this.licensePlateNumbers[i];
                    string gt = groundTruthLicensePlateNumbers[i];

			        int sizeOfSmaller = 0;
                    if (current.Length < gt.Length)
                    {
                        sizeOfSmaller = current.Length;
                    }
                    else
                    {
                        sizeOfSmaller = gt.Length;
                    }

                    for (int j = 0; j < sizeOfSmaller; j++)
                    {
                        if(current[j] == gt[j])
                        {
                            this.scoreForEachLicensePlate[i]++;
                        }
                    }
                    // bonus points for whole license plate
                    if (this.scoreForEachLicensePlate[i] == 7)
                    {
                        this.scoreForEachLicensePlate[i] += 3;
                    }

                    score += this.scoreForEachLicensePlate[i];
			    }
            }
        }
    }
}
