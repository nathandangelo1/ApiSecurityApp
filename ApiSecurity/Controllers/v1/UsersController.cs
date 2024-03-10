using WebApi.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1;

//[Route("api/v1/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }

    // GET: api/v1/<UsersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api//v1/<UsersController>/5
    [HttpGet("{id}")]
    [Authorize(Policy = Policies.MustHaveEmployeeId)]
    [Authorize(Policy = Policies.MustBeAVeteranEmployee)]
    public string Get(int id)
    {
        return _config.GetConnectionString("Default");
    }

    // POST api/v1/<UsersController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/v1/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/v1/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
