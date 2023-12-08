using MongoDB.Bson;

namespace Intermediate.DAL.Interfaces;

public interface IPersistable
{
	/// <summary>
	/// Идентификатор объекта
	/// </summary>
	ObjectId Id { get; set; }
}