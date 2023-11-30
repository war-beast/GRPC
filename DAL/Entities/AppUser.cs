using DAL.Interfaces;

namespace DAL.Entities;

public record AppUser(Guid Id, string Name, string Email, string PasswordHash) : IPersistable;