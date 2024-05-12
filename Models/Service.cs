using System.ComponentModel.DataAnnotations;

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
    
    [Required]
    public int ClientId { get; set; }
    public Client Client { get; set; }
    
    [Required]
    public ServiceType ServiceType { get; set; }
    
    [Required]
    public string LicensePlate { get; set; }
    
    [Required]
    public DateTime DateCreated { get; set; }
    
    [Required]
    public DateTime DateFinished { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    public Service() { }
    
    public Service(int clientId, ServiceType serviceType, string licensePlate, DateTime dateCreated, DateTime dateFinished, Status status)
    {
        ClientId = clientId;
        ServiceType = serviceType;
        LicensePlate = licensePlate;
        DateCreated = dateCreated;
        DateFinished = dateFinished;
        Status = status;
    }
    
}