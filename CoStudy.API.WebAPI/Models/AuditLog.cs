using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Models
{
    [Table("audit_log")]
    public partial class AuditLog
    {
      
        [Column("id")]
        public int Id { get; set; }
      
        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }
    
        [Column("data")]
        public JsonElement Data
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
