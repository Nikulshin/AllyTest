using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
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
        public string Calculate(string valuationDate) {
            //valuationDate = valuationDate.Date;
            var connString = "Host=127.0.0.1;Database=postgres;Username=postgres;Password=blah";
            Console.Out.WriteLine("Entered Calculate");


            using (var conn = new NpgsqlConnection(connString)) {
                conn.Open();
                using (var cmd = new NpgsqlCommand("CALL calculateRatings(@valuationDate)", conn)) {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("valuationDate", valuationDate);
                    return "{\"result\": \"" + (cmd.ExecuteNonQuery() > 0).ToString() + "\"}" ;
                }
            }
        }        
        

        [HttpGet]
        public IEnumerable<TradeLimits> Get(string valuationDate)
        {
            var connString = "Host=127.0.0.1;Database=postgres;Username=postgres;Password=blah";

            using (var conn = new NpgsqlConnection(connString)) {
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
                    select l.id, l.calculated_limit, b.name, l.valuation_date 
                    from tradelimit l
                    inner join bank b on b.id = l.bank_id
                    where valuation_date = TO_DATE( @valDate,'YYYY-MM-DD');", conn)) 
                {
                    cmd.Parameters.AddWithValue("valDate", valuationDate);
                    var list = new List<TradeLimits>();
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            list.Add(new TradeLimits
                                    {
                                        Id = reader.GetInt64(0),
                                        CalcLimit = reader.GetDecimal(1),
                                        Name = reader.GetString(2),
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
