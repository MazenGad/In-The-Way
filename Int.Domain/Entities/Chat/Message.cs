using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Entities.Chat
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; } 
        public string ReceiverId { get; set; } 
        public string Content { get; set; }    
        public DateTime SentAt { get; set; } 
        public bool IsRead { get; set; }      

        // العلاقات (مع جدول المستخدمين)
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}
