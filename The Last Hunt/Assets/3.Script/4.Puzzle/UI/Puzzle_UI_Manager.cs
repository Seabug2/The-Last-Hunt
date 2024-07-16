using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_UI_Manager : MonoBehaviour
{
    Puzzle_Hunter_TileAction tileChecker;

    [SerializeField, Header("����� Ű ��ư")]
    KeyCode[] buttonCode;

    [SerializeField, Header("����Ű �ȳ� UI"), Space(10)]
    RectTransform[] buttonRect;
    Image[] buttonImg;
    [SerializeField, Header("Space Bar ����"), Space(10)]
    Text guideState;

    [Header("Ű�� ������ ���� ���� ����"), Space(10)]
    public Color normalColor = new Color(1, 1, 1, .3f);
    public Vector3 normalSize = Vector3.one;
    [Header("Ű�� ������ �ִ� ����")]
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
        //�Է��� Ű�� ���������� ������
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

        //Ÿ���� ��� �ִ�
        if (tileChecker.HoldingTile)
        {
            if (tileChecker.ViewingTile)
                guideState.text = "���� �� �����ϴ�";
            else
                guideState.text = "Ÿ�� ����";
        }
        //Ÿ���� ��� ���� ���� ��
        else
        {
            if (tileChecker.ViewingTile)
            {
                if (tileChecker.ViewingTile.IsOverlapping())
                {
                    guideState.text = "�� �� �����ϴ�";
                }
                else
                {
                    guideState.text = "Ÿ�� ���";
                }
            }
            else
                guideState.text = "";
        }
    }
}
