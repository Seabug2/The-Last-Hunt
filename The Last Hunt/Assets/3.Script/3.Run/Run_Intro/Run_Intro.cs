using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run_Intro : MonoBehaviour
{
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

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Run_PlayerMove>();
        }
        else
        {
            Debug.LogError("playerObject�� �������� �ʾҽ��ϴ�.");
        }

        if (camera == null)
        {
            Debug.LogError("Run_cameraController ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (player == null)
        {
            Debug.LogError("Run_PlayerMove ������Ʈ�� playerObject���� ã�� �� �����ϴ�.");
        }
    }
    private void Start()
    {
        
        StartIntro();
        camera.ShiftVirtualCam();
        
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
        panel.SetActive(true); // �г� Ȱ��ȭ
        yield return StartCoroutine(FadeIn_co()); // ���̵� �� �ڷ�ƾ ���� �� ���
        yield return new WaitForSeconds(2f); // 2�� ���
        yield return StartCoroutine(FadeOut_co()); // ���̵� �ƿ� �ڷ�ƾ ���� �� ���
        panel.SetActive(false); // �г� ��Ȱ��ȭ
    }
    private IEnumerator FadeIn_co()
    {

        StartCoroutine("FadeInText");
        while (currentTime <= totalTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, currentTime / totalTime));
            
            Debug.Log("fade in ��");
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= textTime)
        {
            band.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, currentTime / textTime));

            Debug.Log("fade in ��");
            currentTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("fade in ��");
        
        
        


    }
    private  IEnumerator FadeOut_co()
    {

        StartCoroutine("FadeOutText");
        while (currentTime <= totalTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, currentTime / totalTime));

            Debug.Log("fade out ��");
            currentTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("fade out ��");
        
       
        
    }

    private IEnumerator FadeInText()
    {
        
        IntroText.color =
            new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, 0f);
        while (IntroText.color.a < 1f)
        {
            IntroText.color =
                new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, IntroText.color.a + (Time.deltaTime / 2f));
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
                new Color(IntroText.color.r, IntroText.color.g, IntroText.color.b, IntroText.color.a - (Time.deltaTime / 1.3f));
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
