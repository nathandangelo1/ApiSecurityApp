﻿using WebApi.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        this._config = config;
    }

    // GET: api/<UsersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Version 2 value1", "Version 2 value2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    //[Authorize(Policy = Policies.MustHaveEmployeeId)]
    //[Authorize(Policy = Policies.MustBeAVeteranEmployee)]
    public string Get(int id)
    {
        return _config.GetConnectionString("Default");
    }

    // POST api/<UsersController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
