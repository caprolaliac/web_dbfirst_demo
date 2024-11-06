using web_db.Models;

namespace web_db.Repository
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAllBrands();
        Brand GetBrandById(int id);
        string AddBrand(Brand brand);
        string UpdateBrand(Brand brand);
        string DeleteBrand(int id);
    }
}