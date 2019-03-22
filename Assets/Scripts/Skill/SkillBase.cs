using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected int id = -1;

    protected abstract void BeginSkill();
    protected abstract void OverSkill();
}
