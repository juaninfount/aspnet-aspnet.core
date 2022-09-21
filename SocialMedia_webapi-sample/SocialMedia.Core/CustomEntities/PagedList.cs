using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Core.CustomEntities
{
    public class PagedList<T>: List<T>
    {
        public int CurrentPage {get;set;}

        public int TotalPages {get;set;}

        public int PageSize {get;set;}

        public int TotalCount {get;set;}

        public bool HasPreviousPage 
        {
            get { return CurrentPage > 0; }            
        }

        public bool HasNextPage
        {
            get { return CurrentPage < TotalPages; }            
        }

        public int? NextPageNumber 
         {
            get { return HasNextPage ? CurrentPage + 1: (int?)null; }            
        }

        public int? PreviousPageNumber
        {
            get {return HasPreviousPage ? CurrentPage - 1: (int?)null; }
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {            
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)count/pageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1)* pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
