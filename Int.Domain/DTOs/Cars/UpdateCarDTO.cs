﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.DTOs.Cars
{
	public class UpdateCarDTO
    {
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? PlateNumber { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
    }
}
