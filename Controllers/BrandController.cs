using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_db.Models;
using web_db.Repository;

namespace web_db.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllBrands()
        {
            var brands = _brandService.GetAllBrands();
            if (brands == null || !brands.Any())
            {
                return NotFound("No brands found.");
            }
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public IActionResult GetBrandById(int id)
        {
            var brand = _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound($"Brand with ID {id} not found.");
            }
            return Ok(brand);
        }

        [HttpPost]
        public IActionResult CreateBrand([FromBody] Brand brand)
        {
            if (brand == null)
            {
                return BadRequest("Brand data is required.");
            }

            _brandService.AddBrand(brand);
            return CreatedAtAction(nameof(GetBrandById), new { id = brand.BrandId }, brand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBrand(int id, [FromBody] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return BadRequest("Brand ID mismatch.");
            }

            var existingBrand = _brandService.GetBrandById(id);
            if (existingBrand == null)
            {
                return NotFound($"Brand with ID {id} not found.");
            }

            _brandService.UpdateBrand(brand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound($"Brand with ID {id} not found.");
            }

            _brandService.DeleteBrand(id);
            return NoContent();
        }
    }
}
