using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class OptionsScreen : MonoBehaviour
    {
        [SerializeField]
        private Slider cpuSmartnessSlider;

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