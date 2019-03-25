using System.ComponentModel.DataAnnotations;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
    public abstract class ClaimsBase
    {
        /// <summary>
        /// Unique identifier of entity
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier of solution
        /// </summary>
        [Required]
        public string SolutionId { get; set; }

        /// <summary>
        /// Unique identifier of supplier Contact who is responsible for this claim
        /// </summary>
        [Required]
        public string OwnerId { get; set; }
    }
#pragma warning restore CS1591
}
