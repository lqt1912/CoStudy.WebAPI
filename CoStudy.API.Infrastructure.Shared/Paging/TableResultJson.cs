using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Paging
{
    public class TableResultJson<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
        public object other { get; set; }
    }
}
