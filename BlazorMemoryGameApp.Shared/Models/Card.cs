using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMemoryGameApp.Shared.Models;

public class Card
{
	public int Id { get; set; }
	public string? Content { get; set; }
	public bool Selected { get; set; } = false;
	public bool Matched { get; set; } = false;
}
