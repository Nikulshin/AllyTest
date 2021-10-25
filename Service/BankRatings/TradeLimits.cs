using System;

namespace BankRatings
{
    public class TradeLimits
    {
        public long Id { get; set; }

        public decimal CalcLimit { get; set; }

        public long BankId { get; set; }

        public string Name {get; set;}

        public DateTime ValuationDate { get; set; }
    }
}
