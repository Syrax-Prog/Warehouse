namespace Warehouse.Models;

public class InboundItem
{
    public int id {get; set;}
    public int inbound_id {get; set;}
    public int item_id {get; set;}
    public int quantity {get; set;}
    public DateTime? created_at {get; set;}
    public string? created_by {get; set;} = String.Empty;
    public DateTime? updated_at {get; set;}
    public string? updated_by {get; set;} = String.Empty;
}