-- tables
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

CREATE TABLE Student (
    IndexNumber varchar(100)  NOT NULL,
    FirstName varchar(100)  NOT NULL,
    LastName varchar(100)  NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int  NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY  (IndexNumber)
);

CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name varchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

-- foreign keys
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);

-- auto-numbering sequence for Enrollment
create sequence idEnrollSeq increment by 1 start with 1;
ALTER TABLE enrollment ALTER COLUMN idEnrollment SET DEFAULT nextval('idEnrollSeq');
ALTER SEQUENCE idEnrollSeq OWNED BY enrollment.idEnrollment;

-- example data
-- studies
insert into studies (IdStudy, name) values (1, 'IT');
insert into studies (IdStudy, name) values (2, 'Art');
-- enrollment
insert into enrollment (IdEnrollment, semester, idStudy, startdate)
values (nextval('idEnrollSeq'), 1, 1, '2019-10-01');
-- student
insert into student (indexNumber, firstName, lastName, birthDate, idEnrollment)
values ('s12345', 'Janusz', 'Tracz', '1990-12-13', 
	   (select idENrollment from enrollment as e where e.startDate = '2019-10-01' and e.idStudy = 1));

-- auth related edit
ALTER TABLE Student ADD Password varchar(256);
ALTER TABLE Student ADD Role varchar(16);
ALTER TABLE Student ADD Salt varchar(256);