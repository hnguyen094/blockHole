using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BlockHole
{
    static class Leaderboard
    {
        private static void OpenOrCreate(string fileName)
        {
            FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
            fileStream.Close();
        }

        public static void Reset(string fileName)
        {
            OpenOrCreate(fileName);

            File.WriteAllLines(fileName, new string[] { });
        }

        public static void Save(string fileName, HighScore newHighScore, int leaderboardSize)
        {
            OpenOrCreate(fileName);

            List<HighScore> highScores = Load(fileName);
            highScores.Add(newHighScore);
            highScores.Sort((s1, s2) => -s1.Score.CompareTo(s2.Score));

            List<HighScore> newHighScores = highScores.GetRange(0, highScores.Count > leaderboardSize ? leaderboardSize : highScores.Count);
            File.WriteAllLines(fileName, newHighScores.Select((highScore) => highScore.ToString()).ToArray<string>());
        }

        public static List<HighScore> Load(string fileName)
        {
            OpenOrCreate(fileName);

            return File.ReadAllLines(fileName).ToList<string>().Select((line) => new HighScore(line)).ToList<HighScore>();
        }
    }

    class HighScore
    {
        public string Name { get; private set; }
        public int Score { get; private set; }

        public HighScore(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public HighScore(string data)
        {
            string[] contents = data.Split(' ');

            Name = contents[0];
            Score = int.Parse(contents[1]);
        }

        public override string ToString()
        {
            return Name + " " + Score.ToString();
        }
    }
}
