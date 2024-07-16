using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_UI_Manager : MonoBehaviour
{
    Puzzle_Hunter_TileAction tileChecker;

    [SerializeField,Header("����� Ű ��ư")]
    KeyCode [] buttonCode;

    [SerializeField,Header("����Ű �ȳ� UI"),Space(10)]
    RectTransform[] buttonRect;
    Image[] buttonImg;
    [SerializeField,Header("Space Bar ����"),Space(10)]
    Text spaceTxt;

    [SerializeField,Header("Ű�� ������ ���� ���� ����"),Space(10)]
    public Color normalColor = new Color(1,1,1,.3f);
    public Vector3 normalSize = Vector3.one;
    [SerializeField,Header("Ű�� ������ �ִ� ����")]
    public Color pressedColor = new Color(1, 1, 1, .85f);
    public Vector3 pressedSize = Vector3.one * 1.1f;

    [SerializeField,Header("�޼���"),Space(10)]
    Text message;
    [SerializeField]RectTransform textBack;

    Tween currentTween = null;

    public void ShowMessage(string _message, float _time = 1)
    {
        // ���� �ִϸ��̼��� ������ �ߴ�
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        message.text = _message;

        // �ִϸ��̼� ����
        textBack.sizeDelta = new Vector2(1920, 0); // ���� ũ��
        textBack.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();

        // 0.5�� ���� ũ�⸦ Ű���
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 126), 0.5f).SetEase(Ease.InOutQuad));

        // _time ���� ���
        mySequence.AppendInterval(_time);

        // 0.5�� ���� ũ�⸦ �ٽ� ���̱�
        mySequence.Append(textBack.DOSizeDelta(new Vector2(1920, 0), 0.5f).SetEase(Ease.InOutQuad));

        // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
        mySequence.OnComplete(() => textBack.gameObject.SetActive(false));

        // ���� �ִϸ��̼� ����
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
        spaceTxt.text = (tileChecker.HoldingTile ? "�����α�" : "Ÿ�� ���");
    }
}
