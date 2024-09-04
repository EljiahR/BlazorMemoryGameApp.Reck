using BlazorMemoryGameApp.Shared.Models;
using System.Net.Http.Json;
using System.Timers;

namespace BlazorMemoryGameApp.Client.Pages
{
	public partial class MemoryGame
	{
		private static Card card1 = new Card { Id = 1, Content = "B" };
		private static Card card2 = new Card { Id = 2, Content = "P" };
		private static Card card3 = new Card { Id = 3, Content = "D" };
		private static Card card4 = new Card { Id = 4, Content = "F" };
		private static Card card5 = new Card { Id = 5, Content = "C", Matched = true, NoPair = true};
		private static Card card6 = card1.Clone();
		private static Card card7 = card2.Clone();
		private static Card card8 = card3.Clone();
		private static Card card9 = card4.Clone();

		private static List<Card> cards = new List<Card> {
			card1, card2, card3, card4, card5, card6, card7, card8, card9
		};
		Card newishcard = cards[1].Clone();
		private enum GameState
		{
			Menu,
			Play,
			Result
		}

		private string? currentPlayer;

		private GameState gameState = GameState.Menu;

		private static Random random = new();

		private Card? previousCard;
		private bool IsCheckingCard = false;

		private bool WonGame = false;

		private int secondsRun = 0;
		private System.Timers.Timer timer = null!;
		private string Time = "00:00";

		private List<Games> scoreboard = new();
		private bool scoreboardIsLoading = true;
		private Games lastGamePlayed = new();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{

			if (firstRender)
			{
				await GetScoresAsync();
				StateHasChanged();
			}
		}

		private async Task SelectCardAsync(Card selectedCard)
		{
			if ((selectedCard.Matched && !selectedCard.NoPair) || selectedCard == previousCard || IsCheckingCard) return;

			selectedCard.Selected = true;

			if (previousCard != null && previousCard.Id == selectedCard.Id)
			{
				selectedCard.Matched = true;
				previousCard.Matched = true;
				previousCard = null;
				await CheckGameStatusAsync();
			}
			else if (previousCard != null && previousCard.Id != selectedCard.Id)
			{
				IsCheckingCard = true;
				await Task.Delay(1000);
				selectedCard.Selected = false;
				previousCard.Selected = false;
				previousCard = null;
				IsCheckingCard = false;
			}
			else
			{
				previousCard = selectedCard;
			}


		}

		private async Task CheckGameStatusAsync()
		{
			if (cards.All(card => card.Matched))
			{
				await EndGameAsync();
			}
		}

		private async Task EndGameAsync()
		{
			StopTimer();
			try
			{
				lastGamePlayed = await CreateGameLogAsync();

			}
			catch (Exception ex)
			{
				// 
				Console.WriteLine(ex.Message);
			}
			ChangeGameState(GameState.Result);
		}

		private void ChangeGameState(GameState newGameState)
		{
			gameState = newGameState;
		}

		private async Task GoToMenu()
		{
			ChangeGameState(GameState.Menu);
			await GetScoresAsync();
			StateHasChanged();
		}

		private void StartGame()
		{
			ReshuffleCards();
			ChangeGameState(GameState.Play);
			StartTimer();
		}

		private void ReshuffleCards()
		{
			foreach (var card in cards)
			{
				card.Selected = false;
				card.Matched = card.NoPair;
			}

			cards = cards.OrderBy(x => random.Next()).ToList();
		}

		private void StartTimer()
		{
			secondsRun = 0;
			Time = "00:00";
			timer = new System.Timers.Timer(1000);
			timer.Elapsed += OnTimedEvent;
			timer.Start();

		}

		private void StopTimer()
		{
			timer.Stop();
			timer.Dispose();
		}

		private async void OnTimedEvent(object? source, ElapsedEventArgs e)
		{
			secondsRun++;
			await InvokeAsync(() =>
			{
				Time = TimeSpan.FromSeconds(secondsRun).ToString(@"mm\:ss");
				StateHasChanged();
			});

		}

		private async Task<Games> CreateGameLogAsync(string gameType = "regular", string difficulty = "easy", int finishedRounds = 1)
		{
			TimeSpan duration = TimeSpan.ParseExact(Time, @"mm\:ss", null);
			Games newGameLog = new Games
			{
				PlayerName = string.IsNullOrEmpty(currentPlayer) ? "Anon" : currentPlayer,
				Duration = duration,
				DatePlayed = DateTime.Today,
				GameType = gameType,
				Difficulty = difficulty,
				FinishedRounds = finishedRounds
			};
			var result = await Http.PostAsJsonAsync<Games>("api/Games", newGameLog);
			return await result.Content.ReadFromJsonAsync<Games>() ?? new Games();
		}

		private async Task GetScoresAsync()
		{
			scoreboardIsLoading = true;
			scoreboard = await Http.GetFromJsonAsync<List<Games>>("api/Games") ?? new();
			scoreboardIsLoading = false;
		}
	}
}
