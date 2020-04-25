using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperCRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperCRUD.Controllers
{
    public class DapperController : Controller
    {
        public IActionResult Index()
        {
            var records = DapperORM.ReturnList<Login>("LoginViewAll");
            return View(records);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int? id)
        {
            if (id != null)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);

                var record = DapperORM.ReturnList<Login>("LoginViewById", param).FirstOrDefault<Login>();
                return View(record);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddOrEdit([Bind(include: "Id, Email, Password")]Login model)
        {
            var param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                @Id = model.Id,
                @Email = model.Email,
                @Password = model.Password,
            });
            //param.Add("@Id", model.Id);
            //param.Add("@Email", model.Email);
            //param.Add("@Password", model.Password);

            DapperORM.ExecuteWithoutReturn("LoginAddOrEdit", param);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var param = new DynamicParameters();
            param.Add("@Id", id);

            DapperORM.ExecuteWithoutReturn("LoginDeleteById", param);

            return RedirectToAction("Index");
        }
    }
}