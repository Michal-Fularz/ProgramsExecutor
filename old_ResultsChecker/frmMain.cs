using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SiSW_SprProjektu
{
    public partial class frmMain : Form
    {
        FolderBrowserDialog folder = new FolderBrowserDialog();
        // lista studentow
        List<Student> listaStudentow = new List<Student>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void chooseDirectory()
        {
            folder.ShowDialog();
            if (folder.SelectedPath != "")
            {
                tbPath.Text = folder.SelectedPath;
            }

            LoadStudentsFromFiles();
        }

        private void LoadStudentsFromFiles()
        {
            if (folder.SelectedPath != "")
            {
                string[] filePaths = Directory.GetFiles(folder.SelectedPath, "*.txt");
                int rozmiar = filePaths.Length;
                int liczbaZnakow = folder.SelectedPath.Length;
                string path = folder.SelectedPath + @"Wyniki.txt";

                // wyswietlanie sciezek z katalogu - nie wyswietla wszystkich, chociaz w filePaths ewidentnie są!
                //PrintArray(filePaths, rtbMain);
                //FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                //StreamWriter sw = new StreamWriter(fs);

                // przeszukanie folderu, czy znajduja sie w nim jakies pliki tekstowe
                if (rozmiar != 0)
                {
                    // pobieranie nazwiska i imienia z nazwy pliku:
                    for (int i = 0; i < rozmiar; ++i)
                    {
                        //C:\Users\Anna\Desktop\SiSW\<-27 Fularz.Michał.txt
                        string nazwaPliku = filePaths[i].Substring(liczbaZnakow + 1);  // -> Fularz.Michał.txt
                        string[] nazwiskoImieTxt = nazwaPliku.Split(new Char[] { '.' });

                        // sprawdzenie zawartosci pliku tekstowego
                        FileStream fs1 = new FileStream(filePaths[i], FileMode.Open, FileAccess.ReadWrite);
                        StreamReader reader = new StreamReader(fs1);
                        // Zapis do pliku tekstowego: nazwisko imie

                        //string dane = imieNazwiskoTxt[0] + ", " + imieNazwiskoTxt[1];
                        //string noweDane = dane.TrimEnd('\r', '\n');
                        //rtbMain.Text += noweDane + ", ";
                        // Zapis do pliku tekstowego: nazwisko imie
                        //sw.Write(noweDane + ", ");

                        Student nowyStudent = new Student();
                        nowyStudent.nazwisko = nazwiskoImieTxt[0];
                        nowyStudent.imie = nazwiskoImieTxt[1];

                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string linia = line.TrimEnd('\r', '\n');
                            string[] wynikLinii = System.Text.RegularExpressions.Regex.Split(linia, ", ");
                            Scena nowaScena = new Scena();
                            for (int j = 0; j < wynikLinii.Length; ++j)
                            {
                                nowaScena.liczbaZelkow[j] = Convert.ToInt32(wynikLinii[j]);
                            }

                            nowyStudent.listaScen.Add(nowaScena);
                        }
                        listaStudentow.Add(nowyStudent);
                    }
                }
                else
                {
                    MessageBox.Show("W folderze nie ma plików tekstowych!", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //sw.Close();
            }
            else
            {
                MessageBox.Show("Nie wybrano folderu!\nNaciśnij Menu -> Wybór katalogu.", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Student LoadStudentFromFile(string filePath)
        {
            // przydzielenie pamięci dla nowego studenta
            Student nowyStudent = new Student();

            // odczyt danych studenta z nazwy pliku (odczytwana z ścieżki)
            string[] skladoweSciezki = filePath.Split('\\');
            string nazwaPliku = skladoweSciezki[skladoweSciezki.Length - 1];//filePaths.Substring(liczbaZnakow + 1);  // -> Fularz.Michał.txt
            // rozdzielenie imienia i nazwiska
            string[] nazwiskoImieTxt = nazwaPliku.Split(new Char[] { '.' });
            nowyStudent.nazwisko = nazwiskoImieTxt[0];
            nowyStudent.imie = nazwiskoImieTxt[1];

            // sprawdzenie zawartosci pliku tekstowego
            FileStream fs1 = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(fs1);

            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string linia = line.TrimEnd('\r', '\n');
                string[] wynikLinii = System.Text.RegularExpressions.Regex.Split(linia, ", ");
                Scena nowaScena = new Scena();
                for (int j = 0; j < wynikLinii.Length; ++j)
                {
                    nowaScena.liczbaZelkow[j] = Convert.ToInt32(wynikLinii[j]);
                }

                nowyStudent.listaScen.Add(nowaScena);
            }

            return nowyStudent;
        }

        private void ProcessStudents()
        {
            CreateGroundTruthData();

            for (int i = 0; i < listaStudentow.Count; i++)
            {
                listaStudentow[i].wynik = CalculateDifference(przykladnyStudent, listaStudentow[i]);
            }

            for (int i = 0; i < listaStudentow.Count; i++)
            {
                rtbMain.Text += listaStudentow[i].imie + " " + listaStudentow[i].nazwisko + " " + listaStudentow[i].wynik + "\n";
            }
        }

        // akcja na nacisniecie klawisza Edytuj
        private void btnEdit_Click(object sender, EventArgs e)
        {
            przykladnyStudent = LoadGroundTruthDataFromFile();
                   
        }

        public static void PrintArray(string[] strArray, RichTextBox textBox)
        {
            foreach (var item in strArray)
            {
                textBox.AppendText(item + "\n");
            }
        }

        Student przykladnyStudent = new Student();

        private Student LoadGroundTruthDataFromFile()
        {
            return LoadStudentFromFile("wzorcowy.student.txt");
        }

        private void CreateGroundTruthData()
        {
            przykladnyStudent.imie = "Michał";
            przykladnyStudent.nazwisko = "Fularz";

            for (int i = 0; i < 4; i++)
            {
                Scena przykladnaScena = new Scena();
                
                for (int j = 0; j < 6; ++j)
                {
                    przykladnaScena.liczbaZelkow[j] = j;
                }

                przykladnyStudent.listaScen.Add(przykladnaScena);
            }
        }

        private int CalculateDifference(Student s1, Student s2)
        {
            int diff = 0;
            int liczbaZelkowScen = 0;

            for (int i = 0; i < s1.listaScen.Count; i++)
            {
                for (int j = 0; j < s1.listaScen[0].liczbaZelkow.Length; j++)
                {
                    liczbaZelkowScen = s1.listaScen[i].liczbaZelkow[j] - s2.listaScen[i].liczbaZelkow[j];
                    diff += Math.Abs(liczbaZelkowScen);
                }
                
            }

            return diff;
        }

        // Wybor katalogu przez uzytkownika
        private void menuItemChooseDir_Click(object sender, EventArgs e)
        {
            chooseDirectory();
        }

        // O programie - pop up
        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program sprawdzający projekty z SiSW \n\rWykonano dla PP - 2013", "O Programie", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnChooseDirectory_Click(object sender, EventArgs e)
        {
            chooseDirectory();
        }

        // akcja na nacisniecie klawisza Otworz Folder
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string myPath = folder.SelectedPath;
            if (myPath != "")
            {
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = myPath;
                prc.Start();
            }
            else
            {
                MessageBox.Show("Nie podano ścieżki dostępu!! \n Naciśnij Menu i Wybór katalogu", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalculateResults_Click(object sender, EventArgs e)
        {
            
            foreach (var item in listaStudentow)
            {
                item.PorownajZWzorem(przykladnyStudent);
                rtbMain.AppendText(item.WyswietlDane());
            }

            rtbMain.AppendText(przykladnyStudent.WyswietlDane());
        }

        SiSW2014_scoreCalculator sw2014 = new SiSW2014_scoreCalculator();

        private void button1_Click(object sender, EventArgs e)
        {
            sw2014.LoadFiles();
        }
    }

    public class SiSW2014_scoreCalculator
    {
        List<Student2014Score> studentsScores;

        List<string> sequecesNames;

        Dictionary<string, Dictionary<string, List<int>>> students = new Dictionary<string, Dictionary<string, List<int>>>();
        Dictionary<string, Dictionary<string, List<int>>> gt = new Dictionary<string, Dictionary<string, List<int>>>();

        public SiSW2014_scoreCalculator()
        {
            sequecesNames = new List<string>();
            studentsScores = new List<Student2014Score>();
        }

        public void LoadFiles()
        {
            string pathToFile = System.IO.Directory.GetCurrentDirectory() + @"\dane_wejsciowe\";
            string filename = "nazwy_sekwencji.txt";

            FileStream fs = new FileStream(pathToFile + filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                sequecesNames.Add(line.TrimEnd('\r', '\n'));
            }

            // odczytaj groundtrue
            LoadStudents(System.IO.Directory.GetCurrentDirectory() + @"\gt\", gt);
            // odczytaj wyniki studentów
            LoadStudents(System.IO.Directory.GetCurrentDirectory() + @"\wynik\", students);

            CompareWithGroundtrue();

            SaveResults(System.IO.Directory.GetCurrentDirectory() + @"\podsumowanie.txt");
        }

        private void LoadStudents(string pathToFiles, Dictionary<string, Dictionary<string, List<int>>> dictionary)
        {
            string[] filesInDirectory = System.IO.Directory.GetFiles(pathToFiles, "*.txt");

            foreach (var file in filesInDirectory)
            {
                string filename = file.Replace(pathToFiles, string.Empty);

                string[] partsOfFilename = System.Text.RegularExpressions.Regex.Split(filename.TrimEnd('.', 't', 'x', 't'), "_");

                string studentIdentifier = partsOfFilename[0] + "_" + partsOfFilename[1];
                string sequenceIdentifer = partsOfFilename[2] + "_" + partsOfFilename[3];

                List<int> numberOfCarsOnEachKeyFrame = LoadNumberOfCarsOnEachKeyFrameFromFile(file);

                // See whether Dictionary contains this string.
                if (!dictionary.ContainsKey(studentIdentifier))
                {
                    Dictionary<string, List<int>> sequences = new Dictionary<string, List<int>>();
                    sequences.Add(sequenceIdentifer, numberOfCarsOnEachKeyFrame);
                    dictionary.Add(studentIdentifier, sequences);
                }
                else
                {
                    if (!dictionary[studentIdentifier].ContainsKey(sequenceIdentifer))
                    {
                        dictionary[studentIdentifier].Add(sequenceIdentifer, numberOfCarsOnEachKeyFrame);
                    }
                }
            }
        }

        private List<int> LoadNumberOfCarsOnEachKeyFrameFromFile(string fileNameWithPath)
        {
            List<int> numberOfCarsOnEachKeyFrame = new List<int>();

            FileStream fs = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                string numberOfCarsAsString = line.TrimEnd('\r', '\n');
                if (line != "")
                {
                    int numberOfCars = Convert.ToInt32(numberOfCarsAsString);
                    numberOfCarsOnEachKeyFrame.Add(numberOfCars);
                }
            }

            return numberOfCarsOnEachKeyFrame;
        }

        public void CompareWithGroundtrue()
        {
            Dictionary<string, List<int>> gtSequence = gt["g_t"];

            foreach (var student in students)
            {
                List<int> partialScores = new List<int>();
                int finalScore = 0;

                Dictionary<string, List<int>> studentSequence = student.Value;

                foreach (var sequence in gtSequence)
                {
                    int scoreForCurrentSequence = 0;
                    List<int> gtNumberOfCarsOnEachKeyFrame = sequence.Value;
                    // sprawdź czy student ma sekwencję z gt
                    if (studentSequence.ContainsKey(sequence.Key))
                    {
                        List<int> studentNumberOfCarsOnEachKeyFrame = studentSequence[sequence.Key];
                        // porównaj z gt
                        // dla każdej klatki kluczowej
                        for (int i = 0; i < gtNumberOfCarsOnEachKeyFrame.Count; i++)
                        {
                            // sprawdź czy student zapisał jakiś wynik
                            if (i < studentNumberOfCarsOnEachKeyFrame.Count)
                            {
                                // różnica = abs(gt - wynik_studenta)
                                scoreForCurrentSequence += Math.Abs(studentNumberOfCarsOnEachKeyFrame[i] - gtNumberOfCarsOnEachKeyFrame[i]);
                            }
                            else
                            {
                                scoreForCurrentSequence += gtNumberOfCarsOnEachKeyFrame[i];
                            }
                        }
                    }
                    else
                    {
                        // w przeciwnym wypadku każdy samochód widoczny liczy się jako błąd
                        foreach (var item in gtNumberOfCarsOnEachKeyFrame)
                        {
                            scoreForCurrentSequence += item;
                        }
                    }
                    partialScores.Add(scoreForCurrentSequence);
                }

                foreach (var item in partialScores)
                {
                    finalScore += item;
                }

                Student2014Score newScore = new Student2014Score();
                newScore.identifier = student.Key;
                newScore.score = finalScore;
                newScore.scoreForEachSequence = partialScores;

                studentsScores.Add(newScore);
            }
        }

        public void SaveResults(string fileNameWithPath)
        {
            using (TextWriter writer = File.CreateText(fileNameWithPath))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("$Identifier$: $finalScore$, partial scores: $s1$, $s2$, $s3$, $s4$, $s5$, $s6$ \r\n");

                foreach (var studentScore in studentsScores)
                {

                    sb.Append(studentScore.identifier).Append(": ").Append(studentScore.score).Append(", partial scores: ");
                    foreach (var partialScore in studentScore.scoreForEachSequence)
                    {
                        sb.Append(partialScore).Append(", ");
                    }
                    sb.Append("\r\n");
                }

                writer.WriteLine(sb);
            }
        }
    }

    public class Student2014Score
    {
        public string identifier;
        public int score;
        public List<int> scoreForEachSequence;

        public Student2014Score()
        {
            identifier = "default";
            score = 0;
            scoreForEachSequence = new List<int>();
        }
        
    }

    #region this code is not used (???)
    public class Student2014
    {
        public string imie;
        public string nazwisko;
        public List<Sequence> sequences;

        public Student2014()
        {
            imie = "";
            nazwisko = "";
            sequences = new List<Sequence>();
        }

        int CompareToGroundTrue(Student2014 model)
        {
            int finalScore = 0;

            List<int> scores = new List<int>();

            for (int i = 0; i < model.sequences.Count; i++)
            {
                int scoreForCurrentSequence = 0;
                // jeśli student zapisał taką sekwencję
                if (i < sequences.Count)
                {
                    // dla każdej klatki kluczowej
                    for (int j = 0; j < model.sequences[i].numberOfCarsOnEachKeyFrame.Count; j++)
                    {
                        // sprawdź czy student zapisał jakiś wynik
                        if (j < sequences[i].numberOfCarsOnEachKeyFrame.Count)
                        {
                            // różnica = abs(gt - wynik_studenta)
                            scoreForCurrentSequence += Math.Abs(sequences[i].numberOfCarsOnEachKeyFrame[j] - model.sequences[i].numberOfCarsOnEachKeyFrame[j]);
                        }
                        else
                        {
                            scoreForCurrentSequence += model.sequences[i].numberOfCarsOnEachKeyFrame[j];
                        }
                    }
                }
                else
                {
                    // w przeciwnym wypadku każdy samochód widoczny liczy się jako błąd
                    foreach (var item in model.sequences[i].numberOfCarsOnEachKeyFrame)
                    {
                        scoreForCurrentSequence += item;
                    }
                }

                scores.Add(scoreForCurrentSequence);
            }

            foreach (var item in scores)
            {
                finalScore += item;
            }

            return finalScore;
        }
    }

    public class Sequence
    {
        public string name;
        public List<int> numberOfCarsOnEachKeyFrame;

        public Sequence()
        {
            name = "default";
            numberOfCarsOnEachKeyFrame = new List<int>();
        }
    }
    #endregion

    public class Scena
    {
        public static int liczbaKolorow = 6;
        public int[] liczbaZelkow = new int[liczbaKolorow];
    }

    public class Student
    {
        public string imie;
        public string nazwisko;
        public int wynik;
        public List<Scena> listaScen;
        public List<int> bledowNaScene;
        public List<double> procentBlednieRozpoznanychaNaScene;
        public List<double> procentBlednieRozpoznanychNaSceneBezNajgorszego;

        public void PorownajZWzorem(Student wzor)
        {
            bledowNaScene.Clear();
            procentBlednieRozpoznanychaNaScene.Clear();

            wzor.bledowNaScene.Clear();

            int liczbaScen = this.listaScen.Count;
            int liczbaKolorowZelkow = Scena.liczbaKolorow;

            for (int i = 0; i < liczbaScen; i++)
            {
                int liczbaBledowNaAktualnejScenie = 0;
                int liczbaZelkowNaWzorcowejScenie = 0;
                for (int j = 0; j < liczbaKolorowZelkow; j++)
                {
                    int roznicaWLiczbieZelkowDanegoKoloru = this.listaScen[i].liczbaZelkow[j] - wzor.listaScen[i].liczbaZelkow[j];
                    liczbaBledowNaAktualnejScenie += Math.Abs(roznicaWLiczbieZelkowDanegoKoloru);

                    liczbaZelkowNaWzorcowejScenie += wzor.listaScen[i].liczbaZelkow[j];
                }

                double procentBlednieRozpoznanych = (double)(liczbaBledowNaAktualnejScenie) / (double)(liczbaZelkowNaWzorcowejScenie) * 100.0;

                wzor.bledowNaScene.Add(liczbaZelkowNaWzorcowejScenie);

                procentBlednieRozpoznanychaNaScene.Add(procentBlednieRozpoznanych);
                bledowNaScene.Add(liczbaBledowNaAktualnejScenie);
            }

            procentBlednieRozpoznanychNaSceneBezNajgorszego = new List<double>(procentBlednieRozpoznanychaNaScene);
            procentBlednieRozpoznanychNaSceneBezNajgorszego.Sort();
            procentBlednieRozpoznanychNaSceneBezNajgorszego.RemoveAt(procentBlednieRozpoznanychNaSceneBezNajgorszego.Count - 1);
        }

        public double SredniaBlednieRozpoznanych()
        {
            double sredniaPrecyzja = 0.0;

            for (int i = 0; i < procentBlednieRozpoznanychaNaScene.Count; i++)
            {
                sredniaPrecyzja += procentBlednieRozpoznanychaNaScene[i];
            }

            return (sredniaPrecyzja / (double)procentBlednieRozpoznanychaNaScene.Count);
        }

        public double SredniaBlednieRozpoznanychBezNajgorszego()
        {
            double sredniaPrecyzja = 0.0;

            for (int i = 0; i < procentBlednieRozpoznanychNaSceneBezNajgorszego.Count; i++)
            {
                sredniaPrecyzja += procentBlednieRozpoznanychNaSceneBezNajgorszego[i];
            }

            return (sredniaPrecyzja / (double)procentBlednieRozpoznanychNaSceneBezNajgorszego.Count);
        }

        public int SumaBledow()
        {
            int sumaBledow = 0;

            for (int i = 0; i < bledowNaScene.Count; i++)
            {
                sumaBledow += bledowNaScene[i];
            }

            return sumaBledow;
        }

        public string WyswietlDane()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.nazwisko);
            sb.Append("; ");
            sb.Append(this.SumaBledow());
            sb.Append("; ");
            sb.Append(String.Format("  {0:F2}", this.SredniaBlednieRozpoznanych()));
            //sb.Append(String.Format("  {0:F2}", this.SredniaBlednieRozpoznanychBezNajgorszego()));
            //sb.Append("\r\n");
            //for(int i=0; i<this.procentBlednieRozpoznanychaNaScene.Count; ++i)
            {
               // sb.Append(", ");
               // sb.Append(String.Format("  {0:F2}", this.procentBlednieRozpoznanychaNaScene[i]));
            }
            sb.Append("\r\n");

            return sb.ToString();
        }

        public Student()
        {
            listaScen = new List<Scena>();
            bledowNaScene = new List<int>();
            procentBlednieRozpoznanychaNaScene = new List<double>();
            procentBlednieRozpoznanychNaSceneBezNajgorszego = new List<double>();
        }

    }
}
