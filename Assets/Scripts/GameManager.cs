using System;
using System.Linq;
using TicTacToe.IO;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace TicTacToe
{
	/// <inheritdoc />
	/// <summary>
	/// Game manager
	/// </summary>
	public class GameManager : UnitySingleton<GameManager>
	{
		#region Fields

		/// <summary>
		/// Pseudo panel prefab
		/// </summary>
		[SerializeField]
		private GameObject pseudoPanelPrefab;
		
		/// <summary>
		/// HUD prefab
		/// </summary>
		[SerializeField]
		private GameObject hudPrefab;

		/// <summary>
		/// Cross prefab
		/// </summary>
		[SerializeField]
		private GameObject crossPrefab;
		
		/// <summary>
		/// Circle prefab
		/// </summary>
		[SerializeField]
		private GameObject circlePrefab;

		/// <summary>
		/// Grid prefab
		/// </summary>
		[SerializeField]
		private GameObject gridPrefab;
		
		/// <summary>
		/// tic tac toe grid
		/// </summary>
		private TicTacToeGrid _grid;

		/// <summary>
		/// Cpu
		/// </summary>
		private Cpu _cpu;
				
		#endregion
		
		#region Properties

		/// <summary>
		/// Pseudo panel instance
		/// </summary>
		public PseudoPanel PseudoPanel { get; private set; }
		
		/// <summary>
		/// Current player
		/// </summary>
		public ReactiveProperty<PlayerType> CurrentPlayer { get; private set; }
		
		/// <summary>
		/// Is game started ?
		/// </summary>
		public BoolReactiveProperty IsGameStarted { get; private set; }

		/// <summary>
		/// Is game over ?
		/// </summary>
		public BoolReactiveProperty IsGameOver { get; private set; }

		/// <summary>
		/// Player type wins the game
		/// </summary>
		public ReactiveCommand<WinnerType> WinTheGame;
		
		#endregion

		#region Unity Related

		private void Awake()
		{
			CurrentPlayer = new ReactiveProperty<PlayerType>();
			IsGameStarted = new BoolReactiveProperty();
			IsGameOver = new BoolReactiveProperty();
			
			_cpu = new Cpu();

			WinTheGame = IsGameOver.Select(_ => IsGameOver.Value).ToReactiveCommand<WinnerType>();
		}

		private void Start ()
		{
			//pseudo panel
			var pseudoPanelInstance = Instantiate(pseudoPanelPrefab);
			PseudoPanel = pseudoPanelInstance.GetComponent<PseudoPanel>();

			//Listen pseudo validation
			PseudoPanel.ValidatePseudo.Subscribe(ListenPseudoGiven);
		}

		private void OnDestroy()
		{
			if (IsGameOver.Value) return;
			
			//save current game state if game is not over
			var snapshot = new GameSnapshot
			{
				currentPlayer = CurrentPlayer.Value.ToString(),
				playerName = PseudoPanel.Pseudo.Value,
				grid = string.Join(",", _grid.Slots.Select(s => s.Symbol.Value != null 
					? s.Symbol.Value.Type.ToString() 
					: "Empty"))
			};

			StreamingAssetsHelper.WriteToJson(snapshot, "snapshot");
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Listen when player has given his pseudo
		/// </summary>
		/// <param name="pseudo">pseudo given by the player</param>
		private void ListenPseudoGiven(string pseudo)
		{
			//HUD
			Instantiate(hudPrefab);

			//grid
			_grid = Instantiate(gridPrefab).GetComponent<TicTacToeGrid>();
			
			//start game
			StartGame();
		}

		/// <summary>
		/// Start the game
		/// </summary>
		private void StartGame()
		{
			IsGameStarted.Value = true;

			//Listen current player change to play turns
			CurrentPlayer.SkipLatestValueOnSubscribe().Subscribe(PlayTurn);
			
			//listen slot clicks
			_grid.Slots.ForEach(slot => slot.PlaceSymbol.Subscribe(_ => PlaceSymbolOnSlot(slot)));
			
			//Choose a player randomly
			var random = new Random(DateTime.Now.Millisecond);
			CurrentPlayer.Value = random.Next(2) == 0 ? PlayerType.Human : PlayerType.Cpu;
		}
		
		/// <summary>
		/// Place a symbol on a slot
		/// </summary>
		/// <param name="slot"></param>
		private void PlaceSymbolOnSlot(GridSlot slot)
		{
			//If game is over, stop playing game
			if (IsGameOver.Value) return;
			
			switch (CurrentPlayer.Value)
			{
				case PlayerType.Human:
					slot.Symbol.Value = Instantiate(circlePrefab).GetComponent<Symbol>();
					break;
				case PlayerType.Cpu:
					slot.Symbol.Value = Instantiate(crossPrefab).GetComponent<Symbol>();
					break;
			}
			
			if(_grid.IsFull.Value && !CheckGameOver()) EndTheGame(true);
			else NextTurn();
		}

		/// <summary>
		/// Go to next turn
		/// </summary>
		private void NextTurn()
		{
			//if game over, stop the game, else go to next turn
			if (CheckGameOver())
			{
				EndTheGame(false);
			}
			else
			{
				//Change current player
				CurrentPlayer.Value = CurrentPlayer.Value == PlayerType.Human ? PlayerType.Cpu : PlayerType.Human;
			}
		}

		/// <summary>
		/// Play the current turn
		/// </summary>
		/// <param name="playerType"></param>
		private void PlayTurn(PlayerType playerType)
		{	
			//If Cpu turn
			if (CurrentPlayer.Value == PlayerType.Cpu)
			{
				_cpu.PlayTurn(_grid);
			}
			
			//if human, waiting for slot click
		}
		
		/// <summary>
		/// Check if game is over
		/// </summary>
		/// <returns></returns>
		private bool CheckGameOver()
		{
			return _grid.Rows.Any(TicTacToeGrid.AreSameSymbol) || 
			       _grid.Cols.Any(TicTacToeGrid.AreSameSymbol) || 
			       _grid.Diagonals.Any(TicTacToeGrid.AreSameSymbol);
		}

		/// <summary>
		/// End the game
		/// </summary>
		private void EndTheGame(bool isDraw)
		{
			AddToHistory();
			
			IsGameOver.Value = true;
			WinTheGame.Execute(isDraw 
				? WinnerType.Draw 
				: (CurrentPlayer.Value == PlayerType.Human 
					? WinnerType.Human 
					: WinnerType.Cpu));

			//We go back to main menu after 3 seconds
			Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(time => GoBackToMainMenu());
		}

		/// <summary>
		/// Add this play to history
		/// </summary>
		private void AddToHistory()
		{
			var history = StreamingAssetsHelper.GetJsonContent<GameHistory>("history");
			
			history.plays.Add(new GameData
			{
				date = DateTime.Now.ToString("g"),
				playerName = PseudoPanel.Pseudo.Value,
				winner = CheckGameOver() ? CurrentPlayer.Value.ToString() : WinnerType.Draw.ToString()
			});
			
			StreamingAssetsHelper.WriteToJson(history, "history");
		}
		
		/// <summary>
		/// Go back to main menu
		/// </summary>
		private static void GoBackToMainMenu()
		{
			SceneManager.LoadSceneAsync("Scenes/MainMenu");
		}
		
		#endregion
	}
}