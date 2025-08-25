using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjects : MonoBehaviour
{
    //variables
    //[Tooltip("DO NOT ALTER")]
    //public GameObject ParentObject;
    [Tooltip("DO NOT ALTER")]
    [SerializeField] private MeshRenderer[] ObjectsToColor;
    [Tooltip("Time Between the Color Change")]
    [SerializeField] private float Delay = 0.5f;
    [Tooltip("Material to use a Highlight")]
    [SerializeField] private Material HighlightMaterial;
    [Tooltip("UI Hint Component")]
    [SerializeField] private GameObject UIHint;
    private bool DoColorSwitch = true;

    /*
        // Update is called once per frame
        void Update()
        {
            if(Input.anyKeyDown)
            {
                GetObjects(ParentObject);
            }
        }
    */
    private void Start()
    {
        UIHint.SetActive(false);
    }
    public void GetObjects(GameObject parent)
    {
        ObjectsToColor = null;
        ObjectsToColor = parent.GetComponentsInChildren<MeshRenderer>();
        DoColorSwitch = true; //turn on the Color Controler
        //send each MeshRenderer to the ColorTheObjects co-routine
        for (int i = 0; i < ObjectsToColor.Length; i++)
        {
            StartCoroutine(ColorTheObjects(ObjectsToColor[i]));
        }
    }
    public IEnumerator ColorTheObjects(MeshRenderer SetThisRenderer)
    {
        //turn on UI HINT
        UIHint.SetActive(true);
        //get the original materials
        Material[] OriginalMaterials = SetThisRenderer.materials;
        //set the temp materials array
        Material[] tempMaterials = SetThisRenderer.materials;

        //set the temp material array to the highlight material
        for (int i = 0; i < SetThisRenderer.materials.Length; i++)
        {
            tempMaterials[i] = HighlightMaterial;
        }

        //set the Meshrender's materials to the hightlight materials - then the original then highlight then original

        while (DoColorSwitch)
        {
            SetThisRenderer.materials = tempMaterials;

            yield return new WaitForSeconds(Delay);

            SetThisRenderer.materials = OriginalMaterials;

            yield return new WaitForSeconds(Delay);
        }
        //turn off UI Hint
        UIHint.SetActive(false);
    }
    public void StopColorSwitch()
    {
        DoColorSwitch = false;
    }
}
