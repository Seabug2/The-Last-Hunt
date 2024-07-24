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
    [SerializeField] private Text skip;
    private bool isNarrationComplete;

    private void Awake()
    {
        Time.timeScale = 1;
        illustration = GetComponentInChildren<Image>();
        TryGetComponent(out audio_s);
        isNarrationComplete = false;
    }

    private IEnumerator Start()
    {
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            illustration.color = new Color(1, 1, 1, i);
            subtitle.color = new Color(0, 0, 0, i);
            skip.color = new Color(0, 0, 0, i);
            yield return null;
        }

        for (int i = 0; i < narrationClips.Length; i++)
        {
            audio_s.PlayOneShot(narrationClips[i]);
            subtitle.text = subtitleClips[i];
            while (audio_s.isPlaying)
            {
                yield return null;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Skip");
                    isNarrationComplete = true;
                    break;
                }
            }
            if (isNarrationComplete)
            {
                break;
            }
        }
        isNarrationComplete = true;

        if (isNarrationComplete)
        {
            for (float i = 3; i >= 0; i -= Time.deltaTime)
            {
                illustration.color = new Color(1, 1, 1, i);
                subtitle.color = new Color(0, 0, 0, i);
                skip.color = new Color(0, 0, 0, i);
                yield return null;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
