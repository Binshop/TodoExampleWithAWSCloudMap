using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Todo.DTOs;

namespace Todo.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MyTodosController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyTodosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await SendAsync(HttpMethod.Get, null);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Content(data, MediaTypeNames.Application.Json);
            }

            return StatusCode((int)response.StatusCode);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var response = await SendAsync(HttpMethod.Get, new TodoItemDto { Id = id });

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Content(data, MediaTypeNames.Application.Json);
            }

            return StatusCode((int)response.StatusCode);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] TodoItemDto todo)
        {
            if (todo.Id <= 0)
            {
                return BadRequest();
            }

            var response = await SendAsync(HttpMethod.Put, todo);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Content(data, MediaTypeNames.Application.Json);
            }

            return StatusCode((int)response.StatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TodoItemDto todo)
        {
            var response = await SendAsync(HttpMethod.Post, todo);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var todoItem = JsonConvert.DeserializeObject<TodoItemDto>(data);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = todoItem.Id }, todoItem);
            }

            return StatusCode((int)response.StatusCode);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, TodoItemDto todo)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var request = BuildRequest(method, todo);
            return await client.SendAsync(request);
        }

        private HttpRequestMessage BuildRequest(HttpMethod method, TodoItemDto todo)
        {
            var endpoint = todo?.Id > 0 ? $"{ServiceEndPoint}/api/todos/{todo.Id}"
                                        : $"{ServiceEndPoint}/api/todos";

            var requestUri = new Uri(endpoint);

            var request = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = method,
            };

            if (todo != null && (method == HttpMethod.Put || method == HttpMethod.Post))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(todo),
                                                    Encoding.UTF8,
                                                    MediaTypeNames.Application.Json);
            }

            return request;
        }

        private string ServiceEndPoint => "https://localhost:44302";
    }
}
