INSERT INTO bank (name) values ('Bank of America');
INSERT INTO bank (name) values ('Wells Fargo');
INSERT INTO bank (name) values ('Bank of Nova Scotia');
INSERT INTO bank (name) values ('Royal Bank of Canada');
INSERT INTO bank (name) values ('Bank of Montreal');

INSERT INTO bank (name) values ('jp Morgan');
INSERT INTO bank (name) values ('Citibank');
INSERT INTO bank (name) values ('Goldman Sachs');


INSERT INTO riskrating (rating, bank_id, rating_date)
SELECT  7, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of America' UNION
SELECT -4, id, CAST('2021-10-16' as date) FROM bank where name = 'Wells Fargo' UNION
SELECT  2, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Nova Scotia' UNION
SELECT -1, id, CAST('2021-10-16' as date) FROM bank where name = 'Royal Bank of Canada' UNION
SELECT  9, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Montreal';

INSERT INTO totalassets (assets, bank_id, valuation_date)
SELECT  1234000, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of America' UNION
SELECT  5657345, id, CAST('2021-10-16' as date) FROM bank where name = 'Wells Fargo' UNION
SELECT  2999002, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Nova Scotia' UNION
SELECT  4346823, id, CAST('2021-10-16' as date) FROM bank where name = 'Royal Bank of Canada' UNION
SELECT 15342679, id, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Montreal';


INSERT INTO approval (bank_id, is_approved, decision_date)
SELECT id,  TRUE, CAST('2021-10-16' as date) FROM bank where name = 'Bank of America' UNION
SELECT id,  TRUE, CAST('2021-10-16' as date) FROM bank where name = 'Wells Fargo' UNION
SELECT id, FALSE, CAST('2021-10-16' as date) FROM bank where name = 'jp Morgan' UNION
SELECT id,  TRUE, CAST('2021-10-16' as date) FROM bank where name = 'Royal Bank of Canada' UNION
SELECT id,  TRUE, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Montreal' UNION
SELECT id,  TRUE, CAST('2021-10-16' as date) FROM bank where name = 'Citibank' UNION
SELECT id, FALSE, CAST('2021-10-16' as date) FROM bank where name = 'Bank of Nova Scotia' UNION
SELECT id, FALSE, CAST('2021-10-16' as date) FROM bank where name = 'Goldman Sachs';








