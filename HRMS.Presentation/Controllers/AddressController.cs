﻿using HRMS.DAL;
using HRMS.DAL.DTOs;
using HRMS.DAL.Handler;
using HRMS.DAL.Interfaces;
using HRMS.DAL.ModelsDto;
using HRMS.DAL.TypeRepository;
using HRMS.DAL.UnitOfWork;
using HRMS.Presentation.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExceptionHandler]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        IGenericRepository<Addresses> addressRepository;

        public AddressController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            addressRepository = new AddressRepository(_unitOfWork, "Address", "EmployeeID", "MSORG");
        }

        [HttpGet("[action]/{employeeID}/{orgID}")]
        public async Task<ActionResult<IEnumerable<Addresses>>> GetAddress(int employeeID, int orgID)
        {
            Dictionary<string, int> whereConditionsDic = new Dictionary<string, int>
            {
                { "EmployeeID", employeeID },
                { "ORGID", orgID }
            };
            var data = await addressRepository.GetListByCustomFields(whereConditionsDic);
            return data;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<Addresses>> CreateAddress(AddressDto Address)
        {
            if (TryValidateModel(Address))
            {
                MappingHandler mapping = new MappingHandler();
                Addresses _Address = mapping.Map<Addresses>(Address);
                return await addressRepository.Add(_Address);
            }
            else
                return BadRequest();

        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, AddressDto Address)
        {
            if (TryValidateModel(Address))
            {
                MappingHandler mapping = new MappingHandler();
                Addresses _Address = mapping.Map<Addresses>(Address);
                return await addressRepository.Update(id, _Address);
            }
            else
                return BadRequest();

        }
    }
}
