using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entyties;

namespace SportsStore.Domain.Abstract
{
     public interface IProductsRepository
    {
         IQueryable<Product> Products { get; }
    }
}
