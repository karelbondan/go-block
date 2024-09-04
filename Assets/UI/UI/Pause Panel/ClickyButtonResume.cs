using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickyButtonResume : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject PausePanel;
    [SerializeField] AudioSource UISound;
    GameObject gameManager;

    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager");
    }

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

    public void ResumeGame()
    {
        StartCoroutine(Resume());
    }

    public void Retry()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Resume()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.3f);
        gameManager.GetComponent<MenuController>().TogglePause();

    }
}
