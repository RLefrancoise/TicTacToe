using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TicTacToe
{
    public class TicTacToeGrid : MonoBehaviour
    {
        [SerializeField]
        private List<GridSlot> slots;

        private void Start()
        {
            slots.ForEach(slot => slot.PlaceSymbol.Subscribe(_ => PlaceSymbolOnSlot(slot)));
        }

        private void PlaceSymbolOnSlot(GridSlot slot)
        {
            
            
        }

        private bool AreSameSymbol(IEnumerable<GridSlot> slots)
        {
            return false;
        }
    }
}