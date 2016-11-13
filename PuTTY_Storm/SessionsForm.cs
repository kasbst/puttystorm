/*
 * Copyright (c) 2016 Karol Sebesta
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * This software is inspired by Jim Radford's http://www.jimradford.com
 * SuperPutty and various http://stackoverflow.com/ user ideas.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PuTTY_Storm
{
    class SessionsForm : FormHelper
    {
        List<ProcessInfo> my_ProcessInfo_List;
        TabControl tabcontrol1;
        const int MYACTION_HOTKEY_ID1 = 1;
        const int MYACTION_HOTKEY_ID2 = 2;

        /// <summary>
        /// Initialize PuTTY STORM sessions form (Form2) and register hotkeys for
        /// easy and fast swithing between TabPages.
        /// </summary>
        public SessionsForm(List<ProcessInfo> _my_ProcessInfo_List, TabControl _tabcontrol1)
        {
            this.tabcontrol1 = _tabcontrol1;
            this.my_ProcessInfo_List = _my_ProcessInfo_List;

            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID1, 2, (int)Keys.Tab);
            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID2, 6, (int)Keys.Tab);

            WindowEvents = new GlobalWindowEvents();
            SessionsForm.WindowEvents.SystemSwitch += new EventHandler<GlobalWindowEventArgs>(OnSystemSwitch);
        }

        bool isSwitchingViaAltTab = false;

        /// <summary>
        /// OnSystemSwitch event handler. Detects when we switch between application windows
        /// using ALT+TAB.
        /// </summary>
        void OnSystemSwitch(object sender, GlobalWindowEventArgs e)
        {
            switch (e.eventType)
            {
                case (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHSTART:
                    this.isSwitchingViaAltTab = true;
                    Console.WriteLine("ATLTAB START");
                    Console.WriteLine(isSwitchingViaAltTab);
                    break;
                case (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHEND:
                    this.isSwitchingViaAltTab = false;
                    Console.WriteLine("ATLTAB  FINISH");
                    Console.WriteLine(isSwitchingViaAltTab);
                    break;
            }
        }

        /// <summary>
        /// Override WndProc, so we can handle focus when switching between applications 
        /// using ALT+TAB and using registered hotkeys.
        /// </summary>
        protected override void WndProc(ref Message m)
        {          
            base.WndProc(ref m);
            if (my_ProcessInfo_List.Count > 0)
            {
                // Trap WM_ACTIVATE when we get active
                if (m.Msg == 6 && m.WParam.ToInt32() == 1)
                {
                    if (Control.FromHandle(m.LParam) == null)
                    {
                        if (isSwitchingViaAltTab == true)
                        {
                            NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                        }
                    }
                }

                if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID1)
                {
                    Console.WriteLine("HOTKEY ctrl+tab pressed");

                    if (tabcontrol1.TabCount == tabcontrol1.SelectedIndex + 1)
                    {
                        tabcontrol1.SelectedIndex = 0;
                    }
                    else
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.SelectedIndex + 1;
                    }
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                }

                if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID2)
                {
                    Console.WriteLine("HOTKEY ctrl+shift+tab pressed");

                    if (tabcontrol1.SelectedIndex == 0)
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.TabCount - 1;
                    }
                    else
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.SelectedIndex - 1;
                    }
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                }
            }

        }

        #region Global HotKeys
        public class KeyHandler
        {
            private int key;
            private IntPtr hWnd;
            private int id;

            public KeyHandler(Keys key, Form form)
            {
                this.key = (int)key;
                this.hWnd = form.Handle;
                id = this.GetHashCode();
            }

            public override int GetHashCode()
            {
                return key ^ hWnd.ToInt32();
            }

            public bool Register()
            {
                return NativeMethods.RegisterHotKey(hWnd, id, 0, key);
            }

            public bool Unregiser()
            {
                return NativeMethods.UnregisterHotKey(hWnd, id);
            }
        }
        #endregion

        #region Properties
        public static GlobalWindowEvents WindowEvents { get; private set; }
        #endregion
    }
}
