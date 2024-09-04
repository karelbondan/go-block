using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuController : MonoBehaviour
{
    private bool isPaused = false;

    // Reference to the Pause Panel
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject upperUI;
    [SerializeField] GameObject timeRemainingUI;
    [SerializeField] GameObject lowerUI;

    Vector2 upperUIOrigPos, lowerUIOrigPos, timeRemainingOrigPos;

    void Start()
    {
        upperUIOrigPos = upperUI.transform.localPosition;
        lowerUIOrigPos = lowerUI.transform.localPosition;
        timeRemainingOrigPos = timeRemainingUI.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Detect 'Esc' key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TransitionScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        //isPaused = PausePanel.gameObject.activeSelf;

        if (isPaused)
        {
            // enable cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Pause the game
            camera.GetComponent<MoveCamera>().enabled = false;
            player.GetComponent<MovePlayer>().enabled = false;
            upperUI.LeanMoveLocalY(Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);
            timeRemainingUI.LeanMoveLocalY(Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);
            lowerUI.LeanMoveLocalY(-Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);
            PausePanel.SetActive(true); // Show the pause panel
            Time.timeScale = 0f;
        }
        else
        {
            // disable cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Resume the game
            camera.GetComponent<MoveCamera>().enabled = true;
            player.GetComponent<MovePlayer>().enabled = true;
            upperUI.LeanMoveLocalY(upperUIOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
            timeRemainingUI.LeanMoveLocalY(timeRemainingOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
            lowerUI.LeanMoveLocalY(lowerUIOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
            PausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
