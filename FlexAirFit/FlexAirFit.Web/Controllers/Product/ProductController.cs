using FlexAirFit.Core.Models;
using FlexAirFit.Web;
using FlexAirFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FlexAirFit.Web.Controllers;
public class ProductController : Controller
{
    private readonly ILogger _logger = Log.ForContext<ProductController>();
    private readonly Context _context;

    public ProductController(ILogger<ProductController> logger, Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> ViewProduct(int page = 1)
    {
        _logger.Information("Executing ViewProduct action with page {Page}", page);
            
        var products = await _context.ProductService.GetProducts(10, 10 * (page - 1));

        var productsModels = products.Select(t => new ProductModel
        {
            Id = t.Id,
            Name = t.Name,
            Price = t.Price,
            Type = t.Type,
            PageCurrent = page
        });

        return View(productsModels);
    }

    public IActionResult BuyProduct()
    {
        try
        {
            Guid id = Guid.Parse(ControllerContext.RouteData.Values["id"].ToString());
            var product = _context.ProductService.GetProductById(id).Result;
            var clientProductModel = new BuyProductModel
            {
                IdProduct = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            return View(clientProductModel);
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    [HttpPost]
    public IActionResult BuyProductByClient(Guid idProduct, bool UseBonus)
    {
        try
        {
            var IdClient = Guid.Parse(Request.Cookies["UserId"]);

            ClientProduct clientProduct = new ClientProduct(IdClient, idProduct);
            int cost = _context.ClientProductService.AddClientProductAndReturnCost(clientProduct, UseBonus).Result;

            return View(new ClientProductModel
            {
                IdProduct = idProduct,
                Name = _context.ProductService.GetProductById(idProduct).Result.Name,
                Cost = cost
            });
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    public IActionResult CreateProduct()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductModel productModel)
    {
        try
        {
            var product = new Product (Guid.NewGuid(), productModel.Type, productModel.Name, productModel.Price);
            await _context.ProductService.CreateProduct(product);
        }
        catch (Exception ex)
        {
            Response.Cookies.Append("errorType", ex.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }

        return RedirectToAction("ViewProduct");
    }

    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        try
        {
            await _context.ProductService.DeleteProduct(productId);
            return RedirectToAction("ViewProduct");
        }
        catch (Exception ex)
        {
            Response.Cookies.Append("errorType", ex.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    public IActionResult UpdateProduct(Guid productId)
    {
        try
        {
            var product = _context.ProductService.GetProductById(productId).Result;
            var productModel = new ProductModel
            {
                Id = productId,
                Name = product.Name,
                Price = product.Price,
                Type = product.Type
            };
            return View(productModel);
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(ProductModel productModel)
    {
        try
        {
            var product = new Product(productModel.Id, productModel.Type, productModel.Name, productModel.Price); 
            await _context.ProductService.UpdateProduct(product);
            return RedirectToAction("ViewProduct");
        }
        catch (Exception e)
        {
            Response.Cookies.Append("errorType", e.GetType().Name);
            return RedirectToAction("Error", "Shared");
        }
    }

}