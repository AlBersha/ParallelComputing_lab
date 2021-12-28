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
            readers[0].Tell(new RequestBook{BookId = Guid.Parse("f0d4c243-cb67-4a78-9a7f-919a5ae17eaf"), IsReadOnly = true});
            readers[0].Tell(new RequestBook{BookId = Guid.Parse("ef4f9db0-10f1-4fa3-a916-54fba8d56387")});
            readers[1].Tell(new RequestBook{BookId = Guid.Parse("7ccd79e7-70a6-4254-b923-f56df6fb32ff")});
            readers[2].Tell(new RequestBook{BookId = Guid.Parse("008defe1-185d-465f-b24c-a6c718745e87")});
        }
    }
}