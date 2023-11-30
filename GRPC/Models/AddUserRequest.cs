using System.ComponentModel.DataAnnotations;

namespace GRPC.Client.Models;

public record AddUserRequest([EmailAddress] string Email, [Required] string Password, [Required] string Name);