using UniRx;
using UnityEngine;

namespace TicTacToe
{
    public class GridSlot : MonoBehaviour
    {
        /// <summary>
        /// Symbol attached to this slot
        /// </summary>
        public ReactiveProperty<Symbol> Symbol { get; private set; }

        /// <summary>
        /// Is slot free ?
        /// </summary>
        private bool IsFree => !Symbol.Value;

        /// <summary>
        /// Place symbol command
        /// </summary>
        public ReactiveCommand PlaceSymbol;
        
        private void Awake()
        {
            Symbol = new ReactiveProperty<Symbol>();
            
            //Place symbol can execute only if no symbol in the slot
            PlaceSymbol = Symbol.Select(s => s == null).ToReactiveCommand();
        }

        private void OnMouseDown()
        {
            Debug.LogFormat("Slot {0} is clicked", name);

            if (!IsFree)
            {
                Debug.LogFormat("Slot {0} is not free, ignore click", name);
            }

            //Execute command
            if(!PlaceSymbol.Execute())
                Debug.LogErrorFormat("Failed to execute PlaceSymbol command on slot {0}", name);
        }
    }
}