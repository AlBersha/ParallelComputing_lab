using System;
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
                new(){ReaderId = Guid.Parse("7a88dfc8-7321-48ab-8faa-64fa03ea0116"), Name = "Pierce"},
                new(){ReaderId = Guid.Parse("38b8b3e5-a22f-4a62-ae69-0180b5c55661"), Name = "James"},
                new(){ReaderId = Guid.Parse("389b5644-f7c1-4efc-8063-5cd9fe878dea"), Name = "Frank"},
                new(){ReaderId = Guid.Parse("4aa4ab4b-cf1d-4788-8968-0196d405e084"), Name = "Kevin"},
                new(){ReaderId = Guid.Parse("e0bc7d66-785c-49da-b3ef-1fbc7971faec"), Name = "Katie"},
            };
        }
    }
}