using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;

namespace MetricsManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly Temperature _temperature;

        public TemperatureController(Temperature temperature)
        {
            this._temperature = temperature;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime inputTime, [FromQuery] int inputTemp)
        {
            _temperature.listTimeTemp.Add((inputTime, inputTemp));
            return Ok();
        }
        
        [HttpGet("read")]
        public IActionResult Read([FromQuery] DateTime startTimeToRead, [FromQuery] DateTime endTimeToRead)
        {
            
            return Ok(_temperature.listTimeTemp.Where(w => w.Item1 >= startTimeToRead && w.Item1 <= endTimeToRead));
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime timeToUpdate, [FromQuery] int temp)
        {
            List<(DateTime, int)> tempList = new List<(DateTime, int)>();
            for (int i = 0; i < _temperature.listTimeTemp.Count; i++)
            {
                if (_temperature.listTimeTemp[i].Item1 == timeToUpdate)
                {
                    _temperature.listTimeTemp[i] = (timeToUpdate, temp);
                }
                        
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime startTimeToDelete, [FromQuery] DateTime endTimeToDelete)
        {
            _temperature.listTimeTemp = _temperature.listTimeTemp.Where(w => w.Item1 >= startTimeToDelete && w.Item1 <= endTimeToDelete).ToList();
            return Ok();
        }
    }
}