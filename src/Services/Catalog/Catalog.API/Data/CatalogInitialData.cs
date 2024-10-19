using Catalog.API.Models;
using Marten.Schema;
using System.Collections.Generic;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().CountAsync() > 0)
                return;

            //Marten UPSERT 
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync();
        }

        
    
        private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product> {
            new Product()
            {
                Id =  new Guid("b1a68733-9c99-4b35-a56f-7c5809ed7aa3"),
                Name = "IPhone 13",
                Description = "Apple is back with new iPhone 13 advertisements that highlight some of its features, such as the durability of Ceramic Shield glass and the longer battery life",
                ImageFile = "http://cloudinary/iphone13.jpg",
                Price = 950.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id = new Guid("f2d0f157-f1ba-4e12-82d4-bf9f39b693a5"),
   
                Name = "Samsung Galaxy S21",
                Description = "Samsung Galaxy S21 offers a dynamic AMOLED display, 120Hz refresh rate, and pro-grade camera features with 8K video recording capabilities.",
                ImageFile = "http://cloudinary/samsungs21.jpg",
                Price = 800.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =  new Guid("83207fcf-5a4e-43e1-8d69-d48d1e7aaf74"),
 
                Name = "Google Pixel 6",
                Description = "Google's flagship phone with a powerful AI-driven camera, smooth performance, and built-in security features powered by Tensor.",
                ImageFile = "http://cloudinary/googlepixel6.jpg",
                Price = 700.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =    new Guid("5b7d89a1-32bc-44db-a4e6-5d3b9c4e9356"),
  
                Name = "OnePlus 9 Pro",
                Description = "OnePlus 9 Pro delivers ultra-smooth performance with Snapdragon 888, 120Hz display, and Hasselblad camera for stunning photography.",
                ImageFile = "http://cloudinary/oneplus9pro.jpg",
                Price = 900.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =   new Guid("3cb564c5-b784-4cf5-9d6b-bd6d399fef24"),

                Name = "Sony Xperia 1 III",
                Description = "Sony Xperia 1 III offers a 4K OLED display with a 120Hz refresh rate and pro-level camera features, including real-time autofocus tracking.",
                ImageFile = "http://cloudinary/sonyxperia1.jpg",
                Price = 1100.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =     new Guid("beecaf9d-911e-42e0-9f5f-2dfe8dc10742"),

                Name = "Xiaomi Mi 11 Ultra",
                Description = "Xiaomi Mi 11 Ultra combines a 120Hz AMOLED display with a triple-camera setup, featuring a 50MP wide-angle lens and 120x digital zoom.",
                ImageFile = "http://cloudinary/xiaomimi11ultra.jpg",
                Price = 950.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =     new Guid("6fbcf894-3d8c-4873-93b7-0540c62a6b68"),
  
                Name = "IPhone 12 Pro Max",
                Description = "IPhone 12 Pro Max comes with a Super Retina XDR display and 5G support, making it one of the most advanced iPhones ever.",
                ImageFile = "http://cloudinary/iphone12promax.jpg",
                Price = 1100.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =   new Guid("742c8a56-06ef-4643-9b0f-e19803a0b891"),
   
                Name = "Samsung Galaxy Note 20 Ultra",
                Description = "Samsung Galaxy Note 20 Ultra offers the best of productivity with its S Pen and powerful Snapdragon 865+ processor.",
                ImageFile = "http://cloudinary/samsungnote20ultra.jpg",
                Price = 950.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =  new Guid("009726ac-799e-4fcb-9d83-c2a8e06ecf58"),
  
                Name = "Huawei P40 Pro",
                Description = "Huawei P40 Pro excels in photography with its Leica quad camera setup, AI features, and 5G performance.",
                ImageFile = "http://cloudinary/huaweip40pro.jpg",
                Price = 850.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =   new Guid("abfbf24f-f17c-4ff0-a017-e284d1b017ff"),
  
                Name = "Oppo Find X3 Pro",
                Description = "Oppo Find X3 Pro offers a 10-bit color display, fast charging, and a flagship Snapdragon 888 processor.",
                ImageFile = "http://cloudinary/oppofindx3pro.jpg",
                Price = 920.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =   new Guid("b8f60ebd-2823-4969-822b-d3c4178d118d"),
 
                Name = "Motorola Edge Plus",
                Description = "Motorola Edge Plus comes with a 108MP camera, a 90Hz display, and 5G connectivity, delivering a flagship experience.",
                ImageFile = "http://cloudinary/motorolaedgeplus.jpg",
                Price = 800.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            },
            new Product()
            {
                Id =   new Guid("bb8df0e1-7c0c-4416-b6f5-5d9a268ec64a"),
                Name = "Nokia 8.3 5G",
                Description = "Nokia 8.3 5G is a future-ready smartphone with 5G support, a PureView quad camera, and Android One.",
                ImageFile = "http://cloudinary/nokia83.jpg",
                Price = 700.00M,
                Category = new List<string> { "Phone", "Smart Phone", "Mobile" }
            }
        };

    }

}
