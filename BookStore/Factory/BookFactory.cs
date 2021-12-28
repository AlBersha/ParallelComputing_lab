using System.Collections.Generic;
using BookStore.Models;

namespace BookStore.Factory
{
    public class BookFactory
    {
        public static List<Book> GetAllBooks()
        {
            return new List<Book>
            {
                new() { },
                new() { },
                new() { },
                new() { },
                new() { },
            };
        }
    }
}