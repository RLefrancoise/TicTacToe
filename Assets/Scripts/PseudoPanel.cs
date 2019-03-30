using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Pseudo panel. Ask a pseudo to the player.
    /// </summary>
    public class PseudoPanel : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// pseudo input field
        /// </summary>
        [SerializeField]
        private TMP_InputField pseudoInput;

        /// <summary>
        /// button to validate input
        /// </summary>
        [SerializeField]
        private Button validateButton;

        #endregion
       
        #region Properties

        /// <summary>
        /// Pseudo of the player
        /// </summary>
        public StringReactiveProperty Pseudo { get; private set; }

        /// <summary>
        /// Validate pseudo command
        /// </summary>
        public ReactiveCommand<string> ValidatePseudo { get; private set; }

        #endregion

        #region Unity related

        private void Awake()
        {
            Pseudo = new StringReactiveProperty();

            pseudoInput.onValueChanged.AsObservable().Subscribe(pseudo => Pseudo.Value = pseudo);
            
            //Create validate pseudo command
            ValidatePseudo = Pseudo.Select(pseudo => !string.IsNullOrEmpty(pseudoInput.text)).ToReactiveCommand<string>();
            
            //Validate pseudo hides the panel
            ValidatePseudo.Subscribe(OnValidatePseudo);
        }

        private void Start()
        {
            //Trigger command when button is clicked
            validateButton.OnClickAsObservable().Subscribe(_ => ValidatePseudo.Execute(Pseudo.Value));
        }

        #endregion

        #region Private methods

        private void OnValidatePseudo(string pseudo)
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}