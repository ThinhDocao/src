﻿using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Contract
{
    public interface IProductCategoryService : IGenenicServiceBase<ProductCategory>
    {
        List<ProductCategory> ListAll();
        ProductCategory ViewDetail(Guid id);
        bool? ChangeStatus(Guid id);
        List<ProductCategory> ListAllOrderByCreateDate();
        bool Create(ProductCategory productCategory);
        bool Update(ProductCategory productCategory);
        bool Delete(Guid id);
    }
}
