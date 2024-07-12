using UnityEngine;
using UnityEngine.UI;

public class Puzzle_WarningUI : MonoBehaviour
{
    Text warningText;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        warningText = GetComponentInChildren<Text>();
    }

    //void ShowMessege(string s)
    //{
    //    warningText.text = s;
    //    anim.SetTrigger("Pop-up");
    //}
}
