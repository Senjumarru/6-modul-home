using System;
using System.Threading.Tasks;
using DesignPatterns.Singleton;
using DesignPatterns.Builder;
using DesignPatterns.Prototype;

namespace DesignPatterns.Tests
{
    public class Tests
    {
        public static void RunSingletonTests()
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

        public static void RunBuilderTests()
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

        public static void RunPrototypeTests()
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

        public static void Main(string[] args)
        {
            RunSingletonTests();
            RunBuilderTests();
            RunPrototypeTests();
        }
    }
}
