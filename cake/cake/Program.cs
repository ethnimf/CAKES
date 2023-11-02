using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace CakeOrder
{
    public class OrderApp
    {
        public static string path = "C:\\Users\\User\\Desktop\\cakes.txt"; 
        public static int Price = 0;
        public static int SelectedMenuIndex = -1;
        public static bool Active = false;
        public static int position = 4;
        public static List<string> MenuTitles = new List<string>()
        {
            "Форма торта",
            "Размер торта",
            "Вкус коржей",
            "Кол-во коржей",
            "Глазурь торта",
            "Конец заказа (backspace)"
        };

        public static List<Settings> SettingsCake = new List<Settings>();

        public static Dictionary<string, List<Settings>> MyMenu = new Dictionary<string, List<Settings>>()
        {
            {"Форма торта", new List<Settings>{new Settings("Куб", 100), new Settings("Ромб", 200), new Settings("Круг", 300), new Settings("Триугольник", 400) } },
            {"Кол-во коржей", new List<Settings> { new Settings("Один ярус", 100), new Settings("Два яруса", 200), new Settings("Три яруса", 300), new Settings("Четыре яруса", 400) } },
            {"Вкус коржей", new List<Settings> { new Settings("Бисквитный", 100), new Settings("Медовый", 200), new Settings("Песочный", 300), new Settings("Шоколадный", 400) } },
            {"Размер торта", new List<Settings>{ new Settings("Большой", 100), new Settings("Средний", 200), new Settings("Маленький", 300), new Settings("Бенто", 400) } },
            {"Глазурь торта", new List<Settings>{ new Settings("Кокосовая", 100), new Settings("Шоколадная", 200), new Settings("Ванильная", 300), new Settings("Манго", 400) } },
        };

        static void Main()
        {
            try
            {
                Page("StartMenu");
                SelectedMenuIndex = -1;
                int pos = 3;
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.DownArrow && pos != 9)
                    {
                        SelectedMenuIndex++;
                        pos++;
                    }
                    else if (key.Key == ConsoleKey.UpArrow && pos != 4)
                    {

                        pos--;
                        SelectedMenuIndex--;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        try
                        {
                            //File.AppendAllText("log.txt", "текст1");


                            string text = JsonConvert.SerializeObject(SettingsCake);
                            string Date = $"{DateTime.Now.Month}.{DateTime.Now.Day} {DateTime.Now.Hour}:{DateTime.Now.Minute}";
                            if (File.Exists(path))
                            {
                                File.AppendAllText(path, $"{JsonConvert.SerializeObject(Date)}" + Environment.NewLine);
                                foreach(var item in SettingsCake)
                                {
                                    File.AppendAllText(path, $" {item.NameItem} - {item.Price}");
                                }
                                File.AppendAllText(path, $"Итоговая стоимость: {Price}");
                                Page("EndCake");
                            }
                            else
                            {
                                File.Create(path);
                                File.AppendAllText(path, JsonConvert.SerializeObject(DateTime.Now.Hour));
                                File.AppendAllText(path, JsonConvert.SerializeObject(DateTime.Now.Minute));
                                File.AppendAllText(path, JsonConvert.SerializeObject(SettingsCake));
                                Console.ReadLine();
                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); return; }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Page("SettingCakeMenu");
                        return;
                    }

                    Page("StartMenu");


                    Console.SetCursorPosition(0, pos);
                    Console.WriteLine("->");
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return; }

        }

        public static void Page(string page)
        {
            switch (page)
            {
                case "StartMenu":
                    Console.Clear();
                    var i = 4;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("“Торт-рай: ваш сладкий выбор и приключение ждут вас здесь!”");
                    Console.WriteLine("Кастомизация торта:");
                    Console.WriteLine();
                    foreach (var item in MenuTitles)
                    {
                        Console.SetCursorPosition(3, i++);
                        Console.WriteLine(item);

                    }
                    Console.SetCursorPosition(0, MenuTitles.Count + 5);
                    Console.WriteLine($"Итог: {Price}");
                    Console.WriteLine($"Торт будет сделан из:");
                    foreach (var item in SettingsCake)
                    {
                        Console.WriteLine($"  {item.NameItem} - {item.Price} |");
                    }
                    break;
                case "SettingCakeMenu":
                    Console.Clear();
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Чтобы выйти нажмите ESCAPE");
                    Console.WriteLine("Выберите пункт из меню:");
                    Console.WriteLine("----------------------------------------");
                    i = 4;
                    foreach (var item in MyMenu[MenuTitles[SelectedMenuIndex]])
                    {
                        Console.SetCursorPosition(3, i++);
                        Console.WriteLine($"{item.NameItem} - {item.Price}");
                    }
                    Console.SetCursorPosition(0, position);
                    Console.WriteLine("->");
                    while (true)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.DownArrow)
                        {
                            position++;
                        }
                        else if (key.Key == ConsoleKey.UpArrow && position != 4)
                        {

                            position--;

                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            SettingsCake.Add(new Settings(MyMenu[MenuTitles[SelectedMenuIndex]][position - 4].NameItem, MyMenu[MenuTitles[SelectedMenuIndex]][position - 4].Price));
                            Price += MyMenu[MenuTitles[SelectedMenuIndex]][position - 4].Price;


                            Main();
                            return;
                        }
                        Page("SettingCakeMenu");
                        return;
                    }
                    break;
            }
        }
        public class Settings
        {
            public string NameItem { get; set; }
            public int Price { get; set; }
            public Settings(string name, int price)
            {
                NameItem = name;
                Price = price;
            }
        }
    }
}
