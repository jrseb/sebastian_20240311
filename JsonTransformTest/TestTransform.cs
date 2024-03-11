using JsonTransformLibrary.services;
using System.Collections.Immutable;
using System.Text.Json;

namespace JsonTransformTest
{
	public class TestTransform
	{
		private const string TEST_RECORD_PATH = @"Assets\TestRecords.json";
		private const string TEMPLATE_PATH = @"Assets\template.html";

		private ImmutableList<JsonElement>? _records = default;
		private string _template = string.Empty;
		private JsonTransformService _service = new();

		[SetUp]
		public void Setup()
		{
			try
			{
				if (File.Exists(TEST_RECORD_PATH))
				{
					var jdoc = JsonDocument.Parse(File.ReadAllText(TEST_RECORD_PATH));
					var edoc = jdoc.RootElement.EnumerateArray();
					_records = edoc.ToImmutableList();

					//_records = jrecs?.ToImmutableList<object>();
					_template = File.ReadAllText(TEMPLATE_PATH);
				}
				else
					throw new Exception($"[Setup Error]{TEST_RECORD_PATH} was not found!");
			}
			catch (Exception e)
			{
				Assert.Fail(e.Message);
			}
		}

		[Test(Description = "Normal test dat using valid file.")]
		public void NormalTransform()
		{
			var html = _service.JsonToTemplate(_records, _template, 5);
			Assert.IsTrue(html != string.Empty);
		}

		[Test(Description = "Passing null records")]
		public void NullRecords()
		{
			var html = _service.JsonToTemplate(null, _template, 5);
			Assert.IsTrue(html == string.Empty);
		}

		[Test(Description = "Passing null template")]
		public void NullTemplate()
		{
			var html = _service.JsonToTemplate(_records, null, 5);
			Assert.IsTrue(html == string.Empty);
		}
	}
}