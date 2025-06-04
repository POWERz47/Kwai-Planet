using UnityEngine;

[CreateAssetMenu(fileName = "KwaiEntity", menuName = "Custom/Kwai")]
public class KwaiEntity : ScriptableObject
{
    [Header("Identity")]
    public string KwaiName;
    public int age;
    public Sex sex;

    [Header("Stats")]
    public float health;
    public float defense;
    public float attack;
    public float speed;

    [Header("Parentage")]
    public KwaiEntity father;
    public KwaiEntity mother;

    [Header("Appearance")]
    public Color color;
    public string socks;
    public string stigma;
    public string faceMarking;

    [Range(0f, 1f)] public float length;
    [Range(0f, 1f)] public float width;
    [Range(0f, 1f)] public float curvature;
    [Range(0f, 1f)] public float fat;

    [Header("Production Quality")]
    [Range(0, 1000)] public int meatQuality;
    [Range(0, 1000)] public int milkQuality;
}

public enum Sex
{
    Male,
    Female,
    Unknown
}
