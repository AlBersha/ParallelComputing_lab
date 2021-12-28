using System;
using BookStore.Models;

namespace BookStore.Messages
{
    public class RequestBook
    {
        public Guid BookId { get; set; }
        public bool IsReadOnly { get; set; }
        public Reader Reader { get; set; }
    }
}