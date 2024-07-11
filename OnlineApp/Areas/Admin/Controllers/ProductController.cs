using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineApp.DataAccess.Repository.IRepository;
using OnlineApp.Models;
using OnlineApp.Models.ViewModels;
using OnlineApp.Utility;

namespace OnlineApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // Get all values in Product table from DB
        public IActionResult Index()
        {
            // the name inside the includeProperties should match with the actual field name given in the Model - Product
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
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
                // handling of saving file in wwwRoot folder - images\product and setting the path in ImageURL of Product Model
                
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        // delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create) )
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                }

                // if id is not present then it is Add
                if (obj.Product.Id == 0 || obj.Product.Id == null)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                // if id is present while submitting, then it is Update
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
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
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var oldImagePath = Path.Combine(wwwRootPath, productObj.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productObj);
            _unitOfWork.Save();

            TempData["success"] = "Product Deleted Successfully!";

            return RedirectToAction("Index");
        }



        // creating an API for getall function which sends the JSON object with all data
        // accessible under URI/admin/product/getall
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json( new { data = objProductList });
        }

        #endregion

    }
}
