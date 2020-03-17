using System;
using System.Collections.Generic;
using S3Train.Domain;

namespace S3Train.Contract
{
    public interface IProductService : IGenenicServiceBase<Product>
    {
        List<Product> ListAllByID(Guid productCategoryID);
        List<Product> ListAll();
        bool Create(Product product);
        bool Update(Product product);
        void UpdateImages(Guid id, string images);
        bool Delete(Guid id);
        List<Product> ListAllOrderByCreateDate();
        bool? ChangeStatus(Guid id);




        //----------------- Hiếu --------------------

        List<Product> ListTopHotALL();
        List<Product> ListTopHotTablet(Guid productCategoryID);
        List<Product> ListTopHotPhone(Guid productCategoryID);
        List<Product> ListTopHotWatch(Guid productCategoryID);
        List<Product> ListTopHotLapTop(Guid productCategoryID);

        List<Product> relatedProduct(Guid id);
    }
}
