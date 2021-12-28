using BookStore.Models;

namespace BookStore.Messages
{
    public class ReturnBook
    {
        public Book Book { get; set; }
        public Reader Reader { get; set; }
    }
}