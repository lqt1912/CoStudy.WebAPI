using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Paging
{
    public class TableRawDataJson
    {
        public string orderby { get; set; }
        public int draw { get; set; }
        public List<TableColumn> columns { get; set; }
        public List<TableColumnOrderBy> order { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public TableColumnSearch search { get; set; }
    }
}
