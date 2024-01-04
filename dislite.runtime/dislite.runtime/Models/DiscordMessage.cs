using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dislite.runtime.Models
{
    public class DiscordMessage
    {
        public Author Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
