using System.ComponentModel.DataAnnotations;

namespace GRPC.Client.Models;

public record UserRequest([EmailAddress] string Email, [Required] string Password);