using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_UI_Manager : MonoBehaviour
{
    Puzzle_Hunter_TileAction tileChecker;

    [SerializeField, Header("사용할 키 버튼")]
    KeyCode[] buttonCode;

    [SerializeField, Header("조작키 안내 UI"), Space(10)]
    RectTransform[] buttonRect;
    Image[] buttonImg;
    [SerializeField, Header("Space Bar 설명"), Space(10)]
    Text guideState;

    [Header("키를 누르고 있지 않은 상태"), Space(10)]
    public Color normalColor = new Color(1, 1, 1, .3f);
    public Vector3 normalSize = Vector3.one;
    [Header("키를 누르고 있는 상태")]
    public Color pressedColor = new Color(1, 1, 1, .85f);
    public Vector3 pressedSize = Vector3.one * 1.1f;


    private void Awake()
    {
        tileChecker = FindObjectOfType<Puzzle_Hunter_TileAction>();
        buttonImg = new Image[buttonRect.Length];
        for (int i = 0; i < buttonRect.Length; i++)
        {
            buttonImg[i] = buttonRect[i].transform.GetComponent<Image>();
        }

        //guideState.transform.parent.gameObject.SetActive(false);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void AllUIPopUp()
    {
        List<RectTransform> rects = new List<RectTransform>();

        foreach (Transform child in transform)
        {
            rects.Add(child.GetComponent<RectTransform>());
        }

        while (rects.Count > 0)
        {
            RectTransform rect = rects[Random.Range(0, rects.Count)];
            rects.Remove(rect);
            Vector2 size = rect.sizeDelta;
            rect.sizeDelta = Vector2.zero;
            rect.gameObject.SetActive(true);
            rect.DOSizeDelta(size, .5f).SetEase(Ease.OutBack).SetDelay(rects.Count * .125f);
        }
        //guideState.transform.parent.gameObject.SetActive(true);
    }

    private void Update()
    {
        //입력한 키를 가시적으로 보여줌
        for (int i = 0; i < buttonCode.Length; i++)
        {
            if (Input.GetKeyDown(buttonCode[i]))
            {
                buttonRect[i].localScale = pressedSize;
                buttonImg[i].color = pressedColor;
            }
            else if (Input.GetKeyUp(buttonCode[i]))
            {
                buttonRect[i].localScale = normalSize;
                buttonImg[i].color = normalColor;
            }
        }

        //타일을 들고 있다
        if (tileChecker.HoldingTile)
        {
            if (tileChecker.ViewingTile)
                guideState.text = "놓을 수 없습니다";
            else
                guideState.text = "타일 놓기";
        }
        //타일을 들고 있지 않을 때
        else
        {
            if (tileChecker.ViewingTile)
            {
                if (tileChecker.ViewingTile.IsOverlapping())
                {
                    guideState.text = "들 수 없습니다";
                }
                else
                {
                    guideState.text = "타일 들기";
                }
            }
            else
                guideState.text = "";
        }
    }
}
