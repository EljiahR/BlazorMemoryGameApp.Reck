using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlazorMemoryGameApp.Shared.Models;

public class Card
{
	public int Id { get; set; }
	public string? Content { get; set; }
	public bool Selected { get; set; } = false;
	public bool Matched { get; set; } = false;

	public bool NoPair { get; set; } = false;
}

public static class SystemExtension
{
	public static T Clone<T>(this T source)
	{
		var serialized = JsonConvert.SerializeObject(source);
		return JsonConvert.DeserializeObject<T>(serialized)!;
	}
}
