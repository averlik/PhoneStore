using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PhoneStore.Context
{
    public class SeedData
    {
        public static void SeedDatabase(AppDbContext context)
        {
            context.Database.Migrate();

            if (!context.Phones.Any())
            {
                Brand Samsung = new Brand { BrandName = "Samsung", Slug = "Samsung" };
                Brand Apple = new Brand { BrandName = "Apple", Slug = "Apple" };
                Brand Sony = new Brand { BrandName = "Sony", Slug = "Sony" };
                Brand Huawei = new Brand { BrandName = "Huawei", Slug = "Huawei" };
                Brand Honor = new Brand { BrandName = "Honor", Slug = "Honor" };

                context.Phones.AddRange(
                        new Phone
                        {
                            Name = "IPhone 15 pro",
                            Slug = "Apple",
                            Description = "",
                            Price = 120000,
                            Brand = Apple,
                            Image = "iphone15pro.jpg",
                            CountSales = 3,
                            IsDeleted = false,
                        },
                        new Phone
                        {
                            Name = "Samsung Galaxy S22 Ultra ",
                            Slug = "Samsung",
                            Description = "",
                            Price = 89999,
                            Brand = Samsung,
                            Image = "",
                            CountSales = 2,
                            IsDeleted = false,
                        },
                        new Phone
                        {
                             Name = "HUAWEI nova",
                             Slug = "Huawei",
                             Description = "",
                             Price = 35000,
                             Brand = Huawei,
                             Image = "",
                             CountSales = 5,
                             IsDeleted = false,
                         
                        },
                         new Phone
                         {
                             Name = "Sony Erexon",
                             Slug = "Sony",
                             Description = "",
                             Price = 59000,
                             Brand = Sony,
                             Image = "",
                             CountSales = 2,
                             IsDeleted = false,

                         },
                          new Phone
                          {
                              Name = "Honor 12",
                              Slug = "Honor",
                              Description = "",
                              Price = 59000,
                              Brand = Honor,
                              Image = "",
                              CountSales = 2,
                              IsDeleted = false,

                          }
                );

                context.SaveChanges();
            }
        }
    }
}
