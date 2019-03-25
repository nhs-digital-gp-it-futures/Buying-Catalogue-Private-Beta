using System;

namespace Gif.Service.Crm
{

    public sealed class AccessToken
    {
        /// <summary>
        /// The access token.
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// The type of the token.
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// The token expiry 
        /// </summary>
        public DateTimeOffset expires_on { get; set; }

        /// <summary>
        /// UTC time when this access token was created
        /// </summary>
        public DateTime CreatedOn { get; } = DateTime.UtcNow;
    }
}
