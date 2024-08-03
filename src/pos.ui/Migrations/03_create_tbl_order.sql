create table if not exists [ORDER]
(
	ID integer primary key,
	PROVIDERID integer not null references PROVIDER(ID) on delete cascade,
	NUMBER text not null,
	DATE text not null
)