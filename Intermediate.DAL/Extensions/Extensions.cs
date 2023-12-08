using MongoDB.Bson;

namespace Intermediate.DAL.Extensions;

public static class Extensions
{
	public static bool IsNotEmpty(this ObjectId objectId) => objectId != ObjectId.Empty;
	public static bool IsEmpty(this ObjectId objectId) => !IsNotEmpty(objectId);
}