create or replace procedure promote (stud varchar(100), sem int)
language plpgsql
as $$
declare
	enr_old int;
	enr_new int;
begin
	enr_old := (select e.idEnrollment from Enrollment as e 
	where e.semester = sem and e.idStudy = (select s.idStudy from studies as s where s.name = stud));
	if enr_old is not NULL
	then
		enr_new := (select e.idEnrollment from Enrollment as e 
		where e.semester = sem+1 and e.idStudy = (select s.idStudy from studies as s where s.name = stud));
		if enr_new is NULL
		then
			enr_new := nextval('idEnrollSeq');
			insert into enrollment (idEnrollment, semester, idStudy, startDate)
			values (enr_new, sem+1, (select idStudy from studies where name = stud), current_date);
		end if;
		update Student set idEnrollment = enr_new where idEnrollment = enr_old;
	end if;
end;
$$;
