using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private int sampleRate = 44100; // サンプルレート
    private int lengthSec = 5; // 録音時間（秒）
    private bool isRecording = false; // 録音中かどうか

    public void StartRecording(int newSampleRate = 44100, int newLengthSec = 5)
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found.");
            return;
        }

        sampleRate = newSampleRate;
        lengthSec = newLengthSec;

        audioClip = Microphone.Start(null, false, lengthSec, sampleRate);
        isRecording = true;
    }

    public void StopRecording()
    {
        if (!isRecording)
        {
            Debug.LogWarning("Not currently recording.");
            return;
        }

        Microphone.End(null);
        isRecording = false;
    }

    public AudioClip GetAudioClip()
    {
        if (isRecording)
        {
            Debug.LogWarning("Still recording, clip may be incomplete.");
        }

        return audioClip;
    }

    public bool IsRecording()
    {
        return isRecording;
    }
}
