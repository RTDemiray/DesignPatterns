﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Models;

namespace BaseProject.Repositories.Decorator
{
    public class BaseProductRepositoryDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;

        public BaseProductRepositoryDecorator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public virtual async Task<Product> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public virtual async Task<List<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public virtual async Task<List<Product>> GetAll(string userId)
        {
            return await _productRepository.GetAll(userId);
        }

        public virtual async Task<Product> Save(Product product)
        {
            return await _productRepository.Save(product);
        }

        public virtual async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }

        public virtual async Task Remove(Product product)
        {
            await _productRepository.Remove(product);
        }
    }
}