using System;
using System.Collections.Generic;
using System.Text;

namespace HttpTriggerListener.Models
{
    public class ClaimRequestResponse: ClaimRequest
    {
        public string ClaimRequestFile { get; set; }
    }
}
