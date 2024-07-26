using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Text TheEnd_text;
    [SerializeField] private Text Skip_text;
    [SerializeField] private AudioSource audio_s;
    [SerializeField] private GameObject EndCredits;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Animator credit_ani;
    private bool isCreditsComplete;


    private void Awake()
    {
        Time.timeScale = 1;
        TryGetComponent(out audio_s);
        credit_ani = GetComponentInChildren<Animator>();
        isCreditsComplete = false;
        Skip_text.gameObject.SetActive(false);
    }

    private IEnumerator Start()
    {
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            TheEnd_text.color = new Color(1, 1, 1, i);
            yield return null;
        }

        TheEnd_text.text = "The End.";
        yield return new WaitForSeconds(0.5f);
        TheEnd_text.text = "The End..";
        yield return new WaitForSeconds(0.5f);
        TheEnd_text.text = "The End...";
        yield return new WaitForSeconds(0.5f);
        TheEnd_text.text = "The End...?";
        audio_s.Play();
        yield return new WaitForSeconds(3f);

        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            TheEnd_text.color = new Color(0, 0, 0, i);
            yield return null;
        }
        TheEnd_text.gameObject.SetActive(false);

        Skip_text.gameObject.SetActive(true);
        for (float i = 0; i <= 3; i += Time.deltaTime)
        {
            Skip_text.color = new Color(1, 1, 1, i);
            yield return null;
        }

        credit_ani.SetTrigger("EndRoll");
        while (audio_s.isPlaying)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Skip Ending");
                isCreditsComplete = true;
                break;
            }
        }
        isCreditsComplete = true;

        if (isCreditsComplete)
        {
            Text[] endCredit_text = GetComponentsInChildren<Text>();
            for (float i = 2; i >= 0; i -= Time.deltaTime)
            {
                Skip_text.color = new Color(0, 0, 0, i);
                endCredit_text[0].color = new Color(0, 0, 0, i);
                endCredit_text[1].color = new Color(0, 0, 0, i);
                yield return null;
            }
            SceneManager.LoadScene(0);
        }
    }
}
