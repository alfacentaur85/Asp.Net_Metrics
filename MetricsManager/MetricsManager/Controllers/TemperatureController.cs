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
        private  List<Temperature> _temperature = new List<Temperature>();

        public TemperatureController(List<Temperature> temperature)
        {
            this._temperature = temperature;
        }

        [HttpPost("create")]
        public IActionResult Create([FromQuery] DateTime inputTime, [FromQuery] int inputTemp)
        {
            _temperature.Add(new Temperature(inputTime, inputTemp));
            return Ok();
        }
        
        [HttpGet("read")]
        public IActionResult Read([FromQuery] DateTime startTimeToRead, [FromQuery] DateTime endTimeToRead)
        {

            return Ok(_temperature.Where(w => w.dt >= startTimeToRead && w.dt <= endTimeToRead));
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] DateTime timeToUpdate, [FromQuery] int temp)
        {
            
            for (int i = 0; i < _temperature.Count; i++)
            {
                if (_temperature[i].dt == timeToUpdate)
                {
                    _temperature[i].vt = temp;
                }
                        
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] DateTime startTimeToDelete, [FromQuery] DateTime endTimeToDelete)
        {
            for (int i = 0; i < _temperature.Count; i++ )
            {
                if(_temperature[i].dt >= startTimeToDelete && _temperature[i].dt <= endTimeToDelete)
                {
                    _temperature.RemoveAt(i);
                    i--;
                }
            }
            
            return Ok(_temperature);
        }
    }
}