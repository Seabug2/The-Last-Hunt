using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image illustration;
    [SerializeField] private AudioSource audio_s;
    [SerializeField] private Text subtitle;
    [SerializeField] private AudioClip[] narrationClips;
    [SerializeField] private string[] subtitleClips;
    [SerializeField] private GameObject skip;
    private bool isNarrationComplete;

    private void Awake()
    {
        Time.timeScale = 1;

        illustration.color = new Color(1, 1, 1, 0);

        subtitle.text = "";
        subtitle.color = new Color(1, 1, 1, 0);

        skip.SetActive(false);
    }

    //검은색 화면 위에 이미지의 알파 값을 0에서 1로 서서히 바꾼다?
    private IEnumerator Start()
    {
        isNarrationComplete = false;

        illustration.DOFade(1, 4f).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(4f);

        skip.SetActive(true);

        for (int i = 0; i < narrationClips.Length; i++)
        {
            audio_s.volume = 1;
            audio_s.PlayOneShot(narrationClips[i]);
            subtitle.color = new Color(1, 1, 1, 0);
            subtitle.text = subtitleClips[i];
            Tween subtitleFade = subtitle.DOFade(1, 2f);

            while (audio_s.isPlaying)
            {
                //스페이스바를 누르면 현재 나래이션을 중단하고 다음 나래이션을 재생
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    subtitleFade.Kill();
                    //오디오 소스 정지
                    audio_s.Stop();
                    //자막 지우기
                    subtitle.text = "";
                }
                //엔터 키를 누르면...
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    subtitleFade.Kill();
                    isNarrationComplete = true;
                    break;
                }
                yield return null;
            }

            if (isNarrationComplete)
            {
                break;
            }
            else
            {
                subtitle.DOFade(0, .8f);
                yield return new WaitForSeconds(.8f);
            }
        }

        skip.SetActive(false);

        audio_s.DOFade(0, 2.5f);
        subtitle.DOFade(0, 2.5f);
        illustration.DOFade(0, 2.5f);
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
