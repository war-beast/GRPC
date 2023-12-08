using Intermediate.DAL.Interfaces;
using MongoDB.Bson;

namespace Intermediate.DAL.Entities;

public class BaseEntity : IPersistable
{
	public ObjectId Id { get; set; }
}