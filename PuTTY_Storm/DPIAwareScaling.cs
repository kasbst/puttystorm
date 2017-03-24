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

using Microsoft.Win32;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public static class DPIAwareScaling
    {
        /// <summary>
        /// DPI Scaling Level                 Value data      Status
        /// ---------------------------------------------------------------
        /// Smaller 100% (default)             96               OK
        /// Medium 125%                        120              OK
        /// Larger 150%                        144              OK
        /// Very Large 175%                    168              OK
        /// Extra Large 200%                   192              OK
        /// Custom 225%                        216              OK
        /// Custom 250%                        240              OK
        /// Custom 275%                        264              N/A
        /// Custom 300%                        288              OK
        /// Custom 400%                        384              N/A
        /// Custom 500%                        480              N/A
        /// </summary>

        private static Graphics graphics { get; set; }

        // PuTTY handle positioning based on DPI of display
        public static int MoveWindowX { get; set; }
        public static int MoveWindowY { get; set; }
        public static int MoveWindowNWidth { get; set; }
        public static int MoveWindowNHeight { get; set; }

        // Additional controls tweaks based on DPI of display
        public static int NewConnectHostTextboxFont { get; set; }
        public static int NewConnectUsernameTextboxFont { get; set; }
        public static int NewConnectPasswordTextboxFont { get; set; }
        public static int NewConnectSearchTextboxFont { get; set; }
        public static int NewConnectNumericUpDownFont { get; set; }
        public static int TabcontrolFont { get; set;  }
        public static int TabControlSplitterDistance { get; set; }
        public static int KotarakSplitterDistance { get; set; }
        public static int SearchSessionConfigTextBoxFont { get; set; }

        /// <summary>
        /// This is evil but we need it to prevent Windows DPI scaling issues :(
        /// </summary>
        public static void SetControlsExtendedDPISettings()
        {
            Console.WriteLine("## Display's DPI:");
            Console.WriteLine("# X: " + graphics.DpiX);
            Console.WriteLine("# Y: " + graphics.DpiY);
            Console.WriteLine("# Windows version: " + Environment.OSVersion.Version);

            if (DPIAwareScaling.graphics.DpiX == 96 && DPIAwareScaling.graphics.DpiY == 96)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -25;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 10;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 10;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 10;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 10;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 10;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 10;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -8;
                    DPIAwareScaling.MoveWindowY = -31;
                    DPIAwareScaling.MoveWindowNWidth = 5;
                    DPIAwareScaling.MoveWindowNHeight = 6;

                    DPIAwareScaling.NewConnectHostTextboxFont = 10;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 10;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 10;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 10;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 10;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 10;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 120 && DPIAwareScaling.graphics.DpiY == 120)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -28;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 11;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 11;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 11;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 11;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 11;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 11;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1050);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -9;
                    DPIAwareScaling.MoveWindowY = -38;
                    DPIAwareScaling.MoveWindowNWidth = 6;
                    DPIAwareScaling.MoveWindowNHeight = 6;

                    DPIAwareScaling.NewConnectHostTextboxFont = 11;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 11;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 11;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 11;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 11;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 11;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1050);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 144 && DPIAwareScaling.graphics.DpiY == 144)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -33;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -12;
                    DPIAwareScaling.MoveWindowY = -50;
                    DPIAwareScaling.MoveWindowNWidth = 15;
                    DPIAwareScaling.MoveWindowNHeight = 12;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 168 && DPIAwareScaling.graphics.DpiY == 168)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -39;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1020);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -12;
                    DPIAwareScaling.MoveWindowY = -52;
                    DPIAwareScaling.MoveWindowNWidth = 15;
                    DPIAwareScaling.MoveWindowNHeight = 12;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1020);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 192 && DPIAwareScaling.graphics.DpiY == 192)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -42;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -13;
                    DPIAwareScaling.MoveWindowY = -58;
                    DPIAwareScaling.MoveWindowNWidth = 15;
                    DPIAwareScaling.MoveWindowNHeight = 13;

                    DPIAwareScaling.NewConnectHostTextboxFont = 12;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 12;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 12;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 12;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 12;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(980);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 216 && DPIAwareScaling.graphics.DpiY == 216)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -45;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1025);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -14;
                    DPIAwareScaling.MoveWindowY = -65;
                    DPIAwareScaling.MoveWindowNWidth = 23;
                    DPIAwareScaling.MoveWindowNHeight = 13;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 11;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1025);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 240 && DPIAwareScaling.graphics.DpiY == 240)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -48;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1050);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -16;
                    DPIAwareScaling.MoveWindowY = -72;
                    DPIAwareScaling.MoveWindowNWidth = 23;
                    DPIAwareScaling.MoveWindowNHeight = 14;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1050);
                }
            }
            else if (DPIAwareScaling.graphics.DpiX == 288 && DPIAwareScaling.graphics.DpiY == 288)
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -51;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1070);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -18;
                    DPIAwareScaling.MoveWindowY = -85;
                    DPIAwareScaling.MoveWindowNWidth = 24;
                    DPIAwareScaling.MoveWindowNHeight = 16;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1070);
                }
            }
            else
            {
                if (DPIAwareScaling.UsingWindows7ClassicTheme())
                {
                    DPIAwareScaling.MoveWindowX = -5;
                    DPIAwareScaling.MoveWindowY = -54;
                    DPIAwareScaling.MoveWindowNWidth = -1;
                    DPIAwareScaling.MoveWindowNHeight = -5;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1070);
                } else
                {
                    DPIAwareScaling.MoveWindowX = -18;
                    DPIAwareScaling.MoveWindowY = -85;
                    DPIAwareScaling.MoveWindowNWidth = 24;
                    DPIAwareScaling.MoveWindowNHeight = 16;

                    DPIAwareScaling.NewConnectHostTextboxFont = 13;
                    DPIAwareScaling.NewConnectUsernameTextboxFont = 13;
                    DPIAwareScaling.NewConnectPasswordTextboxFont = 13;
                    DPIAwareScaling.NewConnectSearchTextboxFont = 13;
                    DPIAwareScaling.NewConnectNumericUpDownFont = 13;
                    DPIAwareScaling.SearchSessionConfigTextBoxFont = 12;
                    DPIAwareScaling.TabcontrolFont = 10;
                    DPIAwareScaling.TabControlSplitterDistance = 75;
                    DPIAwareScaling.KotarakSplitterDistance = DPIAwareScaling._ScaleX(1070);
                }
            }
        }

        public static System.Drawing.Point ScalePoint(int X, int Y)
        {
            DPIAwareScaling.graphics = Graphics.FromHwnd(IntPtr.Zero); // This gets the graphics configuration of the current screen
            var scaleX = DPIAwareScaling.graphics.DpiX / 96; // 96 was our standard design DPI
            var scaleY = DPIAwareScaling.graphics.DpiY / 96;
            return new System.Drawing.Point((int)Math.Round(X * scaleX), (int)Math.Round(Y * scaleY));
        }

        public static System.Drawing.Size ScaleSize(int X, int Y)
        {
            var graphics = Graphics.FromHwnd(IntPtr.Zero); // This gets the graphics configuration of the current screen
            var scaleX = graphics.DpiX / 96; // 96 was our standard design DPI
            var scaleY = graphics.DpiY / 96;

            return new System.Drawing.Size((int)Math.Round(X * scaleX), (int)Math.Round(Y * scaleY));
        }

        public static int _ScaleY(int Y)
        {
            var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var scaleY = graphics.DpiY / 96;

            return (int)Math.Round(Y * scaleY);
        }

        public static int _ScaleX(int X)
        {
            var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var scaleX = graphics.DpiX / 96;

            return (int)Math.Round(X * scaleX);
        }

        /// <summary>
        /// In case still somebody uses Windows Classic Theme on Windows 7, we need to
        /// adjust some styles also for this theme, so it is at least a little bit 
        /// compatible.
        /// </summary>
        public static bool UsingWindows7ClassicTheme()
        {
            bool classic_theme = false;
            string theme = null;

            try
            {               
                string WindowsVersion = Environment.OSVersion.VersionString;
                Regex regex = new Regex(Regex.Escape("6.1"));
                Match match = regex.Match(WindowsVersion);

                // Windows 7
                if (match.Success)
                {
                    string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ThemeManager";
                    theme = (string)Registry.GetValue(RegistryKey, "ThemeActive", string.Empty);
                    theme = theme.Split('\\').Last().Split('.').First().ToString();

                    Console.WriteLine("# Used theme is: " + theme);

                    if (theme == "0")
                    {
                        Console.WriteLine("# User is using Windows Classic Theme");
                        classic_theme = true;
                    } else
                    {
                        Console.WriteLine("# User is using Windows Aero Theme");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return classic_theme;
        }

    }
}
