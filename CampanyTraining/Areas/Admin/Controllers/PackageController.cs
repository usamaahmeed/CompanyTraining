﻿using CompanyTraining.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTraining.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackageController(IPackageRepository packageRepository)
        {
            this._packageRepository = packageRepository;
        }


        private bool isUpdated(PackageRequest packageRequest,Package package)
        {
            bool updated = false;
            if(packageRequest.Name != null)
            {
                package.Name = packageRequest.Name;
                updated = true;
            }
            if(packageRequest.Description != null)
            {
                package.Description = packageRequest.Description;
                updated = true;
            }
            if(packageRequest.Price != package.Price)
            {
                package.Price = packageRequest.Price;
                updated = true;
            }
            if (packageRequest.DurationDay != package.DurationDay) { 
                package.DurationDay = packageRequest.DurationDay;
                updated = true;
            }
            return updated;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PackageRequest packageRequest)
        {
            if (packageRequest == null)
                return BadRequest();
            var package= await _packageRepository.CreateAsync(packageRequest.Adapt<Package>());
            return Ok(new
            {
                Message = "Packages Created Successfully",
                Success = true,
                Data = package
            });
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var packages = _packageRepository.Get(tracked: false);
            return Ok(new{
                Message = "Packages Return Successfully",
                Success = true,
                Data = packages.Adapt<IEnumerable<PackageResponse>>()
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody]PackageRequest packageRequest)
        {
            var packageInDb = _packageRepository.GetOne(expression:e=>e.Id==id);
            if(packageInDb == null)
                return NotFound();
            if (isUpdated(packageRequest, packageInDb))
            {
                packageInDb.Id = id;
                await _packageRepository.CommitAsync();
            }
            return Ok(new
            {
                Message = "Packages Update Successfully",
                Success = true,
                Data = packageInDb.Adapt<PackageResponse>()
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var package = _packageRepository.GetOne(expression:e=>e.Id == id);
            if (package == null)
                return NotFound();
            await _packageRepository.DeleteAsync(package);
            return Ok(new
            {
                Message = "Packages Delete Successfully",
                Success = true,

            });
        }
    }
}
