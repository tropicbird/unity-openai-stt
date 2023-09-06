using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static byte[] ConvertAudioClipToByteArray(AudioClip audioClip)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        // WAVヘッダーの書き込み
        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write(0); // ファイルサイズ（後で更新）
        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[] { 'f', 'm', 't', ' ' });
        writer.Write(16); // fmtチャンクのサイズ
        writer.Write((short)1); // オーディオフォーマット（1 = PCM）
        writer.Write((short)audioClip.channels); // チャンネル数
        writer.Write(audioClip.frequency); // サンプルレート
        writer.Write(audioClip.frequency * audioClip.channels * 2); // バイトレート
        writer.Write((short)(audioClip.channels * 2)); // ブロックサイズ
        writer.Write((short)16); // ビット深度
        writer.Write(new char[] { 'd', 'a', 't', 'a' });
        writer.Write(0); // データサイズ（後で更新）

        // オーディオデータの書き込み
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);
        for (int i = 0; i < samples.Length; i++)
        {
            writer.Write((short)(samples[i] * Int16.MaxValue));
        }

        // ファイルサイズとデータサイズを更新
        long fileSize = stream.Position;
        stream.Position = 4;
        writer.Write((int)(fileSize - 8));
        stream.Position = 40;
        writer.Write((int)(fileSize - 44));

        // バイト配列として取得
        byte[] byteArray = stream.ToArray();

        // リソースの解放
        writer.Close();
        stream.Close();

        return byteArray;
    }
}
