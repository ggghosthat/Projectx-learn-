using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projectx.Entity.Models;
public class Client
{
    //public int Id { get; set; }

    [Required(ErrorMessage = "Client id is a required field.")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Client name is a required field.")]
    [MaxLength(60, ErrorMessage = "Max length for the client name is 60 characters.")]
    public string Name { get; set; }
}