using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCorpOrder.Application.Models
{
    public class ApplicationInsightValue
    {
        public string Environment { get; set; }

        public string Application { get; set; }

        public string InstrumentationKey { get; set; }
    }
}
