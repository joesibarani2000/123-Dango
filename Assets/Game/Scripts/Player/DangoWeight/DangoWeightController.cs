using UnityEngine;
public class DangoWeightController : DangoWeightBehaviour
{
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;

    private void Start()
    {
        activePlatform = transform.parent.GetComponent<Platform>();
    }

    private void Update()
    {
        if (ChangeCharacter.usingDangoWeight)
        {
            Controller();
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
            GameData.Instance.AddStep();
            MoveToPlatform(activePlatform.GetNextNode(currentMove));
        }
    }
}
