using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Options screen
    /// </summary>
    public class OptionsScreen : MonoBehaviour
    {
        /// <summary>
        /// CPU smartness slider
        /// </summary>
        [SerializeField]
        private Slider cpuSmartnessSlider;

        /// <summary>
        /// CPU smartness value text
        /// </summary>
        [SerializeField]
        private TMP_Text cpuSmartnessValue;
        
        /// <summary>
        /// Back button
        /// </summary>
        [SerializeField]
        private Button backButton;
        
        private void Start()
        {
            //go back to main menu when back button clicked
            backButton.OnClickAsObservable().Subscribe(_ => MainMenu.GoBackToMainMenu());

            //Set slider to value in player prefs, or 50 if first time menu is opened
            cpuSmartnessSlider.value = PlayerPrefs.GetInt("CPU Smartness", 50);
            
            //Save CPU smartness
            cpuSmartnessSlider.OnValueChangedAsObservable().Subscribe(value =>
            {
                cpuSmartnessValue.text = value.ToString(CultureInfo.InvariantCulture);
                PlayerPrefs.SetInt("CPU Smartness", (int) value);
            });
        }
    }
}