using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Models.ViewModels
{
    public class InquiryVM
    {
        public InquiryHeader inquiryHeader { get; set; }
        public IEnumerable<InquiryDetail> inquiryDetails { get; set; }
    }
}
