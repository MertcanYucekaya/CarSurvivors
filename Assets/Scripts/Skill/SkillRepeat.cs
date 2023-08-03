using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRepeat : MonoBehaviour
{
    public int maxRepeat;
    public enum skillType {Null,Shield,Hearth}
    public skillType currentSkillType;
    [HideInInspector] public int currentRepeat=0;
    [SerializeField] public Sprite iconImage;
}
