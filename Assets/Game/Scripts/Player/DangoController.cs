using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoController : DangoBehaviour
{
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;
    [SerializeField] private KeyCode moveRewind;

    private void Start()
    {
        Time.timeScale = 1f;
        Init();
    }

    private void Update()
    {
        if (!ChangeCharacter.usingDangoWeight)
        {
            if (!rewind) Controller();
            else RewindController();
        }
    }

    private void Controller()
    {
        currentMove = Movement.IDLE;

        if (Input.GetKeyDown(moveLeft))
        {
            currentMove = Movement.LEFT;
        }

        if (Input.GetKeyDown(moveRight))
        {
            currentMove = Movement.RIGHT;
        }

        if (Input.GetKeyDown(moveUp))
        {
            currentMove = Movement.UP;
        }

        if (Input.GetKeyDown(moveDown))
        {
            currentMove = Movement.DOWN;
        }

        if (currentMove != Movement.IDLE && !onMoving)
        {
            MoveToPlatform(activePlatform.GetNextNode(currentMove));
        }
    }

    private void RewindController()
    {
        if (!onMoving && Input.GetKeyDown(moveRewind) && countPlatform >= 0)
        {
            AudioController.PlaySFX("Move");
            MoveRewindPlatform(savePlatform[countPlatform]); 
        }
    }
}
