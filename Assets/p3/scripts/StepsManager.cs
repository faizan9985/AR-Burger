using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StepsManager : MonoBehaviour
{
    //variables
    [Header("Step Information")]
    [SerializeField] private StepsStruct[] Steps;
    [Tooltip("Data Pointer [DO NOT ALTER]")]
    [SerializeField] private int StepsPointer = 0;

    [Header("General Variables")]
    [Tooltip("UI TextMeshPro Textbox for Step Description")]
    [SerializeField] private TextMeshProUGUI StepDescriptionText;
    [Tooltip("GameObject that hosts the main AudioSource")]
    [SerializeField] private AudioSource TheAudioSource;
    [Tooltip("Pointer to the ColorObjects script")]
    [SerializeField] private ColorObjects ColorObjectsScript;
    [Tooltip("Layer Mask for Interactive Colliders")]
    [SerializeField] private LayerMask m_layerMask;
    [Tooltip("Ending UI Component")]
    [SerializeField] private GameObject EndUIComponent;
    private Vector3 touchPosition;
    private bool ListenForScreenTaps = false;


    // Start is called before the first frame update
    void Start()
    {
        EndUIComponent.SetActive(false);
        //turn off all box colliders
        BoxCollider[] AllColliders = transform.parent.GetComponentsInChildren<BoxCollider>();
        foreach (var BoxC in AllColliders)
        {
            BoxC.enabled = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(!ListenForScreenTaps)
        {
            return;
        }
        #if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
                    {
                        ListenForScreenTaps = false;
                        RunAnimation();
                    }
                }
#else
                    if (Input.touchCount > 0)
                            {
                                Touch touch = Input.GetTouch(0);
                                touchPosition = touch.position;

                                var ray = Camera.main.ScreenPointToRay(touchPosition);
                                RaycastHit hit;
                                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
                                {
                                    ListenForScreenTaps = false;
                                    RunAnimation();
                                }
                            }
#endif

    }


    public void RunStep(int val)
    {
        StepsPointer = val;
        //turn on box collider
        Steps[StepsPointer].ParentObject[0].GetComponent<BoxCollider>().enabled = true;
        StepDescriptionText.text = Steps[StepsPointer].StepText.Replace("*","\n\n");
        TheAudioSource.clip = Steps[StepsPointer].StepAudio;
        TheAudioSource.Play();
        StartCoroutine(WaitForAudio());
    }

    public void RunAnimation()
    {
        //turn off color
        ColorObjectsScript.StopColorSwitch();
        //turn off box collider
        for (int i = 0; i < Steps[StepsPointer].ParentObject.Length; i++)
        {
            TurnOnOffBoxColliders(Steps[StepsPointer].ParentObject[i], false);
        }
        Steps[StepsPointer].ParentAC = Steps[StepsPointer].ParentObject[0].GetComponentInParent<Animator>();
        Steps[StepsPointer].ParentAC.SetTrigger(Steps[StepsPointer].ACParameter);
        StartCoroutine(WaitForAnimation());
    }

    public IEnumerator WaitForAudio()
    {
        float Delay = Steps[StepsPointer].StepAudio.length;
        yield return new WaitForSeconds(Delay);
        for (int i = 0; i < Steps[StepsPointer].ParentObject.Length; i++)
        {
            ColorObjectsScript.GetObjects(Steps[StepsPointer].ParentObject[i]);
            TurnOnOffBoxColliders(Steps[StepsPointer].ParentObject[i], true);
        }
        ListenForScreenTaps = true;
    }

        public IEnumerator WaitForAnimation()
    {
        float Delay = 0.5f; //wait for the system to process
        yield return new WaitForSeconds(Delay);
        Delay = Steps[StepsPointer].ParentAC.GetCurrentAnimatorStateInfo(0).length + 1.0f; //get length of current animation + 1 second
        yield return new WaitForSeconds(Delay); //wait for the length of the animation 
        ++StepsPointer; //add one to the pointer
        if (StepsPointer < Steps.Length)
        {
            RunStep(StepsPointer); //run next step
        }
        else
        {
            //turn off color
            ColorObjectsScript.StopColorSwitch();
            //turn on end UI component
            StepDescriptionText.text = "";
            EndUIComponent.SetActive(true);
        }
    }

    void TurnOnOffBoxColliders(GameObject GO, bool OnOff)
    {
        if(GO.GetComponent<BoxCollider>())
        {
            GO.GetComponent<BoxCollider>().enabled = OnOff;
        }
    }
}
