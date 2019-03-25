using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gif.Service.Models
{

    /// <summary>
    /// All CapabilityStandard N:N records
    /// </summary>
    [DataContract]
    public class PaginatedListCapabilityStandard
    {
        /// <summary>
        /// 1-based index of which page this page  Defaults to 1
        /// </summary>
        /// <value>1-based index of which page this page  Defaults to 1</value>
        [DataMember(Name = "pageIndex", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pageIndex")]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Total number of pages based on NHSD.GPITF.BuyingCatalog.Models.PaginatedList`1.PageSize
        /// </summary>
        /// <value>Total number of pages based on NHSD.GPITF.BuyingCatalog.Models.PaginatedList`1.PageSize</value>
        [DataMember(Name = "totalPages", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "totalPages")]
        public int? TotalPages { get; set; }

        /// <summary>
        /// Maximum number of items in this page  Defaults to 20
        /// </summary>
        /// <value>Maximum number of items in this page  Defaults to 20</value>
        [DataMember(Name = "pageSize", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pageSize")]
        public int? PageSize { get; set; }

        /// <summary>
        /// NHSD.GPITF.BuyingCatalog.Models.CapabilityStandard
        /// </summary>
        /// <value>NHSD.GPITF.BuyingCatalog.Models.CapabilityStandard</value>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "items")]
        public List<CapabilityStandard> Items { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PaginatedListCapabilityStandard {\n");
            sb.Append("  PageIndex: ").Append(PageIndex).Append("\n");
            sb.Append("  TotalPages: ").Append(TotalPages).Append("\n");
            sb.Append("  PageSize: ").Append(PageSize).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
