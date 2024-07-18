﻿using MongoDB.Bson;

namespace TrackOrders.Data.Entities
{
    public class Delivery
    {
        public ObjectId Id { get; set; }
        public string OrderNumber { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}
