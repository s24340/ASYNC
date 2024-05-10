using System.ComponentModel.DataAnnotations;

namespace async.Models;

public abstract class Warehouse
{
    [Required]
    public int IdProduct { get; set; }
    
    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public string CreatedAt { get; set; }
}