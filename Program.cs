using Translation;

namespace Translation
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            Translator translator = new Translator();
            menu.ShowMainMenu();
        }
    }
}
