using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
    [SerializeField] Transform dialog;
    [SerializeField] CanvasGroup container;
    GameObject gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager");
    }

    void OnEnable()
    {
        gameManager.GetComponent<MenuController>().enabled = false;

        container.alpha = 0f;
        container.LeanAlpha(1f, 0.3f).setIgnoreTimeScale(true);

        dialog.localPosition = new Vector2(0, -30f);
        dialog.LeanMoveLocalY(0f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(onComplete);
    }

    void OnDisable()
    {
        dialog.localPosition = new Vector2(0, -30f);
    }

    void onComplete()
    {
        gameManager.GetComponent<MenuController>().enabled = true;
    }
}
