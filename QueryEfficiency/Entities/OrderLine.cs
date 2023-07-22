namespace QueryEfficiency.Entities;

public class OrderLine
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int ProductNumber { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }

#nullable disable
    virtual public Order Order { get; set; }
#nullable enable
}
