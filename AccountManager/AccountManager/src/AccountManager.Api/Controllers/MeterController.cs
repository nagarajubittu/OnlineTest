using System.Threading;
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
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _meterService;

        public MeterController(IMeterService meterService)
        {
            _meterService = meterService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetMeterReadingResponseModel>), StatusCodes.Status200OK)]
        [Route("/meter-reading/{accountId}/account")]
        public async Task<IActionResult> GetByAccountIdAsync(int accountId)
        {
            try
            {
                return Ok(await _meterService.GetAsync(accountId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/meter-reading")]
        public async Task<IActionResult> PostMeterAsync([FromBody] MeterReadingRequestModel model)
        {
            try
            {
                await _meterService.CreateAsync(model);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("/meter-reading-uploads")]
        public async Task<IActionResult> UploadFile(
         IFormFile file,
         CancellationToken cancellationToken)
        {
            if (CheckIfCsvFile(file))
            {
                try
                {
                    return Ok(await _meterService.LoadFromCsvAsync(file));
                }
                catch (System.Exception)
                {
                    return StatusCode(500, "Error occured");
                }
                
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/meter-reading/{meterId}")]
        public async Task<IActionResult> DeleteMeterAsync(int meterId)
        {
            try
            {
                await _meterService.DeleteAsync(meterId);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("/meter-reading/{accountId}/account")]
        public async Task<IActionResult> DeleteByAccountAsync(int accountId)
        {
            try
            {
                await _meterService.DeleteByAccount(accountId);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        private bool CheckIfCsvFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv"); // Change the extension based on your need
        }
    }
}
