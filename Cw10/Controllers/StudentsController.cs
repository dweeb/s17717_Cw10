using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DTOs.Requests;
using Cw10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw10.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var context = new uniContext();
            return Ok(context.Student.ToList());
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(string index)
        {
            var context = new uniContext();
            var res = from Student s in context.Student where s.Indexnumber == index select s;
            if (res.Count() > 0)
                return Ok(res.First());
            else
                return NotFound();
        }

        [HttpPost("{index}")]
        public IActionResult ModifyStudent(string index, ModifyStudentRequest req)
        {
            var context = new uniContext();
            var changed = from Student s in context.Student where s.Indexnumber == index select s;
            foreach(Student s in changed)
            {
                if (req.Firstname != null)
                    s.Firstname = req.Firstname;
                if (req.Lastname != null)
                    s.Lastname = req.Lastname;
                if (req.Birthdate != null)
                {
                    try
                    {
                        var newDate = DateTime.ParseExact(req.Birthdate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        s.Birthdate = newDate;
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Incorrect date format. Correct format: dd.MM.yyyy");
                    }
                }
            }
            context.SaveChanges();
            return Created("api/students/" + index, changed.First());
        }

        [HttpDelete("{index}")]
        public IActionResult DeleteStudent(string index)
        {
            var context = new uniContext();
            var student = (from Student s in context.Student where s.Indexnumber == index select s).FirstOrDefault();
            if(student == null)
                return NotFound();
            context.Student.Remove(student);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost("enroll")]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {
            var context = new uniContext();
            if (context.Student.Where(s => s.Indexnumber == req.Indexnumber).FirstOrDefault() != null)
                return BadRequest("Student already exists.");
            Studies studies = context.Studies.Where(s => s.Name.Equals(req.Studies)).FirstOrDefault();
            if (studies == null)
                return BadRequest("Studies specified not found.");
            Enrollment enrollment = context.Enrollment.Where(e => e.Idstudy == studies.Idstudy && e.Semester == 1).FirstOrDefault();
            if(enrollment == null)
            {
                enrollment = new Enrollment
                {
                    Semester = 1,
                    Idstudy = studies.Idstudy,
                    Startdate = DateTime.Now,
                };
                context.Enrollment.Add(enrollment);
            }
            var stud = new Student
            {
                Indexnumber = req.Indexnumber,
                Firstname = req.Firstname,
                Lastname = req.Lastname,
                Birthdate = DateTime.ParseExact(req.Birthdate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
            };
            enrollment.Student.Add(stud);
            context.Student.Add(stud);
            context.SaveChanges();
            return Created("api/students/" + req.Indexnumber, context.Student.Where(s => s.Indexnumber == req.Indexnumber).First());
        }

        [HttpPost("promote")]
        public IActionResult PromoteStudents(PromoteRequest req)
        {
            var context = new uniContext();
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlInterpolated($"call promote({req.Studies}, {req.Semester})");
            context.Database.CommitTransaction();
            //  still can't get pgsql to return a result set
            var res = context.Enrollment.Where(e => e.Idstudy == context.Studies.Where(s => s.Name == req.Studies).First().Idstudy && e.Semester == req.Semester+1).First();
            return Created("api/enrollments/" + res.Idenrollment, res); // api/enrollments isn't actually implemented
        }
    }
}
