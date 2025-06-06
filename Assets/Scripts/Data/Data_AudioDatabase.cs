using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class Data_AudioDatabase : ScriptableObject
{
    public List<AudioClip> audioClips;
}
