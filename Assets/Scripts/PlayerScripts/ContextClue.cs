using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public GameObject contextClue;
    public bool activeObject;

    public void ChangeActive()
    {
        activeObject = !activeObject;
        contextClue.SetActive(activeObject);
    }
}
