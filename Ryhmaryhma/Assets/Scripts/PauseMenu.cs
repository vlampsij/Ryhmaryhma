using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        bool isPaused = !pauseMenu.activeSelf;

        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }
    [SerializeField] GameObject pauseMenu;

    public void Home()
    {
        SceneManager.LoadScene("Aloitus");
    }

    public void Resume()
    {
        TogglePauseMenu();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
