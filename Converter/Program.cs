using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

class Program
{
	static void Main()
	{
		string geoJsonFilePath = "Enter the path for .geojson file";
		string isoxmlFilePath = "Enter the path for the output of .isoxml file";

		// Read the GeoJSON file
		string geoJsonContent = File.ReadAllText(geoJsonFilePath);

		// Parse the GeoJSON content
		var geoJson = JsonConvert.DeserializeObject<JObject>(geoJsonContent);

		// Generate ISOXML representation
		var isoxml = GenerateIsoXml(geoJson);

		// Write the ISOXML to file
		File.WriteAllText(isoxmlFilePath, isoxml);

		Console.WriteLine("Conversion completed successfully.");
	}

	static string GenerateIsoXml(JObject geoJson)
	{
		// Extract relevant data from the GeoJSON
		var features = geoJson["features"] as JArray;
		if (features == null || features.Count == 0)
		{
			throw new ArgumentException("GeoJSON features not found.");
		}

		var feature = features[0];
		var properties = feature["properties"];
		var geometry = feature["geometry"];

		// Create the ISOXML structure
		var isoxml = new JObject(
			new JProperty("type", "ISOXML"),
			new JProperty("version", "1.0"),
			new JProperty("features",
				new JArray(
					new JObject(
						new JProperty("name", properties["Name"]),
						new JProperty("row", properties["row"]),
						new JProperty("treeCount", properties["treeCoun"]),
						new JProperty("rowLength", properties["rowLengt"]),
						new JProperty("cropName", properties["cropName"]),
						new JProperty("geometry",
							new JObject(
								new JProperty("type", geometry["type"]),
								new JProperty("coordinates", geometry["coordinates"])
							)
						)
					)
				)
			)
		);

		return isoxml.ToString(Formatting.Indented);
	}
}
