using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer objSpr;
    [SerializeField] private Sprite bom;
    [SerializeField] private Sprite[] blankNum;
    public bool isBomb = false;
    public bool unpop = true;
    public bool flagged = false;

    [SerializeField]private int bombNear = 0;

    private float[] radOth = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        objSpr = this.GetComponent<SpriteRenderer>();

        int rng = Random.Range(0, 10);
        if(rng >= 7) //rng bom
        {
            isBomb = true;
        }

        for (int i = 0; i < 6; i++)
        {
            radOth[i] = i * 60 * Mathf.Deg2Rad;
            Debug.Log(radOth[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!unpop)
        {
            if (isBomb) objSpr.sprite = bom;
            else objSpr.sprite = blankNum[bombNear];
        }
        else
        {
            int jum = 0;
            RaycastHit2D[] hitOth = new RaycastHit2D[6];
            for (int i = 0; i < 6; i++)
            {
                hitOth[i] = Physics2D.Raycast(this.transform.position + RadToCoord(radOth[i]), RadToCoord(radOth[i]), 0.5f);
                Debug.DrawRay(this.transform.position + RadToCoord(radOth[i]), RadToCoord(radOth[i]) * 0.5f, Color.white, 0);
                if (hitOth[i].collider != null)
                {
                    if(hitOth[i].collider.gameObject.tag == "pop")
                    {
                        bubbleBehavior othBubb = hitOth[i].collider.gameObject.GetComponent<bubbleBehavior>();
                        if (othBubb.isBomb)
                        {
                            jum++;
                        }
                    }
                }
            }
            bombNear = jum;
        }
    }

    public void OnMouseOver()//mouse input
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            unpop = false;
        }
    }

    private Vector3 RadToCoord(float rad)
    {
        Vector3 coord = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * 0.7f;

        return coord;
    }
}
