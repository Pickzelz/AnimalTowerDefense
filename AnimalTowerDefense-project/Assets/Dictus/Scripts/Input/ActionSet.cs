using UnityEngine;
using System.Collections.Generic;

namespace Dictus
{
    public class ActionSet
    {
        #region methods
        public ActionSet()
        {
            set_ = new Dictionary<string, Action>();
        }
        public ActionSet(List<KeyValuePair<string, KeyCode>> inputSet)
        {
            set_ = new Dictionary<string, Action>();
            InitiateActionSet(inputSet);
        }

        public void InitiateActionSet(List<KeyValuePair<string, KeyCode>> inputSet)
        {
            foreach (KeyValuePair<string, KeyCode> pair in inputSet)
            {
                set_[pair.Key] = new Action(pair.Value);
            }
        }

        public Action GetAction(string actionName)
        {
            try { return set_[actionName]; }
            catch (KeyNotFoundException e)
            {
                Debug.LogError("ERROR KeyNotFound!! " + e.ToString());
                return null;
            }
        }


        #endregion

        #region members
        //the string key references action name
        private Dictionary<string, Action> set_;
        #endregion

        #region indexer
        public Action this[string i] { get { return set_[i]; } }
        #endregion

        #region properties
        public Dictionary<string, Action> entireSet { get { return set_; } }
        #endregion
    }
}
