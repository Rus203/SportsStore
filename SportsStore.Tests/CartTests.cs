using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportsStore.Models;
using Xunit;


namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Add_New_Lines()
        {
            //  Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //  Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 2);
            target.AddItem(p2, 1);

            target.AddItem(p1, 3);
            target.AddItem(p2, 1);

            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(5, results[0].Quantity);
            Assert.Equal(2, results[1].Quantity);
        }

        [Fact]
        public void Remove_Lines()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // Arrange
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 3);
            target.AddItem(p2, 1);
            target.AddItem(p3, 2);
            target.AddItem(p3, 1);
            target.RemoveLine(p3);

            //  Assert
            Assert.Equal(0, target.Lines.Count(p => p.Product == p3));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange
            Cart target = new Cart();

            //  Act
            target.AddItem(p1, 1);
            target.AddItem(p1, 2);
            target.AddItem(p2, 1);
            decimal commonCheck = target.ComputeTotalValue();

            //  Assert
            Assert.Equal(350M, commonCheck);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //  Arrange
            Cart target = new Cart();

            //  Act
            target.AddItem(p1, 2);
            target.AddItem(p2, 1);
            target.Clear();

            //  Assert
            Assert.Empty(target.Lines);
        }
    }
}
