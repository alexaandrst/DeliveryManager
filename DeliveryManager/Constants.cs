using System;

namespace DeliveryManager
{
    public class Constants
    {
        public class ApiSegment
        {
            public const string Api = "delivery-manager";
        }

        public class ApiVersions
        {
            public const string V1 = "1.0";
        }

        public class UrgentDeliverySlot
        {
            public readonly static TimeSpan From = new TimeSpan(08, 00, 00);
            public readonly static TimeSpan To = new TimeSpan(19, 00, 00);
        }
    }
}
