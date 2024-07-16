using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_UI_Manager : MonoBehaviour
{
    Puzzle_Hunter_TileAction tileChecker;

    [SerializeField,Header("사용할 키 버튼")]
    KeyCode [] buttonCode;

    [SerializeField,Header("조작키 안내 UI"),Space(10)]
    RectTransform[] buttonRect;
    Image[] buttonImg;
    [SerializeField,Header("Space Bar 설명"),Space(10)]
    Text spaceTxt;

    [SerializeField,Header("키를 누르고 있지 않은 상태"),Space(10)]
    public Color normalColor = new Color(1,1,1,.3f);
    public Vector3 normalSize = Vector3.one;
    [SerializeField,Header("키를 누르고 있는 상태")]
    public Color pressedColor = new Color(1, 1, 1, .85f);
    public Vector3 pressedSize = Vector3.one * 1.1f;

    [SerializeField,Header("메세지"),Space(10)]
    Text message;
    [SerializeField]RectTransform textBack;

    Tween currentTween = null;

    public void ShowMessage(string _message, float _time = 1)
    {
        // 기존 애니메이션이 있으면 중단
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        message.text = _message;

        // 애니메이션 설정
        textBack.sizeDelta = new Vector2(1920, 0); // 시작 크기
        textBack.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5초 동안 크기를 키우기
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // _time 동안 대기
        mySequence.AppendInterval(_time);

        // 0.5초 동안 크기를 다시 줄이기
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

        // 애니메이션이 끝난 후 비활성화
        mySequence.OnComplete(() => textBack.gameObject.SetActive(false));

        // 현재 애니메이션 저장
        currentTween = mySequence;
    }

    private void Awake()
    {
        tileChecker = FindObjectOfType<Puzzle_Hunter_TileAction>();
        buttonImg = new Image[buttonRect.Length];
        for (int i = 0; i < buttonRect.Length; i++)
        {
            buttonImg[i] = buttonRect[i].transform.GetComponent<Image>();
        }
    }

    public void PopUpUI()
    {

    }


    private void Update()
    {
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

        //if (tileChecker.ViewingTile)
        spaceTxt.text = (tileChecker.HoldingTile ? "내려두기" : "타일 들기");
    }
}
