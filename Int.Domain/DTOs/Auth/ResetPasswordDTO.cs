﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.DTOs.Auth
{
	public class ResetPasswordDTO
	{
		public string Email { get; set; }
		public string Code { get; set; }
		public string NewPassword { get; set; }
	}

}
