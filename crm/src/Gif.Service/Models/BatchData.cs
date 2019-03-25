#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Gif.Service.Enums;

namespace Gif.Service.Models
{
    public class BatchData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EntityData { get; set; }

        public BatchTypeEnum Type { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
