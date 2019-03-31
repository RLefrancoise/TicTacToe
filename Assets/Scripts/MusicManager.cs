using UnityEngine;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Music manager
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : UnitySingleton<MusicManager>
    {
        /// <summary>
        /// Audio source to play the music
        /// </summary>
        [SerializeField]
        private AudioSource audioSource;
        /// <summary>
        /// Main menu music
        /// </summary>
        [SerializeField]
        private AudioClip mainMenu;
        /// <summary>
        /// Game music
        /// </summary>
        [SerializeField]
        private AudioClip game;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Play game music
        /// </summary>
        public void PlayGameMusic()
        {
            if (audioSource.clip == game && audioSource.isPlaying) return;
            
            audioSource.clip = game;
            audioSource.Play();
        }

        /// <summary>
        /// Play main menu music
        /// </summary>
        public void PlayMainMenuMusic()
        {
            if (audioSource.clip == mainMenu && audioSource.isPlaying) return;
            
            audioSource.clip = mainMenu;
            audioSource.Play();
        }
    }
}