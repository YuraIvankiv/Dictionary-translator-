using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class Translator
    {
        public Dictionary<UaDictionary, UsaDictionary> uaToUsaDictionary { set; get; }
        public Dictionary<UsaDictionary, UaDictionary> usaToUaDictionary { set; get; }
        private bool isNewWordAdded;
        public Translator()
        {
            uaToUsaDictionary = new Dictionary<UaDictionary, UsaDictionary>();
            usaToUaDictionary = new Dictionary<UsaDictionary, UaDictionary>();
            isNewWordAdded = false;
        }
        public void AddWordToDictionary(string UAword, string USAword)
        {
            AddUaToUsaWord(UAword, USAword);
            AddUsaToUaWord(USAword, UAword);
            isNewWordAdded = true;
            UpdateMainFile(USAword, UAword);
        }
        public void AddUaToUsaWord(string UAword, string USAword)
        {
            var uaWordObj = new UaDictionary(UAword);
            var usaWordObj = new UsaDictionary(USAword);
            if (!uaToUsaDictionary.ContainsKey(uaWordObj))
            {
                uaToUsaDictionary.Add(uaWordObj, usaWordObj);
            }
        }

        public void AddUsaToUaWord(string USAword, string UAword)
        {
            var usaWordObj = new UsaDictionary(USAword);
            var uaWordObj = new UaDictionary(UAword);
            if (!usaToUaDictionary.ContainsKey(usaWordObj))
            {
                usaToUaDictionary.Add(usaWordObj, uaWordObj);
            }
        }

        private void UpdateMainFile(string USAword, string UAword)//мтеод для запису нових слів в файл
        {
            if (isNewWordAdded)
            {
                string filePath = @"C:\Users\Admin\Desktop\Translation\File\Word.txt";
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"{USAword} - {UAword}");
                    }

                    Console.WriteLine("Слово та його переклад були додані до основного файлу!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                }
            }
        }

        public string TranslateTextToEnglish(string text)
        {
            ReplaceSpecialCharacters();
            StringBuilder translatedText = new StringBuilder();
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string lowerCaseWord = word.ToLower();

                UaDictionary uaSearchKey = new UaDictionary(lowerCaseWord);
                UsaDictionary usaSearchKey = new UsaDictionary(lowerCaseWord);

                if (uaToUsaDictionary.TryGetValue(uaSearchKey, out UsaDictionary usaTranslation))
                {
                    translatedText.Append(usaTranslation.Word);
                }
                else
                {
                    translatedText.Append(word);
                    translatedText.Append(" (not translated)");
                }
                translatedText.Append(' ');
            }
            return translatedText.ToString().Trim();
        }

        public void ReplaceSpecialCharacters()
        {//метод для літери і в юнікод немає української літери і і він її зчитує як ? для кореткного перекладу використовую цей метод який всі літери і які є в основному файлі заміняє на
            // ? так при перекладі з української на англійську буде находити переклад  без помилки інакше свіх слів які містят літеру і не убде знайдено
            Dictionary<UaDictionary, UsaDictionary> updatedDictionary = new Dictionary<UaDictionary, UsaDictionary>();
            foreach (var pair in uaToUsaDictionary)
            {
                string key = pair.Key.Word;
                string normalizedKey = key.Replace("і", "?");
                
                if (!key.Equals(normalizedKey))
                {
                    UaDictionary newKeyObj = new UaDictionary(normalizedKey);
                    updatedDictionary[newKeyObj] = pair.Value;
                }
                else
                {
                    updatedDictionary[pair.Key] = pair.Value;
                }
            }
            uaToUsaDictionary = updatedDictionary;
        }
        public string TranslateTextToUkrainian(string text)
        {
            StringBuilder translatedText = new StringBuilder();
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string lowerCaseWord = word.ToLower();

                UaDictionary uaSearchKey = new UaDictionary(lowerCaseWord);
                UsaDictionary usaSearchKey = new UsaDictionary(lowerCaseWord);

                bool uaTranslationFound = uaToUsaDictionary.ContainsKey(uaSearchKey);
                bool usaTranslationFound = usaToUaDictionary.ContainsKey(usaSearchKey);

                if (usaTranslationFound)
                {
                    translatedText.Append(usaToUaDictionary[usaSearchKey].Word);
                }
                else if (uaTranslationFound)
                {
                    translatedText.Append(uaToUsaDictionary[uaSearchKey].Word);
                }
                else
                {
                    translatedText.Append(word);
                    translatedText.Append(" (not translated)");
                }
                translatedText.Append(' ');
            }
            return translatedText.ToString().Trim();
        }

        public void ReadFromFile(string filePath, Encoding encoding)//метод для зчитування основного словника з файлу ідея в тому, якщо найти гарний повноційни словник з усіма відмінками
            //слів то його можна загружити за домопогою цього методу( врахуварши перні нюанси) і тоді переклад буде більш якісний 
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath, encoding);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string USAword = parts[0].Trim().ToLower();
                        string Uaword = parts[1].Trim().ToLower();
                        AddUsaToUaWord(USAword, Uaword);
                        AddUaToUsaWord(Uaword, USAword);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading from the file: {ex.Message}");
            }
        }
        public bool RemoveWordFromDictionary(string USAword)
        {
            UsaDictionary usaSearchKey = new UsaDictionary(USAword.ToLower());

            if (usaToUaDictionary.TryGetValue(usaSearchKey, out UaDictionary uaTranslation))
            {
                uaToUsaDictionary.Remove(uaTranslation);
                usaToUaDictionary.Remove(usaSearchKey);
                UpdateMainFileOnRemove(USAword);
                return true;
            }

            return false;
        }
        private void UpdateMainFileOnRemove(string USAword)//метод для видалення пари з файлу 
        {
            string filePath = @"C:\Users\Admin\Desktop\Translation\File\Word.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                List<string> updatedLines = new List<string>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string word = parts[0].Trim().ToLower();
                        if (word != USAword.ToLower())
                        {
                            updatedLines.Add(line);
                        }
                    }
                }

                File.WriteAllLines(filePath, updatedLines);

                Console.WriteLine($"Слово '{USAword}' та його переклад були видалені зі словника та з основного файлу!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
        public void DownloadDictionaryToFile()//Для скачування файлу по дефолту на робочий стіл
        {
            string filePath = @"C:\Users\Admin\Desktop\Translation\File\Word.txt";
            string downloadFilePath = @"C:\Users\Admin\Desktop\English language dictionary.txt";

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                using (StreamWriter writer = new StreamWriter(downloadFilePath))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("Словник був успiшно завантажений.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the dictionary: {ex.Message}");
            }
        }
        public void ShowText(string text, bool translateToUsa)
        {
            Console.WriteLine("Оригiнал тексту:");
            Console.WriteLine(text);
            if (translateToUsa)
            {
                Console.WriteLine("Переклад на англiйську:");
                Console.WriteLine(TranslateTextToEnglish(text));
            }
            else
            {
                Console.WriteLine("Переклад на українську:");
                Console.WriteLine(TranslateTextToUkrainian(text));
            }
        }
    }
}






