using Strawhenge.Common;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class SitAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Sit Animator Setup";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<SitAnimatorSetupWizard>(Name, "Set Up");
        }

        [SerializeField] AnimatorController _animatorController;

        AnimatorController _selectedController;
        string[] _layers;
        int _selectedLayerIndex;

        protected override bool DrawWizardGUI()
        {
            if (_selectedController == null)
                return base.DrawWizardGUI();

            var result = base.DrawWizardGUI();

            _selectedLayerIndex = EditorGUILayout.Popup("Layer", _selectedLayerIndex, _layers);

            return result;
        }

        void OnWizardUpdate()
        {
            if (_selectedController == _animatorController) return;
            _selectedController = _animatorController;

            if (_selectedController == null) return;

            UpdateLayers();
        }

        void OnWizardCreate()
        {
            if (_selectedController == null)
            {
                Debug.LogWarning("No controller selected.");
                return;
            }

            SitAnimatorSetup.Setup(_animatorController, _selectedLayerIndex);
        }

        void UpdateLayers()
        {
            _layers = _selectedController.layers.ToArray(layer => layer.name);
        }
    }
}