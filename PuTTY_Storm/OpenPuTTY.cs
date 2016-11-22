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
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;

namespace PuTTY_Storm
{
    public class ProcessInfo
    {
        public IntPtr mainhandle;
        public Process process;
        public int count;
        public Panel panel;
    }

    /// <summary>
    /// Handle creation of new putty.exe window sessions.
    /// </summary>
    class OpenPuTTY
    {
        public ProcessInfo start_putty(Panel panel, int count, Process process, string hostname,
            string username, string password, string putty_path, SplitContainer SessionsSplitContainer)
        {
            ProcessInfo myProcessInfo = new ProcessInfo();

            // Check if hostname is resolvable - if not return empty struct
            IPHostEntry host;
            try
            {
                host = Dns.GetHostEntry(hostname);
            }
            catch
            {
                Label error = new Label();
                error.Dock = DockStyle.Fill;
                error.Size = new Size(800, 200);
                error.AutoSize = false;
                error.Text = "Host " + "'" + hostname + "'" + " does not exist!";
                error.Font = new Font("Calibri", 28);
                error.TextAlign = ContentAlignment.MiddleCenter;
                panel.Controls.Add(error);
                return myProcessInfo;
            }

            ProcessStartInfo processStartInfo;
            if (password != "")
            {
                processStartInfo = new ProcessStartInfo(putty_path, "-ssh -2 -l " + username + " -pw " + password + " " + hostname);
            } else
            {
                processStartInfo = new ProcessStartInfo(putty_path, "-ssh -2 -l " + username + " " + hostname);
            }
            
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;

            process.StartInfo = processStartInfo;         
            process.Start();
            process.WaitForInputIdle();

            myProcessInfo.mainhandle = process.MainWindowHandle;
            myProcessInfo.count = count;

            NativeMethods.SetParent(myProcessInfo.mainhandle, panel.Handle);

            myProcessInfo.panel = panel;
            myProcessInfo.process = process;
            NativeMethods.SetWindowLong(myProcessInfo.mainhandle, NativeMethods.GWL_STYLE, NativeMethods.WS_VISIBLE + NativeMethods.WS_CAPTION);
            NativeMethods.MoveWindow(myProcessInfo.mainhandle, -8, -30, SessionsSplitContainer.Panel1.Width + 5, SessionsSplitContainer.Panel1.Height + 5, true);

            StreamWriter inputWriter = process.StandardInput;
            StreamReader outputReader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;

            return myProcessInfo;
        }
    }
}
