using System.Collections.Generic;
using BookStore.Models;

namespace BookStore.Factory
{
    public class ReaderFactory
    {
        public static List<Reader> GetAllReaders()
        {
            return new List<Reader>
            {
                new() { },
                new(){},
                new(){},
                new(){},
                new(){},
            };
        }
    }
}