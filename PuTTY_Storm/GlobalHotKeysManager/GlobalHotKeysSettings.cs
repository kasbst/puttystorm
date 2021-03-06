﻿/*
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

using System.Configuration;

namespace PuTTY_Storm
{
    /// <summary>
    /// Handle TabPagesForward GlobalHotKeys settings
    /// </summary>
    public class TabPagesForwardGlobalHotKeySettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public int modifier
        {
            get
            {
                return ((int)this["modifier"]);
            }
            set
            {
                this["modifier"] = (int)value;
            }
        }

        [UserScopedSetting()]
        public string key
        {
            get
            {
                return ((string)this["key"]);
            }
            set
            {
                this["key"] = (string)value;
            }
        }
    }

    /// <summary>
    /// Handle TabPagesbackward GlobalHotKeys settings
    /// </summary>
    public class TabPagesBackwardGlobalHotKeySettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public int modifier
        {
            get
            {
                return ((int)this["modifier"]);
            }
            set
            {
                this["modifier"] = (int)value;
            }
        }

        [UserScopedSetting()]
        public string key
        {
            get
            {
                return ((string)this["key"]);
            }
            set
            {
                this["key"] = (string)value;
            }
        }
    }

    /// <summary>
    /// Handle SplitScreen GlobalHotKeys settings
    /// </summary>
    public class SplitScreenGlobalHotKeySettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public int modifier
        {
            get
            {
                return ((int)this["modifier"]);
            }
            set
            {
                this["modifier"] = (int)value;
            }
        }

        [UserScopedSetting()]
        public string key
        {
            get
            {
                return ((string)this["key"]);
            }
            set
            {
                this["key"] = (string)value;
            }
        }
    }

    /// <summary>
    /// Handle SFTP Manager GlobalHotKeys settings
    /// </summary>
    public class SFTPManagerGlobalHotKeySettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public int modifier
        {
            get
            {
                return ((int)this["modifier"]);
            }
            set
            {
                this["modifier"] = (int)value;
            }
        }

        [UserScopedSetting()]
        public string key
        {
            get
            {
                return ((string)this["key"]);
            }
            set
            {
                this["key"] = (string)value;
            }
        }
    }

    /// <summary>
    /// Handle Kotarak GlobalHotKeys settings
    /// </summary>
    public class KotarakGlobalHotKeySettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public int modifier
        {
            get
            {
                return ((int)this["modifier"]);
            }
            set
            {
                this["modifier"] = (int)value;
            }
        }

        [UserScopedSetting()]
        public string key
        {
            get
            {
                return ((string)this["key"]);
            }
            set
            {
                this["key"] = (string)value;
            }
        }
    }

}
