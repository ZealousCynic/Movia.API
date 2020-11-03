using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiNinjectStudio.Domain.Entities;
using WebApiNinjectStudio.V1.Dtos;

namespace WebApiNinjectStudio
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<from, to>();
            #region Category Dto
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, ReturnCategoryDto>();
            #endregion

            #region Product Dto
            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductImageDto, ProductImage>();            
            CreateMap<CreateProductTagDto, ProductTag>();

            CreateMap<UpdateProductDto, Product>();
            CreateMap<UpdateProductImageDto, ProductImage>();
            CreateMap<UpdateProductTagDto, ProductTag>();

            CreateMap<Product, ReturnProductDto>();
            CreateMap<ProductImage, ReturnProductImageDto>();
            CreateMap<ProductTag, ReturnProductTagDto>();
            CreateMap<ProductCategory, ReturnProductCategoryDto>();
            #endregion

            #region Bus Dto
            CreateMap<Bus, ReturnBusDto>();
            CreateMap<CreateBusDto, Bus>();
            CreateMap<UpdateBusDto, Bus>();
            #endregion

            #region BusModel Dto
            CreateMap<BusModel, ReturnBusModelDto>();
            #endregion

            #region RouteBus Dto
            CreateMap<RouteBus, ReturnRouteBusDto>();
            CreateMap<BusWithDriverDto, RouteBus>();            
            CreateMap<RouteBus, ReturnBusAndDriverInRouteDto>();
            CreateMap<RouteBusStop, ReturnBusStopWithOrderDto>();
            #endregion

            #region BusDriver Dto
            CreateMap<BusDriver, ReturnBusDriverDto>();
            CreateMap<CreateBusDriverDto, BusDriver>();
            CreateMap<UpdateBusDriverDto, BusDriver>();
            #endregion

            #region BusStop Dto
            CreateMap<BusStop, ReturnBusStopDto>();
            CreateMap<CreateBusStopDto, BusStop>();
            CreateMap<UpdateBusStopDto, BusStop>();
            #endregion

            #region NumberOfPassenger Dto
            CreateMap<NumberOfPassenger, ReturnNumberOfPassengerDto>();
            CreateMap<CreateNumberOfPassengerDto, NumberOfPassenger>();
            #endregion

            #region Route Dto
            CreateMap<Route, ReturnRouteDto>();
            CreateMap<CreateRouteDto, Route>();
            CreateMap<UpdateRouteDto, Route>();
            #endregion
        }
    }
}
