using Amazon.ServiceDiscovery;
using Amazon.ServiceDiscovery.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("api/[controller]")]
    public class MyTodosController : ControllerBase
    {
        private readonly ILogger<MyTodosController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAmazonServiceDiscovery _serviceDiscovery;

        public MyTodosController(ILogger<MyTodosController> logger,
                                 IHttpClientFactory httpClientFactory,
                                 IAmazonServiceDiscovery serviceDiscovery)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceDiscovery = serviceDiscovery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await SendAsync(HttpMethod.Get);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Content(data, MediaTypeNames.Application.Json);
            }

            return StatusCode((int)response.StatusCode);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
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
            if (todo.Id == Guid.Empty)
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

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, TodoItemDto todo = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var request = await BuildRequestAsync(method, todo);
            return await client.SendAsync(request);
        }

        private async Task<HttpRequestMessage> BuildRequestAsync(HttpMethod method, TodoItemDto todo)
        {
            var host = await GetApiHostAsync();

            var endpoint = $"{host.TrimEnd('/')}/api/todos";
            if (todo != null && todo.Id != Guid.Empty)
            {
                endpoint = $"{endpoint}/{todo.Id}";
            }

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

        private async Task<string> GetApiHostAsync()
        {
            var response = await _serviceDiscovery.DiscoverInstancesAsync(new DiscoverInstancesRequest
            {
                NamespaceName = Constants.NAMESPACE_NAME,
                ServiceName = Constants.SERVICE_NAME_BACK_END,
            });

            if (response.Instances.Count <= 0)
            {
                _logger.LogWarning("No backend service instance found.");
                return string.Empty;
            }

            var instance = response.Instances[0];

            return instance.Attributes["TODO_ENDPOINT"];
        }
    }
}
