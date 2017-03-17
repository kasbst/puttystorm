/*
 * Copyright (c) 2017 Karol Sebesta
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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public class GlobalHotKeysWorker : IDisposable
    {
        public string HotKeyName;
        public short HotKeyID = 0;

        public GlobalHotKeysWorker(string _HotKeyName)
        {
            this.HotKeyName = _HotKeyName;
        }

        /// <summary>
        /// The ID for the hotkey
        /// </summary>
        public short HOTKEYID { get; private set; }

        /// <summary>
        /// SessionsForm handle
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Register the hotkey
        /// </summary>
        public void RegisterGlobalHotKey(int hotkey, int modifiers)
        {
            UnregisterGlobalHotKey();
            _RegisterGlobalHotKey(hotkey, modifiers);
            Console.WriteLine("## REGISTERING GlobalHotKey: " + this.HotKeyName + " With ID: " + HOTKEYID);
        }

        /// <summary>
        /// Helper method to register the hotkey
        /// </summary>
        public void _RegisterGlobalHotKey(int hotkey, int modifiers)
        {
            UnregisterGlobalHotKey();

            try
            {
                string atomName = Thread.CurrentThread.ManagedThreadId.ToString("X8") + this.HotKeyName;
                HOTKEYID = NativeMethods.GlobalAddAtom(atomName);

                if (HOTKEYID == 0)
                    throw new Exception("Unable to get unique hotkey ID. Error: " + Marshal.GetLastWin32Error().ToString());

                // register the hotkey, throw if any error
                if (!NativeMethods.RegisterHotKey(Handle, HOTKEYID, (uint)modifiers, (uint)hotkey))
                    throw new Exception("Unable to register hotkey " + this.HotKeyName + " - Error: " + Marshal.GetLastWin32Error().ToString() +
                        " GlobalHotKeys combination for " + this.HotKeyName + " is already reserved by the system or another application!" +
                        " Please select different combination or exit the application!");

            }
            catch (Exception ex)
            {
                // clean up if hotkey registration failed
                Dispose();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Unregister the hotkey
        /// </summary>
        public void UnregisterGlobalHotKey()
        {
            if (HOTKEYID != 0)
            {
                Console.WriteLine("## UNREGISTERING GlobalHotKey: " + this.HotKeyName + " With ID: " + HOTKEYID);
                NativeMethods.UnregisterHotKey(Handle, HOTKEYID);
                // clean up the atom list
                NativeMethods.GlobalDeleteAtom(HOTKEYID);
                HOTKEYID = 0;            
            }
        }

        public void Dispose()
        {
            UnregisterGlobalHotKey();
        }
    }
}
