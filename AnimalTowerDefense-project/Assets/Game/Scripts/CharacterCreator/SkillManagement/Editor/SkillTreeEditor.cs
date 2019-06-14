using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace FISkill
{
    [CustomEditor(typeof(Skill))]
    [CanEditMultipleObjects]
    public class SkillTreeEditor : TreeView
    {
        public int idObject = 0;

        public SkillTreeEditor(TreeViewState treeViewState): base(treeViewState)
        {
            Reload();
        }
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var animals = new TreeViewItem { id = 1, displayName = "Animals" };
            var mammals = new TreeViewItem { id = 2, displayName = "Mammals" };
            var tiger = new TreeViewItem { id = 3, displayName = "Tiger" };
            var elephant = new TreeViewItem { id = 4, displayName = "Elephant" };
            var okapi = new TreeViewItem { id = 5, displayName = "Okapi" };
            var armadillo = new TreeViewItem { id = 6, displayName = "Armadillo" };
            var reptiles = new TreeViewItem { id = 7, displayName = "Reptiles" };
            var croco = new TreeViewItem { id = 8, displayName = "Crocodile" };
            var lizard = new TreeViewItem { id = 9, displayName = "Lizard" };

            root.AddChild(animals);
            animals.AddChild(mammals);
            animals.AddChild(reptiles);
            mammals.AddChild(tiger);
            mammals.AddChild(elephant);
            mammals.AddChild(okapi);
            mammals.AddChild(armadillo);
            reptiles.AddChild(croco);
            reptiles.AddChild(lizard);

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        private int getId()
        {
            return idObject++;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);

        }
    }
}

