using UnityEngine;

[CreateAssetMenu(menuName = "SO/Audio Data", fileName = "Audio Data")]
public class AudioDataSO : ScriptableObject
{
    [field: SerializeField, Space]
    public AudioClip BackgroundMusic { get; private set; }


    [field: SerializeField, Space]
    public AudioClip JumpClip { get; private set; }

    [field: SerializeField]
    public AudioClip FailClip { get; private set; }

    [field: SerializeField]
    public AudioClip PopClip { get; private set; }

    [field: SerializeField]
    public AudioClip ScoreClip { get; private set; }

    [field: SerializeField]
    public AudioClip WallHitClip { get; private set; }
}
