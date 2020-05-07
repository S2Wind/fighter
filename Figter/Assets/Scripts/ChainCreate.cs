using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCreate : MonoBehaviour
{
    [SerializeField] int numberofChain;

    [SerializeField] int curNumber;

    public int CurNumber { get => curNumber; set => curNumber = value; }

    void Start()
    {
        if (curNumber < numberofChain)
        {
            GameObject next = Instantiate(this.gameObject);
            next.transform.SetParent(this.transform);
            next.transform.localPosition = new Vector3(0f, -0.35f);
            next.GetComponent<ChainCreate>().curNumber = curNumber + 1;
            next.GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }

    }

}
