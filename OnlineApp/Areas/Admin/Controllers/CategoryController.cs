using Microsoft.AspNetCore.Mvc;
using OnlineApp.DataAccess.Data;
using OnlineApp.DataAccess.Repository.IRepository;
using OnlineApp.Models;

namespace OnlineApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /*Listing Page for Category - get all records*/

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }


        /*Create action - to create new records of Category type*/

        /*Treated as Get action method for create or listing the model value page*/
        public IActionResult Create()
        {
            return View();
        }

        /*Treated as Post action method to submit new category object and make an entry in DB*/
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order and Name cannot match.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();

                /*TempData example for success message*/
                TempData["success"] = "Category Created Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        /*Edit action - to update existing records of Category type*/

        /*Treated as Get action method for Edit to get the details of a Model entry from DB*/
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            /*Use of Find method*/
            /*Category? objCategory = _db.Categories.Find(id);*/

            /*Use of FirstOrDefault method*/
            Category? objCategory = _unitOfWork.Category.Get(u => u.Id == id);

            /*Use of Where LINQ*/
            /*Category? objCategory = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();*/

            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }

        /*Treated as Post action method to Update changes in a model entry (once edited) and persist in the DB with changes*/
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();

                /*TempData example for success message*/
                TempData["success"] = "Category Updated Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        /*Delete action - to delete existing records of Category type*/

        /*Treated as Get action method for Delete to get the details of a Model entry from DB*/
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            /*Use of Find method*/
            Category? objCategory = _unitOfWork.Category.Get(u => u.Id == id);

            /*Use of FirstOrDefault method*/
            /*Category? objCategory = _db.Categories.FirstOrDefault(c => c.Id == id);*/

            /*Use of Where LINQ*/
            /*Category? objCategory = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();*/

            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }

        /*Treated as Post action method to Delete the model entry from the DB*/
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? categoryObj = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();

            /*TempData example for success message*/
            TempData["success"] = "Category Deleted Successfully!";

            return RedirectToAction("Index");
        }
    }
}
