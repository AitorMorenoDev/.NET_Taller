using System.ComponentModel.DataAnnotations;

namespace Taller.Models;

public class Bill
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ServiceId { get; set; } 
    public Service Service { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public DateTime GeneratedAt { get; set; }

    [Required]
    public bool IsPaid { get; set; }
        
    public Bill() { }

    public Bill(int serviceId, decimal totalAmount, DateTime generatedAt, bool isPaid)
    {
        ServiceId = serviceId;
        TotalAmount = totalAmount;
        GeneratedAt = generatedAt;
        IsPaid = isPaid;
    }
}