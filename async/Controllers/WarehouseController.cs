using async.Models;
using async.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace async.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseDb _warehouseDb;

        public WarehouseController(IWarehouseDb warehouseDb)
        {
            _warehouseDb = warehouseDb;
        }

        [HttpPost]
        public async Task<IActionResult> GetWarehouses(Warehouse warehouse)
        {
            try
            {
                var result = await _warehouseDb.PostWarehouse(warehouse);

                return Ok(result);
            }
            catch
            {
                return StatusCode(400, "Posting error! There is something wrong");
            }
        }
    }
    [ApiController]
    [Route("api/warehouses2")]
    public class Warehouse2Controller : ControllerBase
    {
        private readonly IWarehouseDb _wearehouseDb;

        public Warehouse2Controller(IWarehouseDb warehouseDb)
        {
            _wearehouseDb = warehouseDb;
        }

        [HttpPost]
        public async Task<IActionResult> GetWarehouses(Warehouse warehouse)
        {
            var result = await _wearehouseDb.PostWarehouseSelect(warehouse);
            return Ok(result);
        }
    }
}

