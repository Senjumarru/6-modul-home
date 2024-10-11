using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DesignPatterns
{
    // Singleton
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, string> _settings;

        private ConfigurationManager()
        {
            _settings = new Dictionary<string, string>();
        }

        public static ConfigurationManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationManager();
                    }
                }
            }
            return _instance;
        }

        public void LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Настройки не найдены.");

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    _settings[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        public string GetSetting(string key)
        {
            if (_settings.ContainsKey(key))
                return _settings[key];

            throw new KeyNotFoundException($"Настройка '{key}' не найдена.");
        }

        public void SetSetting(string key, string value)
        {
            _settings[key] = value;
        }
    }

    // Builder
    public interface IReportBuilder
    {
        void SetHeader(string header);
        void SetContent(string content);
        void SetFooter(string footer);
        Report GetReport();
    }

    public class TextReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = header;
        }

        public void SetContent(string content)
        {
            _report.Content = content;
        }

        public void SetFooter(string footer)
        {
            _report.Footer = footer;
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class HtmlReportBuilder : IReportBuilder
    {
        private Report _report = new Report();

        public void SetHeader(string header)
        {
            _report.Header = "<h1>" + header + "</h1>";
        }

        public void SetContent(string content)
        {
            _report.Content = "<p>" + content + "</p>";
        }

        public void SetFooter(string footer)
        {
            _report.Footer = "<footer>" + footer + "</footer>";
        }

        public Report GetReport()
        {
            return _report;
        }
    }

    public class Report
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }

        public override string ToString()
        {
            return $"{Header}\n{Content}\n{Footer}";
        }
    }

    public class ReportDirector
    {
        public void ConstructReport(IReportBuilder builder)
        {
            builder.SetHeader("Отчет");
            builder.SetContent("Содержимое отчета.");
            builder.SetFooter("Подвал отчета.");
        }
    }

    // Prototype
    public class Product : ICloneable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class Order : ICloneable
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; }

        public object Clone()
        {
            var clonedOrder = (Order)MemberwiseClone();
            clonedOrder.Products = new List<Product>();

            foreach (var product in Products)
            {
                clonedOrder.Products.Add((Product)product.Clone());
            }

            return clonedOrder;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RunSingletonTests();
            RunBuilderTests();
            RunPrototypeTests();
        }

        static void RunSingletonTests()
        {
            Console.WriteLine("=== Тестирование Одиночки (Singleton) ===");
            var configManager1 = ConfigurationManager.GetInstance();
            var configManager2 = ConfigurationManager.GetInstance();

            Console.WriteLine($"Экземпляр 1: {configManager1.GetHashCode()}");
            Console.WriteLine($"Экземпляр 2: {configManager2.GetHashCode()}");
            Console.WriteLine("Экземпляры одинаковые? " + (configManager1 == configManager2));

            try
            {
                configManager1.LoadSettings("config.txt"); // Путь к файлу с настройками
                Console.WriteLine("Настройка A: " + configManager1.GetSetting("SettingA"));
                Console.WriteLine("Настройка B: " + configManager1.GetSetting("SettingB"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            // Тестирование потокобезопасности
            Parallel.For(0, 10, (i) =>
            {
                var instance = ConfigurationManager.GetInstance();
                Console.WriteLine($"Экземпляр {i}: {instance.GetHashCode()}");
            });
        }

        static void RunBuilderTests()
        {
            Console.WriteLine("\n=== Тестирование Строителя (Builder) ===");
            var director = new ReportDirector();

            IReportBuilder textBuilder = new TextReportBuilder();
            director.ConstructReport(textBuilder);
            Report textReport = textBuilder.GetReport();
            Console.WriteLine("\nТекстовый отчет:\n" + textReport);

            IReportBuilder htmlBuilder = new HtmlReportBuilder();
            director.ConstructReport(htmlBuilder);
            Report htmlReport = htmlBuilder.GetReport();
            Console.WriteLine("\nHTML отчет:\n" + htmlReport);
        }

        static void RunPrototypeTests()
        {
            Console.WriteLine("\n=== Тестирование Прототипа (Prototype) ===");
            var order1 = new Order
            {
                ShippingCost = 10.0m,
                Discount = 5.0m,
                PaymentMethod = "Credit Card"
            };
            order1.Products.Add(new Product { Name = "Товар 1", Price = 100.0m });
            order1.Products.Add(new Product { Name = "Товар 2", Price = 200.0m });

            var order2 = (Order)order1.Clone();
            order2.PaymentMethod = "PayPal"; // Изменяем только метод оплаты

            Console.WriteLine($"Order 1 Payment Method: {order1.PaymentMethod}");
            Console.WriteLine($"Order 2 Payment Method: {order2.PaymentMethod}");
            Console.WriteLine($"Order 1 Products Count: {order1.Products.Count}");
            Console.WriteLine($"Order 2 Products Count: {order2.Products.Count}");
        }
    }
}
