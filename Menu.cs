using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class Menu
    {
        private Translator translator;
        public Menu()
        {
            string filePath = @"C:\Users\Admin\Desktop\Translation\File\Word.txt";
            translator = new Translator();
            translator.ReadFromFile(filePath, Encoding.UTF8);
        }
        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("Головне меню:");
                Console.WriteLine("1. Перекласти текст на англiйську");
                Console.WriteLine("2. Перекласти текст на українську");
                Console.WriteLine("3. Додати слово в словник");
                Console.WriteLine("4. Видалити слово зi словника");
                Console.WriteLine("5. Скачати словник");
                Console.WriteLine("6. Завершити роботу програми");
                Console.Write("Введiть номер опцiї: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Clear();
                        TranslateToEnglish();
                        break;
                    case "2":
                        Console.Clear();
                        TranslateToUkrainian();
                        break;
                    case "3":
                        Console.Clear();
                        AddWordToDictionary();
                        break;
                    case "4":
                        Console.Clear();
                        RemoveWordFromDictionary();
                        break;
                    case "5":
                        Console.Clear();
                        DownloadDictionary();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Невiрний номер опцiї. Будь ласка, спробуйте ще раз.");
                        break;
                }
            }
        }
        private void TranslateToEnglish()
        {
            Console.Write("Введiть текст для перекладу на англiйську: ");
            string text = Console.ReadLine();
            translator.ShowText(text, translateToUsa: true);
        }
        private void TranslateToUkrainian()
        {
            Console.Write("Введiть текст для перекладу на українську: ");
            string text = Console.ReadLine();
            translator.ShowText(text, translateToUsa: false);
        }
        private void AddWordToDictionary()
        {
            Console.Write("Введiть слово на англiйськiй: ");
            string usaWord = Console.ReadLine();
            Console.Write("Введiть переклад слова на українську: ");
            string uaWord = Console.ReadLine();
            translator.AddWordToDictionary(uaWord, usaWord);
        }
        private void RemoveWordFromDictionary()
        {
            Console.Write("Введiть слово на англiйськiй для видалення зi словника: ");
            string usaWord = Console.ReadLine();
            if (translator.RemoveWordFromDictionary(usaWord))
            {
                Console.WriteLine($"Слово '{usaWord}' та його переклад були успiшно видаленi зi словника та з основного файлу.");
            }
            else
            {
                Console.WriteLine($"Слово '{usaWord}' не знайдено в словнику або в основному файлi.");
            }
        }
        private void DownloadDictionary()
        {
            translator.DownloadDictionaryToFile();
        }
    }
}

