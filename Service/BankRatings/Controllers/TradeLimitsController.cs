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
        public bool Calculate(string valuationDate) {
            //valuationDate = valuationDate.Date;
            var connString = "Host=127.0.0.1;Database=postgres;Username=postgres;Password=blah";

            using (var conn = new NpgsqlConnection(connString)) {
                conn.Open();
                using (var cmd = new NpgsqlCommand("CALL calculateRatings(@valuationDate)", conn)) {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("valuationDate", valuationDate);
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
