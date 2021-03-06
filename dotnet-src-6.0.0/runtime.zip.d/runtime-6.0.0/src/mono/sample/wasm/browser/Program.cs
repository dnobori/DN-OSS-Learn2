// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace Sample
{
    public class Test
    {
        [DebuggerDisplay ("Some {Val1} Value {Val2} End")]
        class WithDisplayString
        {
            internal string Val1 = "one";

            public int Val2 { get { return 2; } }
        }

        class WithToString
        {
            public override string ToString ()
            {
                return "SomeString";
            }
        }

        [DebuggerDisplay ("{GetDebuggerDisplay(), nq}")]
        class DebuggerDisplayMethodTest
        {
            int someInt = 32;
            int someInt2 = 43;

            string GetDebuggerDisplay ()
            {
                return "First Int:" + someInt + " Second Int:" + someInt2;
            }
        }

        [DebuggerTypeProxy(typeof(TheProxy))]
        class WithProxy
        {
            public string Val1 {
                get { return "one"; }
            }
        }

        class TheProxy
        {
            WithProxy wp;

            public TheProxy (WithProxy wp)
            {
                this.wp = wp;
            }

            public string Val2 {
                get { return wp.Val1; }
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine ("Hello, World!");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int TestMeaning()
        {
            List<int> myList = new List<int>{ 1, 2, 3, 4 };
            Console.WriteLine(myList);
            return 42;
        }
    }
}
