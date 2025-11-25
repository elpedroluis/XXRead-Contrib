using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXRead.Data
{
    public static class StaticContext
    {
        public static XStory.DTO.Story CurrentStory { get; set; }
        public static string DataSource { get; set; }
    }
}
