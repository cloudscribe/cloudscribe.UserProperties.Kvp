using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.UserProperties.Models
{
    public class UserProperty
    {
        public string SiteId { get; set; }
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
    }
}
