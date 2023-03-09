using Microsoft.AspNetCore.Mvc;
using SalesWebProject.Models;



namespace SalesWebProject.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> list = new List<Department>();
            list.Add(new Department { Id = 1, Name = "Electronics" });
            list.Add(new Department { Id = 2, Name = "Fashion" });
            list.Add(new Department { Id = 3, Name = "Tools" });
            return View(list);
        }
    }
}
