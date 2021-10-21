using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BankRatings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeLimitsController : ControllerBase
    {
        private readonly ILogger<TradeLimitsController> _logger;

        public TradeLimitsController(ILogger<TradeLimitsController> logger)
        {
            _logger = logger;

        }

        [HttpPost]
        public bool Calculate(DateTime valuationDate) {
            valuationDate = valuationDate.Date;
            var connString = "Host=127.0.0.1;Database=postgres;Username=postgres;Password=blah";

            using (var conn = new NpgsqlConnection(connString)) {
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
do $$
declare
  baseLimit money := 2000000;
  valuationDate date := @inputDate;
begin

    DELETE FROM tradelimit WHERE valuation_date = valuationDate;

    WITH adj as (
        SELECT b.id, r.rating, t.assets,
               case when -5 <= r.rating and r.rating <= -3 then 0.88 -- 12% deduction
                    when -2 <= r.rating and r.rating <=  0 then 0.91 -- 9% deduction
                    when  1 <= r.rating and r.rating <=  3 then 1.05 -- 5% increase
                    when  4 <= r.rating and r.rating <=  6 then 1.08 -- 8% increase
                    when  7 <= r.rating and r.rating <= 10 then 1.13 -- 13% increase
                    else 1 -- no change
               end as limit_adj1,
               case when t.assets > '3000000'::money then 1.23 else 1 end as limit_adj2
        FROM bank b
        INNER JOIN approval a on b.id = a.bank_id AND a.is_approved = TRUE
        LEFT JOIN riskrating r on b.id = r.bank_id AND r.rating_date = valuationDate
        LEFT JOIN totalassets t on b.id = t.bank_id AND t.valuation_date = valuationDate
    )
    INSERT INTO tradelimit (calculated_limit, bank_id, valuation_date)
    SELECT baseLimit * adj.limit_adj1 * adj.limit_adj2, adj.id, valuationDate
    FROM adj;

end $$;                
                ", conn)) {
                    cmd.Parameters.AddWithValue("inputDate", valuationDate);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }        
        

        [HttpGet]
        public IEnumerable<TradeLimits> Get(DateTime valuationDate)
        {
            valuationDate = valuationDate.Date;
            var connString = "Host=127.0.0.1;Database=postgres;Username=postgres;Password=blah";

            using (var conn = new NpgsqlConnection(connString)) {
                conn.Open();
                using (var cmd = new NpgsqlCommand("select id, calculated_limit, bank_id, valuation_date from tradelimit;", conn)) {
                    var list = new List<TradeLimits>();
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            list.Add(new TradeLimits
                                    {
                                        Id = reader.GetInt64(0),
                                        CalcLimit = reader.GetDecimal(1),
                                        BankId = reader.GetInt64(2),
                                        ValuationDate = reader.GetDateTime(3)
                                    }
                            );
                        }
                        return list.ToArray();
                    }
                }
            }

        }
    }
}
