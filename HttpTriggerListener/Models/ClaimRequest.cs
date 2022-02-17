using System;
using System.Collections.Generic;
using System.Text;

namespace HttpTriggerListener.Models
{
    public class ClaimRequest
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
