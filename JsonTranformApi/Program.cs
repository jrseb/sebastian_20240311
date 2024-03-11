
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using JsonTransformLibrary.interfaces;
using JsonTransformLibrary.services;

namespace JsonTranformApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddMvc(setupAction =>
			{
				setupAction.Filters.Add(
					new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
				setupAction.Filters.Add(
					new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
				setupAction.Filters.Add(
					new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
				setupAction.Filters.Add(
					new ProducesDefaultResponseTypeAttribute());

				setupAction.ReturnHttpNotAcceptable = true;

				setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

				var jsonOutputFormatter = setupAction.OutputFormatters
					.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();

				if (jsonOutputFormatter != null)
				{
					// remove text/json as it isn't the approved media type
					// for working with JSON at API level
					if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
					{
						jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
					}
				}
			});
			builder.Services.AddScoped<ITransform, JsonTransformService>();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
