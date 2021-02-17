




insert into DeviationTypes ([InfoText])
values	('Övertid'),
		('Sjukdom'),
		('Semester'),
		('VAB')



insert into WorkMonths ([IsApproved], [IsSubmitted],[Month],[UserId],[Year])
values	(1,1,1,'Hej',2021),
		(0,0,2,'Hej',2021),
		(0,1,1,'Hej',2021)



insert into Organisation ([Name])
	values	('Nisses plåt & skruv'),
			('Kalle kalas fixare')


insert into Invitations ([UserId],[Header],[Message],[OrganisationId])
	values	('Hej','Hallå','Hejsan bla bla bla',1),
			('Hej','Hej','Nu har vi fixat ett kalas',2)




select * from WorkMonths

select * from Deviations