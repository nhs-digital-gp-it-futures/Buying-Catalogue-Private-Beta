using System;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
    public interface IHasPreviousId : IHasId
    {
        Guid? PreviousId { get; set; }
    }
#pragma warning restore CS1591
}
