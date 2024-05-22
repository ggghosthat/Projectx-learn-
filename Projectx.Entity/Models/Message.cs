﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projectx.Entity.Models;
public class Message
{
    [Column("MessageId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Client id is a required field.")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Creation date is a required field.")]
    public DateTime Created { get; set; }

    [Required(ErrorMessage = "Message content is a required field.")]
    [MaxLength(128, ErrorMessage = "Max length for the message content is 128 characters.")]
    public string Content { get; set; }
}