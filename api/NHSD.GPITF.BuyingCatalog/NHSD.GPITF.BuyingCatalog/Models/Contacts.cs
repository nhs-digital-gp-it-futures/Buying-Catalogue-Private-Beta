using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A means of communicating with an organisation, typically a person, email address, telephone number etc.
  /// Standard MS Dynamics CRM entity
  /// </summary>
  [Table(nameof(Contacts))]
  public sealed class Contacts
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier of organisation
    /// </summary>
    [Required]
    public string OrganisationId { get; set; }

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
    public string EmailAddress1  { get; set; }

    /// <summary>
    /// Primary phone number of contact
    /// </summary>
    public string PhoneNumber1 { get; set; }
  }
}
