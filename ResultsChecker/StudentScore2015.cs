using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
    class StudentScore2015 : StudentScore
    {
        private List<string> licensePlateNumbers;
        private List<double> scoreForEachLicensePlate;
        
        public StudentScore2015(string _firstName, string _lastName)
            : base(_firstName, _lastName)
        {
            this.licensePlateNumbers = new List<string>();
            this.scoreForEachLicensePlate = new List<double>();
        }

        private void ClearAndPrepareAllFields(int numberOfLicensePlates)
        {
            this.score = 0;
            this.others = "";
            this.scoreForEachLicensePlate.Clear();
            for (int i = 0; i < numberOfLicensePlates; i++)
            {
                this.scoreForEachLicensePlate.Add(0);
            }
        }

        private int FindNumberOfCompares(List<string> groundTruthLicensePlateNumbers)
        {
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

            return numberOfCompares;
        }

        public override void LoadResultsFromFile(string filenameWithPath)
        {
            this.licensePlateNumbers.Clear();

            System.IO.FileStream fs = new System.IO.FileStream(filenameWithPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader reader = new System.IO.StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string licensePlateNumber = line.TrimEnd('\r', '\n');
                if (line != "")
                {
                    this.licensePlateNumbers.Add(licensePlateNumber.ToUpper());
                }
                else
                {
                    this.licensePlateNumbers.Add("XXXXXXX");
                }
            }
        }

        public override void CompareWithGroundTruth(StudentScore groundTruthData)
        {
            if (groundTruthData is StudentScore2015)
            {
                // downcasting to access the ground truth scenes
                List<string> groundTruthLicensePlateNumbers = ((StudentScore2015)groundTruthData).licensePlateNumbers;

                this.ClearAndPrepareAllFields(groundTruthLicensePlateNumbers.Count);

                int numberOfCompares = this.FindNumberOfCompares(groundTruthLicensePlateNumbers);
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
                        if (current[j] == gt[j])
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

        public override StringBuilder GetResults()
        {
            StringBuilder sb = this.GetResultsGeneral();
            sb.Append(", partial scores:, ");
            foreach (var partialScore in this.scoreForEachLicensePlate)
            {
                sb.AppendFormat("{0:D}", partialScore).Append(", ");
            }
            sb.Append("\r\n");

            return sb;
        }
    }
}
