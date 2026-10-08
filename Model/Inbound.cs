namespace Warehouse.Models;

public class Inbound
{
    public int id {get; set;}
    public string inbound_no {get; set;} = String.Empty;
    public string supplier {get; set;} = String.Empty;
    public string reference_no {get; set;} = String.Empty; 
    public DateTime? received_date {get; set;}
    public string? received_by {get; set;} = String.Empty;
    public string status {get; set;} = String.Empty;
    public DateTime? created_at {get; set;} 
    public string? created_by {get; set;} = String.Empty;  
    public DateTime? updated_at {get; set;}
}