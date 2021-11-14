﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
    /// <summary>
    ///  A ToolStripButton that can display a popup.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public partial class ToolStripDropDownButton : ToolStripDropDownItem
    {
        private bool showDropDownArrow = true;
        private byte openMouseId;

        /// <summary>
        ///  Constructs a ToolStripButton that can display a popup.
        /// </summary>
        public ToolStripDropDownButton()
        {
            Initialize();
        }

        public ToolStripDropDownButton(string text) : base(text, null, (EventHandler)null)
        {
            Initialize();
        }

        public ToolStripDropDownButton(Image image) : base(null, image, (EventHandler)null)
        {
            Initialize();
        }

        public ToolStripDropDownButton(string text, Image image) : base(text, image, (EventHandler)null)
        {
            Initialize();
        }

        public ToolStripDropDownButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
        {
            Initialize();
        }

        public ToolStripDropDownButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
        {
            Initialize();
        }

        public ToolStripDropDownButton(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
        {
            Initialize();
        }

        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new ToolStripDropDownButtonAccessibleObject(this);
        }

        [DefaultValue(true)]
        public new bool AutoToolTip
        {
            get => base.AutoToolTip;
            set => base.AutoToolTip = value;
        }

        protected override bool DefaultAutoToolTip
        {
            get
            {
                return true;
            }
        }

        [DefaultValue(true)]
        [SRDescription(nameof(SR.ToolStripDropDownButtonShowDropDownArrowDescr))]
        [SRCategory(nameof(SR.CatAppearance))]
        public bool ShowDropDownArrow
        {
            get
            {
                return showDropDownArrow;
            }
            set
            {
                if (showDropDownArrow != value)
                {
                    showDropDownArrow = value;
                    InvalidateItemLayout(PropertyNames.ShowDropDownArrow);
                }
            }
        }

        /// <summary>
        ///  Creates an instance of the object that defines how image and text
        ///  gets laid out in the ToolStripItem
        /// </summary>
        private protected override ToolStripItemInternalLayout CreateInternalLayout()
        {
            return new ToolStripDropDownButtonInternalLayout(this);
        }

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            // AutoGenerate a ToolStrip DropDown - set the property so we hook events
            return new ToolStripDropDownMenu(this, /*isAutoGenerated=*/true);
        }

        /// <summary>
        ///  Called by all constructors of ToolStripButton.
        /// </summary>
        private void Initialize()
        {
            SupportsSpaceKey = true;
        }

        /// <summary>
        ///  Overriden to invoke displaying the popup.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((Control.ModifierKeys != Keys.Alt) &&
                (e.Button == MouseButtons.Left))
            {
                if (DropDown.Visible)
                {
                    ToolStripManager.ModalMenuFilter.CloseActiveDropDown(DropDown, ToolStripDropDownCloseReason.AppClicked);
                }
                else
                {
                    // opening should happen on mouse down.
                    Debug.Assert(ParentInternal is not null, "Parent is null here, not going to get accurate ID");
                    openMouseId = (ParentInternal is null) ? (byte)0 : ParentInternal.GetMouseId();
                    ShowDropDown(/*mousePush =*/true);
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((Control.ModifierKeys != Keys.Alt) &&
                (e.Button == MouseButtons.Left))
            {
                Debug.Assert(ParentInternal is not null, "Parent is null here, not going to get accurate ID");
                byte closeMouseId = (ParentInternal is null) ? (byte)0 : ParentInternal.GetMouseId();
                if (closeMouseId != openMouseId)
                {
                    openMouseId = 0;  // reset the mouse id, we should never get this value from toolstrip.
                    ToolStripManager.ModalMenuFilter.CloseActiveDropDown(DropDown, ToolStripDropDownCloseReason.AppClicked);
                    Select();
                }
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            openMouseId = 0;  // reset the mouse id, we should never get this value from toolstrip.
            base.OnMouseLeave(e);
        }

        /// <summary>
        ///  Inheriting classes should override this method to handle this event.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Owner is not null)
            {
                ToolStripRenderer renderer = Renderer;
                Graphics g = e.Graphics;

                renderer.DrawDropDownButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));

                if ((DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
                {
                    renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(g, this, InternalLayout.ImageRectangle));
                }

                if ((DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
                {
                    renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(g, this, Text, InternalLayout.TextRectangle, ForeColor, Font, InternalLayout.TextFormat));
                }

                if (ShowDropDownArrow)
                {
                    Rectangle dropDownArrowRect = (InternalLayout is ToolStripDropDownButtonInternalLayout layout) ? layout.DropDownArrowRect : Rectangle.Empty;

                    Color arrowColor;
                    if (Selected && !Pressed && SystemInformation.HighContrast)
                    {
                        arrowColor = Enabled ? SystemColors.HighlightText : SystemColors.ControlDark;
                    }
                    else
                    {
                        arrowColor = Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
                    }

                    renderer.DrawArrow(new ToolStripArrowRenderEventArgs(g, this, dropDownArrowRect, arrowColor, ArrowDirection.Down));
                }
            }
        }

        protected internal override bool ProcessMnemonic(char charCode)
        {
            // checking IsMnemonic is not necessary - toolstrip does this for us.
            if (HasDropDownItems)
            {
                Select();
                ShowDropDown();
                return true;
            }

            return false;
        }
    }
}