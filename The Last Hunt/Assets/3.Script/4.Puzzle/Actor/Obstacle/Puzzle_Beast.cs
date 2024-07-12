using UnityEngine;

public class Puzzle_Beast : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //∫Œµ˙»˘ ∞¥√ºø°∞‘ tool¿÷¿ª ∂ß
        if (other.TryGetComponent(out Puzzle_Hunter_Tool tool))
        {
            if (tool.currentItem != null)
            {
                Destroy(gameObject);
                return;
            }
        }
        else if (other.TryGetComponent(out Puzzle_Movement actor))
        {
            actor.Falling();
            return;
        }
    }
}
