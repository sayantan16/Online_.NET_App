using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // we will return multiple Company objects so List<>
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }


        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // create
                // we will send a new Company class instance as an object to set with new user response data
                return View(new Company());
            }
            else
            {
                // update
                // we will return single Company Object so just Company
                Company compObjList = _unitOfWork.Company.Get(u => u.Id == id);
                return View(compObjList);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                // if id is not present then it is Add
                if (obj.Id == 0 || obj.Id == null)
                {
                    _unitOfWork.Company.Add(obj);
                }
                // if id is present while submitting, then it is Update
                else
                {
                    _unitOfWork.Company.Update(obj);
                }
                _unitOfWork.Save();

                TempData["success"] = "Company Created Successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Loading the Delete Page for existing product
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Company? objCompany = _unitOfWork.Company.Get(u => u.Id == id);

            if (objCompany == null)
            {
                return NotFound(id.ToString());
            }

            return View(objCompany);
        }

        // Delete the data from the DB that has been selected
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? objCompany = _unitOfWork.Company.Get(u => u.Id == id);
            if (objCompany == null)
            {
                return NotFound();
            }
            
            _unitOfWork.Company.Remove(objCompany);
            _unitOfWork.Save();

            TempData["success"] = "Company Deleted Successfully!";

            return RedirectToAction("Index");
        }

    }
}
