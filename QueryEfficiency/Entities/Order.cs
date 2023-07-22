using Microsoft.EntityFrameworkCore;

namespace QueryEfficiency.Entities;

public class Order
{
    // orderId
    public int Id { get; set; }

    // other info here

#nullable disable
   virtual public ICollection<OrderLine> Lines { get; set; } = new HashSet<OrderLine>();
#nullable enable
}