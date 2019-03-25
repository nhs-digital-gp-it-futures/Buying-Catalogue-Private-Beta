using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace NHSD.GPITF.BuyingCatalog.Models
{
  /// <summary>
  /// A 'Supplier' is a company who supplies ‘solutions’ to NHS buyers.
  /// A 'Buyer' is a company who purchases 'solutions' from a 'Supplier'.
  /// An 'Admin' is a company, typically NHS Digital, who has ultimate control over all information in the Buying Catalog.
  /// Standard MS Dynamics CRM entity
  /// </summary>
  [Table(nameof(Organisations))]
  public sealed class Organisations
  {
    /// <summary>
    /// Unique identifier of entity
    /// </summary>
    [Required]
    [ExplicitKey]
    public string Id { get; set; }

    /// <summary>
    /// Name of a company, as displayed to the user
    /// </summary>
    /// <example>TPP</example>
    /// <example>EMIS</example>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Primary ODS role code for company
    /// Suppliers have a primary role of "RO92"
    /// </summary>
    [Required]
    public string PrimaryRoleId { get; set; } = PrimaryRole.ApplicationServiceProvider;

    /// <summary>
    /// Operational status of company
    /// Typically:  "Active", "Inactive"
    /// </summary>
    [Required]
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Information about company
    /// </summary>
    public string Description { get; set; }
  }
}
