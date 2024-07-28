using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run_Intro : MonoBehaviour
{
    public Button pauseButton;
    private UI_Pause uiPause;
    Run_cameraController camera;
    Run_PlayerMove player;
    public GameObject panel,band; public Text IntroText;
    private float inTime = 0;
    private float outTime = 1;
    //[SerializeField]  float famdeintime;
    //[SerializeField]  float famdeouttime;
    [SerializeField]  float totalTime;
    [SerializeField]  float textTime;
    
    [SerializeField]  float currentTime;
    [SerializeField]  GameObject playerObject;

    // Update is called once per frame
    private void Awake()
    {
        camera = GetComponent<Run_cameraController>();
        GameObject optionButton = GameObject.Find("Option Button");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Run_PlayerMove>();
        }
        else
        {
            Debug.LogError("playerObject가 설정되지 않았습니다.");
        }

        if (camera == null)
        {
            Debug.LogError("Run_cameraController 컴포넌트를 찾을 수 없습니다.");
        }

        if (player == null)
        {
            Debug.LogError("Run_PlayerMove 컴포넌트를 playerObject에서 찾을 수 없습니다.");
        }

        
        if (optionButton != null)
        {
            pauseButton = optionButton.GetComponent<Button>();
            uiPause = optionButton.GetComponent<UI_Pause>();
            if (pauseButton != null && uiPause != null)
            {
                pauseButton.gameObject.SetActive(false);
                pauseButton.onClick.AddListener(uiPause.PauseToggle);
            }
            else
            {
                Debug.LogError("Button 또는 UI_Pause 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Option Button GameObject를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        
        StartIntro();
        camera.ShiftVirtualCam();
        pauseButton.gameObject.SetActive(true);

    }
   
    private void StartIntro()
    {
        player.RemoveFowardSpeed();
        StartCoroutine("Fadein");

    }
    //public void Fadein()
    //{
    //    panel.SetActive(true);
    //    StartCoroutine("FadeIn_co");
    //    Fadeout();
    //}
    //
    //public void Fadeout()
    //{
    //    StartCoroutine("FadeOut_co");
    //    panel.SetActive(false);
    //    
    //}
    private IEnumerator Fadein()
    {
        panel.SetActive(true); // 패널 활성화
        band.SetActive(true);
        StartCoroutine(FadeIn_co()); // 페이드 인 코루틴 실행 및 대기
        yield return new WaitForSeconds(2f); // 2초 대기
        StartCoroutine(FadeOut_co()); // 페이드 아웃 코루틴 실행 및 대기
        panel.SetActive(false); // 패널 비활성화
        band.SetActive(false);
    }
    private IEnumerator FadeIn_co()
    {

        StartCoroutine("FadeInText");
        while (currentTime <= totalTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, currentTime / totalTime));
            
            Debug.Log("fade in 중");
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= textTime)
        {
            band.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, currentTime / textTime));

            Debug.Log("fade in 중");
            currentTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("fade in 끝");
        
        
        


    }
    private  IEnumerator FadeOut_co()
    {

        StartCoroutine("FadeOutText");
        while (currentTime <= totalTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, currentTime / totalTime));

            Debug.Log("fade out 중");
            currentTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("fade out 끝");
        
       
        
    }

    private IEnumerator FadeInText()
    {
        
        IntroText.color =
            new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, 0f);
        while (IntroText.color.a < 1f)
        {
            IntroText.color =
                new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, IntroText.color.a + (currentTime / textTime));
            yield return null;
        }
    }

   
    private IEnumerator FadeOutText()
    {
        IntroText.color =
           new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, 1f);
        while (IntroText.color.a > 0f)
        {
            IntroText.color =
                new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, IntroText.color.a - (currentTime / textTime));
            yield return null;
        }
    }


    /*
    public void FadeInText()
    {
        if (inTime < famdeintime)
        {
            GetComponent<Text>().color = new Color(1, 1, 1, inTime/ famdeintime);
        }
        else
        {
            inTime = 0;
            FadeOutText();
        }
        inTime += Time.deltaTime;
    }
    private void FadeOutText()
    {
        if (outTime < famdeouttime)
        {
            GetComponent<Text>().color = new Color(1, 1, 1, -outTime/ famdeouttime);
        }
        else
        {
            outTime = 0;
            this.gameObject.SetActive(false);
        }
        outTime += Time.deltaTime;
    }
    private void FadeInImage()
    {
        if (inTime < famdeintime)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, inTime / famdeintime);
        }
        else
        {
            inTime = 0;
            FadeOutImage();
        }
        inTime += Time.deltaTime;
    }
    private void FadeOutImage()
    {
        if (outTime < famdeouttime)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, -outTime / famdeouttime);
        }
        else
        {
            outTime = 0;
            this.gameObject.SetActive(false);
        }
        outTime += Time.deltaTime;
    }
    */
}
