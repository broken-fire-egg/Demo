using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionSettingTool : MonoBehaviour
{
    public bool IsImage;
    public List<Transform> Transforms;
    public List<RectTransform> RectTransforms;
    Vector3 startPosition;
    Vector3 endPosition;
    float startRotation;
    float endRotation;
    // Start is called before the first frame update
    void Start()
    {
        if (IsImage)
        {
            RectTransforms = new List<RectTransform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
            startPosition = RectTransforms[0].position;
            startRotation = RectTransforms[0].rotation.eulerAngles.z;
            endPosition = RectTransforms[RectTransforms.Count - 1].position;
            endRotation = RectTransforms[RectTransforms.Count - 1].rotation.eulerAngles.z;


            print(startPosition);
            print(startRotation);
            print(endPosition);
            print(endRotation);
            for (int i = 1; i < RectTransforms.Count - 1; i++)
            {
                //RectTransforms[i].position = Vector3.zero;
                RectTransforms[i].position = new Vector3(Mathf.Lerp(startPosition.x,endPosition.x, (float)i / (float)(RectTransforms.Count - 1)), Mathf.Lerp(startPosition.y, endPosition.y, (float)i / (float)(RectTransforms.Count - 1)), 0);
                RectTransforms[i].rotation = Quaternion.Euler(0, 0, Mathf.Lerp(startRotation, endRotation, (float)i / (float)(RectTransforms.Count - 1)));
            }

        }
    }
}
