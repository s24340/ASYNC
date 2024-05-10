using System.ComponentModel.DataAnnotations;

namespace async.Models;

public abstract class Warehouse
{
    public int IdProduct { get; set; }
    
    public int IdWarehouse { get; set; }
    
    public int Amount { get; set; }
    
    public string CreatedAt { get; set; }
}