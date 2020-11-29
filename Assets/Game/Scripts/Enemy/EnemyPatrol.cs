using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : EnemyBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int positionStart;
    [SerializeField] private Platform [] platform;
    [SerializeField] private bool moveRight;
    [SerializeField] private bool loopPatrol;

    // Start is called before the first frame update
    void Start()
    {
        onMoving = false;
        currentIndex = positionStart;
        activePlatform = transform.parent.GetComponent<Platform>();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (!onMoving)
        {
            if (moveRight == true) currentIndex++;
            else currentIndex--;

            if (loopPatrol)
            {
                if (currentIndex >= platform.Length)
                {
                    currentIndex = 0;
                }

                if (currentIndex < 0)
                {
                    currentIndex = platform.Length - 1;
                }
            }
            else
            {
                if (currentIndex >= platform.Length)
                {
                    currentIndex = platform.Length - 1;
                    currentIndex--;
                    moveRight = false;
                }

                if (currentIndex < 0)
                {
                    currentIndex = 0;
                    currentIndex++;
                    moveRight = true;
                }
            }
            MoveToPlatform(platform[currentIndex]);
        }
       
    }
}
