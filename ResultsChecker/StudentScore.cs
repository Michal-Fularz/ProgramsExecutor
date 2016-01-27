using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
    abstract class StudentScore
    {
        public string forename;
        public string surname;
        public double score;
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

        public abstract void LoadResultsFromFile(string filenameWithPath);

        public abstract void CompareWithGroundTruth(StudentScore groundTruthData);

        public abstract StringBuilder GetTitleRow();

        public abstract StringBuilder GetResults();
    }
}
