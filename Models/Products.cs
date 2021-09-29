using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganicStoreFunctionApp.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId, AllowTruncation = true)]
        public string _id { get; set; }

        [BsonElement("id")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int id { get; set; }


        [BsonElement("name")]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string name { get; set; }

        [BsonElement("type")]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string type { get; set; }

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double price { get; set; }

        [BsonElement("image")]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string image { get; set; }

        [BsonElement("description")]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string description { get; set; }

        [BsonElement("qty")]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string qty { get; set; }

        [BsonElement("__v")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int __v { get; set; } = 0;

        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string ImagePath { get; set; }

        
    }
}
