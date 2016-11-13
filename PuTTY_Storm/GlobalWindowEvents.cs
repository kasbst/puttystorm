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

namespace PuTTY_Storm
{
    public class GlobalWindowEventArgs : EventArgs
    {
        public IntPtr hwnd { get; private set; }
        public uint eventType { get; private set; }

        public GlobalWindowEventArgs(IntPtr hwnd, uint eventType)
        {
            this.hwnd = hwnd;
            this.eventType = eventType;
        }
    }

    public class GlobalWindowEvents
    {
        public event EventHandler<GlobalWindowEventArgs> SystemSwitch;
        IntPtr m_hWinEventHook;
        NativeMethods.WinEventDelegate lpfnWinEventProc;

        public GlobalWindowEvents()
        {
            uint eventMin = (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHSTART;
            uint eventMax = (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHEND;
            lpfnWinEventProc = new NativeMethods.WinEventDelegate(WinEventProc);
            this.m_hWinEventHook = NativeMethods.SetWinEventHook(eventMin, eventMax, IntPtr.Zero, lpfnWinEventProc, 0, 0, NativeMethods.WINEVENT_OUTOFCONTEXT);
        }

        void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (this.SystemSwitch != null)
                this.SystemSwitch(this, new GlobalWindowEventArgs(hwnd, eventType));
        }

        ~GlobalWindowEvents()
        {
            NativeMethods.UnhookWinEvent(this.m_hWinEventHook);
        }
    }
}
