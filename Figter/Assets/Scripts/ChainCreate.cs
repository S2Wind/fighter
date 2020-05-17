using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCreate : MonoBehaviour
{
    [SerializeField] int numberofChain;

    [SerializeField] int curNumber;

    [SerializeField] Vector3 nextTrans;

    [SerializeField] Vector2 anchor = new Vector2(0,-0.15f) ;
 
    public int CurNumber { get => curNumber; set => curNumber = value; }

    void Start()
    {
        if (curNumber < numberofChain)
        {
            GameObject next = Instantiate(this.gameObject);
            next.transform.SetParent(this.transform);
            next.transform.localPosition = nextTrans;
            next.GetComponent<HingeJoint2D>().anchor = anchor;
            next.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = true;
            next.GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            next.GetComponent<ChainCreate>().curNumber = curNumber + 1;
        }
    }

}
