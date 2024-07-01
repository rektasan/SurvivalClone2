using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Survival/Dialog")]
public class DialogSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string[] Texts;
    public int[] AnimInd;
}