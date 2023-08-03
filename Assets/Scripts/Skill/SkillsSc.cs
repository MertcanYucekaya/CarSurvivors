using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillsSc : MonoBehaviour
{
    [SerializeField] private float magnetRadiusMultiplier;
    [Header("Rocket")]
    [SerializeField] private float rocketDamagePlus;
    [SerializeField] private float rocketRadiusPlus;
    [SerializeField] private float rocketCooldownPlus;
    [SerializeField] private float rocketDistancePlus;

    [Header("GettingsSkills")]
    [SerializeField] private GameObject[] gettingSkillsObject;
    [SerializeField] private Image[] gettingSkillsImage;
    int gettingSkillsCount;
    [Header("Private")]
    [SerializeField] private CarCombat carCombat;
    [SerializeField] private GameObject[] skillObjectsArray;
    [SerializeField] private RectTransform[] skillPosesArray;
    private List<GameObject> skillsObejct = new();
    private List<SkillRepeat> skillsRepeat = new();
    private void Start()
    {
        gettingSkillsCount = 0;
        for(int i = 0; i < skillObjectsArray.Length; i++)
        {
            skillsObejct.Add(skillObjectsArray[i]);
            skillsRepeat.Add(skillObjectsArray[i].GetComponent<SkillRepeat>());
        }
        skillsSetActive();
    }
    public void skillEnabledMethod()
    {
        int skillCount = 0;
        while (true)
        {
            int skill = Random.Range(0, skillsObejct.Count);
            if (skillsObejct[skill].activeSelf == false)
            {
                if(skillsRepeat[skill].currentSkillType == SkillRepeat.skillType.Shield && carCombat.checkShield()==false)
                {
                    continue;
                }
                if(skillsRepeat[skill].currentSkillType == SkillRepeat.skillType.Hearth && carCombat.checkHeal() == false)
                {
                    continue;
                }
                skillsObejct[skill].SetActive(true);
                skillsObejct[skill].transform.position = skillPosesArray[skillCount].position;
                skillCount++;
                skillsRepeat[skill].currentRepeat++;
                if (skillsRepeat[skill].currentRepeat == skillsRepeat[skill].maxRepeat 
                    && skillsRepeat[skill].currentSkillType == SkillRepeat.skillType.Null)
                {
                    skillsObejct.RemoveAt(skill);
                    skillsRepeat.RemoveAt(skill);
                }
            }
            if (skillCount >= 3)
            {
                return;
            }
        }
    }
    public void setRocketDamage()
    {
        carCombat.setRocketDamage(rocketDamagePlus);
        skillIconActive(0);
        skillsSetActive();
    }

    public void setFireballCount()
    {
        carCombat.setFireballCount();
        skillIconActive(1);
        skillsSetActive();
    }
    
    public void setRocketRadius()
    {
        carCombat.setRocketRadius(rocketRadiusPlus);
        skillIconActive(2);
        skillsSetActive();
    }

    public void setRocketDistance()
    {
        carCombat.setRocketDistance(rocketDistancePlus);
        skillIconActive(3);
        skillsSetActive();
    }

    public void setRocketCooldown()
    {
        carCombat.setRocketCooldown(rocketCooldownPlus);
        skillIconActive(4);
        skillsSetActive();
    }

    public void setRocketCount()
    {
        carCombat.setRocketCount();
        skillIconActive(5);
        skillsSetActive();
    }

    public void getShield()
    {
        carCombat.getShield();
        skillIconActive(6);
        skillsSetActive();
    }

    public void getHeal()
    {
        carCombat.getHeal();
        skillIconActive(7);
        skillsSetActive();
    }

    public void setMagnetRadius()
    {
        carCombat.setMagnetRadius(magnetRadiusMultiplier);
        skillIconActive(8);
        skillsSetActive();
    }

    void skillsSetActive()
    {
        foreach(GameObject g in skillObjectsArray)
        {
            g.SetActive(false);
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    void skillIconActive(int chooseSkill)
    {
        Debug.Log(chooseSkill);
        gettingSkillsObject[gettingSkillsCount].SetActive(true);
        gettingSkillsImage[gettingSkillsCount].sprite = skillObjectsArray[chooseSkill].GetComponent<SkillRepeat>().iconImage;
        gettingSkillsCount++;
    }

}
