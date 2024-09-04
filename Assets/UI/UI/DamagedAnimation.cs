using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup damagedAmbiance;
    LTDescr currentAnimation;

    // Start is called before the first frame update
    void OnEnable()
    {
        damagedAmbiance.alpha = 1f;
    }

    public void Hide()
    {
        damagedAmbiance.alpha = 1f;
        currentAnimation = damagedAmbiance.LeanAlpha(0f, 0.5f).setIgnoreTimeScale(true).setOnComplete(onComplete);
    }

    public void Activate()
    {
        if (currentAnimation != null)
        {
            currentAnimation.reset();
            damagedAmbiance.alpha = 1f;
            damagedAmbiance.LeanAlpha(1f, 0f).setIgnoreTimeScale(true);
        }
    }

    void onComplete()
    {
        gameObject.SetActive(false);
    }
}
