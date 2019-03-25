using System;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
    public interface IHasId
    {
        Guid Id { get; set; }
    }
#pragma warning restore CS1591
}
