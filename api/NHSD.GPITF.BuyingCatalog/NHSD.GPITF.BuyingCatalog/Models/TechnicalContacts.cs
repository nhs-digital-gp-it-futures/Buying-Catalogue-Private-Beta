using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A means of communicating with an Organisation, typically a person, email address, telephone number etc,
  /// in the context of a Solution
  /// </summary>
  [Table(nameof(TechnicalContacts))]
  public sealed class TechnicalContacts
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier of Solution
    /// </summary>
    [Required]
    public string SolutionId { get; set; }

    /// <summary>
    /// Description of type of TechnicalContact eg
    /// <list type="bullet">
    /// Lead Contact
    /// Technical Contact
    /// Executive Sponsor
    /// Clinical Safety Officer
    /// Connection Agreement Signatory
    /// Other...
    /// </list>
    /// </summary>
    [Required]
    public string ContactType { get; set; }

    /// <summary>
    /// First name of contact
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of contact
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Primary email address of contact
    /// </summary>
    [Required]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Primary phone number of contact
    /// </summary>
    public string PhoneNumber { get; set; }
  }
}
