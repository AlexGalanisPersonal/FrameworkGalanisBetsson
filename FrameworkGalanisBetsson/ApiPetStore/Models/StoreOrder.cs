namespace SauceDemoUiBetsson.ApiPetStore.Models;

public class StoreOrder
{
    public long Id { get; set; }
    public long PetId { get; set; }
    public int Quantity { get; set; }
    public string? ShipDate { get; set; }
    public string Status { get; set; } = "placed";
    public bool Complete { get; set; }
}