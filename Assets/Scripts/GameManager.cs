using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Random = System.Random;

namespace TicTacToe
{
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
		
		#endregion

		#region Unity Related

		private void Awake()
		{
			CurrentPlayer = new ReactiveProperty<PlayerType>();
			IsGameStarted = new BoolReactiveProperty();
			IsGameOver = new BoolReactiveProperty();
		}

		private void Start ()
		{
			//pseudo panel
			var pseudoPanelInstance = Instantiate(pseudoPanelPrefab);
			PseudoPanel = pseudoPanelInstance.GetComponent<PseudoPanel>();

			//Listen pseudo validation
			PseudoPanel.ValidatePseudo.Subscribe(ListenPseudoGiven);
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
			
			//listen slot clicks
			_grid.Slots.ForEach(slot => slot.PlaceSymbol.Subscribe(_ => PlaceSymbolOnSlot(slot)));
			
			//start game
			StartCoroutine(StartGame());
		}

		private IEnumerator StartGame()
		{
			IsGameStarted.Value = true;
			
			//Choose a player randomly
			var random = new Random(DateTime.Now.Millisecond);
			CurrentPlayer.Value = random.Next(0, 1) == 0 ? PlayerType.Human : PlayerType.Cpu;

			
			
			yield return null;
		}
		
		/// <summary>
		/// Place a symbol on a slot
		/// </summary>
		/// <param name="slot"></param>
		private void PlaceSymbolOnSlot(GridSlot slot)
		{
			switch (CurrentPlayer.Value)
			{
				case PlayerType.Human:
					slot.Symbol.Value = Instantiate(circlePrefab).GetComponent<Symbol>();
					break;
				case PlayerType.Cpu:
					slot.Symbol.Value = Instantiate(crossPrefab).GetComponent<Symbol>();
					break;
			}

			//if game over, stop the game, else go to next turn
			if (CheckGameOver()) IsGameOver.Value = true;
			else NextTurn();
		}

		private void NextTurn()
		{
			//Change current player
			CurrentPlayer.Value = CurrentPlayer.Value == PlayerType.Human ? PlayerType.Cpu : PlayerType.Human;

			//If Cpu turn
			if (CurrentPlayer.Value == PlayerType.Cpu)
			{
				
			}
		}
		
		/// <summary>
		/// Check if game is over
		/// </summary>
		/// <returns></returns>
		private bool CheckGameOver()
		{
			return _grid.rows.Any(TicTacToeGrid.AreSameSymbol) || 
			       _grid.cols.Any(TicTacToeGrid.AreSameSymbol) || 
			       _grid.diagonals.Any(TicTacToeGrid.AreSameSymbol);
		}
		
		#endregion
	}
}