-- Base limit default is $2M
do $$
declare
  baseLimit money := 2000000;
  valuationDate date := CAST('2021-10-16' as date);
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


/*  if not found then
     raise notice'The film % could not be found',
	    input_film_id;
  end if;*/
end $$;


WITH adj as (
    SELECT b.name, r.rating, t.assets,
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
    LEFT JOIN riskrating r on b.id = r.bank_id AND r.rating_date = CAST('2021-10-18' as date)
    LEFT JOIN totalassets t on b.id = t.bank_id AND t.valuation_date = CAST('2021-10-18' as date)
)
SELECT adj.name, '2000000'::money * adj.limit_adj1 * adj.limit_adj2
FROM adj;


/*CREATE FUNCTION calculateTradeLimits (valuationDate date, baseLimit money)
RETURNS TABLE(name varchar(250)) AS $$
    SELECT name FROM bank;
$$ LANGUAGE SQL;*/

drop function calculateTradeLimits;
create or replace function calculateTradeLimits(valuationDate date, baseLimit money)
returns table (name1 varchar, bsLim money) as $$
begin
    if baseLimit is null then
        baseLimit := 2000000;
    end if;
  return query
    SELECT name, baseLimit FROM bank;
end;
$$ language plpgsql;

SELECT * FROM calculateTradeLimits(CAST('2021-10-16' as date), null)