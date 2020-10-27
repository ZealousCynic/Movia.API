using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiNinjectStudio.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUrls",
                columns: table => new
                {
                    ApiUrlID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiUrlString = table.Column<string>(nullable: true),
                    ApiRequestMethod = table.Column<string>(nullable: true),
                    ApiUrlRegex = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUrls", x => x.ApiUrlID);
                });

            migrationBuilder.CreateTable(
                name: "BusDrivers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusDrivers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BusModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Length = table.Column<string>(nullable: true),
                    Width = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    PowerTrain = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BusStops",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StopNumber = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Zone = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusStops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Busses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    CapacityBoundary = table.Column<int>(nullable: false),
                    SeatingPlace = table.Column<int>(nullable: false),
                    StandingPlace = table.Column<int>(nullable: false),
                    BusModelID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Busses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Busses_BusModels_BusModelID",
                        column: x => x.BusModelID,
                        principalTable: "BusModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductID, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ProductImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ProductImageId);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductTagID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.ProductTagID);
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionApiUrls",
                columns: table => new
                {
                    ApiUrlID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionApiUrls", x => new { x.RoleID, x.ApiUrlID });
                    table.ForeignKey(
                        name: "FK_RolePermissionApiUrls_ApiUrls_ApiUrlID",
                        column: x => x.ApiUrlID,
                        principalTable: "ApiUrls",
                        principalColumn: "ApiUrlID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionApiUrls_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteBusStops",
                columns: table => new
                {
                    BusStopID = table.Column<int>(nullable: false),
                    RouteID = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteBusStops", x => new { x.RouteID, x.BusStopID });
                    table.ForeignKey(
                        name: "FK_RouteBusStops_BusStops_BusStopID",
                        column: x => x.BusStopID,
                        principalTable: "BusStops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteBusStops_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteBusses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteID = table.Column<int>(nullable: false),
                    BusID = table.Column<int>(nullable: false),
                    BusDriverID = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteBusses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RouteBusses_BusDrivers_BusDriverID",
                        column: x => x.BusDriverID,
                        principalTable: "BusDrivers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteBusses_Busses_BusID",
                        column: x => x.BusID,
                        principalTable: "Busses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteBusses_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NumberOfPassengers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<int>(nullable: false),
                    CreateDT = table.Column<DateTime>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    RouteBusID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfPassengers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NumberOfPassengers_RouteBusses_RouteBusID",
                        column: x => x.RouteBusID,
                        principalTable: "RouteBusses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApiUrls",
                columns: new[] { "ApiUrlID", "ApiRequestMethod", "ApiUrlRegex", "ApiUrlString", "Description", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Get", null, "/api/User", null, false },
                    { 19, "Get", "^\\/api\\/v1\\/integrations\\/customer\\/userid$", "/api/v1/integrations/customer/userid", "Get id of current user.", false },
                    { 18, "Delete", "^\\/api\\/v1\\/products\\/\\d+$", "/api/v1/products/{productId}", "Remove the product by product id", false },
                    { 17, "Put", "^\\/api\\/v1\\/products\\/\\d+$", "/api/v1/products/{productId}", "Update the product by product id", false },
                    { 16, "Get", "^\\/api\\/v1\\/products\\/\\d+$", "/api/v1/products/{productId}", "Get info about product by product id", false },
                    { 15, "Post", "^\\/api\\/v1\\/products$", "/api/v1/products", "Create product", false },
                    { 14, "Get", "^\\/api\\/v1\\/products(\\?.*)*$", "/api/v1/products", "Get product list", false },
                    { 13, "Delete", "^\\/api\\/v1\\/categories\\/\\d+\\/products\\/\\d+$", "/api/v1/categories/{categoryId}/products/{productId}", "Remove the product assignment from the category by category id and product id", false },
                    { 11, "Get", "^\\/api\\/v1\\/categories\\/\\d+\\/products(\\?.*)*$", "/api/v1/categories/{categoryId}/products", "Get products assigned to category", false },
                    { 12, "Put", "^\\/api\\/v1\\/categories\\/\\d+\\/products$", "/api/v1/categories/{categoryId}/products", "Assign a product to the required category", false },
                    { 9, "Put", "^\\/api\\/v1\\/categories\\/\\d+$", "/api/v1/categories/{categoryId}", "Update category by identifier", false },
                    { 8, "Delete", "^\\/api\\/v1\\/categories\\/\\d+$", "/api/v1/categories/{categoryId}", "Delete category by identifier", false },
                    { 7, "Get", "^\\/api\\/v1\\/categories\\/\\d+$", "/api/v1/categories/{categoryId}", "Get info about category by category id", false },
                    { 6, "Get", "^\\/api\\/v1\\/categories$", "/api/v1/categories", "Get category list", false },
                    { 5, "Post", "^\\/api\\/v1\\/categories$", "/api/v1/categories", "Create category", false },
                    { 4, "Post", null, "/api/product", null, false },
                    { 3, "Get", null, "/api/product", null, false },
                    { 2, "Get", null, "/api/User/GetUserID", null, false },
                    { 10, "Post", "^\\/api\\/v1\\/categories\\/\\d+\\/products$", "/api/v1/categories/{categoryId}/products", "Assign a product to the required category", false }
                });

            migrationBuilder.InsertData(
                table: "BusDrivers",
                columns: new[] { "ID", "FirstName", "IsDeleted", "LastName", "PersonnelNumber", "PhoneNumber" },
                values: new object[,]
                {
                    { 8, "Christian", false, "Sørensen", "D-0108", "59771920" },
                    { 12, "Anders", false, "Madsen", "D-0112", "37882691" },
                    { 10, "Martin", false, "Jørgensen", "D-0110", "93270098" },
                    { 9, "Jan", false, "Rasmussen", "D-0109", "36159850" },
                    { 7, "Søren", false, "Larsen", "D-0107", "83920099" },
                    { 11, "Niels", false, "Petersen", "D-0111", "91025134" },
                    { 5, "Thomas", false, "Andersen", "D-0104", "98205272" },
                    { 4, "Lars", false, "Pedersen", "D-0103", "85570209" },
                    { 3, "Michael", false, "Hansen", "D-0102", "24804732" },
                    { 2, "Jens", false, "Jensen", "D-0101", "32533534" },
                    { 6, "Henrik", false, "Christensen", "D-0105", "87740418" },
                    { 1, "Peter", false, "Nielsen", "D-0100", "41539333" }
                });

            migrationBuilder.InsertData(
                table: "BusModels",
                columns: new[] { "ID", "Height", "IsDeleted", "Length", "Manufacturer", "Model", "PowerTrain", "Width" },
                values: new object[,]
                {
                    { 6, "3.4 m", false, "12.135 m", "Mercedes-Benz", "eCitaro", "ZF AVE 130 elmotorer", "2.55 m" },
                    { 4, "3.12 m", false, "12.135 m", "Mercedes-Benz", "Citaro", "Mercedes-Benz OM 936 h", "2.55 m" },
                    { 5, "3.12 m", false, "12.135 m", "Mercedes-Benz", "Citaro NGT", "Mercedes-Benz M 936 G", "2.55 m" },
                    { 2, "3,28 m", false, "10,6/12,0 m (2 aksler)", "Volvo", "7900 HYBRID", "Volvo D5K 240, 4-cylindret, dieselrækkemotor, 240 hk.; Volvo I-SAM, 120 kW/800 Nm (2 aksler)", "2,55 m" },
                    { 1, "3,30 m", false, "13,7/14,7 m (lav indstigning 3 aksler)", "Volvo", "8900", "Volvo D8K, 6-cylindret dieselrækkemotor. 280, 320 eller 350 hk.", "2,55 m" },
                    { 3, "3.30 m", false, "12 m", "Volvo", "7900 ELECTRIC", "High capacity Lithium-Ion battery", "2.55 m" }
                });

            migrationBuilder.InsertData(
                table: "BusStops",
                columns: new[] { "ID", "IsDeleted", "Label", "Latitude", "Longitude", "StopNumber", "Zone" },
                values: new object[,]
                {
                    { 60, false, "Farum Midtpunkt (Frederiksborgvej)", 12.375667999999999, 55.81926, "3716", 0 },
                    { 67, false, "Gl. Holte, Øverødvej (Helsingørmotorvejen)", 12.522701, 55.832183000000001, "5712", 0 },
                    { 66, false, "Ubberødvej (Hørsholm Kongevej)", 12.494365999999999, 55.860885000000003, "3274", 0 },
                    { 65, false, "Dr. Neergaards Vej (Hørsholm Kongevej)", 12.496853, 55.868622999999999, "5482", 0 },
                    { 64, false, "Forskerparken (Agern Allé)", 12.493067999999999, 55.870375000000003, "2078", 0 },
                    { 68, false, "Nærum St. (Helsingørmotorvejen)", 12.528288, 55.814003, "2189", 0 },
                    { 63, false, "Søhuset (Bøge Allé)", 12.488821, 55.874419000000003, "5557", 0 },
                    { 62, false, "Allerød St.", 12.357018999999999, 55.871133, "5402", 0 },
                    { 61, false, "Allerød Gymnasium (Rådhusvej)", 12.335891999999999, 55.867185999999997, "7362", 0 },
                    { 59, false, "Farum Bytorv (Frederiksborgvej)", 12.375762, 55.813946999999999, "2117", 0 },
                    { 53, false, "Lautrupbjerg (Lautrupparken)", 12.3916, 55.738115999999998, "5302", 0 },
                    { 57, false, "Værløse St., under broen (Fiskebækvej)", 12.370144, 55.782913999999998, "7139", 0 },
                    { 56, false, "Egebjerg Bygade (Egebjergvej)", 12.378088, 55.751412000000002, "7325", 0 },
                    { 55, false, "Agernhaven (Nordbuen)", 12.383082999999999, 55.748660999999998, "9106", 0 },
                    { 54, false, "Ring 4 (Nordbuen)", 12.387179, 55.743895999999999, "4603", 0 },
                    { 52, false, "Lautruphøj (Lautrupparken)", 12.390688000000001, 55.734614999999998, "4149", 0 },
                    { 51, false, "Ballerup, Borupvang (Lautrupvang)", 12.388489, 55.731686000000003, "2810", 0 },
                    { 50, false, "Malmparken St. (Malmparken)", 12.385842999999999, 55.724221, "4409", 0 },
                    { 49, false, "Ballerup, Borupvang (Lautrupvang)", 12.388102999999999, 55.731493999999998, "2350", 0 },
                    { 69, false, "Rævehøjvej, DTU(Helsingørmotorvejen)", 12.528307, 55.787021000000003, "8282", 0 },
                    { 58, false, "Bavnestedet (Fiskebækvej)", 12.373661999999999, 55.789603999999997, "5360", 0 },
                    { 70, false, "Brogårdsvej (Lyngbyvej)", 12.523123999999999, 55.752217999999999, "2178", 0 },
                    { 88, false, "Gl. Holte, Øverødvej (Helsingørmotorvejen)", 12.522786999999999, 55.833568999999997, "4343", 0 },
                    { 72, false, "Haraldsgade (Lyngbyvej)", 12.561465999999999, 55.709420999999999, "5885", 0 },
                    { 92, false, "Søhuset (Bøge Allé)", 12.489032999999999, 55.873584999999999, "8069", 0 },
                    { 91, false, "Forskerparken (Agern Allé)", 12.493052, 55.870477000000001, "2465", 0 },
                    { 90, false, "Hørsholm Kongevej (Dr Neergaards Vej)", 12.498816, 55.868288, "2567", 0 },
                    { 89, false, "Ubberødvej (Hørsholm Kongevej)", 12.494656000000001, 55.8613, "4204", 0 },
                    { 48, false, "Lautruphøj (Lautrupparken)", 12.390323, 55.734479, "3997", 0 },
                    { 87, false, "Nærum St. (Helsingørmotorvejen)", 12.529267000000001, 55.814498999999998, "5342", 0 },
                    { 86, false, "Rævehøjvej, DTU (Helsingørmotorvejen)", 12.529572999999999, 55.788415000000001, "3190", 0 },
                    { 85, false, "Brogårdsvej (Lyngbyvej)", 12.523312000000001, 55.752909000000002, "3499", 0 },
                    { 84, false, "Ryparken St. (Lyngbyvej)", 12.558147999999999, 55.714987000000001, "8987", 0 },
                    { 71, false, "Ryparken St. (Lyngbyvej)", 12.558866999999999, 55.715463999999997, "9110", 0 },
                    { 83, false, "Haraldsgade (Lyngbyvej)", 12.561144000000001, 55.709851, "8271", 0 },
                    { 81, false, "Universitetsparken (Nørre Allé)", 12.56209, 55.700273000000003, "2970", 0 },
                    { 80, false, "Nørre Campus (Nørre Allé)", 12.562493999999999, 55.696044999999998, "6311", 0 },
                    { 79, false, "Rigshospitalet Syd (Tagensvej)", 12.567138, 55.693283000000001, "3125", 0 },
                    { 78, false, "Nørreport St. (Nørre Voldgade)", 12.573342999999999, 55.684462000000003, "8720", 0 },
                    { 77, false, "Nørreport St. (Nørre Voldgade)", 12.572521999999999, 55.684201999999999, "1700", 0 },
                    { 76, false, "Rigshospitalet Syd (Fredensgade)", 12.565609, 55.694014000000003, "3469", 0 },
                    { 75, false, "Nørre Campus (Tagensvej)", 12.56165, 55.697166000000003, "9784", 0 },
                    { 74, false, "Universitetsparken (Nørre Allé)", 12.561970000000001, 55.699846000000001, "8877", 0 },
                    { 73, false, "Vibenshus Runddel St. (Nørre Allé)", 12.562765000000001, 55.705649999999999, "8948", 0 },
                    { 82, false, "Vibenshus Runddel St. (Lyngbyvej)", 12.562897, 55.706088999999999, "4190", 0 },
                    { 47, false, "Lautrupbjerg (Lautrupparken)", 12.39181, 55.738190000000003, "2925", 0 },
                    { 37, false, "Benløseparken Øst (Benløseparken)", 11.797974999999999, 55.466487999999998, "3774", 0 },
                    { 45, false, "Agernhaven (Nordbuen)", 12.383271000000001, 55.748339999999999, "6826", 0 },
                    { 20, false, "Abelsvej (Bengerds Allé)", 11.782117, 55.424131000000003, "4563", 0 },
                    { 19, false, "Campus (Bengerds Allé)", 11.783718, 55.423717000000003, "7260", 0 },
                    { 18, false, "Abelsvej (Bengerds Allé)", 11.78209, 55.424128000000003, "6570", 0 },
                    { 17, false, "ZBC Ringsted (Ahorn Allé)", 11.782598, 55.427204000000003, "6601", 0 },
                    { 16, false, "Campusskolen (Ahorn Allé)", 11.783491, 55.429358000000001, "4681", 0 },
                    { 15, false, "Bøgevej (Søndre Parkvej)", 11.784288999999999, 55.431578000000002, "4839", 0 },
                    { 46, false, "Ring 4 (Klausdalsbrovej)", 12.389023999999999, 55.742930000000001, "5971", 0 },
                    { 13, false, "Sdr. Parkvej (Søndervang)", 11.789818, 55.435381999999997, "5985", 0 },
                    { 12, false, "Odinsvej (Næstvedvej)", 11.791536000000001, 55.436075000000002, "6191", 0 },
                    { 21, false, "ZBC Ringsted (Ahorn Allé) ", 11.782671000000001, 55.427320000000002, "9790", 0 },
                    { 11, false, "Ringsted St.", 11.785506, 55.438549000000002, "4137", 0 },
                    { 9, false, "Torvet (Nørregade)", 11.789265, 55.445296999999997, "6402", 0 },
                    { 8, false, "Ringsted Centret (Nørregade)", 11.788986, 55.446610999999997, "4627", 0 },
                    { 7, false, "Ringsted Sygehus (Nørregade)", 11.789141000000001, 55.449607, "4444", 0 },
                    { 6, false, "Ringstedet(Nordre Ringvej)", 11.789790999999999, 55.452938000000003, "5469", 0 },
                    { 5, false, "Benløse Kro (Roskildevej)", 11.794427000000001, 55.458179000000001, "8821", 0 },
                    { 4, false, "Magleagervej (Fredensvej)", 11.792797999999999, 55.460130999999997, "4618", 0 },
                    { 3, false, "Benløse Centret (Fredensvej)", 11.789993000000001, 55.461945999999998, "6175", 0 },
                    { 2, false, "Benløseparken", 11.79177, 55.463639000000001, "3306", 0 },
                    { 1, false, "Benløseparken Øst (Benløseparken)", 11.795572, 55.465448000000002, "5733", 0 },
                    { 10, false, "Hovmarksvej (Sjællandsgade)", 11.789595, 55.441873999999999, "5766", 0 },
                    { 22, false, "Campusskolen (Ahorn Allé) ", 11.783505999999999, 55.429372000000001, "8093", 0 },
                    { 14, false, "Birkevej (Søndre Parkvej)", 11.788266999999999, 55.433338999999997, "4977", 0 },
                    { 24, false, "Birkevej (Søndre Parkvej)", 11.788773000000001, 55.433591, "6543", 0 },
                    { 44, false, "Egebjerg Bygade (Egebjergvej)", 12.378091, 55.751409000000002, "1979", 0 },
                    { 23, false, "Bøgevej (Søndre Parkvej)", 11.784329, 55.431688999999999, "9447", 0 },
                    { 42, false, "Bavnestedet (Fiskebækvej)", 12.374015999999999, 55.790146999999997, "4981", 0 },
                    { 41, false, "Farum Bytorv (Frederiksborgvej)", 12.375888, 55.813478000000003, "1852", 0 },
                    { 40, false, "Farum Midtpunkt (Frederiksborgvej)", 12.375425999999999, 55.819657999999997, "6864", 0 },
                    { 39, false, "Allerød Gymnasium (Rådhusvej)", 12.336021000000001, 55.866481999999998, "5232", 0 },
                    { 38, false, "Allerød St.", 12.35623, 55.871028000000003, "4733", 0 },
                    { 36, false, "Benløse Byvej (Smålodsvej)", 11.798098, 55.466450999999999, "8049", 0 },
                    { 35, false, "Byskovskolen, Asgård (Smålodsvej)", 11.800668999999999, 55.464838999999998, "8291", 0 },
                    { 43, false, "Værløse St., under broen (Fiskebækvej)", 12.369901, 55.782882999999998, "9230", 0 },
                    { 33, false, "Eilekiersvej (Roskildevej)", 11.797739, 55.459739999999996, "6894", 0 },
                    { 32, false, "Benløse Kro (Roskildevej)", 11.795324000000001, 55.458644999999997, "5887", 0 },
                    { 31, false, "Ringstedet (Nordre Ringvej)", 11.790023, 55.452941000000003, "2569", 0 },
                    { 30, false, "Ringsted Sygehus (Nørregade)", 11.789178, 55.449683999999998, "4205", 0 },
                    { 29, false, "Ringsted Centret (Nørregade)", 11.788992, 55.446778000000002, "5597", 0 },
                    { 28, false, "Torvet", 11.789296999999999, 55.445372999999996, "7250", 0 },
                    { 27, false, "Hovmarksvej (Sjællandsgade)", 11.78951, 55.441791000000002, "3837", 0 },
                    { 26, false, "Ringsted St.", 11.785740000000001, 55.438633000000003, "1864", 0 },
                    { 25, false, "Sdr. Parkvej (Søndervang)", 11.790101999999999, 55.435380000000002, "9433", 0 },
                    { 34, false, "Benløse Leragervej (Smålodsvej)", 11.803171000000001, 55.463380000000001, "2663", 0 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "IsDeleted" },
                values: new object[,]
                {
                    { 8, "Indretning", false },
                    { 7, "Have", false },
                    { 6, "Opbevaring", false },
                    { 5, "Spisestue", false },
                    { 4, "Stue", false },
                    { 2, "Badeværelse", false },
                    { 1, "Soveværelse", false },
                    { 3, "Kontor", false }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Description", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Bremsehjul, Højdejusterbar, Nylonbase, Trinløs vippefunktion", false, "Kontorstol REGSTRUP sort/grå", 300m },
                    { 2, "Højdejusterbar", false, "Barstol KLARUP sort/krom", 250m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, false, "Administrator" },
                    { 2, null, false, "Guest" }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "ID", "Description", "IsDeleted", "Label" },
                values: new object[,]
                {
                    { 5, "Mod Nørreport St.", false, "15E" },
                    { 1, "Mod Campus (Bengerds Allé)", false, "401A" },
                    { 2, "Mod Benløseparken Øst (Benløseparken)", false, "401A" },
                    { 3, "Mod Malmparken St.", false, "55E" },
                    { 4, "Mod Allerød St.", false, "55E" },
                    { 6, "Mod Søhuset, Forskerparken", false, "15E" }
                });

            migrationBuilder.InsertData(
                table: "Busses",
                columns: new[] { "ID", "BusModelID", "CapacityBoundary", "IsDeleted", "RegistrationNumber", "SeatingPlace", "StandingPlace" },
                values: new object[,]
                {
                    { 1, 1, 45, false, "BJ32621", 35, 40 },
                    { 12, 6, 35, false, "BV28091", 45, 30 },
                    { 10, 5, 30, false, "AB11139", 20, 60 },
                    { 9, 5, 50, false, "MD21233", 40, 50 },
                    { 8, 4, 30, false, "AF25463", 30, 45 },
                    { 7, 3, 45, false, "XT33042", 32, 40 },
                    { 11, 6, 60, false, "BP54379", 45, 40 },
                    { 5, 3, 45, false, "AF22454", 25, 60 },
                    { 4, 2, 45, false, "YU57648", 35, 45 },
                    { 3, 2, 45, false, "MD21233", 35, 45 },
                    { 2, 1, 40, false, "HG30202", 35, 35 },
                    { 6, 3, 45, false, "AF33912", 25, 60 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "ProductID", "CategoryId", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 3, false },
                    { 1, 5, false },
                    { 2, 5, false }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "ProductImageId", "IsDeleted", "ProductID", "Url" },
                values: new object[,]
                {
                    { 2, false, 2, "jysk.dk/spisestue/barborde-stole/barstol-klarup-sort-krom-0" },
                    { 1, false, 1, "jysk.dk/kontor/kontorstole/basic/kontorstol-regstrup-sort-graa" }
                });

            migrationBuilder.InsertData(
                table: "ProductTags",
                columns: new[] { "ProductTagID", "IsDeleted", "Name", "ProductID" },
                values: new object[,]
                {
                    { 9, false, "polypropylen", 2 },
                    { 8, false, "metal", 2 },
                    { 7, false, "skum", 2 },
                    { 6, false, "kunstlæder", 2 },
                    { 3, false, "skum", 1 },
                    { 4, false, "metal", 1 },
                    { 2, false, "sort", 1 },
                    { 1, false, "kontorstol", 1 },
                    { 5, false, "krydsfiner", 1 }
                });

            migrationBuilder.InsertData(
                table: "RolePermissionApiUrls",
                columns: new[] { "RoleID", "ApiUrlID", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 12, false },
                    { 2, 4, false },
                    { 1, 19, false },
                    { 1, 18, false },
                    { 1, 17, false },
                    { 1, 16, false },
                    { 1, 15, false },
                    { 1, 14, false },
                    { 1, 13, false },
                    { 1, 11, false },
                    { 2, 3, false },
                    { 1, 9, false },
                    { 1, 1, false },
                    { 1, 2, false },
                    { 1, 3, false },
                    { 1, 4, false },
                    { 1, 10, false },
                    { 1, 6, false },
                    { 1, 7, false },
                    { 1, 8, false },
                    { 1, 5, false }
                });

            migrationBuilder.InsertData(
                table: "RouteBusStops",
                columns: new[] { "RouteID", "BusStopID", "IsDeleted", "Order" },
                values: new object[,]
                {
                    { 5, 65, false, 3 },
                    { 4, 58, false, 9 },
                    { 5, 64, false, 2 },
                    { 5, 63, false, 1 },
                    { 4, 62, false, 13 },
                    { 4, 61, false, 12 },
                    { 4, 60, false, 11 },
                    { 4, 59, false, 10 },
                    { 4, 57, false, 8 },
                    { 4, 52, false, 3 },
                    { 4, 55, false, 6 },
                    { 4, 54, false, 5 },
                    { 4, 53, false, 4 },
                    { 5, 66, false, 4 },
                    { 4, 51, false, 2 },
                    { 4, 50, false, 1 },
                    { 3, 50, false, 13 },
                    { 3, 49, false, 12 },
                    { 3, 48, false, 11 },
                    { 3, 47, false, 10 },
                    { 4, 56, false, 7 },
                    { 5, 67, false, 5 },
                    { 5, 72, false, 10 },
                    { 5, 69, false, 7 },
                    { 6, 90, false, 13 },
                    { 3, 46, false, 9 },
                    { 6, 89, false, 12 },
                    { 6, 88, false, 11 },
                    { 6, 87, false, 10 },
                    { 6, 86, false, 9 },
                    { 6, 85, false, 8 },
                    { 6, 84, false, 7 },
                    { 6, 83, false, 6 },
                    { 6, 82, false, 5 },
                    { 6, 81, false, 4 },
                    { 6, 80, false, 3 },
                    { 6, 79, false, 2 },
                    { 6, 78, false, 1 },
                    { 5, 77, false, 15 },
                    { 5, 76, false, 14 },
                    { 5, 75, false, 13 },
                    { 5, 74, false, 12 },
                    { 5, 73, false, 11 },
                    { 5, 71, false, 9 },
                    { 5, 70, false, 8 },
                    { 5, 68, false, 6 },
                    { 3, 45, false, 8 },
                    { 2, 22, false, 4 },
                    { 3, 43, false, 6 },
                    { 1, 19, false, 19 },
                    { 1, 18, false, 18 },
                    { 1, 17, false, 17 },
                    { 1, 16, false, 16 },
                    { 1, 15, false, 15 },
                    { 1, 14, false, 14 },
                    { 1, 13, false, 13 },
                    { 1, 12, false, 12 },
                    { 1, 11, false, 11 },
                    { 1, 10, false, 10 },
                    { 1, 9, false, 9 },
                    { 1, 8, false, 8 },
                    { 1, 7, false, 7 },
                    { 1, 6, false, 6 },
                    { 1, 5, false, 5 },
                    { 1, 4, false, 4 },
                    { 1, 3, false, 3 },
                    { 1, 2, false, 2 },
                    { 1, 1, false, 1 },
                    { 2, 19, false, 1 },
                    { 3, 44, false, 7 },
                    { 2, 20, false, 2 },
                    { 6, 91, false, 14 },
                    { 3, 42, false, 5 },
                    { 3, 41, false, 4 },
                    { 3, 40, false, 3 },
                    { 3, 39, false, 2 },
                    { 3, 38, false, 1 },
                    { 2, 37, false, 19 },
                    { 2, 36, false, 18 },
                    { 2, 35, false, 17 },
                    { 2, 21, false, 3 },
                    { 2, 33, false, 15 },
                    { 2, 34, false, 16 },
                    { 2, 31, false, 13 },
                    { 2, 30, false, 12 },
                    { 2, 29, false, 11 },
                    { 2, 28, false, 10 },
                    { 2, 27, false, 9 },
                    { 2, 26, false, 8 },
                    { 2, 25, false, 7 },
                    { 2, 24, false, 6 },
                    { 2, 23, false, 5 },
                    { 2, 32, false, 14 },
                    { 6, 92, false, 15 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhoneNumber", "RoleID" },
                values: new object[,]
                {
                    { 2, "two@gmail.com", "Martin", false, "Jensen", "FOHqRDbYuVdIBvLS6r2YMVU4Yu7E54DJJJxrWGh5YZc=", null, 2 },
                    { 1, "one@gmail.com", "Kim", false, "Nielsen", "M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=", null, 1 }
                });

            migrationBuilder.InsertData(
                table: "RouteBusses",
                columns: new[] { "ID", "BusDriverID", "BusID", "IsDeleted", "Latitude", "Longitude", "RouteID", "Status" },
                values: new object[,]
                {
                    { 1, 12, 1, false, 11.795572, 55.465448000000002, 1, 1 },
                    { 2, 11, 2, false, 11.783491, 55.429358000000001, 1, 1 },
                    { 3, 10, 3, false, 11.783718, 55.423717000000003, 2, 1 },
                    { 4, 9, 4, false, 11.800668999999999, 55.464838999999998, 2, 1 },
                    { 5, 8, 5, false, 12.35623, 55.871028000000003, 3, 1 },
                    { 6, 7, 6, false, 12.389023999999999, 55.742930000000001, 3, 1 },
                    { 7, 6, 7, false, 12.385842999999999, 55.724221, 4, 1 },
                    { 8, 5, 8, false, 12.383082999999999, 55.748660999999998, 4, 1 },
                    { 9, 4, 9, false, 12.488821, 55.874419000000003, 5, 1 },
                    { 10, 3, 10, false, 12.523123999999999, 55.752217999999999, 5, 1 },
                    { 11, 2, 11, false, 12.573342999999999, 55.684462000000003, 6, 1 },
                    { 12, 1, 12, false, 12.558147999999999, 55.714987000000001, 6, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Busses_BusModelID",
                table: "Busses",
                column: "BusModelID");

            migrationBuilder.CreateIndex(
                name: "IX_NumberOfPassengers_RouteBusID",
                table: "NumberOfPassengers",
                column: "RouteBusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_ProductID",
                table: "ProductTags",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionApiUrls_ApiUrlID",
                table: "RolePermissionApiUrls",
                column: "ApiUrlID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteBusses_BusDriverID",
                table: "RouteBusses",
                column: "BusDriverID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteBusses_BusID",
                table: "RouteBusses",
                column: "BusID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteBusses_RouteID",
                table: "RouteBusses",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteBusStops_BusStopID",
                table: "RouteBusStops",
                column: "BusStopID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberOfPassengers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "RolePermissionApiUrls");

            migrationBuilder.DropTable(
                name: "RouteBusStops");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RouteBusses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ApiUrls");

            migrationBuilder.DropTable(
                name: "BusStops");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "BusDrivers");

            migrationBuilder.DropTable(
                name: "Busses");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "BusModels");
        }
    }
}
