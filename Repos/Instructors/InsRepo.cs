using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;

namespace WebApplication2.Repos.Instructors
{
    public class InsRepo:IInsRepo
    {
        ITIContext ITI;
       
        public InsRepo(ITIContext iTIContext) {
            ITI = iTIContext; 
                
        }
        public List<Instructor> LoadInstructors()
        {
            var instructors=ITI.Instructors.ToList();
            return instructors;
        }
        public bool InsNameInDept(string FName, int DepartmentId)
        {
            var instructorname = ITI.Departments.Where(d => d.Id == DepartmentId).SelectMany(d => d.Instructors).Any(i => i.FName == FName);

            return instructorname;
        }
        public Instructor InsWithHisCourses(int id)
        {
           var Instructor =ITI.Instructors.Include(i => i.Courses).FirstOrDefault(I => I.Id == id);
            return Instructor;
        }
        public void Add(Instructor ins)
        {
            ITI.Instructors.Add(ins);
            ITI.SaveChanges();
        }
        public Instructor FindInstructor(int id)
        {
            var instructor = ITI.Instructors.Find(id);
            return instructor;
        }
        public void UpdateInstructor(Instructor instructor)
        {
            ITI.Instructors.Update(instructor);
            ITI.SaveChanges();
        }
        public void Delete(int id) { 
        var instructor=FindInstructor(id);
            ITI.Instructors.Remove(instructor);
            ITI.SaveChanges();
        }

    }
}
