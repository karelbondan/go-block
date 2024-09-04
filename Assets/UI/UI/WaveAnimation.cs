using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup waveIndicatorContainer;
    [SerializeField] GameObject waveIndicator;
    MenuController menuController;

    void Awake()
    {
        menuController = GameObject.Find("Game Manager").GetComponent<MenuController>();
    }

    void OnEnable()
    {
        menuController.enabled = false;

        waveIndicatorContainer.alpha = 0f;
        waveIndicatorContainer.LeanAlpha(1f, 0.3f).setIgnoreTimeScale(true);

        waveIndicator.transform.localPosition = new Vector2(0, -30f);
        waveIndicator.LeanMoveLocalY(0f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        menuController.enabled = true;

        waveIndicatorContainer.alpha = 1f;
        waveIndicatorContainer.LeanAlpha(0f, 0.3f).setIgnoreTimeScale(true);

        waveIndicator.LeanMoveLocalY(30f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(onComplete);
    }

    void onComplete()
    {
        gameObject.SetActive(false);
    }
}
