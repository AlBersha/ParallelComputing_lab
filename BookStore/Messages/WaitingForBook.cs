using BookStore.Models;

namespace BookStore.Messages
{
    public class WaitingForBook
    {
        public Reader Reader { get; set; }
        public Book Book { get; set; }
    }
}