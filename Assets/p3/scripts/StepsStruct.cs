using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public struct StepsStruct
{
    [SerializeField] string scriptaction;
    [Tooltip("Parent Object for Highlighting")]
    public GameObject[] ParentObject;
    [Tooltip("DO NOT ALTER - Animator Controller")]
    public Animator ParentAC;
    [Tooltip("Parameter to Start Animation [string value]")]
    public string ACParameter;
    [Tooltip("Text description for this step")]
    public string StepText;
    [Tooltip("AudioClip for this step")]
    public AudioClip StepAudio;
}
