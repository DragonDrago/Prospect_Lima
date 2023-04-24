using System;

namespace Lima.Dictionaries.Api.Domain
{
    public class Page
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; }

        public Page(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
