using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dictus.Utils;

namespace Dictus
{
    public class Adapter : Singleton<Adapter>
    {
        private List<IControllable> controllables_ = new List<IControllable>();
        protected override void Initialize()
        {
        }

        private void Update()
        {
            foreach (IControllable con in controllables_)
            {
                foreach (KeyValuePair<string, Action> set in con.actionSet.entireSet)
                {
                    Action action = set.Value;
                    if (Input.GetKeyDown(action.associatedKey))
                    {
                        action.ExecuteKeyDownAction();
                    }
                    if (Input.GetKey(action.associatedKey))
                    {
                        action.ExecuteKeyPressedAction();
                    }
                    if (Input.GetKeyUp(action.associatedKey))
                    {
                        action.ExecuteKeyUpAction();
                    }
                    action.ExecuteContinuosAction(Input.mousePosition);
                }
            }
        }

        public void Register(IControllable controllable)
        {
            IControllable con = controllables_.Find(ctrl => ctrl == controllable);
            if (con == null)
                controllables_.Add(controllable);
        }

        public void ReleaseAllControl()
        {
            controllables_.Clear();
        }

        public void ReleaseControl(IControllable toBeReleased)
        {
            controllables_.Remove(toBeReleased);
        }

        public List<IControllable> controllables { get { return controllables_; } }
    }
}
