using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_UI_Manager : MonoBehaviour
{
    Puzzle_Hunter_TileAction tileChecker;

    [SerializeField]
    KeyCode [] buttonCode;

    [SerializeField]
    RectTransform[] buttonRect;
    Image[] buttonImg;
    [SerializeField]
    Text spaceTxt;

    public Color normalColor;
    public Vector3 normalSize;
    public Color pressedColor;
    public Vector3 pressedSize;

    private void Start()
    {
        tileChecker = FindObjectOfType<Puzzle_Hunter_TileAction>();

        buttonImg = new Image[buttonRect.Length];

        for (int i = 0; i < buttonRect.Length; i++)
        {
            buttonImg[i] = buttonRect[i].transform.GetComponent<Image>();
        }
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
        spaceTxt.text = "SPACE : " + (tileChecker.HoldingTile ? "Set Down" : "Pick Up");

    }
}
