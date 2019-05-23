using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [HideInInspector] public bool IsCharacterMove = false;

    public string Name = "";
    public float Range = 0f;

    [Header("Animation Options")]
    public string AnimationName = "";
    public int AnimationIndex = 0;

    [Header("Skill Options")]
    public float SkillTime = 0f;
    public bool isCharacterCanMove = true;

    [Header("Skill Damage Options")]
    public int Damage = 0;
    public float DealDamageOnTime = 0f;

    [Header("AOE Options")]
    public bool IsAOE = false;
    public float AOERange = 0f;
    public float AOEAngle = 180f;
    public float AOEDirection = 0f;


    public Color colorAttackArea = Color.red;

    public float Angle1 { get; private set; }
    public float Angle2 { get; private set; }

    public void CalculateSkill()
    {
        if (!IsAOE)
            return;

        Angle1 = (AOEAngle / 2) - AOEDirection;
        Angle2 = (AOEAngle / 2) + AOEDirection;

        if (Angle1 < 0)
        {
            Angle1 *= -1;
        }
        else
        {
            Angle1 = 360 - Angle1;
        }

        if (Angle2 > 360)
        {
            Angle2 -= 360;
        }

    }
}