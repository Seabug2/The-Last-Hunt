using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_AnimalController : MonoBehaviour
{
    [SerializeField]
    KeyCode correctKeyCode;
    public KeyCode CorrectKeyCode => correctKeyCode;
    public int animalIndex;
    public RectTransform arrowsRT;
    public GameObject arrowPrefab, note_obj;
    public Canvas ingameCanvas;

    public double t { get; private set; }
    private double checkTime;

    Vector3 startPosition, endPosition;
    Vector3 UI_StartPos, UI_EndPos;


    private void Awake()
    {
        startPosition = Rhythm_AnimalPooling.instance.Spawner.position;
        endPosition = Rhythm_AnimalPooling.instance.targetPoint.position;
        UI_StartPos = arrowsRT.position +  Vector3.up * 600;
        UI_EndPos = UI_StartPos + Vector3.down * (600 / 0.83f);
        note_obj = Instantiate(arrowPrefab, ingameCanvas.transform);
    }

    private void OnEnable()
    {
        note_obj.SetActive(true);
        note_obj.GetComponent<RectTransform>().position = UI_StartPos;
        if(CorrectKeyCode == KeyCode.LeftArrow)
        {
            note_obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (CorrectKeyCode == KeyCode.RightArrow)
        {
            note_obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 180);
        }
        t = 0;
        checkTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (Rhythm_ChapterManager.instance.BGMisPausing)
        {
            checkTime = AudioSettings.dspTime;
            return;
        }
        t += (AudioSettings.dspTime - checkTime) * 0.9;
        checkTime = AudioSettings.dspTime;

        transform.position = Vector3.Lerp(startPosition, endPosition, (float)t);
        note_obj.GetComponent<RectTransform>().position = Vector2.Lerp(UI_StartPos, UI_EndPos, (float)t);

        if (t > 0.99f)
        {
            Rhythm_ChapterManager.instance.JudgeResult(0);
            Inactive();
        }
    }

    public void Inactive()
    {
        Rhythm_AnimalPooling.instance.ReturnObjectToPool(this);
        note_obj.SetActive(false);
        gameObject.SetActive(false);
    }
}
