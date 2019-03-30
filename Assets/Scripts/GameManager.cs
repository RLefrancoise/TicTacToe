using System;
using System.Linq;
using TicTacToe.IA;
using TicTacToe.IO;
using UnityEngine;
using UniRx;
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
		
		#endregion
		
		#region Properties

		/// <summary>
		/// Pseudo of the player
		/// </summary>
		public string PlayerPseudo { get; private set; }
		
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

			WinTheGame = IsGameOver.Select(_ => IsGameOver.Value).ToReactiveCommand<WinnerType>();
		}

		private void Start ()
		{
			//if new game, ask for a pseudo
			if (BetweenSceneData.Mode == BetweenSceneData.GameMode.NewGame)
			{
				//pseudo panel
				var pseudoPanelInstance = Instantiate(pseudoPanelPrefab);
				var pseudoPanel = pseudoPanelInstance.GetComponent<PseudoPanel>();

				//Listen pseudo validation
				pseudoPanel.ValidatePseudo.Subscribe(AfterPseudoGiven);
			}
			//if continue game, set pseudo from snapshot
			else
			{
				AfterPseudoGiven(BetweenSceneData.SnapShot.playerName);
			}
		}

		private void OnApplicationQuit()
		{
			if (IsGameOver.Value) return;
			if (string.IsNullOrEmpty(PlayerPseudo)) return;
			
			//save current game state if game is not over
			var snapshot = new GameSnapshot
			{
				currentPlayer = CurrentPlayer.Value.ToString(),
				playerName = PlayerPseudo,
				grid = string.Join(",", _grid.Slots.Select(s => s.Symbol.Value != null 
					? s.Symbol.Value.Type.ToString() 
					: "Empty"))
			};

			StreamingAssetsHelper.WriteToJson(snapshot, "snapshot");
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// After pseudo has been given
		/// </summary>
		/// <param name="pseudo">pseudo given by the player</param>
		private void AfterPseudoGiven(string pseudo)
		{
			PlayerPseudo = pseudo;
			
			//HUD
			Instantiate(hudPrefab);

			//grid
			_grid = Instantiate(gridPrefab).GetComponent<TicTacToeGrid>();
			
			//listen slot clicks
			_grid.Slots.ForEach(slot => slot.PlaceSymbol.Subscribe(_ => PlaceSymbolOnSlot(slot)));
			
			//Listen current player change to play turns
			CurrentPlayer.SkipLatestValueOnSubscribe().Subscribe(PlayTurn);
			
			//new game
			if(BetweenSceneData.Mode == BetweenSceneData.GameMode.NewGame)
				StartGame();
			//continue game
			else
				ContinuePreviousGame();
		}

		/// <summary>
		/// Start the game
		/// </summary>
		private void StartGame()
		{
			IsGameStarted.Value = true;

			//Choose a player randomly
			var random = new Random(DateTime.Now.Millisecond);
			CurrentPlayer.Value = random.Next(2) == 0 ? PlayerType.Human : PlayerType.Cpu;
		}

		/// <summary>
		/// Continue previous game
		/// </summary>
		private void ContinuePreviousGame()
		{
			//init grid
			var snapshot = BetweenSceneData.SnapShot;
			var symbols = snapshot.grid.Split(',');
			
			if(symbols.Length != _grid.Slots.Count) throw new Exception("Grid snapshot has invalid slots count");

			for (var i = 0; i < symbols.Length; ++i)
			{
				//Ignore empty slot
				if (symbols[i] == "Empty") continue;
				
				Symbol.SymbolType symbolType;
				if(!Enum.TryParse(symbols[i], out symbolType))
					throw new Exception("Grid snapshot has invalid slot symbol");

				switch (symbolType)
				{
					case Symbol.SymbolType.Cross:
						_grid.Slots[i].Symbol.Value = Instantiate(crossPrefab).GetComponent<Symbol>();
						break;
					case Symbol.SymbolType.Circle:
						_grid.Slots[i].Symbol.Value = Instantiate(circlePrefab).GetComponent<Symbol>();
						break;
				}
			}
			
			IsGameStarted.Value = true;
			
			//Set current player
			PlayerType playerType;
			if(!Enum.TryParse(snapshot.currentPlayer, out playerType))
				throw new Exception("Snapshot has invalid current player type");

			CurrentPlayer.Value = playerType;
			
			//delete snapshot
			StreamingAssetsHelper.DeleteFile("snapshot");
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
				Cpu.PlayTurn(_grid);
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
			Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(time => MainMenu.GoBackToMainMenu());
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
				playerName = PlayerPseudo,
				winner = CheckGameOver() ? CurrentPlayer.Value.ToString() : WinnerType.Draw.ToString()
			});
			
			StreamingAssetsHelper.WriteToJson(history, "history");
		}
		
		#endregion
	}
}