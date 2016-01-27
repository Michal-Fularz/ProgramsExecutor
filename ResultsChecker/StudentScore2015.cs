using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
    class StudentScore2015
    {
        public string forename;
        public string surname;
        public int score;
        public List<int> scoreForEachLicensePlate;
        public List<string> licensePlateNumbers;
        public string others;
        
        public StudentScore2015()
        {
            this.forename = "John";
            this.surname = "Doe";
            this.score = 0;
            this.scoreForEachLicensePlate = new List<int>();
            this.licensePlateNumbers = new List<string>();
            this.others = "";
        }

        public StudentScore2015(string _firstName, string _lastName)
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
