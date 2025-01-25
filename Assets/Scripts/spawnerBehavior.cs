using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bubble;

    public int laneNum;
    private int manyBubble;

    // Start is called before the first frame update
    void Start()
    {
        if (laneNum % 2 == 0)
        {
            manyBubble = 11;
        }
        else manyBubble = 12;

        for (int i = 0; i < manyBubble; i++)
        {
            GameObject bub = Instantiate(bubble, this.transform);
            //bub.transform.parent = this.transform;
            bub.transform.position = new Vector3(1.5f * i + this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
