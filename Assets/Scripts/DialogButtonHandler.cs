using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogButtonHandler : MonoBehaviour
{
    public FiniteStateMachineManager FSMmanager;

    private void Update()
    {
        if (FSMmanager.isDialogClickable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FSMmanager.isDialogClickable = false;
            }
        }
    }
}
