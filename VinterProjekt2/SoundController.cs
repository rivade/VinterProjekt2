using Raylib_cs;

public class SoundController //Hanterar spelets ljud
{
    public static Music backgroundMusic = Raylib.LoadMusicStream("Sounds/bgmusic.mp3"); //Spelets bakgrundsmusik
    public static Sound[] sounds = new Sound[] { Raylib.LoadSound("Sounds/win.mp3"), Raylib.LoadSound("Sounds/death.mp3"), Raylib.LoadSound("Sounds/cheer.mp3") }; //Spelets ljudeffekter

    public static void SoundInit() //Initierar volymen för allt ljud i spelet
    {
        foreach (var sound in sounds)
        {
            Raylib.SetSoundVolume(sound, 0.5f); //Ljudeffekter ska vara lägre volym för att inte dränka ut bakgrundsmusiken
        }
        Raylib.SetMusicVolume(backgroundMusic, 1);
    }
}