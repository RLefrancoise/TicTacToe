using System;
using UniRx;
using UnityEngine;

namespace TicTacToe
{
    [RequireComponent(typeof(BoxCollider))]
    public class GridSlot : MonoBehaviour
    {
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
        
        private void SetSymbol(Symbol symbol)
        {
            if (symbol == null) return;
            
            symbol.AttachToSlot(this);
        }
        
        private void OnMouseDown()
        {
            Debug.LogFormat("Slot {0} is clicked", name);

            if (!IsFree)
            {
                Debug.LogFormat("Slot {0} is not free, ignore click", name);
                return;
            }

            //Execute command
            if(!PlaceSymbol.Execute())
                Debug.LogErrorFormat("Failed to execute PlaceSymbol command on slot {0}", name);
        }
    }
}