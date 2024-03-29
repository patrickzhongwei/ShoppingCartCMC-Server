﻿using ShoppingCartCMC.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.Factory
{
    public class ProductFactory
    {
        public iProductDto? CreateDto(iProduct product)
        {
            if (product == null)
                return null;
            else
                return new ProductDto(
                     product.Productkey,
                     product.ProductId,
                     product.ProductName,
                     product.ProductCategory,
                     product.ProductPrice,
                     product.ProductDescription,
                     product.ProductImageUrl,
                     product.ProductAdded,
                     product.ProductQuatity,
                     product.Ratings,
                     product.Favourite,
                     product.ProductSeller,
                     product.Currency);
        }


        public iProductDto[] CreateDtoBatch(iProduct[] batch)
        {
            List<iProductDto> dtos = new List<iProductDto>();

            foreach (var product in batch)
            {
                var newDto = CreateDto(product);

                if(newDto != null)
                    dtos.Add(newDto);
            }

            return dtos.ToArray();
        }
    }
}
