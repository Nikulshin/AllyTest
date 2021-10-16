DROP TABLE IF EXISTS RiskRating;
DROP TABLE IF EXISTS TotalAssets;
DROP TABLE IF EXISTS TradeLimit;
DROP TABLE IF EXISTS Approval;
DROP TABLE IF EXISTS Bank;

CREATE TABLE Bank (
    id SERIAL8,
    name varchar(250),
    primary key (id)
);

CREATE TABLE Approval (
    id SERIAL8,
    bank_id bigint,
    is_approved boolean,
    decision_date date,
    primary key (id),
    constraint fk_bank_id foreign key (bank_id) references Bank (id),
    unique (bank_id, decision_date)
);

CREATE TABLE RiskRating (
    id SERIAL8,
    rating integer,
    bank_id bigint,
    rating_date date,
    primary key (id),
    constraint fk_bank_id foreign key (bank_id) references Bank (id),
    unique (bank_id, rating_date)
);

CREATE TABLE TotalAssets (
    id SERIAL8,
    assets money,
    bank_id bigint,
    valuation_date date,
    primary key (id),
    constraint fk_bank_id foreign key (bank_id) references Bank (id),
    unique (bank_id, valuation_date)
);

CREATE TABLE TradeLimit (
    id SERIAL8,
    calculated_limit money,
    bank_id bigint,
    valuation_date date,
    primary key (id),
    constraint fk_bank_id foreign key (bank_id) references Bank (id),
    unique (bank_id, valuation_date)
);