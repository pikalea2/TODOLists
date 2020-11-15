using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TODOLists.Models;
using TODOLists.DAL;
using System.Data.Entity.Validation;

namespace TODOLists.Controllers
{
    public class TaskController : Controller
    {
        static IList<Tbl_Task> tasks;
        Tbl_User loginUser;

        // GET: Task
        public ActionResult Index()
        {
            return View();
        }


        //GET: ToDoList
        [HttpGet]
        public ActionResult ToDo()
        {
            using (var dataContext = new TODOListDBEntities())
            {
                loginUser = new Tbl_User();
                if (Session["UserInfo"] != null)
                    loginUser = (Tbl_User)Session["UserInfo"];
                tasks = new List<Tbl_Task>();
                foreach (Tbl_Task task in dataContext.Tbl_Task.Where(c => c.UserId == loginUser.Id))
                {
                    tasks.Add(task);
                }
            }
            return View(tasks);
        }


        //GET: CreateToDo
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST:CreateToDo
        [HttpPost]
        public ActionResult Create(Tbl_Task task)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new TODOListDBEntities())
                {
                    try
                    {

                        if (Session["UserInfo"] != null)
                            loginUser = (Tbl_User)Session["UserInfo"];
                        task.UserId = loginUser.Id;
                        task.CreatedDate = DateTime.Now;
                        task.ModifiedDate = DateTime.Now;
                        databaseContext.Tbl_Task.Add(task);
                        databaseContext.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        ViewBag.Message= "All fields are compulsory";
                        return View("Create");
                    }
                    

                }
                ViewBag.Message = "Task Added";
                return View("Create");
            }
            else
                return View();
        }


        //GET: EditToDo
        public ActionResult Edit(int id)
        {
            using (var dataContext = new TODOListDBEntities())
            {
                Tbl_Task editTask = dataContext.Tbl_Task.Where(query => query.Id == id).FirstOrDefault();
                return View(editTask);
            }
        }

        //POST: EditToDo
        [HttpPost]
        public ActionResult Edit(Tbl_Task task)
        {
            using (var dataContext = new TODOListDBEntities())
            {

                Tbl_Task editTask = dataContext.Tbl_Task.Where(query => query.Id == task.Id).FirstOrDefault();
                editTask.Id = task.Id;
                editTask.TaskName = task.TaskName;
                editTask.Description = task.Description;
                editTask.IsCompleted = task.IsCompleted;
                editTask.ModifiedDate = DateTime.Now;
                dataContext.Entry(editTask).State = System.Data.Entity.EntityState.Modified;
                dataContext.SaveChanges();

            }
            ViewBag.Message = "Task Updated";
            return View();
        }

        //POST: DeleteToDo
        public ActionResult Delete(int id)
        {
            using (var dataContext = new TODOListDBEntities())
            {
                Tbl_Task _Task = dataContext.Tbl_Task.Where(query => query.Id == id).FirstOrDefault();
                dataContext.Tbl_Task.Remove(_Task);
                dataContext.SaveChanges();
                return RedirectToAction("ToDo");
            }
        }
    }
}