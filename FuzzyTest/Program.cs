using System;
using System.Collections.Generic;

namespace FuzzyTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var word = "Mohammed Hayder";
            var words = new List<string>
            {
                "Mohammed Hayder",
                "Mohammed Hayder",
                "mohammedhayder",
                "Mohammed Haider",
                "Mohamed Haider",
                "Mohanad Hayder"
            };

            var foundWords = Search(word, words, 0.70);

            foundWords.ForEach(foundWord => Console.WriteLine(foundWord));
            Console.ReadKey();
        }

        private static List<string> Search(
            string word,
            IEnumerable<string> wordList,
            double fuzzyness)
        {
            var foundWords = new List<string>();

            foreach (string s in wordList)
            {
                // Calculate the Levenshtein-distance:
                var levenshteinDistance =
                    LevenshteinDistance(word, s);

                // Length of the longer string:
                var length = Math.Max(word.Length, s.Length);

                // Calculate the score:
                var score = 1.0 - (double) levenshteinDistance / length;

                // Match?
                if (score > fuzzyness)
                    foundWords.Add(s);
            }

            return foundWords;
        }

        private static int LevenshteinDistance(string src, string dest)
        {
            var d = new int[src.Length + 1, dest.Length + 1];
            int i, j;
            var str1 = src.ToCharArray();
            var str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
            {
                d[i, 0] = i;
            }

            for (j = 0; j <= str2.Length; j++)
            {
                d[0, j] = j;
            }

            for (i = 1; i <= str1.Length; i++)
            {
                for (j = 1; j <= str2.Length; j++)
                {
                    int cost;
                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    d[i, j] =
                        Math.Min(
                            d[i - 1, j] + 1, // Deletion
                            Math.Min(
                                d[i, j - 1] + 1, // Insertion
                                d[i - 1, j - 1] + cost)); // Substitution

                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
                                               str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[str1.Length, str2.Length];
        }

       
    }
}