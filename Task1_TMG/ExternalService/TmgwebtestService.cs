using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Task1_TMG
{
    public class TmgwebtestService : IIntegrationService
    {
        private readonly HttpClient _client;
        
        private readonly TimeSpan _baseTimeout = TimeSpan.FromMilliseconds(1000);

        public TmgwebtestService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://tmgwebtest.azurewebsites.net");
            client.DefaultRequestHeaders.Add("TMG-Api-Key", "0J/RgNC40LLQtdGC0LjQutC4IQ==");
            client.Timeout = _baseTimeout;
            _client = client;
        }

        public ServiceDataModel GetStringById(int id)
        {
            var errorMassages = new List<string>();

            try
            {
                var task = AsyncRequest(id);
                
                if (task.IsFaulted || task.IsCanceled)
                {
                    throw task.Exception;
                }

                var (result, success) = task.Result;

                if (success == HttpStatusCode.OK)
                {
                    var jsonString = JsonSerializer.Deserialize<TmgwebtestResponse>(result);

                    if (jsonString != null)
                    {
                        return new ServiceDataModel
                        {
                            Data = jsonString.Text,
                            ErrorMassages = null
                        };
                    }

                    errorMassages.Add($"Для строки с id={id} произошла ошибка десериализации ответа сервера. Обратитесь к Сис.Администратору.");
                }
                else
                {
                    errorMassages.Add($"Для строки с id={id} cервер ответил ошибкой с кодом \"{success.ToString()}\". Обратитесь к Сис.Администратору.");
                }
            }
            catch (Exception ex)
            {
                errorMassages.Add($"Произошла ошибка \"{ex.Message}\". Возможно превышено время ожидания ответа. Попробуйте повторить запрос на расчет или обратитесь к Сис.Администратору.");
            }
            
            return new ServiceDataModel()
            {
                Data = "",
                ErrorMassages = errorMassages
            };
        }

        private async Task<(string, HttpStatusCode)> AsyncRequest(int id)
        {
            var response = await _client.GetAsync($"https://tmgwebtest.azurewebsites.net/api/textstrings/{id}")
                .ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode) return ("", response.StatusCode);
            
            var responseBody = await response.Content.ReadAsStringAsync();

            return (responseBody, response.StatusCode);

        }
    }
}