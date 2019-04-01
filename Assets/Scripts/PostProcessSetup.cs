using UnityEngine;
using UnityEngine.PostProcessing;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Post processing setup
    /// </summary>
    public class PostProcessSetup : MonoBehaviour
    {
        /// <summary>
        /// post process behaviour
        /// </summary>
        [SerializeField]
        private PostProcessingBehaviour postProcess;

        /// <summary>
        /// PC profile
        /// </summary>
        [SerializeField]
        private PostProcessingProfile pcProfile;
		
        /// <summary>
        /// Android profile
        /// </summary>
        [SerializeField]
        private PostProcessingProfile androidProfile;
        
        private void Start()
        {
            //Choose right post process profile
            postProcess.profile = Application.platform == RuntimePlatform.Android ? androidProfile : pcProfile;
        }
    }
}