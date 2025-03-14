using System.Collections.Generic;
using System.Linq;

namespace GS.Employee.Infra.CrossCutting.Models
{
    public class PaginatedResult<T>where T: class 
    {
        public PaginatedResult(long totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            Items = items ?? Items;
        }

        public long TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    }
}
