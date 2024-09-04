using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickyButtonMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        _source.PlayOneShot(_uncompressClip);
    }

    public void ChangeScene()
    {
        StartCoroutine(MainMenu());
    }

    private IEnumerator MainMenu()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("MainMenu");
    }
}
