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
            //Main2015();

            Main2015SE();
        }

        #region 2015 Second Edition

        static void Main2015SE()
        {
            // prepare ground truth data
            Student2015Score_SE groundTruth = new Student2015Score_SE("ground", "truth");
            groundTruth.LoadResultsFromFile(@"gt\gt.txt");

            // load students from file
            List<Student2015Score_SE> studentsScores = new List<Student2015Score_SE>();
            
            string pathToFiles = System.IO.Directory.GetCurrentDirectory() + @"\wyniki\";
            string[] filesInDirectory = System.IO.Directory.GetFiles(pathToFiles, "*.txt");

            foreach (var file in filesInDirectory)
            {
                string filename = file.Replace(pathToFiles, string.Empty);

                string[] partsOfFilename = System.Text.RegularExpressions.Regex.Split(filename.TrimEnd('.', 't', 'x', 't'), "_");

                Student2015Score_SE newStudent = new Student2015Score_SE(partsOfFilename[0], partsOfFilename[1]);
                newStudent.LoadResultsFromFile(file);

                studentsScores.Add(newStudent);
            }

            // for each student compare its result with ground truth
            foreach (var studentScore in studentsScores)
            {
                studentScore.CompareWithGroundTruth(groundTruth);
            }

            // save results to a file
            using (TextWriter writer = File.CreateText(System.IO.Directory.GetCurrentDirectory() + @"\podsumowanie.txt"))
            {
                StringBuilder sb = Student2015Score_SE.PrepareTitleRow(groundTruth.GetTheNumberOfScores());

                foreach (var studentScore in studentsScores)
                {
                    sb.Append(studentScore.GetResultForSaving());
                }
                writer.WriteLine(sb);
            }
        }

        #endregion

        #region 2015

        static private void Main2015()
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

        #endregion
    }

    class StudentScore
    {
        public string forename;
        public string surname;
        public int score;
        public string others;

        public StudentScore()
            : this("John", "Doe")
        {
        }

        public StudentScore(string _firstName, string _lastName)
        {
            this.forename = _firstName;
            this.surname = _lastName;
            this.score = 0;
            this.others = "";
        }

        public virtual void LoadResultsFromFile(string filenameWithPath)
        {
        }
    }

    // SE - second edition
    class Student2015Score_SE : StudentScore
    {    
        private class NumberOfRoadSignsOnImage
        {
            public const int NumberOfRoadSignTypes = 4;

            public int Warning;
            public int Mandatory;
            public int Prohibition;
            public int Information;

            public NumberOfRoadSignsOnImage(int _warning, int _mandatory, int _prohibition, int _information)
            {
                this.Warning = _warning;
                this.Mandatory = _mandatory;
                this.Prohibition = _prohibition;
                this.Information = _information;
            }

            public NumberOfRoadSignsOnImage(string textToParse)
            {
                textToParse = textToParse.Replace("  ", " ");

                string[] numbers = textToParse.Split(' ');

                if (numbers.Length > NumberOfRoadSignTypes)
                {
                    throw new Exception("Too many numbers in line!");
                }

                this.Warning = Int32.Parse(numbers[0]);
                this.Mandatory = Int32.Parse(numbers[1]);
                this.Prohibition = Int32.Parse(numbers[2]);
                this.Information = Int32.Parse(numbers[3]);
            }

            private int CalculateScoreForPair(int currentValue, int groundTruthValue)
            {
                int score = 0;

                if (currentValue <= groundTruthValue)
                {
                    score += currentValue;
                }
                else
                {
                    int proposedScore = groundTruthValue - (currentValue - groundTruthValue);

                    if (proposedScore < 0)
	                {
		                score += 0;
	                }
                    else
                    {
                        score += proposedScore;
                    }
                }

                return score;
            }

            private int CheckForBonusPoint(NumberOfRoadSignsOnImage groundTruth)
            {
                int bonus = 0;

                if (
                    this.Warning == groundTruth.Warning
                    &&
                    this.Mandatory == groundTruth.Mandatory
                    &&
                    this.Prohibition == groundTruth.Prohibition
                    &&
                    this.Information == groundTruth.Information
                    )
                {
                    bonus = 1;
                }

                return bonus;
            }

            public int CountScore(NumberOfRoadSignsOnImage groundTruth)
            {
                int score = 0;

                score += this.CalculateScoreForPair(this.Warning, groundTruth.Warning);
                score += this.CalculateScoreForPair(this.Mandatory, groundTruth.Mandatory);
                score += this.CalculateScoreForPair(this.Prohibition, groundTruth.Prohibition);
                score += this.CalculateScoreForPair(this.Information, groundTruth.Information);

                score += this.CheckForBonusPoint(groundTruth);

                return score;
            }
        }

        private List<int> scoreForEachRoadSignImage;
        private List<NumberOfRoadSignsOnImage> dataForEachRoadSignImage;       

        public Student2015Score_SE(string _firstName, string _lastName)
            : base(_firstName, _lastName)
        {
            this.scoreForEachRoadSignImage = new List<int>();
            this.dataForEachRoadSignImage = new List<NumberOfRoadSignsOnImage>();
        }

        public int GetTheNumberOfScores()
        {
            return this.dataForEachRoadSignImage.Count;
        }

        public override void LoadResultsFromFile(string filenameWithPath)
        {
            this.dataForEachRoadSignImage.Clear();

            FileStream fs = new FileStream(filenameWithPath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string lineWithoutEnding = line.TrimEnd('\r', '\n');
                if (lineWithoutEnding != "")
                {
                    this.dataForEachRoadSignImage.Add(new NumberOfRoadSignsOnImage(lineWithoutEnding));
                }
                else
                {
                    this.dataForEachRoadSignImage.Add(new NumberOfRoadSignsOnImage(0, 0, 0, 0));
                }
            }
        }

        public void CompareWithGroundTruth(Student2015Score_SE groundTruthData)
        {
            List<NumberOfRoadSignsOnImage> groundTruthRoadSignNumbers = groundTruthData.dataForEachRoadSignImage;
            // prepare all the members
            this.score = 0;
            this.others = "";
            this.scoreForEachRoadSignImage.Clear();
            for (int i = 0; i < groundTruthRoadSignNumbers.Count; i++)
            {
                this.scoreForEachRoadSignImage.Add(0);
            }          

            for (int i = 0; i < this.dataForEachRoadSignImage.Count; i++)
			{
                NumberOfRoadSignsOnImage current = this.dataForEachRoadSignImage[i];
                NumberOfRoadSignsOnImage gt = groundTruthRoadSignNumbers[i];

                this.scoreForEachRoadSignImage[i] = current.CountScore(gt);

                this.score += this.scoreForEachRoadSignImage[i];
			}
        }

        public static StringBuilder PrepareTitleRow(int numberOfScores)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("$Forename$, $Surname$, $finalScore$, $Other:$, partial scores:, ");
            for (int i = 0; i < numberOfScores; i++)
            {
                sb.Append("$" + i + "$, ");
            }
            sb.Append("\r\n");

            return sb;
        }

        public StringBuilder GetResultForSaving()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.forename).Append(", ").Append(this.surname).Append(", ");
            sb.Append(this.score).Append(", ").Append(this.others).Append(", partial scores:, ");
            foreach (var partialScore in this.scoreForEachRoadSignImage)
            {
                sb.Append(partialScore).Append(", ");
            }
            sb.Append("\r\n");

            return sb;
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

        public void CompareWithGroundTruth(List<string> groundTruthData)
        {
            List<string> groundTruthLicensePlateNumbers = groundTruthData;
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
