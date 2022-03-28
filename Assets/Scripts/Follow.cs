using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Vector3 Offset = new Vector3(0,0,0);
    public bool FollowTag = false;
    public GameObject ToFollow;
    public string Tag;

    // Update is called once per frame
    void Update()
    {
        if (FollowTag) ToFollow = GameObject.FindGameObjectWithTag(Tag);
        Vector3 TargetPosition = ToFollow.GetComponent<Transform>().position;
        transform.position = TargetPosition + Offset;
    }
}
