namespace CoStudy.API.Infrastructure.Shared.Paging
{
    public class TableColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public TableColumnSearch search { get; set; }
    }
}
