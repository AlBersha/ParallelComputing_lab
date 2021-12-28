using System;
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
                new() {BookId = Guid.Parse("f0d4c243-cb67-4a78-9a7f-919a5ae17eaf"), Name = "Harry potter and the philosopher's stone", IsReadOnly = true},
                new() {BookId = Guid.Parse("ef4f9db0-10f1-4fa3-a916-54fba8d56387"), Name = "Star wars", IsReadOnly = true},
                new() {BookId = Guid.Parse("7ccd79e7-70a6-4254-b923-f56df6fb32ff"), Name = "Why we sleep", IsReadOnly = false},
                new() {BookId = Guid.Parse("008defe1-185d-465f-b24c-a6c718745e87"), Name = "Pride and prejudice", IsReadOnly = false},
                new() {BookId = Guid.Parse("277903e0-25f2-4a46-9422-8bdf905d13ff"), Name = "Sense and sensibility", IsReadOnly = false},
            };
        }
    }
}