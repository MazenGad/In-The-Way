using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Entities
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string Query { get; set; } 
        public DateTime SearchedAt { get; set; } = DateTime.UtcNow;

        // علاقة مع اليوزر
        public string UserId { get; set; }
        public virtual User? User { get; set; } 
    }
}
