using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonHandler : MonoBehaviour
{
    public FiniteStateMachineManager FSMmanager;

    public void onButtonClicked()
    {
        if (FSMmanager.choice_num > 0)
        {
            if (this.gameObject.name == "choice1")
            {
                FSMmanager.deleteClickedChoiceAndGiveNextValue(1);
            }
            else if (this.gameObject.name == "choice2")
            {
                FSMmanager.deleteClickedChoiceAndGiveNextValue(2);
            }
            if (this.gameObject.name == "choice3")
            {
                FSMmanager.deleteClickedChoiceAndGiveNextValue(3);
            }
        }
    }
}
