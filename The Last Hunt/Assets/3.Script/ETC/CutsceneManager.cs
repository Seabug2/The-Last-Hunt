using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image illustration;
    [SerializeField] private AudioSource audio_s;
    [SerializeField] private AudioClip[] narrationClips;
    [SerializeField] private Text subtitle;
    [SerializeField] private string[] subtitleClips;
    private bool isNarrationComplete;

    private void Awake()
    {
        Time.timeScale = 1;
        illustration = GetComponentInChildren<Image>();
        TryGetComponent(out audio_s);
        isNarrationComplete = false;
    }


    private void Start()
    {
        StartCoroutine(FadeImage_co(true));
        StartCoroutine(AudioSubtitle_co());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Skip");
            StopCoroutine(AudioSubtitle_co());
            isNarrationComplete = true;
        }
        if (isNarrationComplete)
        {
            StartCoroutine(FadeImage_co(false));
            StartCoroutine(LoadNextScene_co());
        }
    }

    private IEnumerator FadeImage_co(bool fadeIn)
    {
        if (fadeIn)
        {
            for (float i = 0; i <= 3; i += Time.deltaTime)
            {
                illustration.color = new Color(1, 1, 1, i);
                subtitle.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 3; i >= 0; i -= Time.deltaTime)
            {
                illustration.color = new Color(1, 1, 1, i);
                subtitle.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }

    private IEnumerator AudioSubtitle_co()
    {
        yield return null;
        for (int i = 0; i < narrationClips.Length; i++)
        {
            audio_s.PlayOneShot(narrationClips[i]);
            subtitle.text = subtitleClips[i];
            while (audio_s.isPlaying)
            {
                yield return null;
            }
        }
        isNarrationComplete = true;
    }
    
    private IEnumerator LoadNextScene_co()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
