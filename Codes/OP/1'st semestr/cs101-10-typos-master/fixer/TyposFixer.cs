using System;
using System.Collections.Generic;
using System.Text;

namespace fixer
{
	public class WordTypo
	{
		public WordTypo(string originalWord, string wordWithTypo)
		{
			OriginalWord = originalWord;
			WordWithTypo = wordWithTypo;
		}

		public string OriginalWord;
		public string WordWithTypo;
	}

	public class TyposFixer
	{
        char[,] keyboard = {{'1','2','3','4','5','6','7','8','9','0','-','=',' '},
                            {' ','q','w','e','r','t','y','u','i','o','p','[',']'},
                            {' ','a','s','d','f','g','h','j','k','l',';','\'', '\\'},
                            {' ','z','x','c','v','b','n','m',',','.','/',' ',' '}};
		/// <summary>
		/// тут вы можете собрать некоторую статистику про виды опечаток, которую потом использовать в методе FixWord
		/// </summary>
		/// <param name="typos">список пар "оригинальное слово" — "слово, которое набрал наборщик"</param>
        /// 

        public struct WordInRating
        {
            public WordInRating(String word, int count)
            {
                Word = word;
                Count = count;
            }

            public String Word;
            public int Count;
        }

        public class DictionaryComp : IComparer<WordInRating>
        {
            public int Compare(WordInRating x, WordInRating y)
            {
                return x.Count.CompareTo(y.Count);
            }
        }

        public class DictionaryComp2 : IComparer<WordInRating>
        {
            public int Compare(WordInRating x, WordInRating y)
            {
                return x.Word.CompareTo(y.Word);
            }
        }

        List<string> dictionary = new List<string>();
        List<WordInRating> rating = new List<WordInRating>();

        //---------------------------------//

        public void MakeRating()
        {
            var str = "";
            var count = 0;
            foreach (var i in dictionary)
            {               
                if (i == str)
                    count++;
                else
                {
                    if (count > 0)
                    {
                        rating.Add(new WordInRating(str, count));
                    }
                    str = i;
                    count = 1;
                }
            }
            if (count > 0)
            {
                rating.Add(new WordInRating(str, count));
            }
            var DC = new DictionaryComp2();
            rating.Sort(DC);
        }

		public void Learn(WordTypo[] typos)
		{
            foreach (var i in typos)
            {
                StringBuilder verbs = new StringBuilder(i.OriginalWord);
                if (verbs.Length >= 3)
                {
                    if (verbs[verbs.Length - 1] == 's')
                    {
                        verbs.Remove(verbs.Length - 1, 1);
                        dictionary.Add(verbs.ToString());
                    }
                    else
                    {
                        verbs.Insert(verbs.Length, 's');
                        dictionary.Add(verbs.ToString());
                    }
                }
                verbs = new StringBuilder(i.OriginalWord);
                if (verbs.Length >= 5)
                    if (verbs[verbs.Length - 1] == 'g' && verbs[verbs.Length - 2] == 'n' && verbs[verbs.Length - 3] == 'i')
                    {
                        verbs.Remove(verbs.Length - 3, 3);
                        verbs.Insert(verbs.Length, "ed");
                        dictionary.Add(verbs.ToString());
                    }
                verbs = new StringBuilder(i.OriginalWord);
                if (verbs.Length >= 4)
                    if (verbs[verbs.Length - 1] == 'd' && verbs[verbs.Length - 2] == 'e')
                    {
                        verbs.Remove(verbs.Length - 2, 2);
                        verbs.Insert(verbs.Length, "ing");
                        dictionary.Add(verbs.ToString());
                    }
                dictionary.Add(i.OriginalWord);
            }
            dictionary.Sort();
            MakeRating();
			//В первой задачи тут нужно составить словарь из переданных данных
			//Для второй задачи тут можно считать любую статистику про виды опечаток, которую потом использовать в методе FixWord
		}

		public string[] GetSimilarWords(string s)
		{
			// Первая задача
			return new string[0];
		}

        public string[] FindTypes(StringBuilder word, int i)
        {
            int hor = -1;
            int ver = -1;
            for (int j = 0; j < 4; ++j)
                for (int k = 0; k < 12; ++k)
                    if (keyboard[j, k] == word[i])
                    {
                        hor = j;
                        ver = k;
                        break;
                    }
            string hashWord = word.ToString();
            List<string> newWords = new List<string>();
            for (int j = Math.Max(hor - 1, 0); j <= Math.Min(hor + 1, 3); ++j)
                for (int k = Math.Max(ver - 1, 0); k <= Math.Min(ver + 1, 11); ++k)
                {
                    StringBuilder typeWord = new StringBuilder(hashWord);
                    typeWord[i] = keyboard[j, k];
                    newWords.Add(typeWord.ToString());
                }
            return newWords.ToArray();
        }

        public int CheckTypes(StringBuilder word)
        {
            var DC2 = new DictionaryComp2();
            int localRating = 0;
            int localNumber = -1;
            int number = -1;
            int hugeRating = 0;
            for (int i = 0; i < word.Length; ++i)
            {
                string[] newWords = FindTypes(word, i);
                for (int j = 0; j < newWords.Length; ++j)
                {
                    localNumber = rating.BinarySearch(new WordInRating(newWords[j], 0), DC2);
                    if (localNumber >= 0)
                    {
                        localRating = rating[localNumber].Count;
                        if (localRating > hugeRating)
                        {
                            hugeRating = localRating;
                            number = localNumber;
                        }
                    }
                }
            }
            return number;
        }

        public StringBuilder Swap(StringBuilder typo, int i, int j)
        {
            char c = typo[i];
            typo[i] = typo[j];
            typo[j] = c;
            return typo;
        }

        public int CheckSwapLetter(string word)
        {
            int localRating = 0;
            int localNumber = -1;
            int number = -1;
            int hugeRating = 0;
            for (int i = 0; i < word.Length - 1; ++i)
            {
                StringBuilder typeWord = new StringBuilder(word);
                typeWord = Swap(typeWord, i, i + 1);
                localNumber = rating.BinarySearch(new WordInRating(typeWord.ToString(), 0), new DictionaryComp2());
                if (localNumber >= 0)
                {
                    localRating = rating[localNumber].Count;
                    if (localRating > hugeRating)
                    {
                        hugeRating = localRating;
                        number = localNumber;
                    }
                }
            }
            return number;
        }

        public string ChekForNumberInWord(string word)
        {
            StringBuilder newWord = new StringBuilder(word);
            for (int i = 0; i < newWord.Length; ++i)
            {
                if (newWord[i] == '\\')
                    newWord[i] = 'z';
                if (newWord[i] == ';')
                    newWord[i] = 'l';
                if (newWord[i] == ',')
                    newWord[i] = 'm';
                if (newWord[i] == '[')
                    newWord[i] = 'p';
                for (int j = 0; j < 10; ++j)
                    if (newWord[i] == keyboard[0, j])
                        newWord[i] = keyboard[1, j + 1];
            }
            return newWord.ToString();
        }

        public string CheckLetter(string word, int i, char last, char newChar)
        {
            StringBuilder newWord = new StringBuilder(word);
            if (newWord[i] == last)
            {               
                newWord[i] = newChar;
                if (rating.BinarySearch(new WordInRating(newWord.ToString(), 0), new DictionaryComp2()) >= 0)
                    return newWord.ToString();
            }
            return null;
        }

        char[] lastLetter = { 'w', 'v', 'y', 'i', 'c', 'k', 'w', 'u', 'h', 'n' };    // фонетические ошибки
        char[] nextLetter = { 'v', 'w', 'i', 'y', 'k', 'c', 'u', 'w', 'n', 'h' };    // фонетические ошибки

        public string CheckPfoneticMistakes(string word)
        {
            string MayBeAnswer = null;
            for (int i = 0; i < word.Length; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    MayBeAnswer = CheckLetter(word, i, lastLetter[j], nextLetter[j]);
                    if (MayBeAnswer != null)
                        return MayBeAnswer;
                }
            }
            return null;
        }

/*        char[] soglasnie = { 'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };

        public string ChekManyLetter(string word)
        {
            StringBuilder newWord = new StringBuilder(word);
            int count = 0;
            for (int i = 0; i < newWord.Length; ++i)
            {
                for (int j = 0; j < soglasnie.Length; ++j)
                {
                    if (newWord[i] == soglasnie[i])
                    {
                        count++;
                        break;
                    }
                    if (j == soglasnie.Length - 1)
                        count--;
                }
                if (Math.Abs(count) >= 4)
                {
                    int hor = -1;
                    int ver = -1;
                    for (int l = 0; l < 4; ++l)
                        for (int k = 0; k < 12; ++k)
                            if (keyboard[l, k] == newWord[i - 2])
                            {
                                hor = l;
                                ver = k;
                                break;
                            }
                    for (int j = Math.Max(hor - 1, 0); j <= Math.Min(hor + 1, 3); ++j)
                        for (int k = Math.Max(ver - 1, 0); k <= Math.Min(ver + 1, 11); ++k)
                        {
                            for (int p = 0; p < soglasnie.Length; ++p)
                            {
                                if (keyboard[j, k] == soglasnie[p])
                                    break;
                                if (p == soglasnie.Length - 1)
                                {
                                    newWord[i - 2] = keyboard[j, k];
                                    return newWord.ToString();
                                }
                            }
                        }
                }
            }
            return word;
        }*/

        public string CheckOtherMistakes(string word)
        {
            string answer = ChekForNumberInWord(word);
            string MayBeAnswer = CheckPfoneticMistakes(answer);
            if (MayBeAnswer != null)
                return MayBeAnswer;
            //MayBeAnswer = ChekManyLetter(answer);
            return answer;
        }

        Dictionary<string, int> newDictionary = new Dictionary<string,int>();

        public string FixWord(string word)
		{
            if (newDictionary.ContainsKey(word))
                newDictionary[word]++;
            else
                newDictionary.Add(word, 1);
            if (newDictionary[word] >= 4)
                return word;
            if (rating.BinarySearch(new WordInRating(word, 0), new DictionaryComp2()) >= 0)
                return word;
			// Вторая задача
            StringBuilder newWord = new StringBuilder(word);
            int number1 = CheckTypes(newWord);
            int number2 = CheckSwapLetter(word);
            int rating1 = -1;
            int rating2 = -1;

            if (number1 >= 0)
                rating1 = rating[number1].Count;
            if (number2 >= 0)
                rating2 = rating[number2].Count;

            int number;
            if (rating1 > rating2)
                number = number1;
            else
                number = number2;

            if (number >= 0)
                return rating[number].Word;
            else
                return CheckOtherMistakes(word);
            
		}

		public static void Main(string[] args)
		{
			//Если вы решаете первую задачу, напишите тут код, демонстрирующий работоспособность метода GetSimilarWords.

			//Если вы решаете вторую задачу, то раскомментируйте эту строку:
			AccuracyTester.Test(args);
		}

	}
}