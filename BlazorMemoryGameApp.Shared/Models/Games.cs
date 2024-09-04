using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMemoryGameApp.Shared.Models;

public class Games
{
	public int Id { get; set; }
	[Required]
	public string? PlayerName { get; set; }
	[Required]
	public DateTime DatePlayed { get; set; }
	[Required]
	public TimeSpan Duration { get; set; }
	public string? GameType { get; set; }
	public string? Difficulty { get; set; }
	public int? FinishedRounds { get; set; }
}
