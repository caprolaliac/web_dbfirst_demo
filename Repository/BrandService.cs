using web_db.Models;

namespace web_db.Repository
{
    public class BrandService : IBrandService
    {
        private readonly BikestoreContext _context;
        public BrandService(BikestoreContext context)
        {
            _context = context;
        }
        public string AddBrand(Brand brand)
        {
            if (brand != null && !string.IsNullOrEmpty(brand.BrandName))
            {
                var existingBrand = _context.Brands.FirstOrDefault(x => x.BrandName == brand.BrandName);
                if (existingBrand != null)
                {
                    return "Brand with the name '" + brand.BrandName + "' already exists.";
                }
                _context.Brands.Add(brand);

                _context.SaveChanges();

                return "Brand with name '" + brand.BrandName + "' added successfully.";
            }

            return "Invalid brand data. Brand name cannot be null or empty.";
        }


        public string DeleteBrand(int id)
        {
            if (id != 0)
            {
                var brand = _context.Brands.FirstOrDefault(x => x.BrandId == id);
                if (brand != null)
                {
                    _context.Brands.Remove(brand);
                    _context.SaveChanges();
                    return "The given Brand ID " + id + " was removed successfully.";
                }
                else
                {
                    return "Something went wrong with deletion, Brand not found.";
                }
            }
            return "ID should not be null or zero.";
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _context.Brands;
        }

        public Brand GetBrandById(int id)
        {
            if (id != 0)
            {
                var brand = _context.Brands.FirstOrDefault(x => x.BrandId == id);
                if (brand != null)
                    return brand;
                else
                    return null;
            }
            return null;
        }

        public string UpdateBrand(Brand brand)
        {
            if (brand != null && brand.BrandId != 0)
            {
                var existingBrand = _context.Brands.FirstOrDefault(x => x.BrandId == brand.BrandId);
                if (existingBrand == null)
                {
                    return "Brand not found with the given ID.";
                }

                existingBrand.BrandName = brand.BrandName;
                existingBrand.Products = brand.Products;
                _context.Entry(existingBrand).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();

                return "Brand with ID " + brand.BrandId + " updated successfully.";
            }

            return "Invalid brand data or ID.";
        }

    }
}
