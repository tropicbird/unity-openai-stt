using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SpeechToText : MonoBehaviour
{
    public AudioRecorder audioRecorder; // AudioRecorderの参照
    private string url = "https://api.openai.com/v1/audio/transcriptions";
    private string apiKey = "<OpenAI_API_KEY>";
    public event Action<string> OnTextReceived;

    public void StartSendingAudio()
    {
        StartCoroutine(SendAudio());
    }

    private IEnumerator SendAudio()
    {
        AudioClip clip = audioRecorder.GetAudioClip();
        byte[] audioBytes = WavUtility.ConvertAudioClipToByteArray(clip);
        //System.IO.File.WriteAllBytes("Assets/sample.wav", audioBytes);

        //Debug.Log("Audio Bytes Length: " + audioBytes.Length);  // 音声データの長さをログに出力

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", audioBytes, "sample.wav", "multipart/form-data"));
        formData.Add(new MultipartFormDataSection("model", "whisper-1"));  // モデルを指定

        UnityWebRequest request = UnityWebRequest.Post(url, formData);
        request.SetRequestHeader("Authorization", "Bearer " +apiKey);
        //request.SetRequestHeader("Content-Type", "multipart/form-data");
　　　　　Debug.Log("Sending request to: " + url);
        Debug.Log("Authorization header: Bearer " + apiKey);
        Debug.Log("Audio Bytes Length: " + audioBytes.Length);
        yield return request.SendWebRequest();

        Debug.Log(request.result);
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            JObject json = JObject.Parse(jsonResponse);
            string transcribedText = json["text"].ToString();

            OnTextReceived?.Invoke(transcribedText);  // イベントを発火
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
    }
}

