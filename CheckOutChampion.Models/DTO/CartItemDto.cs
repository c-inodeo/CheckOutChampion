﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Models.DTO
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsIncrement { get; set; }
    }
}
