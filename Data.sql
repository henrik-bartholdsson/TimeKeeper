




insert into DeviationTypes ([InfoText])
values ('�vertid')


insert into Deviations([DayInMonth],[StartTime],[StopTime],[Comment],[IsPredefined],[DeviationTypeId],[WorkMonthId])
values(1,'16:00', '16:20', 'Jobbade extra med n�got',0,1,1)



insert into WorkMonths ([IsApproved], [IsSubmitted],[Month],[UserId],[Year])
values	(1,1,1,'Hej',2021),
		(0,0,2,'Hej',2021),
		(0,1,1,'Hej',2021)

select * from WorkMonths

select * from Deviations