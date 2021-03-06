// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Collections.Specialized;

namespace System.Windows.Forms
{
    // this class encapsulates the logic for Renderer and RenderMode so it can
    // be shared across classes.
    internal class ToolStripRendererSwitcher
    {
        private static readonly int stateUseDefaultRenderer = BitVector32.CreateMask();
        private static readonly int stateAttachedRendererChanged = BitVector32.CreateMask(stateUseDefaultRenderer);

        private ToolStripRenderer renderer;
        private Type currentRendererType = typeof(Type);
        private BitVector32 state;

        private readonly ToolStripRenderMode defaultRenderMode = ToolStripRenderMode.ManagerRenderMode;

        public ToolStripRendererSwitcher(Control owner, ToolStripRenderMode defaultRenderMode) : this(owner)
        {
            this.defaultRenderMode = defaultRenderMode;
            RenderMode = defaultRenderMode;
        }

        public ToolStripRendererSwitcher(Control owner)
        {
            state[stateUseDefaultRenderer] = true;
            state[stateAttachedRendererChanged] = false;
            owner.Disposed += new EventHandler(OnControlDisposed);
            owner.VisibleChanged += new EventHandler(OnControlVisibleChanged);
            if (owner.Visible)
            {
                OnControlVisibleChanged(owner, EventArgs.Empty);
            }
        }

        public ToolStripRenderer Renderer
        {
            get
            {
                if (RenderMode == ToolStripRenderMode.ManagerRenderMode)
                {
                    return ToolStripManager.Renderer;
                }

                // always return a valid renderer so our paint code
                // doesn't have to be bogged down by checks for null.

                state[stateUseDefaultRenderer] = false;
                if (renderer is null)
                {
                    Renderer = ToolStripManager.CreateRenderer(RenderMode);
                }

                return renderer;
            }
            set
            {
                // if the value happens to be null, the next get
                // will autogenerate a new ToolStripRenderer.
                if (renderer != value)
                {
                    state[stateUseDefaultRenderer] = (value is null);
                    renderer = value;
                    currentRendererType = (renderer is not null) ? renderer.GetType() : typeof(Type);

                    OnRendererChanged(EventArgs.Empty);
                }
            }
        }

        public ToolStripRenderMode RenderMode
        {
            get
            {
                if (state[stateUseDefaultRenderer])
                {
                    return ToolStripRenderMode.ManagerRenderMode;
                }

                if (renderer is not null && !renderer.IsAutoGenerated)
                {
                    return ToolStripRenderMode.Custom;
                }

                // check the type of the currently set renderer.
                // types are cached as this may be called frequently.
                if (currentRendererType == ToolStripManager.s_professionalRendererType)
                {
                    return ToolStripRenderMode.Professional;
                }

                if (currentRendererType == ToolStripManager.s_systemRendererType)
                {
                    return ToolStripRenderMode.System;
                }

                return ToolStripRenderMode.Custom;
            }
            set
            {
                //valid values are 0x0 to 0x3
                SourceGenerated.EnumValidator.Validate(value);
                if (value == ToolStripRenderMode.Custom)
                {
                    throw new NotSupportedException(SR.ToolStripRenderModeUseRendererPropertyInstead);
                }

                if (value == ToolStripRenderMode.ManagerRenderMode)
                {
                    if (!state[stateUseDefaultRenderer])
                    {
                        state[stateUseDefaultRenderer] = true;
                        OnRendererChanged(EventArgs.Empty);
                    }
                }
                else
                {
                    state[stateUseDefaultRenderer] = false;
                    Renderer = ToolStripManager.CreateRenderer(value);
                }
            }
        }

        public event EventHandler RendererChanged;

        private void OnRendererChanged(EventArgs e)
        {
            RendererChanged?.Invoke(this, e);
        }

        private void OnDefaultRendererChanged(object sender, EventArgs e)
        {
            if (state[stateUseDefaultRenderer])
            {
                OnRendererChanged(e);
            }
        }

        private void OnControlDisposed(object sender, EventArgs e)
        {
            if (state[stateAttachedRendererChanged])
            {
                ToolStripManager.RendererChanged -= new EventHandler(OnDefaultRendererChanged);
                state[stateAttachedRendererChanged] = false;
            }
        }

        private void OnControlVisibleChanged(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                if (control.Visible)
                {
                    if (!state[stateAttachedRendererChanged])
                    {
                        ToolStripManager.RendererChanged += new EventHandler(OnDefaultRendererChanged);
                        state[stateAttachedRendererChanged] = true;
                    }
                }
                else
                {
                    if (state[stateAttachedRendererChanged])
                    {
                        ToolStripManager.RendererChanged -= new EventHandler(OnDefaultRendererChanged);
                        state[stateAttachedRendererChanged] = false;
                    }
                }
            }
        }

        public bool ShouldSerializeRenderMode()
        {
            // We should NEVER serialize custom.
            return (RenderMode != defaultRenderMode && RenderMode != ToolStripRenderMode.Custom);
        }

        public void ResetRenderMode()
        {
            RenderMode = defaultRenderMode;
        }
    }
}
