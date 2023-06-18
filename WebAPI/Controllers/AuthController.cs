using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public AuthController(HttpClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> TestAuth()
        {
            return Ok("Hello, you are authorized.");
        }

        [HttpGet("token/full")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> GetFullTokenAsync()
        {
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _configuration["IdentityServerTokenEndpoint"],
                ClientId = _configuration["ClientId"] ?? throw new ArgumentNullException(nameof(ClientCredentialsTokenRequest.ClientId)),
                ClientSecret = _configuration["ClientSecret"],
                Scope = _configuration["ScopeFull"]
            });

            return tokenResponse;
        }

        [HttpGet("token/read")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> GetReadTokenAsync()
        {
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _configuration["IdentityServerTokenEndpoint"],
                ClientId = _configuration["ClientId"] ?? throw new ArgumentNullException(nameof(ClientCredentialsTokenRequest.ClientId)),
                ClientSecret = _configuration["ClientSecret"],
                Scope = _configuration["ScopeRead"]
            });

            return tokenResponse;
        }

        [HttpGet("token/write")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> GetWriteTokenAsync()
        {
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _configuration["IdentityServerTokenEndpoint"],
                ClientId = _configuration["ClientId"] ?? throw new ArgumentNullException(nameof(ClientCredentialsTokenRequest.ClientId)),
                ClientSecret = _configuration["ClientSecret"],
                Scope = _configuration["ScopeWrite"]
            });

            return tokenResponse;
        }
    }
}
