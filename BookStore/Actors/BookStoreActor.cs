using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.IO;
using BookStore.Factory;
using BookStore.Messages;
using BookStore.Models;

namespace BookStore.Actors
{
    public class BookStoreActor: ReceiveActor
    {
        private List<WaitingReader> WaitingReaders { get; } = new();
        private Dictionary<Guid, Book> Books { get; }

        public BookStoreActor()
        {
            Books = BookFactory.GetAllBooks().ToDictionary(x => x.BookId);

            Receive<RequestBook>(req =>
            {
                var book = Books[req.BookId];

                if (book.IsAvailable)
                {
                    Sender.Tell(new ReceiveBook { Book = book });
                    book.IsAvailable = false;
                }
                else
                {
                    WaitingReaders.Add(new WaitingReader{Book = book, Reader = req.Reader, ActorRef = Sender});
                    Sender.Tell(new WaitingReader{Book = book, Reader = req.Reader});
                }
            });

            Receive<ReturnBook>(req =>
            {
                var waitingReader = WaitingReaders.FirstOrDefault(x => x.Book == req.Book);

                if (waitingReader == null)
                {
                    req.Book.IsAvailable = true;
                    return;
                }

                WaitingReaders.Remove(waitingReader);
                waitingReader.ActorRef.Tell(new ReceiveBook { Book = req.Book });
            });
        }
    }
}