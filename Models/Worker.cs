using System.ComponentModel.DataAnnotations;

namespace Taller.Models;

public class Worker
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Role { get; set; }
    
    public Worker() { }

    public Worker(string name, string login, string password, string role)
    {
        Name = name;
        Login = login;
        Password = password;
        Role = role;
    }
}