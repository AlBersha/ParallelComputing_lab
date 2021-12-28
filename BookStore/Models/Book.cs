using System;
using System.Threading;

namespace BookStore.Models
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Name { get; set; }       
        public bool IsAvailable { get; set; }
        public bool IsReadOnly { get; set; }

        public void Read()
        {
            Thread.Sleep(TimeToRead);
        }

        private static int TimeToRead => 3000;
    }
}