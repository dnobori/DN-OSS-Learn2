// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Diagnostics;

namespace System.Windows.Forms
{
    internal class Command : WeakReference
    {
        private static Command[] cmds;
        private static int icmdTry;
        private static readonly object internalSyncObject = new object();
        private const int idMin = 0x00100;
        private const int idLim = 0x10000;

        internal int id;

        public Command(ICommandExecutor target)
            : base(target, false)
        {
            AssignID(this);
        }

        public virtual int ID
        {
            get
            {
                return id;
            }
        }

        protected static void AssignID(Command cmd)
        {
            lock (internalSyncObject)
            {
                int icmd;

                if (cmds is null)
                {
                    cmds = new Command[20];
                    icmd = 0;
                }
                else
                {
                    Debug.Assert(cmds.Length > 0, "why is cmds.Length zero?");
                    Debug.Assert(icmdTry >= 0, "why is icmdTry negative?");

                    int icmdLim = cmds.Length;

                    if (icmdTry >= icmdLim)
                    {
                        icmdTry = 0;
                    }

                    // First look for an empty slot (starting at icmdTry).
                    for (icmd = icmdTry; icmd < icmdLim; icmd++)
                    {
                        if (cmds[icmd] is null)
                        {
                            goto FindSlotComplete;
                        }
                    }

                    for (icmd = 0; icmd < icmdTry; icmd++)
                    {
                        if (cmds[icmd] is null)
                        {
                            goto FindSlotComplete;
                        }
                    }

                    // All slots have Command objects in them. Look for a command
                    // with a null referent.
                    for (icmd = 0; icmd < icmdLim; icmd++)
                    {
                        if (cmds[icmd].Target is null)
                        {
                            goto FindSlotComplete;
                        }
                    }

                    // Grow the array.
                    icmd = cmds.Length;
                    icmdLim = Math.Min(idLim - idMin, 2 * icmd);

                    if (icmdLim <= icmd)
                    {
                        // Already at maximal size. Do a garbage collect and look again.
                        GC.Collect();
                        for (icmd = 0; icmd < icmdLim; icmd++)
                        {
                            if (cmds[icmd] is null || cmds[icmd].Target is null)
                            {
                                goto FindSlotComplete;
                            }
                        }

                        throw new ArgumentException(SR.CommandIdNotAllocated);
                    }
                    else
                    {
                        Command[] newCmds = new Command[icmdLim];
                        Array.Copy(cmds, 0, newCmds, 0, icmd);
                        cmds = newCmds;
                    }
                }

            FindSlotComplete:

                cmd.id = icmd + idMin;
                Debug.Assert(cmd.id >= idMin && cmd.id < idLim, "generated command id out of range");

                cmds[icmd] = cmd;
                icmdTry = icmd + 1;
            }
        }

        public static bool DispatchID(int id)
        {
            Command cmd = GetCommandFromID(id);
            if (cmd is null)
            {
                return false;
            }

            return cmd.Invoke();
        }

        protected static void Dispose(Command cmd)
        {
            lock (internalSyncObject)
            {
                if (cmd.id >= idMin)
                {
                    cmd.Target = null;
                    if (cmds[cmd.id - idMin] == cmd)
                    {
                        cmds[cmd.id - idMin] = null;
                    }

                    cmd.id = 0;
                }
            }
        }

        public virtual void Dispose()
        {
            if (id >= idMin)
            {
                Dispose(this);
            }
        }

        public static Command GetCommandFromID(int id)
        {
            lock (internalSyncObject)
            {
                if (cmds is null)
                {
                    return null;
                }

                int i = id - idMin;
                if (i < 0 || i >= cmds.Length)
                {
                    return null;
                }

                return cmds[i];
            }
        }

        public virtual bool Invoke()
        {
            object target = Target;
            if (!(target is ICommandExecutor executor))
            {
                return false;
            }

            executor.Execute();
            return true;
        }
    }
}
