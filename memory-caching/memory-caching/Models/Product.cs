using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models;

[Index("Name", Name = "Name")]
public class Product 
{
   
    [Key]
    public string? ProductId { get;set;}

    public string? Name {get;set;}
    public  decimal Price {get;set;}  
}