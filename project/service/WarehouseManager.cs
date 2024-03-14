using project.dao;

using System;
using System.Collections.Generic;

namespace project.entity
{
    public class WarehouseManager
    {
        private ProductDao productDao;
        private RackDao rackDao;
        private OrderDao orderDao;

        public WarehouseManager(ProductDao productDao, RackDao rackDao, OrderDao orderDao)
        {
            this.productDao = productDao;
            this.rackDao = rackDao;
            this.orderDao = orderDao;
        }

        // Метод для добавления товара на склад
        public void AddProduct(Product product)
        {
            productDao.AddProduct(product);
        }

        // Метод для выполнения поставки товара
        public void PerformSupply(int productId, int quantity)
        {
            // Логика поставки товара
            // Например, увеличение количества товара в базе данных
            Product product = productDao.GetById(productId);
            product.Quantity += quantity;
            productDao.UpdateProduct(product);
        }

        // Метод для выполнения отгрузки товара
        public void PerformShipment(int productId, int quantity)
        {
            // Логика отгрузки товара
            // Например, уменьшение количества товара в базе данных
            Product product = productDao.GetById(productId);
            product.Quantity -= quantity;
            productDao.UpdateProduct(product);
        }

        // Метод для размещения товара на складе
        public void PlaceProduct(int productId, int rackId)
        {
            // Логика размещения товара на стеллаже
            rackDao.PlaceProductOnRack(productId, rackId);
        }

        // Метод для вывода отчета по наличию и размещению товаров
        public void GenerateInventoryReport()
        {
            // Получение всех товаров с их количеством
            List<Product> products = productDao.GetAllProducts();

            // Вывод отчета
            Console.WriteLine("Отчет по наличию и размещению товаров:");

            foreach (var product in products)
            {
                Console.WriteLine($"Товар: {product.Name}, Категория: {product.Category}, Количество: {product.Quantity}");

                // Построение гистограммы для количества товаров
                Console.Write("Гистограмма: ");
                for (int i = 0; i < product.Quantity; i++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        public void GenerateOrderReport(DateTime month)
        {
            // Получение всех заказов за указанный месяц
            List<Order> orders = orderDao.GetAllOrders().Where(order => order.OrderDate.Month == month.Month && order.OrderDate.Year == month.Year).ToList();

            // Вывод отчета
            Console.WriteLine($"Отчет по поставке и отгрузке товара за {month.ToString("MMMM yyyy")}:");
            foreach (var order in orders)
            {
                Console.WriteLine($"Заказ ID: {order.Id}, Товар ID: {order.ProductId}, Количество: {order.Quantity}, Дата: {order.OrderDate}, Тип заказа: {order.OrderType}");

                // Построение гистограммы для количества товаров в заказе
                Console.Write("Гистограмма: ");
                for (int i = 0; i < order.Quantity; i++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }
    }

}


   


