using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup container;
    [SerializeField] GameObject dialog;
    [SerializeField] GameObject upperUI;
    [SerializeField] GameObject lowerUI;
    [SerializeField] GameObject timeRemainingUI;
    [SerializeField] GameObject camera;
    MenuController menuController;

    Vector2 upperUIOrigPos, lowerUIOrigPos, timeRemainingOrigPos;


    void Awake()
    {
        menuController = GameObject.Find("Game Manager").GetComponent<MenuController>();
        upperUIOrigPos = upperUI.transform.localPosition;
        lowerUIOrigPos = lowerUI.transform.localPosition;
        timeRemainingOrigPos = timeRemainingUI.transform.localPosition;
    }

    void OnEnable()
    {
        menuController.enabled = false;
        
        // enable cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camera.GetComponent<MoveCamera>().enabled = false;

        upperUI.LeanMoveLocalY(Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);
        timeRemainingUI.LeanMoveLocalY(Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);
        lowerUI.LeanMoveLocalY(-Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);

        container.alpha = 0f;
        container.LeanAlpha(1f, 0.3f).setIgnoreTimeScale(true);

        dialog.transform.localPosition = new Vector2(0, -30f);
        dialog.LeanMoveLocalY(0f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    public void Hide()
    {

        menuController.enabled = true;

        upperUI.LeanMoveLocalY(upperUIOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
        timeRemainingUI.LeanMoveLocalY(timeRemainingOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
        lowerUI.LeanMoveLocalY(lowerUIOrigPos.y, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);

        container.alpha = 1f;
        container.LeanAlpha(0f, 0.3f).setIgnoreTimeScale(true);

        dialog.LeanMoveLocalY(30f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(onComplete);
    }

    void onComplete()
    {
        gameObject.SetActive(false);
    }
}
