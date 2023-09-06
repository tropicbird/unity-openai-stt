using UnityEngine;
using UnityEngine.UI;

public class AudioControlUI : MonoBehaviour
{
    public AudioRecorder audioRecorder; // AudioRecorderの参照
    public SpeechToText speechToText;  // SpeechToTextの参照
    public Button recordButton; // 録音ボタンの参照
    public Text buttonText; // ボタンのテキストの参照

    private bool isRecording = false; // 録音中かどうか

    // Start is called before the first frame update
    void Start()
    {
        // ボタンにクリックイベントを追加
        recordButton.onClick.AddListener(ToggleRecording);
    }

    // 録音の開始・停止を切り替える
    public void ToggleRecording()
    {
        if (isRecording)
        {
            audioRecorder.StopRecording();
            buttonText.text = "Start Recording";
            speechToText.StartSendingAudio();  // 録音が停止したら音声データを送信
        }
        else
        {
            audioRecorder.StartRecording();
            buttonText.text = "Stop Recording";
        }

        isRecording = !isRecording;
    }
}
