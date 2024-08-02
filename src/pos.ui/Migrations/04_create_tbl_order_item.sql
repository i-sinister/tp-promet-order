create table if not exists ORDERITEM
(
	ID integer primary key,
	ORDERID integer not null references [ORDER](ID) on delete cascade,
	NAME text not null,
	QUANTITY real not null,
	UNIT text not null
)