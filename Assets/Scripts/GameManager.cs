using System;
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
		/// <summary>
		/// Pseudo panel prefab
		/// </summary>
		[SerializeField]
		private GameObject pseudoPanelPrefab;
		
		[SerializeField]
		private GameObject hudPrefab;
		
		/// <summary>
		/// Pseudo panel instance
		/// </summary>
		public PseudoPanel PseudoPanel { get; private set; }

		private Hud _hud;
		
		public ReactiveProperty<PlayerType> CurrentPlayer { get; private set; }

		private void Awake()
		{
			CurrentPlayer = new ReactiveProperty<PlayerType>();
		}

		private void Start ()
		{
			//pseudo panel
			var pseudoPanelInstance = Instantiate(pseudoPanelPrefab);
			PseudoPanel = pseudoPanelInstance.GetComponent<PseudoPanel>();

			//Listen pseudo validation
			PseudoPanel.ValidatePseudo.Subscribe(ListenPseudoGiven);
		}

		/// <summary>
		/// Listen when player has given his pseudo
		/// </summary>
		/// <param name="pseudo">pseudo given by the player</param>
		private void ListenPseudoGiven(string pseudo)
		{
			//HUD
			_hud = Instantiate(hudPrefab).GetComponent<Hud>();
		}

		private void BeginGame()
		{
			//Choose a player randomly
			var random = new Random(DateTime.Now.Millisecond);
			CurrentPlayer.Value = random.Next(0, 1) == 0 ? PlayerType.Human : PlayerType.Cpu;
		}
	}
}