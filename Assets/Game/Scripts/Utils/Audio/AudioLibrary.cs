using System.Linq;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    [System.Serializable]
    struct ClipDetail
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private ClipDetail[] bgmClips;
    [SerializeField] private ClipDetail[] sfxClips;

    public AudioClip GetBGMClips(string name)
    {
        if (bgmClips.Any(e => e.name == name))
        {
            return bgmClips.First(e => e.name == name).clip;
        }

        return null;
    }

    public AudioClip GetSFXClips(string name)
    {
        if (sfxClips.Any(e => e.name == name))
        {
            return sfxClips.First(e => e.name == name).clip;
        }

        return null;
    }
}
