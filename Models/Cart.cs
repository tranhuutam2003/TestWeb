﻿namespace TestWeb.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Books book, int quantity)
        {
            var item = Items.Find(i => i.Book.BookID == book.BookID);
            if (item == null)
            {
                Items.Add(new CartItem { Book = book, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(int bookId)
        {
            Items.RemoveAll(i => i.Book.BookID == bookId);
        }

        public decimal TotalAmount => Items.Sum(i => (i.Book.Price ?? 0) * i.Quantity);
    }
}
