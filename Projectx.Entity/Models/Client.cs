using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projectx.Entity.Models;
public class Client
{
    [Column("ClientId")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Client name is a required field.")]
    [MaxLength(60, ErrorMessage = "Max length for the client name is 60 characters.")]
    public string Name { get; set; }
}