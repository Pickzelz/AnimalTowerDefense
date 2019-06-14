using System.Collections.Generic;

public interface ISkillDamageHolder
{
    float Damage { get; set; }
    float Range { get; set; }
    List<string> TargetTags { get; set; }
}
