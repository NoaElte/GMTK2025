using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer audioMixer;
    public AudioSource soundtrack;
    public AudioSource blackholeSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //cannot stop the game because of music
        //Time.timeScale = 1f;
        GameIsPaused = false;
        soundtrack.UnPause();
        blackholeSound.UnPause();


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        //cannot stop the game because of music
        //Time.timeScale = 0f;
        GameIsPaused = true;
        soundtrack.Pause();
        blackholeSound.Pause();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void SetVolum(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
