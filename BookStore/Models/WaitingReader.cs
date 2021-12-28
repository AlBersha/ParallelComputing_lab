using Akka.Actor;

namespace BookStore.Models
{
    public class WaitingReader
    {
        public IActorRef ActorRef { get; set; }
        public Reader Reader { get; set; }
        public Book Book { get; set; }
    }
}