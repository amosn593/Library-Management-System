using DOMAIN.IConfiguration;
using DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using REPORTS.Dtos;

namespace VIEW.Controllers
{
    [Authorize(Policy = "Basic")]
    public class FormController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly INotyfService _notyf;

        private readonly IMapper _mapper;
        public FormController(IUnitOfWork unitOfWork, INotyfService notyf, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _mapper = mapper;
        }
        // GET All Forms: FormController
        public async Task<IActionResult> Index()
        {
            try
            {
                var form = await _unitOfWork.Form.GetAll();
                var formDto = _mapper.Map<IEnumerable<FormDto>>(form);
                return View(formDto);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return View();
            }
            
        }

        
        // GET: FormController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] FormDto formDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var form = _mapper.Map<Form>(formDto);
                    _unitOfWork.Form.Add(form);
                    await _unitOfWork.Complete();
                    _notyf.Success("Form Added successfully.");
                    return RedirectToAction(nameof(Index));

                }
                catch(Exception)
                {
                    throw new Exception("Someting went wrong, try again later!!!");
                }

            }
            else
            {
                return View(formDto);
            }
                
        }



        // GET: FormController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _notyf.Error("Form With that Id is not found!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var form = await _unitOfWork.Form.GetById((int)id);
                if (form == null)
                {
                    _notyf.Error("Form With that Id is not found!!!");
                    return RedirectToAction(nameof(Index));
                }
                var formDto = _mapper.Map<FormDto>(form);
                return View(formDto);
            }
            catch(Exception)
            {
                _notyf.Error("Someting went wrong, try again later!!!");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: FormController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FormDto formDto)
        {
            if (id != formDto.Id)
            {
                _notyf.Error("Form With that Id is not found!!!");
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var form = _mapper.Map<Form>(formDto);
                    _unitOfWork.Form.Update(form);
                    await _unitOfWork.Complete();
                    _notyf.Success("Form Updated Successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _notyf.Error("Error while updating database!!!");
                    return RedirectToAction(nameof(Index));
                }
               
            }
            else
            {
                _notyf.Error("Correct below errors!!!");
                return View(formDto);

            }
            
        }

        // POST: Books/Delete/5
        //[Authorize(Policy = "PrincipalUsers")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _notyf.Error("Form With that Id is not found!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                 await _unitOfWork.Form.Delete((int)id); ;
                await _unitOfWork.Complete();
                _notyf.Success("Form Deleted Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                _notyf.Error("Something went wrong, try again!!!");
                return RedirectToAction(nameof(Index));
            }

        }

    }
}
