using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineApp.DataAccess.Repository.IRepository;
using OnlineApp.Models;
using OnlineApp.Models.ViewModels;

namespace OnlineApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all values in Product table from DB
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }





        // Separate Create Function
        // Loading the create new product page view
        /*public IActionResult Create()
        {

            // Using Projection to get items from Category and setting a new object with selected fields of type SelectListItem - useful to create dropdown
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            // Use of ViewBag to pass values of Category as temp data to the View for dropdown
            *//*ViewBag.CategoryList = CategoryList;*//*

            // Use of ViewData to pass values of Category as temp data to the View for dropdown
            *//*ViewData["CategoryList"] = CategoryList;*//*

            // Use of ViewModel to pass values of Category as temp data to the View for dropdown
            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            return View(productVM);
        }
        */



        // Upsert function to handle create and update together
        public IActionResult Upsert(int? id)
        {

            // Using Projection to get items from Category and setting a new object with selected fields of type SelectListItem - useful to create dropdown
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            /*// Use of ViewBag to pass values of Category as temp data to the View for dropdown
            ViewBag.CategoryList = CategoryList;

            // Use of ViewData to pass values of Category as temp data to the View for dropdown
            ViewData["CategoryList"] = CategoryList;*/


            // Use of ViewModel to pass values of Category as temp data to the View for dropdown
            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // create
                return View(productVM);
            }
            else
            {
                // update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }



        /*// Separate Post for Create Function
        // Adding new Product in DB from the create page
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();

                TempData["success"] = "Product Created Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Loading the Edit for existing product page view
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? objProduct = _unitOfWork.Product.Get(u => u.Id == id);

            if (objProduct == null)
            {
                return NotFound(id.ToString());
            }

            return View(objProduct);
        }*/

        // Upsert new Product in DB from the create page
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();

                TempData["success"] = "Product Created Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }




        /*// Separate Edit Method
        // Loading the Edit for existing product page view
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? objProduct = _unitOfWork.Product.Get(u => u.Id == id);

            if (objProduct == null)
            {
                return NotFound(id.ToString());
            }

            return View(objProduct);
        }

        // Separate Edit Method to update DB
        // Modify the data from the Edit of existing product after done
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product Updated Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }*/




        // Loading the Delete Page for existing product
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? objProduct = _unitOfWork.Product.Get(u => u.Id == id);

            if (objProduct == null)
            {
                return NotFound(id.ToString());
            }

            return View(objProduct);
        }

        // Delete the data from the DB that has been selected
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? productObj = _unitOfWork.Product.Get(u => u.Id == id);
            if (productObj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(productObj);
            _unitOfWork.Save();

            TempData["success"] = "Product Deleted Successfully!";

            return RedirectToAction("Index");
        }
    }
}
