using project.dao;
using project.entity;

using System;

namespace project
{
    // Класс для управления консольным интерфейсом
    public class ConsoleInterface
    {
        private static string server = "localhost";
        private static string database = "project";
        private static string uid = "root";
        private static string password = "jmXzj3eV";
        private static string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";

        private static ProductDao productDao = new ProductDao(connectionString);
        private static OrderDao orderDao = new OrderDao(connectionString);
        private static RackDao rackDao = new RackDao(connectionString);
        private WarehouseManager manager = new WarehouseManager(productDao, rackDao, orderDao);

        // Метод для запуска консольного приложения
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить товар на склад");
                Console.WriteLine("2. Выполнить поставку товара");
                Console.WriteLine("3. Выполнить отгрузку товара");
                Console.WriteLine("4. Разместить товар на стеллаже");
                Console.WriteLine("5. Вывести отчет по наличию и размещению товаров");
                Console.WriteLine("6. Вывести отчет по поставке и отгрузке товара за указанный месяц");
                Console.WriteLine("7. Добавить стеллаж");
                Console.WriteLine("8. Посмотреть стеллажи");
                Console.WriteLine("9. Посмотреть продукты");
                Console.WriteLine("10.Посмотреть заказы");
                Console.WriteLine("11.Создать заказ");
                Console.WriteLine("0. Выйти");
                Console.Write("Введите номер действия: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        PerformSupply();
                        break;
                    case "3":
                        PerformShipment();
                        break;
                    case "4":
                        PlaceProductOnRack();
                        break;
                    case "5":
                        GenerateInventoryReport();
                        break;
                    case "6":
                        GenerateOrderReport();
                        break;
                    case "7":
                        AddRack();
                        break;
                    case "8":
                        DisplayRacks();
                        break;
                    case "9":
                        DisplayProducts();
                        break;
                    case "10":
                        DisplayOrders();
                        break;
                    case "11":
                        CreateOrder();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите существующее действие.");
                        break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            Console.Write("Введите категорию товара: ");
            string category = Console.ReadLine();
            Console.Write("Введите количество товара: ");
            int quantity = int.Parse(Console.ReadLine());
            Console.Write("Введите минимальное рекомендуемое количество товара: ");
            int minQuantity = int.Parse(Console.ReadLine());

            Product product = new Product
            {
                Name = name,
                Category = category,
                Quantity = quantity,
                MinQuantity = minQuantity
            };

            manager.AddProduct(product);
            Console.WriteLine("Товар успешно добавлен на склад.");
        }
        private void CreateOrder()
        {
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Введите количество товара для заказа: ");
            int quantity = int.Parse(Console.ReadLine());

            // Создаем заказ
            Order order = new Order
            {
                ProductId = productId,
                Quantity = quantity,
                OrderDate = DateTime.Now, // Используем текущую дату для заказа
                OrderType = "ORDER" // Укажите тип заказа, например, "Поставка" или "Отгрузка"
            };

            // Добавляем заказ в базу данных
            orderDao.AddOrder(order);

            Console.WriteLine("Заказ успешно создан.");
        }

        private void PerformSupply()
        {
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Введите количество товара для поставки: ");
            int quantity = int.Parse(Console.ReadLine());

            manager.PerformSupply(productId, quantity);
            Console.WriteLine("Поставка товара выполнена успешно.");
        }
        private void AddRack()
        {
            Console.Write("Введите местоположение стеллажа: ");
            string location = Console.ReadLine();
            Console.Write("Введите вместимость стеллажа: ");
            int capacity = int.Parse(Console.ReadLine());

            Rack rack = new Rack
            {
                Location = location,
                Capacity = capacity
            };

            rackDao.AddRack(rack);
            Console.WriteLine("Стеллаж успешно добавлен на склад.");
        }
        private void PerformShipment()
        {
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Введите количество товара для отгрузки: ");
            int quantity = int.Parse(Console.ReadLine());

            manager.PerformShipment(productId, quantity);
            Console.WriteLine("Отгрузка товара выполнена успешно.");
        }

        private void PlaceProductOnRack()
        {
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Введите ID стеллажа: ");
            int rackId = int.Parse(Console.ReadLine());

            manager.PlaceProduct(productId, rackId);
            Console.WriteLine("Товар размещен на стеллаже.");
        }

        private void GenerateInventoryReport()
        {
            manager.GenerateInventoryReport();
            Console.WriteLine("Отчет по наличию и размещению товаров сгенерирован.");
        }

        private void GenerateOrderReport()
        {
            Console.Write("Введите месяц (число от 1 до 12): ");
            int month = int.Parse(Console.ReadLine());

            manager.GenerateOrderReport(new DateTime(DateTime.Now.Year, month, 1));
            Console.WriteLine("Отчет по поставке и отгрузке товара за указанный месяц сгенерирован.");
        }
        
        private void DisplayRacks()
        {
            var racks = rackDao.GetAllRacks();
            if (racks.Count == 0)
            {
                Console.WriteLine("На складе нет стеллажей.");
            }
            else
            {
                Console.WriteLine("Список стеллажей на складе:");
                foreach (var rack in racks)
                {
                    Console.WriteLine($"ID: {rack.Id}, Местоположение: {rack.Location}, Вместимость: {rack.Capacity}");
                }
            }
        }

        private void DisplayProducts()
        {
            var products = productDao.GetAllProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("На складе нет товаров.");
            }
            else
            {
                Console.WriteLine("Список товаров на складе:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Название: {product.Name}, Категория: {product.Category}, Количество: {product.Quantity}, Мин. количество: {product.MinQuantity}");
                }
            }
        }

        private void DisplayOrders()
        {
            var orders = orderDao.GetAllOrders();
            if (orders.Count == 0)
            {
                Console.WriteLine("На складе нет заказов.");
            }
            else
            {
                Console.WriteLine("Список заказов на складе:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"ID: {order.Id}, ID товара: {order.ProductId}, Количество: {order.Quantity}, Дата: {order.OrderDate}, Тип заказа: {order.OrderType}");
                }
            }
        }
    }
}
