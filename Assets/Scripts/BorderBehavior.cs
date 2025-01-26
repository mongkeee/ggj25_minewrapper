using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBehavior : MonoBehaviour
{

    [SerializeField] private gameEngine ge;
    // Start is called before the first frame update
    void Start()
    {
        ge = GameObject.Find("LevelConfig").GetComponent<gameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) //ambil bool flagged sama !isBomb, kalo iya alert game + lose
    {
        bubbleBehavior babelnya = other.gameObject.GetComponent<bubbleBehavior>();
        if (babelnya.lane - ge.laneRn <= -7)
        {
            GameObject parentnya = other.gameObject.transform.parent.gameObject;
            Destroy(parentnya);
        }
        else if (babelnya.flagged && !babelnya.isBomb) 
        { 
            ge.LoseByFalseFlagged(babelnya.lane);
            babelnya.falseFlagged();
        }
    }
}
