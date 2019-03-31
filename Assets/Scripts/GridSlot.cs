using UniRx;
using UnityEngine;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Slot of the tic tac toe grid.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class GridSlot : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip circle;
        [SerializeField]
        private AudioClip cross;
        
        /// <summary>
        /// Symbol attached to this slot
        /// </summary>
        public ReactiveProperty<Symbol> Symbol { get; private set; }

        /// <summary>
        /// Is slot free ?
        /// </summary>
        public bool IsFree => !Symbol.Value;

        /// <summary>
        /// Place symbol command
        /// </summary>
        public ReactiveCommand PlaceSymbol { get; private set; }
        
        private void Awake()
        {
            Symbol = new ReactiveProperty<Symbol>();
            Symbol.Subscribe(SetSymbol);   
            
            //Place symbol can execute only if no symbol in the slot & game is started
            PlaceSymbol = Symbol.Select(s => s == null).ToReactiveCommand();
        }
        
        /// <summary>
        /// Set the symbol on the slot
        /// </summary>
        /// <param name="symbol">symbol to set on the slot</param>
        private void SetSymbol(Symbol symbol)
        {
            if (symbol == null) return;
            
            symbol.AttachToSlot(this);

            audioSource.clip = symbol.Type == TicTacToe.Symbol.SymbolType.Cross ? cross : circle;
            audioSource.Play();
        }
        
        private void OnMouseDown()
        {
            Debug.LogFormat("Slot {0} is clicked", name);

            if (!IsFree)
            {
                Debug.LogFormat("Slot {0} is not free, ignore click", name);
                return;
            }

            //If current player is not human, ignore click
            if (GameManager.Instance.CurrentPlayer.Value != PlayerType.Human) return;
            
            //Execute command
            if(!PlaceSymbol.Execute())
                Debug.LogErrorFormat("Failed to execute PlaceSymbol command on slot {0}", name);
        }
    }
}