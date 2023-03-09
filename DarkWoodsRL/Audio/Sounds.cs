using System.IO;

namespace DarkWoodsRL.Audio;

public abstract class Sounds
{
    public static string Shatter => "Resources/Audio/shatter.wav";
    public static string Poison => "Resources/Audio/poison.wav";
    public static FileStream HitA => new("Resources/Audio/impact_a.wav", FileMode.Open);
    public static FileStream HitB => new("Resources/Audio/impact_a.wav", FileMode.Open);
}