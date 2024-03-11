using JsonTransformLibrary.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace JsonTranformApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class JsonTemplatedController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<JsonTemplatedController> _logger;
		public readonly ITransform _transform;

		public JsonTemplatedController(ILogger<JsonTemplatedController> logger, ITransform transformsvc)
		{
			_logger = logger;
			_transform = transformsvc;
		}

		/// <summary>
		/// Tranform a Json array to a templated output
		/// </summary>
		/// <param name="jsonRecords">json records</param>
		/// <param name="template">template format to use</param>
		/// <returns>string</returns>
		[HttpPost(Name = "TransformJson")]
		public IActionResult ConvertToTemplate([FromBody] object[] jsonRecords, string template)
		{
			try
			{
				var recs = jsonRecords.ToImmutableList();
				List<JsonElement> items = new();
				recs.ForEach(rec => items.Add((JsonElement)rec));


				if (_transform.ValidateParameters(items.ToImmutableList(), template))
				{
					var result = _transform.JsonToTemplate(items.ToImmutableList(), template, 10);
					return Ok(result);
				}
				else
					return NotFound("Template/Tags or json element is empty");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.InnerException, ex.Message);
				throw;
			}
		}
	}
}
