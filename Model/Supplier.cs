namespace Warehouse.Models;

public class Supplier
{
    public int id {get; set;}
    public string supplier_name {get; set;} = String.Empty;
    public DateTime? create_date {get; set;} 
    public string? create_by {get; set;} = String.Empty;  
    public DateTime? update_date {get; set;}
    public string? update_by {get; set;} = String.Empty;  
}