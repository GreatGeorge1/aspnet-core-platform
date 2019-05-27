using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Orders.Dtos
{
    public class CreateOrderDto
    {
        public long UserId { get; set; }
        public long PackageId { get; set; }
    }
}
