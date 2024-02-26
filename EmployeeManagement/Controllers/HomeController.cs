using EmployeeManagement.Models;
using EmployeeManagement.Service;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService service;
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            service = new EmployeeService();
            _configuration = configuration;
        }

        public ViewResult Index()
        {
            var model = service.GetEmployees();
            return View(model);
        }

        public ViewResult Details(int Id)
        {
            var model = (EmployeeViewModel)service.GetEmployee(Id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            bool isDelete = false;
            //View Bag is used here to show/hide fields
            ViewBag.IsDelete = isDelete;
            if (Id == null)
            {
                Employee newEmployee = new Employee();
                newEmployee.Id = 0;
                return PartialView("_Edit", newEmployee);
            }
            else
            {
                var empModel = service.GetEmployee((int)Id);
                return PartialView("_Edit", empModel);
            }
        }
        [HttpGet]
        //Reused Edit page hiding all fields but passwords
        public ActionResult Delete(int Id)
        {
            bool isDelete = true;
            ViewBag.IsDelete = isDelete;
            var empModel = service.GetEmployee((int)Id);
            return PartialView("_Edit", empModel);
        }

        [HttpPost]
        public IActionResult Edit(Employee employeeModel)
        {
            //setting IsDelete ViewBag is to differentiate between edit /delete
            bool isDelete = false;
            ViewBag.IsDelete = isDelete;

            if (employeeModel.Id > 0)
            {
                return View("_Edit", employeeModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remove(int Id, string password, string confirmPassword)
        {
            // Assuming you have the logic to retrieve the employee for the given Id
            string configPassword = _configuration["AppSettings:Password"];

            // Validate the password before deleting
            if (configPassword == confirmPassword && configPassword == password)
            {
                service.DeleteEmployee(Id);

                // Return a JSON success response
                return Json(new { success = true, message = "Employee deleted successfully" });
            }
            else if (password != confirmPassword)
            {
                // Passwords should match
                return Json(new { success = false, message = "Passwords should match" });
            }
            else
            {
                // Incorrect Password
                return Json(new { success = false, message = "Incorrect Password" });
            }
        }


        [HttpPost]
        public JsonResult SaveDetails(Employee employeeModel)
        {
            string password = _configuration["AppSettings:Password"];

            if (employeeModel.Password == employeeModel.ConfirmPassword && employeeModel.Password == password)
            {
                if (employeeModel.Id > 0)
                {
                    // If id exists then updates existing value
                    service.UpdateEmployee(employeeModel);
                    return Json("Updated Successfully");
                }
                else
                {
                    // Adds Employee based on id value
                    service.SaveEmployee(employeeModel);
                    return Json("Saved Successfully");
                }
            }
            else if (employeeModel.Password != employeeModel.ConfirmPassword)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json("Passwords should match");
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json("Incorrect Password");
        }

        public ActionResult GetLogData()
        {
            return PartialView("_Log", service.GetLogData());
        }
        
    }
}
