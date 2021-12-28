using System;
using Akka.Actor;
using BookStore.Actors;

namespace BookStore
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var system = ActorSystem.Create("BookStoreSystem");
            var library = system.ActorOf<BookStoreActor>("BookStore");
            
        }
    }
}