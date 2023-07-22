namespace QueryEfficiency.Entities;

public class CartItem
{
    public int ProductNumber { get; set; }
    public int Quantity { get; set; }

    private static bool IsEqual(CartItem cartItem, Product product)
    {
        if(cartItem is null || product is null) 
            return false; 
        else if(cartItem.ProductNumber != product.Number)
            return false;
        return true;
    }
}