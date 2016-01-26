using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
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

        public virtual void CompareWithGroundTruth(StudentScore groundTruthData)
        {
        }
    }
}
