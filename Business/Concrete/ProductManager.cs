using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _ProductDal;
        ICategoryService _categoryService;



        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _ProductDal = productDal;
            _categoryService = categoryService;


        }

        [SecuredOperation("product.add,admin")]
       [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            IResult result=  BusinessRules.Run(CheckIfProductNameExists(product.ProductName), 
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

            if (result!=null)
            {
                return result;
            }
            _ProductDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
                       
          
           
            //business codes



        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_ProductDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_ProductDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {

            if (DateTime.Now.Hour >= 0)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_ProductDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            var result = _ProductDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }



            throw new NotImplementedException();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        { 
             var result = _ProductDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(String productname)
        {
            var result = _ProductDal.GetAll(p => p.ProductName == productname).Any();
            if (result==true )
            {
                return new ErrorResult(Messages.ProductNameAllreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CheckIfCategoryLimitExceded);
            }
            return new SuccessResult();
        }

    }

}
