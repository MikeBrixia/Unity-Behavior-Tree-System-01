using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using BT.Runtime;

namespace BT.Editor
{
    ///<summary>
    /// Class used to display service views.
    ///</summary>
    public class BT_ServiceView : BT_NodeVisualElement
    {
        private VisualElement serviceBorder;
        private Label serviceNameLabel;
        private Label serviceFrequencyLabel;
        private Label serviceDescriptionLabel;
        private Label serviceUpdateLabel;

        public BT_ServiceView(BT_NodeView parentView, BT_Node node, string filepath) : base(parentView, node, filepath)
        {
        }
        
        ///<summary>
        /// Called on node view creation and used to initialize UI elements
        /// for this node view.
        ///</summary>
        protected override void InitializeUIElements()
        {
            // Get visual tree asset elements
            serviceBorder = contentContainer.Q<VisualElement>("ServiceBorder");
            serviceNameLabel = contentContainer.Q<Label>("ServiceName");
            serviceFrequencyLabel = contentContainer.Q<Label>("ServiceUpdateFrequencyLabel");
            serviceDescriptionLabel = contentContainer.Q<Label>("ServiceDescription");
            serviceUpdateLabel = contentContainer.Q<Label>("ServiceUpdateFrequencyLabel");
            
            // Initialize frequency label
            serviceFrequencyLabel.bindingPath = "frequencyDescription";
            serviceFrequencyLabel.Bind(new SerializedObject(node));

            // Initialize name label
            serviceNameLabel.bindingPath = "nodeName";
            serviceNameLabel.Bind(new SerializedObject(node));
            
            // Initialize description label
            serviceDescriptionLabel.bindingPath = "description";
            serviceDescriptionLabel.Bind(new SerializedObject(node));
            
            parentView.serviceContainer.Add(this);
            parentView.serviceViews.Add(this);
        }
        
        ///<summary>
        /// Called when this service view it's selected
        ///</summary>
        public override void OnSelected(MouseDownEvent evt)
        {
            base.OnSelected(evt);
            ShowSelectionBorder(serviceBorder, 2f, Color.yellow);
        }
        
        ///<summary>
        /// Called when this service view it's unselected
        ///</summary>
        public override void OnUnselected()
        {
           ShowSelectionBorder(serviceBorder, 2f, Color.black);
        }
        
        ///<summary>
        /// Called when the mouse cursor enters the visual element.
        ///</summary>
        ///<param name="evt"> Mouse event </param>
        protected override void OnMouseEnter(MouseEnterEvent evt)
        {
            base.OnMouseEnter(evt);
        }
        
        ///<summary>
        /// Called when the mouse cursor leaves the visual element.
        ///</summary>
        ///<param name="evt"> Mouse event </param>
        protected override void OnMouseLeave(MouseLeaveEvent evt)
        {
            base.OnMouseLeave(evt);
        }
    }
}

