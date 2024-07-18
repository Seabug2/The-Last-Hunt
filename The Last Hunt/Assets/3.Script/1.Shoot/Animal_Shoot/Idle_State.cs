using System;

[Serializable]
public class Idle_State : AI_State
{
    public float minStateTime = 20f;
    public float maxStateTime = 40f;
    // Change of choosing this state to other states
    public int stateWeight = 20;
}
