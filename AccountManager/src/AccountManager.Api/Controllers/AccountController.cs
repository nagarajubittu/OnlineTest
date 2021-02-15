using System.Threading.Tasks;
using System.Collections.Generic;
using AccountManager.Api.Models;
using AccountManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.Api.Controllers
{
    [ApiController]
    // [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/account")]
        public async Task<ActionResult> PostAccountAsync([FromBody] PostAccountRequestModel model)
        {
            try
            {
                await _accountService.CreateAsync(model);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/account")]
        public async Task<ActionResult> PutAccountAsync([FromBody] PutAccountRequestModel model)
        {
            try
            {
                await _accountService.UpdateAsync(model);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountModel>), StatusCodes.Status200OK)]
        [Route("/account")]
        public async Task<ActionResult> GetAccountsAsync()
        {
            try
            {
                return Ok(await _accountService.GetAllAsync());
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
        [Route("/account/{accountId}")]
        public async Task<ActionResult> GetAccountAsync(int accountId)
        {
            try
            {
                return Ok(await _accountService.GetAsync(accountId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/account/{accountId}")]
        public async Task<ActionResult> DeleteAccountAsync(int accountId)
        {
            try
            {
                await _accountService.DeleteAsync(accountId);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
    }
}
