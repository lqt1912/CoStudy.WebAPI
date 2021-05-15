using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Paging
{

    public class TableRequest
    {
        public string Is_Active { get; set; }
        public string Name { get; set; }
        public object Token { get; set; }
        public object orderby { get; set; }
        public int draw { get; set; }
        public List<TableColumn> columns { get; set; }
        public List<TableColumnOrderBy> order { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public TableColumnSearch search { get; set; }
    }
}
