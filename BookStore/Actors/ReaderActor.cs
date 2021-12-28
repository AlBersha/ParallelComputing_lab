using System;
using Akka.Actor;
using BookStore.Messages;
using BookStore.Models;

namespace BookStore.Actors
{
    public class ReaderActor: ReceiveActor
    {
        private Reader Reader { get; }
        private IActorRef BookStore { get; }

        public ReaderActor(Guid readerId, string readerName, IActorRef bookStore)
        {
            Reader = new Reader { ReaderId = readerId, Name = readerName };
            BookStore = bookStore;

            Receive<ReceiveBook>(req =>
            {
                Console.Out.WriteLine($"Reader {Reader.Name} reading {req.Book.Name}");
                req.Book.Read();

                Console.Out.WriteLine($"Reader {Reader.Name} reading {req.Book.Name}");
                Sender.Tell(new ReturnBook { Book = req.Book, Reader = Reader });
            });

            Receive<WaitingReader>(req =>
            {
                Console.Out.WriteLine($"Reader {Reader.Name} is waiting for book {req.Book.Name}");
            });
        }
    }
}