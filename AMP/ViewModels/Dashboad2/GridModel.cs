using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class GridModel<T> where T : class
    {
        //For Creation
        public T Record { get; set; }

        private List<T> _records;
        public string SearchText { get; set; }
        public int PageSize { get; set; }
        public bool MyRecords { get; set; }
        public List<SelectListItem> PageSizes {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "10" , Value = "10" },
                    new SelectListItem() {Text = "20" , Value = "20" },
                    new SelectListItem() {Text = "50" , Value = "50" },
                    new SelectListItem() {Text = "100" , Value = "100" },
                };
            }
        }
        public int PageNo { get; set; }
        public List<SelectListItem> Pages {
            get
            {
                return Enumerable.Range(1, ((_records ?? new List<T>()).Count / PageSize)+1).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    };
                }).ToList();
            }
        }
        public List<T> Records {
            get
            {
                return (_records ?? (_records = new List<T>())).Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
            }
            set
            {
                _records = value;
            }
        }

        public List<T> CompleteRecords
        {
            get
            {
                return _records.ToList();
            }
            set
            {
                _records = value;
            }
        }

        public int TotalRecords
        {
            get
            {
                return (_records ?? new List<T>()).Count;
            }
        }

        public string Display
        {
            get
            {
                return string.Format(
                    "Showing {0}  to {1} of {2} entries"
                    , ((_records ?? new List<T>()).Count < 1 ? 0 : (((PageNo - 1) * PageSize) + 1))
                    , ((PageNo * PageSize) > (_records ?? new List<T>()).Count ? (_records ?? new List<T>()).Count : (PageNo * PageSize))
                    , (_records ?? new List<T>()).Count
                    );
            }
        }

        public List<AlphabetPagerModel> AlphabetPagers
        {
            get;
            set;
        }
        public NavigatePageNumber Next { get; set; }
        public NavigatePageNumber Previous { get; set; }

        public void SetNavigation(int currentPageNumber)
        {
            var totalPages = Pages.Count();
            Previous = new NavigatePageNumber() { Enable = currentPageNumber > 1 ? true : false,
                PageNumber = currentPageNumber + 1 <= totalPages ? currentPageNumber - 1 : totalPages - 1
            };
            Next = new NavigatePageNumber() { Enable = currentPageNumber < totalPages ? true : false,
                PageNumber = currentPageNumber + 1 <= totalPages ? currentPageNumber + 1 : currentPageNumber};
        }


    }

    public class NavigatePageNumber
    {
        public int PageNumber { get; set; }
        public bool Enable { get; set; }
    }
}