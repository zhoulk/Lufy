// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 20:44:43
// ========================================================
using LF;
using LF.Sound;

public static class SoundExtention
{
    public static void PlayMusic(this SoundManager sound, MusicId musicId)
    {
        string path = Utility.Text.Format("Assets/Game/Res/Sounds/Music/{0}.mp3", musicId.ToString());
        GameEntry.Sound.PlaySound(path, false);
    }

    public static void PlaySound(this SoundManager sound, SoundId soundId)
    {
        string path = Utility.Text.Format("Assets/Game/Res/Sounds/Sound/{0}.mp3", soundId.ToString());
        GameEntry.Sound.PlaySound(path, true);
    }

    public static void PauseMusic(this SoundManager sound)
    {
        GameEntry.Sound.PauseSound(false);
    }

    public static void PauseSound(this SoundManager sound)
    {
        GameEntry.Sound.PauseSound(true);
    }

    public static void ResumeMusic(this SoundManager sound)
    {
        GameEntry.Sound.ResumeSound(false);
    }

    public static void ResumeSound(this SoundManager sound)
    {
        GameEntry.Sound.ResumeSound(true);
    }

    public static void MuteMusic(this SoundManager sound, bool mute)
    {
        GameEntry.Sound.Mute(mute, false);
    }

    public static void MuteSound(this SoundManager sound, bool mute)
    {
        GameEntry.Sound.Mute(mute, true);
    }
}
