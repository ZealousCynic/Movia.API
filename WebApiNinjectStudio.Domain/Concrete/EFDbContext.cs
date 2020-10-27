using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFDbContext : DbContext
    {

        public EFDbContext(DbContextOptions<EFDbContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Bus> Busses { get; set; }
        public DbSet<BusModel> BusModels { get; set; }
        public DbSet<BusDriver> BusDrivers { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteBusStop> RouteBusStops { get; set; }
        public DbSet<RouteBus> RouteBusses { get; set; }
        public DbSet<NumberOfPassenger> NumberOfPassengers { get; set; }


        public DbSet<ApiUrl> ApiUrls { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermissionApiUrl> RolePermissionApiUrls { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Database relationships
            //one to many Role -> User
            modelBuilder.Entity<Role>()
                .HasMany(u => u.Users)
                .WithOne(r => r.Role)
                .HasForeignKey(u => u.RoleID);

            //many to many Role <-- RolePermissionApiUrl --> ApiUrl
            //composite key
            modelBuilder.Entity<RolePermissionApiUrl>()
                .HasKey(ra => new { ra.RoleID, ra.ApiUrlID });
            //one to many RolePermissionApiUrl -> Role
            modelBuilder.Entity<RolePermissionApiUrl>()
                .HasOne(rpa => rpa.Role)
                .WithMany(r => r.RolePermissionApiUrls)
                .HasForeignKey(rpa => rpa.RoleID);
            //one to many RolePermissionApiUrl -> ApiUrl
            modelBuilder.Entity<RolePermissionApiUrl>()
                .HasOne(rpa => rpa.ApiUrl)
                .WithMany(r => r.RolePermissionApiUrls)
                .HasForeignKey(rpa => rpa.ApiUrlID);


            //one to one Product -> ProductImage
            modelBuilder.Entity<Product>()
                .HasOne(pi => pi.ProductImage)
                .WithOne(p => p.Product)
                .HasForeignKey<ProductImage>(pi => pi.ProductID);

            //one to many Product -> ProductTag
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductTag)
                .WithOne(pt => pt.Product)
                .HasForeignKey(pt => pt.ProductID);

            //many to many Product <-- ProductCategory --> Category
            //composite key
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductID, pc.CategoryId });
            //one to many ProductCategory -> Product
            modelBuilder.Entity<ProductCategory>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.ProductID);
            //one to may ProductCategory -> Category
            modelBuilder.Entity<ProductCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            //one to many BusModel -> Bus
            modelBuilder.Entity<BusModel>()
                .HasMany(bm => bm.Bus)
                .WithOne(b => b.BusModel)
                .HasForeignKey(b => b.BusModelID);

            ////Composite Primary Key BusDriver
            //modelBuilder.Entity<BusDriver>().HasKey(bd => new { bd.ID, bd.PersonnelNumber });

            //many to many Route <-- RouteBusStop --> BusStop
            //composite key
            modelBuilder.Entity<RouteBusStop>()
                .HasKey(rbs => new { rbs.RouteID, rbs.BusStopID });
            //one to many RouteBusStop -> Route
            modelBuilder.Entity<RouteBusStop>()
                .HasOne(rbs => rbs.Route)
                .WithMany(r => r.RouteBusStops)
                .HasForeignKey(rbs => rbs.RouteID);
            //one to many RouteBusStop -> BusStop
            modelBuilder.Entity<RouteBusStop>()
                .HasOne(rbs => rbs.BusStop)
                .WithMany(b => b.RouteBusStops)
                .HasForeignKey(rbs => rbs.BusStopID);

            //many to many:
            //              Route
            //                ^
            //                |            
            //BusDriver <-- RouteBus --> Bus
            //composite key
            //modelBuilder.Entity<RouteBus>()
            //    .HasKey(rb => new { rb.RouteID, rb.BusDriverID, rb.BusID });
            //one to many RouteBus -> Route
            modelBuilder.Entity<RouteBus>()
                .HasOne(rb => rb.Route)
                .WithMany(r => r.RouteBusses)
                .HasForeignKey(rb => rb.RouteID);
            //one to many RouteBus -> BusDriver
            modelBuilder.Entity<RouteBus>()
                .HasOne(rb => rb.BusDriver)
                .WithMany(bd => bd.RouteBusses)
                .HasForeignKey(rb => rb.BusDriverID);
            //one to many RouteBus -> Bus
            modelBuilder.Entity<RouteBus>()
                .HasOne(rb => rb.Bus)
                .WithMany(bd => bd.RouteBusses)
                .HasForeignKey(rb => rb.BusID);

            //one to many RouteBus -> NumberOfPassenger
            modelBuilder.Entity<RouteBus>()
                .HasMany(rb => rb.NumberOfPassengers)
                .WithOne(nop => nop.RouteBus)
                .HasForeignKey(rb => rb.RouteBusID);

            #endregion

            #region  Soft Delete using Query Filters
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //Add the IsDeleted property
                entityType.AddProperty("IsDeleted", typeof(bool));

                //Create the query filter
                var parameter = Expression.Parameter(entityType.ClrType);

                //EF.Property<bool>(post, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

                //EF.Property<bool>(post, "IsDeleted") == false
                var compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                //post => EF.Property<bool>(post, "IsDeleted") == false
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
            #endregion

            #region Initial Data 

            #region ApiUrl            
            modelBuilder.Entity<ApiUrl>().HasData(
                new { ApiUrlID = 1, ApiUrlString = "/api/User", ApiRequestMethod = "Get", IsDeleted = false },
                new { ApiUrlID = 2, ApiUrlString = "/api/User/GetUserID", ApiRequestMethod = "Get", IsDeleted = false },
                new { ApiUrlID = 3, ApiUrlString = "/api/product", ApiRequestMethod = "Get", IsDeleted = false },
                new { ApiUrlID = 4, ApiUrlString = "/api/product", ApiRequestMethod = "Post", IsDeleted = false },
                //Categories
                new { ApiUrlID = 5, ApiRequestMethod = "Post", ApiUrlString = @"/api/v1/categories", ApiUrlRegex = @"^\/api\/v1\/categories$", Description = "Create category", IsDeleted = false },
                new { ApiUrlID = 6, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/categories", ApiUrlRegex = @"^\/api\/v1\/categories$", Description = "Get category list", IsDeleted = false },
                new { ApiUrlID = 7, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/categories/{categoryId}", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+$", Description = "Get info about category by category id", IsDeleted = false },
                new { ApiUrlID = 8, ApiRequestMethod = "Delete", ApiUrlString = @"/api/v1/categories/{categoryId}", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+$", Description = "Delete category by identifier", IsDeleted = false },
                new { ApiUrlID = 9, ApiRequestMethod = "Put", ApiUrlString = @"/api/v1/categories/{categoryId}", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+$", Description = "Update category by identifier", IsDeleted = false },
                new { ApiUrlID = 10, ApiRequestMethod = "Post", ApiUrlString = @"/api/v1/categories/{categoryId}/products", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+\/products$", Description = "Assign a product to the required category", IsDeleted = false },
                new { ApiUrlID = 11, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/categories/{categoryId}/products", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+\/products(\?.*)*$", Description = "Get products assigned to category", IsDeleted = false },
                new { ApiUrlID = 12, ApiRequestMethod = "Put", ApiUrlString = @"/api/v1/categories/{categoryId}/products", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+\/products$", Description = "Assign a product to the required category", IsDeleted = false },
                new { ApiUrlID = 13, ApiRequestMethod = "Delete", ApiUrlString = @"/api/v1/categories/{categoryId}/products/{productId}", ApiUrlRegex = @"^\/api\/v1\/categories\/\d+\/products\/\d+$", Description = "Remove the product assignment from the category by category id and product id", IsDeleted = false },
                //Products
                new { ApiUrlID = 14, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/products", ApiUrlRegex = @"^\/api\/v1\/products(\?.*)*$", Description = "Get product list", IsDeleted = false },
                new { ApiUrlID = 15, ApiRequestMethod = "Post", ApiUrlString = @"/api/v1/products", ApiUrlRegex = @"^\/api\/v1\/products$", Description = "Create product", IsDeleted = false },
                new { ApiUrlID = 16, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/products/{productId}", ApiUrlRegex = @"^\/api\/v1\/products\/\d+$", Description = "Get info about product by product id", IsDeleted = false },
                new { ApiUrlID = 17, ApiRequestMethod = "Put", ApiUrlString = @"/api/v1/products/{productId}", ApiUrlRegex = @"^\/api\/v1\/products\/\d+$", Description = "Update the product by product id", IsDeleted = false },
                new { ApiUrlID = 18, ApiRequestMethod = "Delete", ApiUrlString = "/api/v1/products/{productId}", ApiUrlRegex = @"^\/api\/v1\/products\/\d+$", Description = "Remove the product by product id", IsDeleted = false },
                // integrations
                new { ApiUrlID = 19, ApiRequestMethod = "Get", ApiUrlString = @"/api/v1/integrations/customer/userid", ApiUrlRegex = @"^\/api\/v1\/integrations\/customer\/userid$", Description = "Get id of current user.", IsDeleted = false }
            );
            #endregion

            #region Role
            modelBuilder.Entity<Role>().HasData(
                new { RoleID = 1, Name = "Administrator", IsDeleted = false },
                new { RoleID = 2, Name = "Guest", IsDeleted = false }
            );
            #endregion

            # region RolePermissionApiUrl
            modelBuilder.Entity<RolePermissionApiUrl>().HasData(
                new { RoleID = 1, ApiUrlID = 1, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 2, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 3, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 4, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 5, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 6, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 7, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 8, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 9, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 10, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 11, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 12, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 13, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 14, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 15, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 16, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 17, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 18, IsDeleted = false },
                new { RoleID = 1, ApiUrlID = 19, IsDeleted = false },
                new { RoleID = 2, ApiUrlID = 3, IsDeleted = false },
                new { RoleID = 2, ApiUrlID = 4, IsDeleted = false }
            );
            #endregion

            #region User
            modelBuilder.Entity<User>().HasData(
                //Password = "HelloWorld"
                new
                {
                    UserID = 1,
                    Email = "one@gmail.com",
                    FirstName = "Kim",
                    LastName = "Nielsen",
                    Password = "M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=",
                    RoleID = 1,
                    IsDeleted = false
                },
                //Password = "Abc123"
                new
                {
                    UserID = 2,
                    Email = "two@gmail.com",
                    FirstName = "Martin",
                    LastName = "Jensen",
                    Password = "FOHqRDbYuVdIBvLS6r2YMVU4Yu7E54DJJJxrWGh5YZc=",
                    RoleID = 2,
                    IsDeleted = false
                }
            );
            #endregion

            #region Category
            modelBuilder.Entity<Category>().HasData(
                new { CategoryId = 1, CategoryName = "Soveværelse", IsDeleted = false },
                new { CategoryId = 2, CategoryName = "Badeværelse", IsDeleted = false },
                new { CategoryId = 3, CategoryName = "Kontor", IsDeleted = false },
                new { CategoryId = 4, CategoryName = "Stue", IsDeleted = false },
                new { CategoryId = 5, CategoryName = "Spisestue", IsDeleted = false },
                new { CategoryId = 6, CategoryName = "Opbevaring", IsDeleted = false },
                new { CategoryId = 7, CategoryName = "Have", IsDeleted = false },
                new { CategoryId = 8, CategoryName = "Indretning", IsDeleted = false }
            );
            #endregion

            #region Product
            modelBuilder.Entity<Product>().HasData(
                new
                {
                    ProductID = 1,
                    Name = "Kontorstol REGSTRUP sort/grå",
                    Description = "Bremsehjul, Højdejusterbar, Nylonbase, Trinløs vippefunktion",
                    Price = 300m,
                    IsDeleted = false
                },
                new
                {
                    ProductID = 2,
                    Name = "Barstol KLARUP sort/krom",
                    Description = "Højdejusterbar",
                    Price = 250m,
                    IsDeleted = false
                }
            );
            #endregion

            #region ProductImage
            modelBuilder.Entity<ProductImage>().HasData(
                new
                {
                    ProductImageId = 1,
                    ProductID = 1,
                    Url = "jysk.dk/kontor/kontorstole/basic/kontorstol-regstrup-sort-graa",
                    IsDeleted = false
                },
                new
                {
                    ProductImageId = 2,
                    ProductID = 2,
                    Url = "jysk.dk/spisestue/barborde-stole/barstol-klarup-sort-krom-0",
                    IsDeleted = false
                }
            );
            #endregion

            #region ProductTag
            modelBuilder.Entity<ProductTag>().HasData(
                new { ProductTagID = 1, ProductID = 1, Name = "kontorstol", IsDeleted = false },
                new { ProductTagID = 2, ProductID = 1, Name = "sort", IsDeleted = false },
                new { ProductTagID = 3, ProductID = 1, Name = "skum", IsDeleted = false },
                new { ProductTagID = 4, ProductID = 1, Name = "metal", IsDeleted = false },
                new { ProductTagID = 5, ProductID = 1, Name = "krydsfiner", IsDeleted = false },
                new { ProductTagID = 6, ProductID = 2, Name = "kunstlæder", IsDeleted = false },
                new { ProductTagID = 7, ProductID = 2, Name = "skum", IsDeleted = false },
                new { ProductTagID = 8, ProductID = 2, Name = "metal", IsDeleted = false },
                new { ProductTagID = 9, ProductID = 2, Name = "polypropylen", IsDeleted = false }
            );
            #endregion

            #region ProductCategory
            modelBuilder.Entity<ProductCategory>().HasData(
                new { CategoryId = 3, ProductID = 1, IsDeleted = false },
                new { CategoryId = 5, ProductID = 1, IsDeleted = false },
                new { CategoryId = 5, ProductID = 2, IsDeleted = false }
            );
            #endregion

            #region BusModel
            modelBuilder.Entity<BusModel>().HasData(
                new { ID = 1, Manufacturer = "Volvo", Model = "8900", Length = "13,7/14,7 m (lav indstigning 3 aksler)", Width = "2,55 m", Height = "3,30 m", PowerTrain = "Volvo D8K, 6-cylindret dieselrækkemotor. 280, 320 eller 350 hk.", IsDeleted = false },
                new { ID = 2, Manufacturer = "Volvo", Model = "7900 HYBRID", Length = "10,6/12,0 m (2 aksler)", Width = "2,55 m", Height = "3,28 m", PowerTrain = "Volvo D5K 240, 4-cylindret, dieselrækkemotor, 240 hk.; Volvo I-SAM, 120 kW/800 Nm (2 aksler)", IsDeleted = false },
                new { ID = 3, Manufacturer = "Volvo", Model = "7900 ELECTRIC", Length = "12 m", Width = "2.55 m", Height = "3.30 m", PowerTrain = "High capacity Lithium-Ion battery", IsDeleted = false },
                new { ID = 4, Manufacturer = "Mercedes-Benz", Model = "Citaro", Length = "12.135 m", Width = "2.55 m", Height = "3.12 m", PowerTrain = "Mercedes-Benz OM 936 h", IsDeleted = false },
                new { ID = 5, Manufacturer = "Mercedes-Benz", Model = "Citaro NGT", Length = "12.135 m", Width = "2.55 m", Height = "3.12 m", PowerTrain = "Mercedes-Benz M 936 G", IsDeleted = false },
                new { ID = 6, Manufacturer = "Mercedes-Benz", Model = "eCitaro", Length = "12.135 m", Width = "2.55 m", Height = "3.4 m", PowerTrain = "ZF AVE 130 elmotorer", IsDeleted = false }
            );
            #endregion

            #region Bus
            modelBuilder.Entity<Bus>().HasData(
                new { ID = 1, RegistrationNumber = "BJ32621", CapacityBoundary = 45, SeatingPlace = 35, StandingPlace = 40, BusModelID = 1, IsDeleted = false },
                new { ID = 2, RegistrationNumber = "HG30202", CapacityBoundary = 40, SeatingPlace = 35, StandingPlace = 35, BusModelID = 1, IsDeleted = false },
                new { ID = 3, RegistrationNumber = "MD21233", CapacityBoundary = 45, SeatingPlace = 35, StandingPlace = 45, BusModelID = 2, IsDeleted = false },
                new { ID = 4, RegistrationNumber = "YU57648", CapacityBoundary = 45, SeatingPlace = 35, StandingPlace = 45, BusModelID = 2, IsDeleted = false },
                new { ID = 5, RegistrationNumber = "AF22454", CapacityBoundary = 45, SeatingPlace = 25, StandingPlace = 60, BusModelID = 3, IsDeleted = false },
                new { ID = 6, RegistrationNumber = "AF33912", CapacityBoundary = 45, SeatingPlace = 25, StandingPlace = 60, BusModelID = 3, IsDeleted = false },
                new { ID = 7, RegistrationNumber = "XT33042", CapacityBoundary = 45, SeatingPlace = 32, StandingPlace = 40, BusModelID = 3, IsDeleted = false },
                new { ID = 8, RegistrationNumber = "AF25463", CapacityBoundary = 30, SeatingPlace = 30, StandingPlace = 45, BusModelID = 4, IsDeleted = false },
                new { ID = 9, RegistrationNumber = "AD32343", CapacityBoundary = 50, SeatingPlace = 40, StandingPlace = 50, BusModelID = 5, IsDeleted = false },
                new { ID = 10, RegistrationNumber = "AB11139", CapacityBoundary = 30, SeatingPlace = 20, StandingPlace = 60, BusModelID = 5, IsDeleted = false },
                new { ID = 11, RegistrationNumber = "BP54379", CapacityBoundary = 60, SeatingPlace = 45, StandingPlace = 40, BusModelID = 6, IsDeleted = false },
                new { ID = 12, RegistrationNumber = "BV28091", CapacityBoundary = 35, SeatingPlace = 45, StandingPlace = 30, BusModelID = 6, IsDeleted = false }
            );


            #endregion

            #region BusDriver
            modelBuilder.Entity<BusDriver>().HasData(                              
                new { ID = 1, PersonnelNumber = "D-0100", FirstName = "Peter", LastName = "Nielsen", PhoneNumber = "41539333", IsDeleted = false },
                new { ID = 2, PersonnelNumber = "D-0101", FirstName = "Jens", LastName = "Jensen", PhoneNumber = "32533534", IsDeleted = false },
                new { ID = 3, PersonnelNumber = "D-0102", FirstName = "Michael", LastName = "Hansen", PhoneNumber = "24804732", IsDeleted = false },
                new { ID = 4, PersonnelNumber = "D-0103", FirstName = "Lars", LastName = "Pedersen", PhoneNumber = "85570209", IsDeleted = false },
                new { ID = 5, PersonnelNumber = "D-0104", FirstName = "Thomas", LastName = "Andersen", PhoneNumber = "98205272", IsDeleted = false },
                new { ID = 6, PersonnelNumber = "D-0105", FirstName = "Henrik", LastName = "Christensen", PhoneNumber = "87740418", IsDeleted = false },
                new { ID = 7, PersonnelNumber = "D-0107", FirstName = "Søren", LastName = "Larsen", PhoneNumber = "83920099", IsDeleted = false },
                new { ID = 8, PersonnelNumber = "D-0108", FirstName = "Christian", LastName = "Sørensen", PhoneNumber = "59771920", IsDeleted = false },
                new { ID = 9, PersonnelNumber = "D-0109", FirstName = "Jan", LastName = "Rasmussen", PhoneNumber = "36159850", IsDeleted = false },
                new { ID = 10, PersonnelNumber = "D-0110", FirstName = "Martin", LastName = "Jørgensen", PhoneNumber = "93270098", IsDeleted = false },
                new { ID = 11, PersonnelNumber = "D-0111", FirstName = "Niels", LastName = "Petersen", PhoneNumber = "91025134", IsDeleted = false },
                new { ID = 12, PersonnelNumber = "D-0112", FirstName = "Anders", LastName = "Madsen", PhoneNumber = "37882691", IsDeleted = false }
            );


            #endregion

            #region BusStop
            modelBuilder.Entity<BusStop>().HasData(
                new { ID = 1, StopNumber = "5733", Label = "Benløseparken Øst (Benløseparken)", Longitude = 55.465448, Latitude = 11.795572, Zone = 0, IsDeleted = false },
                new { ID = 2, StopNumber = "3306", Label = "Benløseparken", Longitude = 55.463639, Latitude = 11.791770, Zone = 0, IsDeleted = false },
                new { ID = 3, StopNumber = "6175", Label = "Benløse Centret (Fredensvej)", Longitude = 55.461946, Latitude = 11.789993, Zone = 0, IsDeleted = false },
                new { ID = 4, StopNumber = "4618", Label = "Magleagervej (Fredensvej)", Longitude = 55.460131, Latitude = 11.792798, Zone = 0, IsDeleted = false },
                new { ID = 5, StopNumber = "8821", Label = "Benløse Kro (Roskildevej)", Longitude = 55.458179, Latitude = 11.794427, Zone = 0, IsDeleted = false },
                new { ID = 6, StopNumber = "5469", Label = "Ringstedet(Nordre Ringvej)", Longitude = 55.452938, Latitude = 11.789791, Zone = 0, IsDeleted = false },
                new { ID = 7, StopNumber = "4444", Label = "Ringsted Sygehus (Nørregade)", Longitude = 55.449607, Latitude = 11.789141, Zone = 0, IsDeleted = false },
                new { ID = 8, StopNumber = "4627", Label = "Ringsted Centret (Nørregade)", Longitude = 55.446611, Latitude = 11.788986, Zone = 0, IsDeleted = false },
                new { ID = 9, StopNumber = "6402", Label = "Torvet (Nørregade)", Longitude = 55.445297, Latitude = 11.789265, Zone = 0, IsDeleted = false },
                new { ID = 10, StopNumber = "5766", Label = "Hovmarksvej (Sjællandsgade)", Longitude = 55.441874, Latitude = 11.789595, Zone = 0, IsDeleted = false },
                new { ID = 11, StopNumber = "4137", Label = "Ringsted St.", Longitude = 55.438549, Latitude = 11.785506, Zone = 0, IsDeleted = false },
                new { ID = 12, StopNumber = "6191", Label = "Odinsvej (Næstvedvej)", Longitude = 55.436075, Latitude = 11.791536, Zone = 0, IsDeleted = false },
                new { ID = 13, StopNumber = "5985", Label = "Sdr. Parkvej (Søndervang)", Longitude = 55.435382, Latitude = 11.789818, Zone = 0, IsDeleted = false },
                new { ID = 14, StopNumber = "4977", Label = "Birkevej (Søndre Parkvej)", Longitude = 55.433339, Latitude = 11.788267, Zone = 0, IsDeleted = false },
                new { ID = 15, StopNumber = "4839", Label = "Bøgevej (Søndre Parkvej)", Longitude = 55.431578, Latitude = 11.784289, Zone = 0, IsDeleted = false },
                new { ID = 16, StopNumber = "4681", Label = "Campusskolen (Ahorn Allé)", Longitude = 55.429358, Latitude = 11.783491, Zone = 0, IsDeleted = false },
                new { ID = 17, StopNumber = "6601", Label = "ZBC Ringsted (Ahorn Allé)", Longitude = 55.427204, Latitude = 11.782598, Zone = 0, IsDeleted = false },
                new { ID = 18, StopNumber = "6570", Label = "Abelsvej (Bengerds Allé)", Longitude = 55.424128, Latitude = 11.782090, Zone = 0, IsDeleted = false },
                new { ID = 19, StopNumber = "7260", Label = "Campus (Bengerds Allé)", Longitude = 55.423717, Latitude = 11.783718, Zone = 0, IsDeleted = false },
                new { ID = 20, StopNumber = "4563", Label = "Abelsvej (Bengerds Allé)", Longitude = 55.424131, Latitude = 11.782117, Zone = 0, IsDeleted = false },
                new { ID = 21, StopNumber = "9790", Label = "ZBC Ringsted (Ahorn Allé) ", Longitude = 55.427320, Latitude = 11.782671, Zone = 0, IsDeleted = false },
                new { ID = 22, StopNumber = "8093", Label = "Campusskolen (Ahorn Allé) ", Longitude = 55.429372, Latitude = 11.783506, Zone = 0, IsDeleted = false },
                new { ID = 23, StopNumber = "9447", Label = "Bøgevej (Søndre Parkvej)", Longitude = 55.431689, Latitude = 11.784329, Zone = 0, IsDeleted = false },
                new { ID = 24, StopNumber = "6543", Label = "Birkevej (Søndre Parkvej)", Longitude = 55.433591, Latitude = 11.788773, Zone = 0, IsDeleted = false },
                new { ID = 25, StopNumber = "9433", Label = "Sdr. Parkvej (Søndervang)", Longitude = 55.435380, Latitude = 11.790102, Zone = 0, IsDeleted = false },
                new { ID = 26, StopNumber = "1864", Label = "Ringsted St.", Longitude = 55.438633, Latitude = 11.785740, Zone = 0, IsDeleted = false },
                new { ID = 27, StopNumber = "3837", Label = "Hovmarksvej (Sjællandsgade)", Longitude = 55.441791, Latitude = 11.789510, Zone = 0, IsDeleted = false },
                new { ID = 28, StopNumber = "7250", Label = "Torvet", Longitude = 55.445373, Latitude = 11.789297, Zone = 0, IsDeleted = false },
                new { ID = 29, StopNumber = "5597", Label = "Ringsted Centret (Nørregade)", Longitude = 55.446778, Latitude = 11.788992, Zone = 0, IsDeleted = false },
                new { ID = 30, StopNumber = "4205", Label = "Ringsted Sygehus (Nørregade)", Longitude = 55.449684, Latitude = 11.789178, Zone = 0, IsDeleted = false },
                new { ID = 31, StopNumber = "2569", Label = "Ringstedet (Nordre Ringvej)", Longitude = 55.452941, Latitude = 11.790023, Zone = 0, IsDeleted = false },
                new { ID = 32, StopNumber = "5887", Label = "Benløse Kro (Roskildevej)", Longitude = 55.458645, Latitude = 11.795324, Zone = 0, IsDeleted = false },
                new { ID = 33, StopNumber = "6894", Label = "Eilekiersvej (Roskildevej)", Longitude = 55.459740, Latitude = 11.797739, Zone = 0, IsDeleted = false },
                new { ID = 34, StopNumber = "2663", Label = "Benløse Leragervej (Smålodsvej)", Longitude = 55.463380, Latitude = 11.803171, Zone = 0, IsDeleted = false },
                new { ID = 35, StopNumber = "8291", Label = "Byskovskolen, Asgård (Smålodsvej)", Longitude = 55.464839, Latitude = 11.800669, Zone = 0, IsDeleted = false },
                new { ID = 36, StopNumber = "8049", Label = "Benløse Byvej (Smålodsvej)", Longitude = 55.466451, Latitude = 11.798098, Zone = 0, IsDeleted = false },
                new { ID = 37, StopNumber = "3774", Label = "Benløseparken Øst (Benløseparken)", Longitude = 55.466488, Latitude = 11.797975, Zone = 0, IsDeleted = false },
                new { ID = 38, StopNumber = "4733", Label = "Allerød St.", Longitude = 55.871028, Latitude = 12.356230, Zone = 0, IsDeleted = false },
                new { ID = 39, StopNumber = "5232", Label = "Allerød Gymnasium (Rådhusvej)", Longitude = 55.866482, Latitude = 12.336021, Zone = 0, IsDeleted = false },
                new { ID = 40, StopNumber = "6864", Label = "Farum Midtpunkt (Frederiksborgvej)", Longitude = 55.819658, Latitude = 12.375426, Zone = 0, IsDeleted = false },
                new { ID = 41, StopNumber = "1852", Label = "Farum Bytorv (Frederiksborgvej)", Longitude = 55.813478, Latitude = 12.375888, Zone = 0, IsDeleted = false },
                new { ID = 42, StopNumber = "4981", Label = "Bavnestedet (Fiskebækvej)", Longitude = 55.790147, Latitude = 12.374016, Zone = 0, IsDeleted = false },
                new { ID = 43, StopNumber = "9230", Label = "Værløse St., under broen (Fiskebækvej)", Longitude = 55.782883, Latitude = 12.369901, Zone = 0, IsDeleted = false },
                new { ID = 44, StopNumber = "1979", Label = "Egebjerg Bygade (Egebjergvej)", Longitude = 55.751409, Latitude = 12.378091, Zone = 0, IsDeleted = false },
                new { ID = 45, StopNumber = "6826", Label = "Agernhaven (Nordbuen)", Longitude = 55.748340, Latitude = 12.383271, Zone = 0, IsDeleted = false },
                new { ID = 46, StopNumber = "5971", Label = "Ring 4 (Klausdalsbrovej)", Longitude = 55.742930, Latitude = 12.389024, Zone = 0, IsDeleted = false },
                new { ID = 47, StopNumber = "2925", Label = "Lautrupbjerg (Lautrupparken)", Longitude = 55.738190, Latitude = 12.391810, Zone = 0, IsDeleted = false },
                new { ID = 48, StopNumber = "3997", Label = "Lautruphøj (Lautrupparken)", Longitude = 55.734479, Latitude = 12.390323, Zone = 0, IsDeleted = false },
                new { ID = 49, StopNumber = "2350", Label = "Ballerup, Borupvang (Lautrupvang)", Longitude = 55.731494, Latitude = 12.388103, Zone = 0, IsDeleted = false },
                new { ID = 50, StopNumber = "4409", Label = "Malmparken St. (Malmparken)", Longitude = 55.724221, Latitude = 12.385843, Zone = 0, IsDeleted = false },
                new { ID = 51, StopNumber = "2810", Label = "Ballerup, Borupvang (Lautrupvang)", Longitude = 55.731686, Latitude = 12.388489, Zone = 0, IsDeleted = false },
                new { ID = 52, StopNumber = "4149", Label = "Lautruphøj (Lautrupparken)", Longitude = 55.734615, Latitude = 12.390688, Zone = 0, IsDeleted = false },
                new { ID = 53, StopNumber = "5302", Label = "Lautrupbjerg (Lautrupparken)", Longitude = 55.738116, Latitude = 12.391600, Zone = 0, IsDeleted = false },
                new { ID = 54, StopNumber = "4603", Label = "Ring 4 (Nordbuen)", Longitude = 55.743896, Latitude = 12.387179, Zone = 0, IsDeleted = false },
                new { ID = 55, StopNumber = "9106", Label = "Agernhaven (Nordbuen)", Longitude = 55.748661, Latitude = 12.383083, Zone = 0, IsDeleted = false },
                new { ID = 56, StopNumber = "7325", Label = "Egebjerg Bygade (Egebjergvej)", Longitude = 55.751412, Latitude = 12.378088, Zone = 0, IsDeleted = false },
                new { ID = 57, StopNumber = "7139", Label = "Værløse St., under broen (Fiskebækvej)", Longitude = 55.782914, Latitude = 12.370144, Zone = 0, IsDeleted = false },
                new { ID = 58, StopNumber = "5360", Label = "Bavnestedet (Fiskebækvej)", Longitude = 55.789604, Latitude = 12.373662, Zone = 0, IsDeleted = false },
                new { ID = 59, StopNumber = "2117", Label = "Farum Bytorv (Frederiksborgvej)", Longitude = 55.813947, Latitude = 12.375762, Zone = 0, IsDeleted = false },
                new { ID = 60, StopNumber = "3716", Label = "Farum Midtpunkt (Frederiksborgvej)", Longitude = 55.819260, Latitude = 12.375668, Zone = 0, IsDeleted = false },
                new { ID = 61, StopNumber = "7362", Label = "Allerød Gymnasium (Rådhusvej)", Longitude = 55.867186, Latitude = 12.335892, Zone = 0, IsDeleted = false },
                new { ID = 62, StopNumber = "5402", Label = "Allerød St.", Longitude = 55.871133, Latitude = 12.357019, Zone = 0, IsDeleted = false },
                new { ID = 63, StopNumber = "5557", Label = "Søhuset (Bøge Allé)", Longitude = 55.874419, Latitude = 12.488821, Zone = 0, IsDeleted = false },
                new { ID = 64, StopNumber = "2078", Label = "Forskerparken (Agern Allé)", Longitude = 55.870375, Latitude = 12.493068, Zone = 0, IsDeleted = false },
                new { ID = 65, StopNumber = "5482", Label = "Dr. Neergaards Vej (Hørsholm Kongevej)", Longitude = 55.868623, Latitude = 12.496853, Zone = 0, IsDeleted = false },
                new { ID = 66, StopNumber = "3274", Label = "Ubberødvej (Hørsholm Kongevej)", Longitude = 55.860885, Latitude = 12.494366, Zone = 0, IsDeleted = false },
                new { ID = 67, StopNumber = "5712", Label = "Gl. Holte, Øverødvej (Helsingørmotorvejen)", Longitude = 55.832183, Latitude = 12.522701, Zone = 0, IsDeleted = false },
                new { ID = 68, StopNumber = "2189", Label = "Nærum St. (Helsingørmotorvejen)", Longitude = 55.814003, Latitude = 12.528288, Zone = 0, IsDeleted = false },
                new { ID = 69, StopNumber = "8282", Label = "Rævehøjvej, DTU(Helsingørmotorvejen)", Longitude = 55.787021, Latitude = 12.528307, Zone = 0, IsDeleted = false },
                new { ID = 70, StopNumber = "2178", Label = "Brogårdsvej (Lyngbyvej)", Longitude = 55.752218, Latitude = 12.523124, Zone = 0, IsDeleted = false },
                new { ID = 71, StopNumber = "9110", Label = "Ryparken St. (Lyngbyvej)", Longitude = 55.715464, Latitude = 12.558867, Zone = 0, IsDeleted = false },
                new { ID = 72, StopNumber = "5885", Label = "Haraldsgade (Lyngbyvej)", Longitude = 55.709421, Latitude = 12.561466, Zone = 0, IsDeleted = false },
                new { ID = 73, StopNumber = "8948", Label = "Vibenshus Runddel St. (Nørre Allé)", Longitude = 55.705650, Latitude = 12.562765, Zone = 0, IsDeleted = false },
                new { ID = 74, StopNumber = "8877", Label = "Universitetsparken (Nørre Allé)", Longitude = 55.699846, Latitude = 12.561970, Zone = 0, IsDeleted = false },
                new { ID = 75, StopNumber = "9784", Label = "Nørre Campus (Tagensvej)", Longitude = 55.697166, Latitude = 12.561650, Zone = 0, IsDeleted = false },
                new { ID = 76, StopNumber = "3469", Label = "Rigshospitalet Syd (Fredensgade)", Longitude = 55.694014, Latitude = 12.565609, Zone = 0, IsDeleted = false },
                new { ID = 77, StopNumber = "1700", Label = "Nørreport St. (Nørre Voldgade)", Longitude = 55.684202, Latitude = 12.572522, Zone = 0, IsDeleted = false },
                new { ID = 78, StopNumber = "8720", Label = "Nørreport St. (Nørre Voldgade)", Longitude = 55.684462, Latitude = 12.573343, Zone = 0, IsDeleted = false },
                new { ID = 79, StopNumber = "3125", Label = "Rigshospitalet Syd (Tagensvej)", Longitude = 55.693283, Latitude = 12.567138, Zone = 0, IsDeleted = false },
                new { ID = 80, StopNumber = "6311", Label = "Nørre Campus (Nørre Allé)", Longitude = 55.696045, Latitude = 12.562494, Zone = 0, IsDeleted = false },
                new { ID = 81, StopNumber = "2970", Label = "Universitetsparken (Nørre Allé)", Longitude = 55.700273, Latitude = 12.562090, Zone = 0, IsDeleted = false },
                new { ID = 82, StopNumber = "4190", Label = "Vibenshus Runddel St. (Lyngbyvej)", Longitude = 55.706089, Latitude = 12.562897, Zone = 0, IsDeleted = false },
                new { ID = 83, StopNumber = "8271", Label = "Haraldsgade (Lyngbyvej)", Longitude = 55.709851, Latitude = 12.561144, Zone = 0, IsDeleted = false },
                new { ID = 84, StopNumber = "8987", Label = "Ryparken St. (Lyngbyvej)", Longitude = 55.714987, Latitude = 12.558148, Zone = 0, IsDeleted = false },
                new { ID = 85, StopNumber = "3499", Label = "Brogårdsvej (Lyngbyvej)", Longitude = 55.752909, Latitude = 12.523312, Zone = 0, IsDeleted = false },
                new { ID = 86, StopNumber = "3190", Label = "Rævehøjvej, DTU (Helsingørmotorvejen)", Longitude = 55.788415, Latitude = 12.529573, Zone = 0, IsDeleted = false },
                new { ID = 87, StopNumber = "5342", Label = "Nærum St. (Helsingørmotorvejen)", Longitude = 55.814499, Latitude = 12.529267, Zone = 0, IsDeleted = false },
                new { ID = 88, StopNumber = "4343", Label = "Gl. Holte, Øverødvej (Helsingørmotorvejen)", Longitude = 55.833569, Latitude = 12.522787, Zone = 0, IsDeleted = false },
                new { ID = 89, StopNumber = "4204", Label = "Ubberødvej (Hørsholm Kongevej)", Longitude = 55.861300, Latitude = 12.494656, Zone = 0, IsDeleted = false },
                new { ID = 90, StopNumber = "2567", Label = "Hørsholm Kongevej (Dr Neergaards Vej)", Longitude = 55.868288, Latitude = 12.498816, Zone = 0, IsDeleted = false },
                new { ID = 91, StopNumber = "2465", Label = "Forskerparken (Agern Allé)", Longitude = 55.870477, Latitude = 12.493052, Zone = 0, IsDeleted = false },
                new { ID = 92, StopNumber = "8069", Label = "Søhuset (Bøge Allé)", Longitude = 55.873585, Latitude = 12.489033, Zone = 0, IsDeleted = false }
            );


            #endregion

            #region Route
            modelBuilder.Entity<Route>().HasData(
                new { ID = 1, Label = "401A", Description = "Mod Campus (Bengerds Allé)", IsDeleted = false },
                new { ID = 2, Label = "401A", Description = "Mod Benløseparken Øst (Benløseparken)", IsDeleted = false },
                new { ID = 3, Label = "55E", Description = "Mod Malmparken St.", IsDeleted = false },
                new { ID = 4, Label = "55E", Description = "Mod Allerød St.", IsDeleted = false },
                new { ID = 5, Label = "15E", Description = "Mod Nørreport St.", IsDeleted = false },
                new { ID = 6, Label = "15E", Description = "Mod Søhuset, Forskerparken", IsDeleted = false }
            );
            #endregion

            #region RouteBusStop
            modelBuilder.Entity<RouteBusStop>().HasData(
                #region 15E Mod Nørreport ST.
                new { ID = 1, RouteID = 5, BusStopID = 63, Order = 1, IsDeleted = false },
                new { ID = 2, RouteID = 5, BusStopID = 64, Order = 2, IsDeleted = false },
                new { ID = 3, RouteID = 5, BusStopID = 65, Order = 3, IsDeleted = false },
                new { ID = 4, RouteID = 5, BusStopID = 66, Order = 4, IsDeleted = false },
                new { ID = 5, RouteID = 5, BusStopID = 67, Order = 5, IsDeleted = false },
                new { ID = 6, RouteID = 5, BusStopID = 68, Order = 6, IsDeleted = false },
                new { ID = 7, RouteID = 5, BusStopID = 69, Order = 7, IsDeleted = false },
                new { ID = 8, RouteID = 5, BusStopID = 70, Order = 8, IsDeleted = false },
                new { ID = 9, RouteID = 5, BusStopID = 71, Order = 9, IsDeleted = false },
                new { ID = 10, RouteID = 5, BusStopID = 72, Order = 10, IsDeleted = false },
                new { ID = 11, RouteID = 5, BusStopID = 73, Order = 11, IsDeleted = false },
                new { ID = 12, RouteID = 5, BusStopID = 74, Order = 12, IsDeleted = false },
                new { ID = 13, RouteID = 5, BusStopID = 75, Order = 13, IsDeleted = false },
                new { ID = 14, RouteID = 5, BusStopID = 76, Order = 14, IsDeleted = false },
                new { ID = 15, RouteID = 5, BusStopID = 77, Order = 15, IsDeleted = false },
            #endregion
                #region 15E Mod Søhuset, Forskerparken
                new { ID = 15, RouteID = 6, BusStopID = 78, Order = 1, IsDeleted = false },
                new { ID = 16, RouteID = 6, BusStopID = 79, Order = 2, IsDeleted = false },
                new { ID = 17, RouteID = 6, BusStopID = 80, Order = 3, IsDeleted = false },
                new { ID = 18, RouteID = 6, BusStopID = 81, Order = 4, IsDeleted = false },
                new { ID = 19, RouteID = 6, BusStopID = 82, Order = 5, IsDeleted = false },
                new { ID = 20, RouteID = 6, BusStopID = 83, Order = 6, IsDeleted = false },
                new { ID = 21, RouteID = 6, BusStopID = 84, Order = 7, IsDeleted = false },
                new { ID = 22, RouteID = 6, BusStopID = 85, Order = 8, IsDeleted = false },
                new { ID = 23, RouteID = 6, BusStopID = 86, Order = 9, IsDeleted = false },
                new { ID = 24, RouteID = 6, BusStopID = 87, Order = 10, IsDeleted = false },
                new { ID = 25, RouteID = 6, BusStopID = 88, Order = 11, IsDeleted = false },
                new { ID = 26, RouteID = 6, BusStopID = 89, Order = 12, IsDeleted = false },
                new { ID = 27, RouteID = 6, BusStopID = 90, Order = 13, IsDeleted = false },
                new { ID = 28, RouteID = 6, BusStopID = 91, Order = 14, IsDeleted = false },
                new { ID = 29, RouteID = 6, BusStopID = 92, Order = 15, IsDeleted = false },
                #endregion

                #region 401A Mod Campus (Bengerds Allé)
                new { ID = 30, RouteID = 1, BusStopID = 1, Order = 1, IsDeleted = false },
                new { ID = 31, RouteID = 1, BusStopID = 2, Order = 2, IsDeleted = false },
                new { ID = 32, RouteID = 1, BusStopID = 3, Order = 3, IsDeleted = false },
                new { ID = 33, RouteID = 1, BusStopID = 4, Order = 4, IsDeleted = false },
                new { ID = 34, RouteID = 1, BusStopID = 5, Order = 5, IsDeleted = false },
                new { ID = 35, RouteID = 1, BusStopID = 6, Order = 6, IsDeleted = false },
                new { ID = 36, RouteID = 1, BusStopID = 7, Order = 7, IsDeleted = false },
                new { ID = 37, RouteID = 1, BusStopID = 8, Order = 8, IsDeleted = false },
                new { ID = 38, RouteID = 1, BusStopID = 9, Order = 9, IsDeleted = false },
                new { ID = 39, RouteID = 1, BusStopID = 10, Order = 10, IsDeleted = false },
                new { ID = 40, RouteID = 1, BusStopID = 11, Order = 11, IsDeleted = false },
                new { ID = 41, RouteID = 1, BusStopID = 12, Order = 12, IsDeleted = false },
                new { ID = 42, RouteID = 1, BusStopID = 13, Order = 13, IsDeleted = false },
                new { ID = 43, RouteID = 1, BusStopID = 14, Order = 14, IsDeleted = false },
                new { ID = 44, RouteID = 1, BusStopID = 15, Order = 15, IsDeleted = false },
                new { ID = 45, RouteID = 1, BusStopID = 16, Order = 16, IsDeleted = false },
                new { ID = 46, RouteID = 1, BusStopID = 17, Order = 17, IsDeleted = false },
                new { ID = 47, RouteID = 1, BusStopID = 18, Order = 18, IsDeleted = false },
                new { ID = 48, RouteID = 1, BusStopID = 19, Order = 19, IsDeleted = false },
                #endregion
                #region 401A Mod Benløseparken Øst (Benløseparken)
                new { ID = 49, RouteID = 2, BusStopID = 19, Order = 1, IsDeleted = false },
                new { ID = 50, RouteID = 2, BusStopID = 20, Order = 2, IsDeleted = false },
                new { ID = 51, RouteID = 2, BusStopID = 21, Order = 3, IsDeleted = false },
                new { ID = 52, RouteID = 2, BusStopID = 22, Order = 4, IsDeleted = false },
                new { ID = 53, RouteID = 2, BusStopID = 23, Order = 5, IsDeleted = false },
                new { ID = 54, RouteID = 2, BusStopID = 24, Order = 6, IsDeleted = false },
                new { ID = 55, RouteID = 2, BusStopID = 25, Order = 7, IsDeleted = false },
                new { ID = 56, RouteID = 2, BusStopID = 26, Order = 8, IsDeleted = false },
                new { ID = 57, RouteID = 2, BusStopID = 27, Order = 9, IsDeleted = false },
                new { ID = 58, RouteID = 2, BusStopID = 28, Order = 10, IsDeleted = false },
                new { ID = 59, RouteID = 2, BusStopID = 29, Order = 11, IsDeleted = false },
                new { ID = 60, RouteID = 2, BusStopID = 30, Order = 12, IsDeleted = false },
                new { ID = 61, RouteID = 2, BusStopID = 31, Order = 13, IsDeleted = false },
                new { ID = 62, RouteID = 2, BusStopID = 32, Order = 14, IsDeleted = false },
                new { ID = 63, RouteID = 2, BusStopID = 33, Order = 15, IsDeleted = false },
                new { ID = 64, RouteID = 2, BusStopID = 34, Order = 16, IsDeleted = false },
                new { ID = 65, RouteID = 2, BusStopID = 35, Order = 17, IsDeleted = false },
                new { ID = 66, RouteID = 2, BusStopID = 36, Order = 18, IsDeleted = false },
                new { ID = 67, RouteID = 2, BusStopID = 37, Order = 19, IsDeleted = false },
                #endregion

                #region 55E Mod Malmparken St.
                new { ID = 68, RouteID = 3, BusStopID = 38, Order = 1, IsDeleted = false },
                new { ID = 69, RouteID = 3, BusStopID = 39, Order = 2, IsDeleted = false },
                new { ID = 70, RouteID = 3, BusStopID = 40, Order = 3, IsDeleted = false },
                new { ID = 71, RouteID = 3, BusStopID = 41, Order = 4, IsDeleted = false },
                new { ID = 72, RouteID = 3, BusStopID = 42, Order = 5, IsDeleted = false },
                new { ID = 73, RouteID = 3, BusStopID = 43, Order = 6, IsDeleted = false },
                new { ID = 74, RouteID = 3, BusStopID = 44, Order = 7, IsDeleted = false },
                new { ID = 75, RouteID = 3, BusStopID = 45, Order = 8, IsDeleted = false },
                new { ID = 76, RouteID = 3, BusStopID = 46, Order = 9, IsDeleted = false },
                new { ID = 77, RouteID = 3, BusStopID = 47, Order = 10, IsDeleted = false },
                new { ID = 78, RouteID = 3, BusStopID = 48, Order = 11, IsDeleted = false },
                new { ID = 79, RouteID = 3, BusStopID = 49, Order = 12, IsDeleted = false },
                new { ID = 80, RouteID = 3, BusStopID = 50, Order = 13, IsDeleted = false },
                #endregion
                #region 55E Mod Allerød St.
                new { ID = 81, RouteID = 4, BusStopID = 50, Order = 1, IsDeleted = false },
                new { ID = 82, RouteID = 4, BusStopID = 51, Order = 2, IsDeleted = false },
                new { ID = 83, RouteID = 4, BusStopID = 52, Order = 3, IsDeleted = false },
                new { ID = 84, RouteID = 4, BusStopID = 53, Order = 4, IsDeleted = false },
                new { ID = 85, RouteID = 4, BusStopID = 54, Order = 5, IsDeleted = false },
                new { ID = 86, RouteID = 4, BusStopID = 55, Order = 6, IsDeleted = false },
                new { ID = 87, RouteID = 4, BusStopID = 56, Order = 7, IsDeleted = false },
                new { ID = 88, RouteID = 4, BusStopID = 57, Order = 8, IsDeleted = false },
                new { ID = 89, RouteID = 4, BusStopID = 58, Order = 9, IsDeleted = false },
                new { ID = 90, RouteID = 4, BusStopID = 59, Order = 10, IsDeleted = false },
                new { ID = 91, RouteID = 4, BusStopID = 60, Order = 11, IsDeleted = false },
                new { ID = 92, RouteID = 4, BusStopID = 61, Order = 12, IsDeleted = false },
                new { ID = 93, RouteID = 4, BusStopID = 62, Order = 13, IsDeleted = false }
                #endregion
            );
            #endregion

            #region RouteBus
            modelBuilder.Entity<RouteBus>().HasData(
                new { ID = 1, RouteID = 1, BusID = 1, BusDriverID = 12, Status = 1, Longitude = 55.465448, Latitude = 11.795572, IsDeleted = false },
                new { ID = 2, RouteID = 1, BusID = 2, BusDriverID = 11, Status = 1, Longitude = 55.429358, Latitude = 11.783491, IsDeleted = false },
                new { ID = 3, RouteID = 2, BusID = 3, BusDriverID = 10, Status = 1, Longitude = 55.423717, Latitude = 11.783718, IsDeleted = false },
                new { ID = 4, RouteID = 2, BusID = 4, BusDriverID = 9, Status = 1, Longitude = 55.464839, Latitude = 11.800669, IsDeleted = false },
                new { ID = 5, RouteID = 3, BusID = 5, BusDriverID = 8, Status = 1, Longitude = 55.871028, Latitude = 12.35623, IsDeleted = false },
                new { ID = 6, RouteID = 3, BusID = 6, BusDriverID = 7, Status = 1, Longitude = 55.74293, Latitude = 12.389024, IsDeleted = false },
                new { ID = 7, RouteID = 4, BusID = 7, BusDriverID = 6, Status = 1, Longitude = 55.724221, Latitude = 12.385843, IsDeleted = false },
                new { ID = 8, RouteID = 4, BusID = 8, BusDriverID = 5, Status = 1, Longitude = 55.748661, Latitude = 12.383083, IsDeleted = false },
                new { ID = 9, RouteID = 5, BusID = 9, BusDriverID = 4, Status = 1, Longitude = 55.874419, Latitude = 12.488821, IsDeleted = false },
                new { ID = 10, RouteID = 5, BusID = 10, BusDriverID = 3, Status = 1, Longitude = 55.752218, Latitude = 12.523124, IsDeleted = false },
                new { ID = 11, RouteID = 6, BusID = 11, BusDriverID = 2, Status = 1, Longitude = 55.684462, Latitude = 12.573343, IsDeleted = false },
                new { ID = 12, RouteID = 6, BusID = 12, BusDriverID = 1, Status = 1, Longitude = 55.714987, Latitude = 12.558148, IsDeleted = false }
            );
            #endregion

            #region NumberOfPassenger
            modelBuilder.Entity<NumberOfPassenger>().HasData(
                new { ID = 1, RouteBusID = 2, Total = 8, CreateDT = DateTime.Parse("2020-10-27 08:48"), Longitude = 55.464245, Latitude = 11.792656, IsDeleted = false },
                new { ID = 2, RouteBusID = 2, Total = 12, CreateDT = DateTime.Parse("2020-10-27 08:58"), Longitude = 55.463223, Latitude = 11.790853, IsDeleted = false },
                new { ID = 3, RouteBusID = 2, Total = 19, CreateDT = DateTime.Parse("2020-10-27 09:03"), Longitude = 55.461715, Latitude = 11.790682, IsDeleted = false },
                new { ID = 4, RouteBusID = 2, Total = 27, CreateDT = DateTime.Parse("2020-10-27 09:11"), Longitude = 55.459573, Latitude = 11.793171, IsDeleted = false },
                new { ID = 5, RouteBusID = 2, Total = 37, CreateDT = DateTime.Parse("2020-10-27 09:19"), Longitude = 55.457773, Latitude = 11.793428, IsDeleted = false },
                new { ID = 6, RouteBusID = 2, Total = 44, CreateDT = DateTime.Parse("2020-10-27 09:27"), Longitude = 55.450326, Latitude = 11.786647, IsDeleted = false },
                new { ID = 7, RouteBusID = 2, Total = 56, CreateDT = DateTime.Parse("2020-10-27 09:34"), Longitude = 55.447990, Latitude = 11.780639, IsDeleted = false },
                new { ID = 8, RouteBusID = 2, Total = 61, CreateDT = DateTime.Parse("2020-10-27 09:42"), Longitude = 55.442440, Latitude = 11.780639, IsDeleted = false },
                new { ID = 9, RouteBusID = 2, Total = 67, CreateDT = DateTime.Parse("2020-10-27 09:51"), Longitude = 55.438350, Latitude = 11.787162, IsDeleted = false },
                new { ID = 10, RouteBusID = 2, Total = 58, CreateDT = DateTime.Parse("2020-10-27 09:59"), Longitude = 55.434454, Latitude = 11.790596, IsDeleted = false }
            );                   
            #endregion

            #endregion


            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = 10.0.6.13; Database=test; User ID = sa; Password=Passw0rd");
        //}

    }
}
