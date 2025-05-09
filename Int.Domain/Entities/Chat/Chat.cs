﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Entities.Chat
{
	public class Chat
	{
		public int Id { get; set; }
		public string SenderId { get; set; }
		public string ReceiverId { get; set; }
		public string Message { get; set; }
		public DateTime Timestamp { get; set; }

		public virtual User Sender { get; set; }
		public virtual User Receiver { get; set; }
	}

}
