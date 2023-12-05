using Raylib_cs;

public class SoundController
{
    public static Music backgroundMusic = Raylib.LoadMusicStream("Sounds/bgmusic.mp3");
    public static Sound[] sounds = new Sound[] { Raylib.LoadSound("Sounds/win.mp3"), Raylib.LoadSound("Sounds/death.mp3"), Raylib.LoadSound("Sounds/cheer.mp3") };

    public static void SoundInit()
    {
        foreach (var sound in sounds)
        {
            Raylib.SetSoundVolume(sound, 0.5f);
        }
    }
}