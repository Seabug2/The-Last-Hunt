using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_CountChange : MonoBehaviour
{
    [SerializeField]
    Text count;
    [SerializeField]
    Text count_new;
    private void CountChange()
    {
        count.text = count_new.text;
    }
}
