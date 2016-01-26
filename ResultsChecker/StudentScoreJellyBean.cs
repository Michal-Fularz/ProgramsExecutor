using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultsChecker
{
    class StudentScoreJellyBean : StudentScore
    {
        private class Scene
        {
            public static int numberOfColors = 6;
            public int[] numberOfJellyBeans = new int[numberOfColors];

            public Scene()
            {
                for (int i = 0; i < Scene.numberOfColors; i++)
                {
                    this.numberOfJellyBeans[i] = 0;
                }
            }

            public Scene(string textToParse)
            {
                textToParse = textToParse.Replace("  ", " ");

                string[] numbers = textToParse.Split(' ');

                if (numbers.Length > Scene.numberOfColors)
                {
                    throw new Exception("Too many numbers in line!");
                }

                for(int i=0; i<Scene.numberOfColors; i++)
                {
                    this.numberOfJellyBeans[i] = Int32.Parse(numbers[i]);
                }
            }
        }

        public string forename;
        public string surname;
        public int score;
        public List<Scene> scenes;
        public List<int> numberOfErrorsOnEachScene;
        public List<double> percentageOfErrorsOnEachScene;
        public string others;

        public StudentScoreJellyBean(string _firstName, string _lastName)
            : base(_firstName, _lastName)
        {
            this.scenes = new List<Scene>();
            this.numberOfErrorsOnEachScene = new List<int>();
            this.percentageOfErrorsOnEachScene = new List<double>();
        }

        public override void LoadResultsFromFile(string filenameWithPath)
        {
            this.scenes.Clear();

            System.IO.FileStream fs = new System.IO.FileStream(filenameWithPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader reader = new System.IO.StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string lineWithoutEnding = line.TrimEnd('\r', '\n');
                if (lineWithoutEnding != "")
                {
                    this.scenes.Add(new Scene(lineWithoutEnding));
                }
                else
                {
                    this.scenes.Add(new Scene());
                }
            }
        }

        public void CompareWithGroundTruth(StudentScore groundTruthData)
        {

        }
    }
}
