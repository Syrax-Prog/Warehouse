namespace Warehouse.Models;

public class Item
{
    public int id {get; set;}
    public string name {get; set;} = String.Empty;
    public int quantity {get; set;}
    public decimal price {get; set;} 
    public DateTime createdate {get; set;}  
    public string createby {get; set;} = String.Empty;  
    public DateTime updatedate {get; set;}
    public string updateby {get; set;} = String.Empty;  
}