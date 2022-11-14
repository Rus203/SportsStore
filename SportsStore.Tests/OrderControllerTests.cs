using System;
using Xunit;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Microsoft.AspNetCore.Mvc;


namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Car()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Arrange 
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            ViewResult result = target.Checkout(order) as ViewResult;
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Assert
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Организация - создание корзины с одним элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Организация - создание экземпляра контроллера
            OrderController target = new OrderController(mock.Object, cart);
            // Действие - попытка перехода к оплате
            RedirectToActionResult result =
            target.Checkout(new Order()) as RedirectToActionResult;
            // Утверждение - проверка, что заказ был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            // Утверждение - проверка, что метод перенаправляется на действие Completed
            Assert.Equal("Completed", result.ActionName);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Организация - создание корзины с одним элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Организация - создание экземпляра контроллера
            OrderController target = new OrderController(mock.Object, cart);
            // Организация - добавление ошибки в модель
            target.ModelState.AddModelError("error", "error");
            // Действие - попытка перехода к оплате
            ViewResult result = target.Checkout(new Order()) as ViewResult;
            // Утверждение - проверка, что заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Утверждение - проверка, что метод возвращает стандартное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Утверждение - проверка, что представлению передается недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid); 
        }
    }
}
