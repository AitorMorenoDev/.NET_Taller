using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Taller.Models;

public enum ServiceType
{
    Maintenance,
    Repair,
    Tuning
}

public enum Status
{
    Accepted,
    Repairing,
    Finished
}

public class Service
{
    [Key]
    public int Id { get; set; }
    
    // A service belongs to a client (manyToOne)
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    
    [Required]
    public ServiceType ServiceType { get; set; }
    
    [Required]
    public string LicensePlate { get; set; }
    
    [Required]
    public DateTime DateCreated { get; set; }
    
    [AllowNull]
    public DateTime DateFinished { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    // A service belongs to a bill (manyToOne)
    public int BillId { get; set; }
    public Bill? Bill { get; set; }
    
    public Service() { }
    
    public Service(int clientId, ServiceType serviceType, string licensePlate, DateTime dateCreated, DateTime dateFinished, Status status, decimal amount, int billId)
    {
        ClientId = clientId;
        ServiceType = serviceType;
        LicensePlate = licensePlate;
        DateCreated = dateCreated;
        DateFinished = dateFinished;
        Status = status;
        Amount = amount;
        BillId = billId;
    }
}