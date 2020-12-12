using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHintBox : MonoBehaviour
{
    #region Singleton
    private static LocationHintBox instance;
    public static LocationHintBox Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public Transform targetTransform1;
    public Transform targetTransform2;
    public Vector3 targetPos1;
    public Vector3 targetPos2;
    public Material material;

    bool enableLocationHint = false;
    GameObject hintBox;

    // Start is called before the first frame update
    void Awake()
    {
        #region Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        #endregion
        hintBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(hintBox.GetComponent<Collider>());
        hintBox.SetActive(enableLocationHint);
        if(material == null)
        {
            Debug.LogWarning("didn't set material for hint box");
        }
        else
        {
            hintBox.GetComponent<Renderer>().material = material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform1 != null && targetTransform2 != null)
        {
            targetPos1 = targetTransform1.position;
            targetPos2 = targetTransform2.position;
        }
        if (enableLocationHint)
        {
            hintBox.transform.position = (targetPos1 + targetPos2) / 2;
            Vector3 scale;
            scale.x = (targetPos1.x - targetPos2.x);
            scale.y = (targetPos1.y - targetPos2.y);
            scale.z = (targetPos1.z - targetPos2.z);
            hintBox.transform.localScale = scale;
        }
    }
    public void ShowAxisHintBox()
    {
        enableLocationHint = true;
        hintBox.SetActive(true);
    }
    public void HideAxisHintBox()
    {
        enableLocationHint = false;
        hintBox.SetActive(false);
    }
}
