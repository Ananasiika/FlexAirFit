using System.ComponentModel.DataAnnotations;
using FlexAirFit.Application.Exceptions.ServiceException;
using FlexAirFit.Application.IServices;
using FlexAirFit.Core.Enums;
using FlexAirFit.Core.Models;
using FlexAirFit.Web2._0.Atributes;
using FlexAirFit.Web2._0.Dto.Converters;
using FlexAirFit.Web2._0.Dto.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlexAirFit.Web2._0.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IClientProductService _clientProductService;

    public ProductController(IProductService productService, IClientProductService clientProductService)
    {
        _productService = productService;
        _clientProductService = clientProductService;
    }
    
    /// <summary>
    /// Получение списка товаров
    /// </summary>
    /// <response code="200">Товары успешно получены.</response>
    /// <response code="500">Внутренняя ошибка сервера.</response>
    /// <returns>Список товаров.</returns>
    [HttpGet()]
    [SwaggerResponse(statusCode: 200,description: "Товары успешно получены.")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> GetProducts(int? limit, int? offset)
    {
        try
        {
            var products = await _productService.GetProducts(limit, offset);

            return Ok(products.ConvertAll(ProductDtoConverter.ToDto));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Создание нового товара
    /// </summary>
    /// <response code="201">Товар успешно создан</response>
    /// <response code="400">Неверные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Добавленный товар.</returns>
    [HttpPost()]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 201,description: "Товар успешно создан")]
    [SwaggerResponse(statusCode: 400,description: "Неверные данные")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера.")]
    public async Task<IActionResult> AddProduct([Required][FromBody] CreateProductRequestDto productDto)
    {
        try
        {
            var product = CreateProductRequestDtoConverter.ToCore(productDto);
            await _productService.CreateProduct(product);

            return Created("/api/products/" + product.Id, ProductDtoConverter.ToDto(product));
        }
        catch (CreateProductRequestException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    
    /// <summary>
    /// Обновление информации о товаре
    /// </summary>
    /// <response code="200">Товар успешно обновлен</response>
    /// <response code="400">Ошибка в запросе</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Товар не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Обновленный товар.</returns>
    [HttpPut("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 200, description: "Товар успешно обновлен")]
    [SwaggerResponse(statusCode: 400, description: "Ошибка в запросе")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Товар не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> UpdateProduct([Required] Guid id, [FromBody] UpdateProductRequestDto request)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProduct(UpdateProductRequestDtoConverter.ToCore(request, id));
            return Ok(ProductDtoConverter.ToDto(updatedProduct));
        }
        catch (ProductNotFoundException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Удаление товара
    /// </summary>
    /// <response code="204">Товар успешно удален</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Товар не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{id}")]
    [Authorize(UserRole.Admin)]
    [SwaggerResponse(statusCode: 204, description: "Товар успешно удален")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Товар не найден")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> DeleteProduct([Required] Guid id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
        catch (ProductNotFoundException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Покупка товара
    /// </summary>
    /// <response code="200">Товар успешно куплен</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="403">У пользователя нет прав</response>
    /// <response code="404">Товар или пользователь не найдены</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    /// <returns>Итоговая стоимость.</returns>
    [HttpPatch("{id}")]
    [Authorize(UserRole.Admin, UserRole.Client)]
    [SwaggerResponse(statusCode: 200, description: "Товар успешно куплен")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь не авторизован")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя нет прав")]
    [SwaggerResponse(statusCode: 404, description: "Товар или пользователь не найдены")]
    [SwaggerResponse(statusCode: 500, description: "Внутренняя ошибка сервера")]
    public async Task<IActionResult> BuyProduct(Guid id, [FromBody] BuyProductRequestDto request)
    {
        try
        {
            var buyProductResponse =
                await _clientProductService.AddClientProductAndReturnCost(BuyProductRequestDtoConverter.ToCore(request, id), request.UsesBonuses);
            return Ok(BuyProductResponseDtoConverter.ToDto(buyProductResponse));
        }
        catch (ProductNotFoundException)
        {
            return StatusCode(400);
        }
        catch (UserNotAuthorizedException)
        {
            return StatusCode(401);
        }
        catch (UserForbiddenException)
        {
            return StatusCode(403);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}