using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Image fadeBoard;
    [SerializeField] private Text TheEnd_text;
    [SerializeField] private Text Skip_text;
    [SerializeField] private AudioSource audio_s;
    [SerializeField] private RectTransform EndCredits;
    [SerializeField] private RectTransform thankYou;

    private void Awake()
    {
        Time.timeScale = 1;

        fadeBoard.color = Color.black;
        fadeBoard.gameObject.SetActive(true);

        GetComponent<AudioSource>();

        thankYou.gameObject.SetActive(false);
        Skip_text.gameObject.SetActive(false);
        EndCredits.localPosition = new Vector3(0, -650, 0);
    }

    private IEnumerator Start()
    {
        fadeBoard.DOFade(0, 3f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            fadeBoard.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(3f);

        TheEnd_text.DOFade(0, 3f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            TheEnd_text.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(4f);

        audio_s.Play();

        Skip_text.transform.localScale = Vector3.zero;
        Skip_text.gameObject.SetActive(true);
        Skip_text.transform.DOScale(1, 1f).SetEase(Ease.OutBack);

        Vector3 start = EndCredits.localPosition;
        Vector3 end = new Vector3(0, 6900, 0);

        while (audio_s.isPlaying)
        {
            EndCredits.localPosition = Vector3.Lerp(start, end, audio_s.time / audio_s.clip.length);

            yield return null;

            if (!fadeBoard.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
            {
                Skip_text.transform.DOScale(0, 1f).SetEase(Ease.OutBack);

                audio_s.DOFade(0, 1f).OnComplete(() => audio_s.Stop());

                fadeBoard.color = new Color(0, 0, 0, 0);
                fadeBoard.gameObject.SetActive(true);
                fadeBoard.DOFade(1, 1f).SetEase(Ease.InQuart).OnComplete(() => SceneManager.LoadScene(0));
            }
        }

        if (!fadeBoard.gameObject.activeSelf)
        {
            Skip_text.transform.DOScale(0, 0.8f).SetEase(Ease.OutBack);

            fadeBoard.color = new Color(0, 0, 0, 0);
            fadeBoard.gameObject.SetActive(true);
            fadeBoard.DOFade(1, 1f).SetEase(Ease.InQuart);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(0);
        }
    }
}
