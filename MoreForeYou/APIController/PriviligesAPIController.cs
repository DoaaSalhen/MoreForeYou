using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoreForYou.APIController
{

    [Route("api/[controller]")]
    [ApiController]
    public class PriviligesAPIController : ControllerBase
    {
        private readonly IPrivilegeService _privilegeService;

        public PriviligesAPIController(IPrivilegeService privilegeService)
        {
            _privilegeService = privilegeService;
        }


        // GET: api/<PriviligesAPIController>
        [HttpGet("Getprivilges")]
        public IActionResult Getprivilges()
        {
            try
            {
                List<PrivilegeModel> privilges = _privilegeService.GetAllPrivileges().Result;
                List<PriviligeAPIModel> priviligeAPIModels = _privilegeService.CreatePriviligeAPIModel(privilges);
                return Ok(new { Message = "Success Process", Data = priviligeAPIModels });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }

        }


    }
}
