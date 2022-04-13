using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;

namespace BT
{
    [SerializeField]
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeGraphView behaviorTreeView;
        private BehaviorTreeInspector nodeInspectorView;
        private IMGUIContainer blackboardInspectorView;
        private Label treeViewLabel;

        private SerializedObject serializedBlackboard;
        private SerializedProperty blackboardProperty;

        [MenuItem("Window/AI/Behavior Tree Editor")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("Behavior Tree Editor");
        }

        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            if (Selection.activeObject is BehaviorTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // Load UXML
            var VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UnityBehaviorTreeSystem/Editor/BehaviorTree/BehaviorTreeEditor.uxml");
            VisualTree.CloneTree(rootVisualElement);

            behaviorTreeView = rootVisualElement.Q<BehaviorTreeGraphView>();
            nodeInspectorView = rootVisualElement.Q<BehaviorTreeInspector>();
            
            // Initialize blackboard inspector view in behavior tree editor
            blackboardInspectorView = rootVisualElement.Q<IMGUIContainer>("BlackboardInspector");
            blackboardInspectorView.onGUIHandler = () =>
            {
                if(serializedBlackboard != null && serializedBlackboard.targetObject == behaviorTreeView.Tree.blackboard)
                {
                    serializedBlackboard.Update();
                    EditorGUILayout.PropertyField(blackboardProperty);
                    serializedBlackboard.ApplyModifiedProperties();
                }
                else
                {
                    EditorGUILayout.LabelField("No blackboard assigned");
                }
            };
            treeViewLabel = rootVisualElement.Q<Label>("Tree_View_Label");

            // Initialize Callback for when the node selection changes from a node to another node
            behaviorTreeView.OnNodeSelected = OnNodeSelectionChange;
            behaviorTreeView.onNodeVisualElementSelected = OnNodeVisualElementSelectionChange;

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (tree != null)
            {
                treeViewLabel.text = " Tree View: " + tree.name;
                behaviorTreeView.Tree = tree;
                
                // serialized properties used for inspecting blackboard asset in the 
                // behavior tree editor view.
                if(tree.blackboard != null)
                {
                    serializedBlackboard = new SerializedObject(tree.blackboard);
                    blackboardProperty = serializedBlackboard.FindProperty("blackboardProperties");
                }
                behaviorTreeView.PopulateView(tree);
            }
        }

        // Called when the node selection changes
        private void OnNodeSelectionChange(BT_NodeView nodeView)
        {
            BehaviorTreeSelectionManager.selectedObject = nodeView;
            if (nodeInspectorView != null)
            {
                nodeInspectorView.UpdateInspector(nodeView.node);
            }
        }

        private void OnNodeVisualElementSelectionChange(BT_NodeVisualElement nodeVisualElement)
        {
            BehaviorTreeSelectionManager.selectedObject = nodeVisualElement;
            if (nodeInspectorView != null)
            {
                BT_DecoratorView decoratorView = nodeVisualElement as BT_DecoratorView;
                if(decoratorView != null)
                {
                    nodeInspectorView.UpdateInspector(decoratorView.node);
                }

                BT_ServiceView serviceView = nodeVisualElement as BT_ServiceView;
                if(serviceView != null)
                {
                    nodeInspectorView.UpdateInspector(serviceView.node);
                }
            }
        }

    }
}