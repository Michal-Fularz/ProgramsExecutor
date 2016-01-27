using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
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

        public override void LoadResultsFromFile(string filenameWithPath)
        {
            this.dataForEachRoadSignImage.Clear();

            System.IO.FileStream fs = new System.IO.FileStream(filenameWithPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader reader = new System.IO.StreamReader(fs);
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

        public override void CompareWithGroundTruth(StudentScore groundTruthData)
        {
            if (groundTruthData is Student2015Score_SE)
            {
                Student2015Score_SE groundTruthData_downcasted = (Student2015Score_SE)groundTruthData;

                List<NumberOfRoadSignsOnImage> groundTruthRoadSignNumbers = groundTruthData_downcasted.dataForEachRoadSignImage;
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
            else
            {
                throw new Exception("Wrong class passed!");
            }
        }

        public override StringBuilder GetResults()
        {
            StringBuilder sb = this.GetResultsGeneral();
            sb.Append(", partial scores:, ");
            foreach (var partialScore in this.scoreForEachRoadSignImage)
            {
                sb.Append(partialScore).Append(", ");
            }
            sb.Append("\r\n");

            return sb;
        }
    }
}
