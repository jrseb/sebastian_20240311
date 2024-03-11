using System.Collections.Immutable;
using System.Text.Json;

namespace JsonTransformLibrary.interfaces
{
	public interface ITransform
	{
		/// <summary>
		/// Interface for the actual json to template processing
		/// </summary>
		/// <param name="json">list of json objects</param>
		/// <param name="template">complete template</param>
		/// <param name="propertySampleRow">no of records to use to fetch json properties</param>
		/// <param name="prefix">the prefix used before the property name</param>
		/// <returns>templated json record output</returns>
		public string JsonToTemplate(ImmutableList<JsonElement>? json, string template, int propertySampleRow = 10, string prefix = "");
		public bool ValidateParameters(ImmutableList<JsonElement>? json, string? template);
	}
}