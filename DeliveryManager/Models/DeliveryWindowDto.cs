using System;

namespace DeliveryManager.Models
{
    public class DeliveryWindowDto : BaseDeliveryWindow
    {
        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }
    }
}
