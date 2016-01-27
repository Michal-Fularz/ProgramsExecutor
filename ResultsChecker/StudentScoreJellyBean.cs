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
                textToParse = textToParse.Replace(" ", "");

                string[] numbers = textToParse.Split(',');

                if (numbers.Length > Scene.numberOfColors)
                {
                    throw new Exception("Too many numbers in line!");
                }

                for(int i=0; i<Scene.numberOfColors; i++)
                {
                    this.numberOfJellyBeans[i] = Int32.Parse(numbers[i]);
                }
            }

            public double CountScore(Scene gt)
            {
                int numberOfDifferences = 0;
                int numberOfJellyBeans = 0;
                for(int i=0; i<Scene.numberOfColors; i++)
                {
                    numberOfDifferences += Math.Abs(this.numberOfJellyBeans[i] - gt.numberOfJellyBeans[i]);
                    numberOfJellyBeans += gt.numberOfJellyBeans[i];
                }

                double score = (double)numberOfDifferences / (double)numberOfJellyBeans;

                return score;
            }
        }

        private List<Scene> scenes;
        private List<double> scoreForEachScene;

        public StudentScoreJellyBean(string _firstName, string _lastName)
            : base(_firstName, _lastName)
        {
            this.scenes = new List<Scene>();
            this.scoreForEachScene = new List<double>();
        }

        private void ClearAndPrepareAllFields(int numberOfScenes)
        {
            this.score = 0;
            this.others = "";
            this.scoreForEachScene.Clear();
            for (int i = 0; i < numberOfScenes; i++)
            {
                this.scoreForEachScene.Add(0);
            }
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

        public override void CompareWithGroundTruth(StudentScore groundTruthData)
        {
            if (groundTruthData is StudentScoreJellyBean)
            {
                // downcasting to access the ground truth scenes
                List<Scene> groundTruthScenes = ((StudentScoreJellyBean)groundTruthData).scenes;

                this.ClearAndPrepareAllFields(groundTruthScenes.Count);

                for (int i = 0; i < this.scenes.Count; i++)
                {
                    Scene current = this.scenes[i];
                    Scene gt = groundTruthScenes[i];

                    this.scoreForEachScene[i] = current.CountScore(gt);
                }

                this.score = this.scoreForEachScene.Average();
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
            foreach (var partialScore in this.scoreForEachScene)
            {
                sb.Append(partialScore).Append(", ");
            }
            sb.Append("\r\n");

            return sb;
        }
    }
}
