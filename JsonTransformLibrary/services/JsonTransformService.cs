using JsonTransformLibrary.interfaces;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json;

namespace JsonTransformLibrary.services
{
	public class JsonTransformService : ITransform
	{
		private const string TEMPLATE_START_TAG = @"<template-row>";
		private const string TEMPLATE_END_TAG = @"</template-row>";
		private const string TEMPLATE_PREFIX = @"field-";


		/// <summary>
		/// Driving procedure to complete the output
		/// </summary>
		/// <param name="json">list of json objects</param>
		/// <param name="template">complete template</param>
		/// <param name="propertySampleRow">no of records to use to fetch json properties</param>
		/// <param name="prefix">the prefix used before the property name</param>
		/// <returns>templated json record output</returns>
		public string JsonToTemplate(ImmutableList<JsonElement>? json, string? template, int propertySampleRow = 10, string prefix = "")
		{
			if (propertySampleRow <= 0) propertySampleRow = 10;
			if (!ValidateParameters(json, template)) return string.Empty;

			try
			{
				var tmp = template ?? string.Empty;

				prefix = prefix.Length == 0 ? TEMPLATE_PREFIX : prefix;
				//-- extract the template parts
				var templateHead = this.GetHeadPart(tmp);
				var templateRow = this.GetTemplateRow(tmp);
				var templateTail = this.GetTailsPart(tmp);
				var properties = this.GetJsonProperties(json, propertySampleRow);
				var templateTag = this.GetTemplateProperies(properties, tmp, prefix);

				var rowLines = BuildRecordRows(json, templateTag, templateRow);

				return templateHead.Trim() +
						rowLines +
						templateTail.Trim();
			}
			catch(Exception ex) 
			{
				throw new ApplicationException("Something bad happened on the server.",  ex);
			}
		}


		/// <summary>
		/// Validate input
		/// </summary>
		/// <param name="json"></param>
		/// <param name="template"></param>
		/// <returns></returns>
		public bool ValidateParameters(ImmutableList<JsonElement>? json, string? template)
		{
			bool good = true;
			var tmp = template ?? string.Empty;

			if (tmp.Length == 0  || !tmp.Contains(TEMPLATE_START_TAG) || !tmp.Contains(TEMPLATE_END_TAG)) good = false;
			if (json is null) good = false;

			return good;
		}

		/// <summary>
		/// Build the Row data contents
		/// </summary>
		/// <param name="json">json elements</param>
		/// <param name="properties">dictionary of fields found in template</param>
		/// <param name="templateRow">Template for the row</param>
		/// <returns>formated data row</returns>
		private string BuildRecordRows(ImmutableList<JsonElement> json,
										ImmutableDictionary<string, string> properties,
										string templateRow)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (var elem in json)
			{
				var rowLine = GetRecordRow(elem, properties, templateRow);
				stringBuilder.Append(rowLine);
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Generate the data row to display
		/// </summary>
		/// <param name="element">json elements list</param>
		/// <param name="tagProperties">template row tags</param>
		/// <param name="templateRow">template row format</param>
		/// <returns>data row for a record</returns>
		private string GetRecordRow(JsonElement element, ImmutableDictionary<string, string> tagProperties, string templateRow)
		{
			string rowTemplate = templateRow;

			var items = element.EnumerateObject();
			while (items.MoveNext())
			{
				var item = items.Current;
				var name = FieldNameTransform(item.Name);

				//-- we change the template line
				if (tagProperties.ContainsKey(name))
				{
					rowTemplate = ReplaceTagValue(rowTemplate, tagProperties[name], item.Value.ToString());
				}
			}
			return rowTemplate.Trim();
		}

		/// <summary>
		/// Making sure we replace all occurance of tag
		/// </summary>
		/// <param name="templaterow">current data row</param>
		/// <param name="tagName">tag to replace</param>
		/// <param name="value">value to place</param>
		/// <returns>new data row</returns>
		private string ReplaceTagValue(string templaterow, string tagName, string value)
		{
			try
			{
				while (templaterow.Contains(tagName))
				{
					templaterow = templaterow.Replace(tagName, value);
				}
			}
			catch
			{
				throw;
			}
			return templaterow.ToString();
		}

		/// <summary>
		/// get all field tags that needs to be replace
		/// </summary>
		/// <param name="properties">all json properties</param>
		/// <param name="templateRow">template row format</param>
		/// <param name="prefix">field tag prefix</param>
		/// <returns>dictionary of field and tag name</returns>
		private ImmutableDictionary<string, string> GetTemplateProperies(ImmutableList<string> properties,
																		string templateRow,
																		string prefix)
		{
			Dictionary<string, string> templateProperties = new();
			try
			{
				foreach (var item in properties)
				{
					var tag = FieldTagsTransform(item, prefix);

					if (templateRow.Contains(tag))
						templateProperties.Add(item, tag);
				}
			}
			catch (Exception)
			{
				throw;
			}
			return templateProperties.ToImmutableDictionary();
		}

		/// <summary>
		/// Gets all the possible property in a json object for a given sample set
		/// </summary>
		/// <param name="json">complete json record</param>
		/// <param name="propertySampleRow">no of sample</param>
		/// <returns>list of json properties</returns>
		private ImmutableList<string> GetJsonProperties(ImmutableList<JsonElement> json, int propertySampleRow)
		{
			List<string> properties = new List<string>();
			long count = 0;

			try
			{
				foreach (var elem in json)
				{
					properties = GetRecordProperty(elem, properties);
					count++;
					if (count == propertySampleRow) break;
				}
			}
			catch
			{
				throw;
			}
			return properties.ToImmutableList();
		}

		/// <summary>
		/// Normalize the casing of element names
		/// </summary>
		/// <param name="a">Name of element</param>
		/// <returns>new lower case name with no extra space</returns>
		private string FieldNameTransform(string a) => a.ToLower().Trim();

		/// <summary>
		/// Normalize the field tag searching
		/// </summary>
		/// <param name="fieldName">Normalize fieldname</param>
		/// <param name="prefix">tag prefix to use</param>
		/// <returns>complete prefixed tag</returns>
		private string FieldTagsTransform(string fieldName, string prefix) => $"<{prefix}{fieldName}>";

		/// <summary>
		/// Get sample record property name
		/// </summary>
		/// <param name="element">json record</param>
		/// <param name="existingProperties">existing list</param>
		/// <returns>new property list</returns>
		private List<string> GetRecordProperty(JsonElement element, List<string> existingProperties)
		{
			List<string> properties = existingProperties.ToList();
			try
			{
				var items = element.EnumerateObject();

				while (items.MoveNext())
				{
					var item = items.Current;
					if (!properties.Exists(a => a == FieldNameTransform(item.Name)))
						properties.Add(FieldNameTransform(item.Name));
				}
			}
			catch
			{
				throw;
			}
			return properties;
		}

		/// <summary>
		/// Extract the tail of the template
		/// </summary>
		/// <param name="template">complete template</param>
		/// <returns></returns>
		private string GetTailsPart(string template)
		{
			string tailPart = string.Empty;

			try
			{
				var endPosition = template.IndexOf(TEMPLATE_END_TAG);
				endPosition += TEMPLATE_END_TAG.Length;
				tailPart = template.Substring(endPosition);
			}
			catch
			{
				throw;
			}
			return tailPart;
		}

		/// <summary>
		/// Extract the head of the template
		/// </summary>
		/// <param name="template">complete template</param>
		/// <returns></returns>
		private string GetHeadPart(string template)
		{
			string headPart = string.Empty;

			try
			{
				var startPosition = template.IndexOf(TEMPLATE_START_TAG, 0);
				headPart = template.Substring(0, startPosition);
			}
			catch
			{
				throw;
			}
			return headPart;
		}

		/// <summary>
		/// Extract the template row exclusing the actual template row tag
		/// </summary>
		/// <param name="template">complete template</param>
		/// <returns></returns>
		private string GetTemplateRow(string template)
		{
			string templateRow = string.Empty;

			try
			{
				var startPosition = template.IndexOf(TEMPLATE_START_TAG, 0);
				startPosition += TEMPLATE_START_TAG.Length;

				var endPosition = template.IndexOf(TEMPLATE_END_TAG);
				endPosition -= startPosition;

				templateRow = template.Substring(startPosition, endPosition);
			}
			catch
			{
				throw;
			}
			return templateRow;
		}
	}
}