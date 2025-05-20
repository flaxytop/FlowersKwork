using Flowers.Domain.Entities;
using Flowers.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;

namespace Flowers.Services
{
    public class ApiProductService(HttpClient httpClient) : IProductService
    {
        public async Task<ResponseData<FN>> CreateProductAsync(FN product, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<FN>();

            // Послать запрос к API для сохранения объекта
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, product);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект:{response.StatusCode}";
                return responseData;
            }

            // Если файл изображения передан клиентом
            if (formFile != null)
            {

                // получить созданный объект из ответа Api-сервиса
                var FN = await response.Content.ReadFromJsonAsync<FN>();

                // создать объект запроса
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{FN.Id}")
                };

                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();

                // создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());

                // добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);

                // поместить контент в запрос
                request.Content = content;

                // послать запрос к Api-сервису
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение:{response.StatusCode} ";
                }
            }
            return responseData;
        }



        public async Task DeleteProductAsync(FN fn, int id)
        {
            if (fn == null)
            {
                throw new ArgumentNullException(nameof(fn), "Объект FN не может быть null.");
            }

            var response = await httpClient.DeleteAsync($"{httpClient.BaseAddress}{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка удаления продукта: {response.StatusCode}");
            }
        }
        public async Task<ResponseData<FN>> GetProductByIdAsync(int id)
        {
            var apiUrl = $"{httpClient.BaseAddress.AbsoluteUri}{id}";
            var response = await httpClient.GetFromJsonAsync<FN>(apiUrl);
            return new ResponseData<FN>() { Data = response };
        }

        public async Task<ResponseData<ListModel<FN>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;
            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }
            var query = QueryString.Create(queryData);
            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content
                .ReadFromJsonAsync<ResponseData<ListModel<FN>>>();
            }
            ;
            var response = new ResponseData<ListModel<FN>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
            return response;
        }

        public Task GetProductListAsync(int? id)
        {
            throw new NotImplementedException();
        }




      

        public async Task<ResponseData<FN>> UpdateProductAsync(int id, FN product, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = new ResponseData<FN>();

            // Отправляем запрос на обновление объекта
            var response = await httpClient.PutAsJsonAsync($"{httpClient.BaseAddress}{id}", product);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Ошибка обновления объекта: {response.StatusCode}";
                return responseData;
            }

            // Если файл изображения передан клиентом
            if (formFile != null)
            {
                // Получаем обновлённый объект из ответа API
                var updatedProduct = await response.Content.ReadFromJsonAsync<FN>();

                // Создаём объект запроса для загрузки изображения
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{updatedProduct.Id}/upload")
                };

                // Создаём контент типа multipart form-data
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(formFile.OpenReadStream());

                // Добавляем потоковый контент в общий контент по имени "image"
                content.Add(streamContent, "image", formFile.FileName);
                request.Content = content;

                // Отправляем запрос на загрузку изображения
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Ошибка загрузки изображения: {response.StatusCode}";
                    return responseData;
                }

                // Обновляем путь к изображению в объекте
                updatedProduct.Image = formFile.FileName;
                responseData.Data = updatedProduct;
            }

            responseData.Success = true;
            return responseData;
        }

    }
}

