/****************************************************
    文件：AudioSvc.cs
	作者：Happy-11
    日期：2020/11/12 17:7:55
	功能：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{

    public static AudioSvc Instance=null;

    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;
    }
    /// <summary>
    /// 音乐播放服务
    /// </summary>
    /// <param name="name">音乐名</param>
    /// <param name="isLoop">是否循环</param>
    public void PlayBGMusic(string name,bool isLoop=true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        //当前没有播放音乐或者当前播放的音乐和要播放的音乐不同，则切换的这个音乐
        if (bgAudio.clip==null||bgAudio.clip.name!=audio.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play(); 
    }
}