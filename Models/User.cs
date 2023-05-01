using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthAPI.Models;

public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }

  [BsonElement("UserName")]
  public string UserName { get; set; }

  [BsonElement("Email")]
  public string Email { get; set; }

  [BsonElement("Password")]
  public string PasswordHash { get; set; }
}