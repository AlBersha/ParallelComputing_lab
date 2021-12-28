using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using BookStore.Actors;
using BookStore.Factory;
using BookStore.Messages;

namespace BookStore
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var system = ActorSystem.Create("BookStoreSystem");
            var bookStore = system.ActorOf<BookStoreActor>("BookStore");

            var readers = ReaderFactory.GetAllReaders();
            var readerRefs = readers.Select(reader =>
                system.ActorOf(Props.Create(() => new ReaderActor(reader.ReaderId, reader.Name, bookStore)))).ToList();
            
            Show(readerRefs);
            
        }

        private static void Show(List<IActorRef> readers)
        {
            readers[0].Tell(new RequestBook{BookId = Guid.NewGuid()});
            readers[0].Tell(new RequestBook{BookId = Guid.NewGuid()});
            readers[1].Tell(new RequestBook{BookId = Guid.NewGuid()});
            readers[2].Tell(new RequestBook{BookId = Guid.NewGuid()});
        }
    }
}