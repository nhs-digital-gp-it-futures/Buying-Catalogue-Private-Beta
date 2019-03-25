namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IHasPreviousId : IHasId
  {
    string PreviousId { get; set; }
  }
#pragma warning restore CS1591
}
