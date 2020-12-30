using Amazon.DynamoDBv2.DataModel;
using System;

namespace Todo.WebApi.Models
{
    public class TodoItem
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        public string Task { get; set; }

        [DynamoDBRangeKey]
        public DateTime Created { get; set; }

        public bool Completed { get; set; }

        // Property to store version number for optimistic locking.
        [DynamoDBVersion]
        public int? Version
        {
            get; set;
        }
    }
}
