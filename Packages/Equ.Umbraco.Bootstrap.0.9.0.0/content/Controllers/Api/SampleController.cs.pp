using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.WebApi;

namespace $rootnamespace$.Controllers.Api
{
    //http://our.umbraco.org/documentation/Reference/WebApi/
    //http://www.nibble.be/?p=224

    
    public class SampleController : UmbracoApiController
    {
        /* sample method
        *  /Umbraco/Api/Products/GetProductNames?name=xyz
        */
        public IEnumerable<string> GetProductNames(string name)
        {
            return new[] { name, "Table", "Chair", "Desk", "Computer", "Beer fridge" };
        }

        /* //note nust use id as parameter name
        *  /Umbraco/Api/Products/GetProduct/7
        */
       public Product GetProduct(string id)
        {
            var product = new Product()
            {
                Name = "Product 1",
                Id = id,
                PictureSrc = "http://placehold.it/140x100",
                Url = "http://equ.com.au/"
            };

			return product;
        }
    }
}

/*----------------------------------------------
* Return objects
-------------------------------------------------*/

public class Product
{
    public string Name {get;set;}
    public string Id {get;set;}
	public string Url { get; set; }	
	public string PictureSrc { get; set; }

}

