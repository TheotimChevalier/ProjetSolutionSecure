using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetSolutionSecure.Data;
using ProjetSolutionSecure.Models;


namespace ProjetSolutionSecure.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "Admin,Manager_Carrosserie,Manager_Moteur,Manager_Peinture")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!User.IsInRole($"Manager_{product.Type}") && !User.IsInRole("Admin"))
            {
                ModelState.AddModelError("", "Tu peux pas LOL.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);      
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            else
            {
                return NotFound();
            }
        }
        // GET: Products/Create
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        public IActionResult Create()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole("Admin"))
                {
                    ViewData["Type"] = new SelectList(Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Where(x => x == ProductType.Carrosserie));
                }
                else if (User.IsInRole("Manager_Moteur"))
                {
                    ViewData["Type"] = new SelectList(Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Where(x => x == ProductType.Moteur));
                }
                else if (User.IsInRole("Manager_Peinture"))
                {
                    ViewData["Type"] = new SelectList(Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Where(x => x == ProductType.Peinture));
                }
                else if (User.IsInRole("Manager_Carrosserie"))
                {

                    ViewData["Type"] = new SelectList(Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Where(x => x == ProductType.Carrosserie));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (id == null)
                {
                    return NotFound();
                }
                if (User.IsInRole("Admin"))
                {
                    var product = await _context.Product.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Carrosserie"))
                {   
                    var product = await _context.Product.FindAsync(id);
                    if (product == null | product.Type != ProductType.Carrosserie)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Moteur"))
                {
                    var product = await _context.Product.FindAsync(id);
                    if (product == null | product.Type != ProductType.Moteur)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Peinture"))
                {
                    var product = await _context.Product.FindAsync(id);
                    if (product == null | product.Type != ProductType.Peinture)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Products/Edit/5
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Manufacturer,Price,AdditionalInformation,Type")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (!User.IsInRole($"Manager_{product.Type}") && !User.IsInRole("Admin"))
            {
                ModelState.AddModelError("", "Tu peux pas LOL.");
            }

            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Manager_Carrosserie, Admin") && product.Type == ProductType.Carrosserie)
                {
                    return NotFound();
                }
                else if (!User.IsInRole("Manager_Peinture, Admin") && product.Type == ProductType.Peinture)
                {
                    return NotFound(); 
                }
                else if (!User.IsInRole("Manager_Moteur, Admin") && product.Type == ProductType.Moteur)
                {
                    return NotFound(); 
                }
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }



        // GET: Products/Delete/5
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (id == null)
                {
                    return NotFound();
                }
                if (User.IsInRole("Admin"))
                {
                    var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Carrosserie"))
                {
                    var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                    if (product == null | product.Type != ProductType.Carrosserie)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Moteur"))
                {
                    var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                    if (product == null | product.Type != ProductType.Moteur)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else if (User.IsInRole("Manager_Peinture"))
                {
                    var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                    if (product == null | product.Type != ProductType.Peinture)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Manager_Carrosserie,Manager_Moteur,Manager_Peinture,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var product = await _context.Product.FindAsync(id);
                if (product != null)
                {
                    _context.Product.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }


    }
}
