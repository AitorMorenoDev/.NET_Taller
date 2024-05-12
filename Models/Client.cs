using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Taller.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    public string Gender { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public long Phone { get; set; }
    
    public string Email { get; set; }
    public long MobilePhone { get; set; }
    
    public Client() { }
    
    public Client(int id, string name, DateTime birthDate, string gender, string address, long phone, string email, long mobilePhone)
    {
        Id = id;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        Phone = phone;
        Email = email;
        MobilePhone = mobilePhone;
    }
}