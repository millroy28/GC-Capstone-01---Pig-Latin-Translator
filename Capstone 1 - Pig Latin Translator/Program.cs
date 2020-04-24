using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Capstone_1___Pig_Latin_Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool userCont;
            Console.WriteLine("Welcome to the Pig Latin Translator!");

            do
            {
                string userPhrase = GetUserString("Please enter a word or phrase you would like translated:");

                string translatedPhrase = PigLatinator(userPhrase);

                Console.WriteLine($"\n\nYour translated phrase is:\n {translatedPhrase}\n\n");
                
                userCont = GetYesOrNo("Would you like to translate another phrase? y/n");
            } while (userCont);

            Console.WriteLine("Goodbye!");
        }
        
        public static string GetUserString(string prompt)
        {
            /*  Outputs prompt to console, reads user input, 
             *  gives error and prompts again if input is null,
             *  returns string with user input. */

            Console.WriteLine(prompt);
            string input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("I'm sorry, it appears you didn't enter anything.");
                return GetUserString(prompt);
            }
            else
                return input;
        }
        public static string PigLatinator(string phrase)
        {
            /*  Takes phrase to be translated to pig latin, 
             *  calls several helper methods to translate each word in phrase */

            string[] splitPhrase = phrase.Split();
            int phraseLength = splitPhrase.Length;
            string[] translatedWords = new string[phraseLength];
            string translatedPhrase = "";
            
            for(int i = 0; i < phraseLength; i++)
            {
                if (ContainsSpecialChars(splitPhrase[i]))
                    translatedWords[i] = splitPhrase[i];
                else if (VowelCheck(splitPhrase[i]))
                    translatedWords[i] = EnglishToPigLatinWordTranslator(splitPhrase[i], true);
                else 
                    translatedWords[i] = EnglishToPigLatinWordTranslator(splitPhrase[i], false);

                translatedPhrase += translatedWords[i] + " ";
            }

            return translatedPhrase;            
        }
        public static string EnglishToPigLatinWordTranslator(string word, bool vowelLeads)
        {
            /* Translates a word according to rules of pig latin
             * depending on value of vowelLeads. Performs translation in
             * three sections: translation, mantaining capitalization, maintaining punctuation*/
            
            string[] wordAndPunctuation = HasPunctuation(word); 
            string wordOnly = wordAndPunctuation[0];
            string punctuation = wordAndPunctuation[1];

            int caseType = WordCaseCount(wordOnly);
            
            wordOnly = wordOnly.ToLower();
            char[] letters = wordOnly.ToCharArray();
            int wordLength = letters.Length;
            
            string translatedWord;

            // TRANSLATION
            if (vowelLeads)
            {
                translatedWord = wordOnly + "way";
            } 
            else
            {
                int lettersToVowel = 0;
                foreach(char letter in letters)
                {
                    if (!VowelCheck(letter))
                        lettersToVowel++;
                    else
                        break;
                }

                string leadConsonants = wordOnly.Substring(0, lettersToVowel);
                string newWordBegin = wordOnly.Substring(lettersToVowel, (wordLength - lettersToVowel));
                translatedWord = newWordBegin + leadConsonants + "ay";
            }

            // CAPITALIZATION
            if (caseType == 1)
            {
                char[] translatedLetters = translatedWord.ToCharArray();
                int newWordLength = translatedLetters.Length;
                translatedLetters[0] = char.ToUpper(translatedLetters[0]);
                translatedWord = char.ToString(translatedLetters[0]) + translatedWord.Substring(1, newWordLength - 1);
            }
            else if (caseType >= 2)
            {
                translatedWord = translatedWord.ToUpper();
            }

            // PUNCTUATION
            if (!string.IsNullOrEmpty(punctuation))
                translatedWord = translatedWord + punctuation;

            return translatedWord;
        }
        public static int WordCaseCount(string word)
        {   //Takes in a string, returns the number of uppercase letters in that string

            int caseReturn = 0;
            char[] letters = word.ToCharArray();

            foreach(char letter in letters)
            {
                if (Char.IsUpper(letter))
                    caseReturn++;
            }
            
            return caseReturn;
        }
        public static bool ContainsSpecialChars(string word)
        {
            // If any characters in the string are special characters, excepting the final character, returns true
            
            char[] letters = word.ToCharArray();
            bool specialCharacterPresent = false;

            for(int i = 0; i < (letters.Length - 1); i++)  
            {
                if (!Char.IsLetter(letters[i]) && letters[i] != '\'')
                    specialCharacterPresent = true;
            }

            return specialCharacterPresent;
        }
        public static string[] HasPunctuation(string word)
        {
            /* Returns a string array in two parts:
             * Element 0: if the string ended in a special character,
                the string minus the special character,
                otherwise, just the original string
             * Element 1: the special character, if present*/
          
            char[] letters = word.ToCharArray();
            int lastChar = letters.Length - 1;
            string[] wordAndPunctuation = new string[2];

            if(Char.IsLetter(letters[lastChar]))
            {
                wordAndPunctuation[0] = word;
            }
            else
            {
                wordAndPunctuation[0] = word.Substring(0, lastChar);
                wordAndPunctuation[1] = word.Substring(lastChar);
            }

            return wordAndPunctuation;
        }
        public static bool VowelCheck(string word)
        {
            // returns true if the string provided begins with a vowel

            char[] vowels = { 'a' , 'e' , 'i' , 'o' , 'u' };
            char[] letters = word.ToLower().ToCharArray();
            bool vowelAtStart = false;

            foreach(char vowel in vowels)
            {
                if (letters[0] == vowel)
                    vowelAtStart = true;
            }
            
            return vowelAtStart;
        }
        public static bool VowelCheck(char letter)
        {
            // returns true if the char provided is a vowel

            char[] vowels = { 'a' , 'e' , 'i' , 'o' , 'u' };
            bool isVowel = false;

            foreach(char vowel in vowels)
            {
                if (letter == vowel)
                    isVowel = true;
            }
            
            return isVowel;
        }
        public static bool GetYesOrNo(string prompt)
        {
            /* Prints prompt string to console and gets user input.
             * If input is other than y or n, gives error message and prompts again.
               Returns true for y and false for n*/

            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine().ToLower();

                if (input == "y")
                    return true;
                else if (input == "n")
                    return false;
                else
                    Console.WriteLine("I'm sorry, I didn't get that.");
            }
        }

    }
}
