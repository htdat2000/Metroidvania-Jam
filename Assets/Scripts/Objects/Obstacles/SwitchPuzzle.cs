using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    [SerializeField] protected Switch[] switches;

    public void ChangeOtherSwitchesState(int switchId)
    {
        if(switchId == 0)
        {     
            switches[0].state = !switches[0].state;
            switches[1].state = !switches[1].state;
            switches[switches.Length - 1].state = !switches[switches.Length - 1].state;
        }
        else if(switchId == (switches.Length - 1))
        {
            switches[0].state = !switches[0].state;
            switches[switches.Length - 2].state = !switches[switches.Length - 2].state;
            switches[switches.Length - 1].state = !switches[switches.Length - 1].state;
        }
        else
        {
            switches[switchId].state = !switches[switchId].state;
            switches[switchId + 1].state = !switches[switchId + 1].state;
            switches[switchId - 1].state = !switches[switchId - 1].state;
        }
        CheckSwitchState();
    }

    void CheckSwitchState()
    {
        int numOfActiveSwitch = 0;
        foreach (Switch _switch in switches)
        {
            if(_switch.state == true)
            {
                numOfActiveSwitch++;
            }
        }
        if(numOfActiveSwitch == switches.Length - 1)
        {
            PuzzleSolved();
        }
    }

    protected void PuzzleSolved()
    {

    }
}
