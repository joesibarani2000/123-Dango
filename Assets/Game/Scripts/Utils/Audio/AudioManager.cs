using UnityEngine;

[RequireComponent(typeof(AudioController), typeof(AudioLibrary))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioController audioController;
    [SerializeField] private AudioLibrary audioLibrary;

    void Awake()
    {
        if (!AudioController.Instance)
        {
            audioController = GetComponent<AudioController>();
            audioLibrary = GetComponent<AudioLibrary>();

            audioController.CreateInstance();
            AudioController.SetUpController(audioLibrary, 2);
        } else
        {
            Destroy(gameObject);
        }
    }
}
