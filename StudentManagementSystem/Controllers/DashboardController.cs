using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Dto;
using StudentManagementSystem.Persistence;

namespace StudentManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var students = _context.Students.Select(x=> new StudentDto {Id = x.Id, Name = x.Name, FatherName = x.FatherName,
            MotherName = x.MotherName, City = x.City, Email = x.Email}).ToList();
            return View(students);
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        public IActionResult UpdateStudent(int studentId)
        {
            var student = _context.Students.Select(x => new StudentDto
            {
                Id = x.Id,
                Name = x.Name,
                FatherName = x.FatherName,
                MotherName = x.MotherName,
                City = x.City,
                Email = x.Email
            }).FirstOrDefault(x=>x.Id == studentId);
            return View(student);
        }


        public IActionResult CreateStudent(StudentDto dto)
        {
            if(dto == null)
            {
                return RedirectToAction("AddStudent");
            }

            if(string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Email) || 
                string.IsNullOrEmpty(dto.FatherName) || string.IsNullOrEmpty(dto.MotherName) || 
                string.IsNullOrEmpty(dto.City))
            {
                return RedirectToAction("AddStudent");
            }


            var isStudentExist = _context.Students.Any(x => x.Email == dto.Email);

            if (isStudentExist)
            {
                return RedirectToAction("AddStudent");
            }

            _context.Students.Add(new Models.Student
            {
                Name = dto.Name,
                Email = dto.Email,
                FatherName = dto.FatherName,
                MotherName = dto.MotherName,
                City = dto.City
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteStudent(int studentId)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == studentId);
            if(student == null)
            {
                return RedirectToAction("Index");
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UpdateStudentRecord(StudentDto dto) 
        { 
            var student = _context.Students.FirstOrDefault(x => x.Id == dto.Id);    

            student.Name = dto.Name;
            student.Email = dto.Email;  
            student.FatherName = dto.FatherName;    
            student.MotherName = dto.MotherName;
            student.City = dto.City;

            _context.Students.Update(student);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
