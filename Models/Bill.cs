using System.ComponentModel.DataAnnotations;

namespace Taller.Models;

public class Bill
{
    [Key]
    public int Id { get; set; }
    
    // A bill can have many services (oneToMany)
    public ICollection<Service> Services { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public DateTime GeneratedAt { get; set; }

    [Required]
    public bool IsPaid { get; set; }

    public Bill()
    {
        Services = new HashSet<Service>();
    }

    public Bill(decimal totalAmount, DateTime generatedAt, bool isPaid)
    {
        TotalAmount = totalAmount;
        GeneratedAt = generatedAt;
        IsPaid = isPaid;
        Services = new HashSet<Service>();
    }
}