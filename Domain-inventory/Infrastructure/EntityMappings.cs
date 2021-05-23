using System.Net.Mime;
using Domain_inventory.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Domain_inventory.Infrastructure
{
    public class EntityMappings
    {
        public static void Map()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            
            BsonClassMap.RegisterClassMap<User>(x =>
            {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
            });
        }
    }
}