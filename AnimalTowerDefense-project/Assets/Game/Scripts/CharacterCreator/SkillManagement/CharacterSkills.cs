using UnityEngine;
namespace FISkill
{
    public class CharacterSkills : Actions
    {

        public override void OnRpcUseFinish()
        {
            base.OnRpcUseFinish();
            Debug.Log("OnRpcUseFinish override");

        }
    }
}
