using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sniffer.UI
{
    [Serializable]
    public class PackageModel
    {
        public DateTime Time { get; set; }
        public string FromIP { get; set; }
        public string FromPort { get; set; }
        public string ToIP { get; set; }
        public string ToPort { get; set; }
        public byte[] Data { get; set; }
        public string ProcName { get; set; }
    }
}
