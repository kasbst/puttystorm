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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public partial class GlobalHotKeysManager : Form
    {
        // Configuration settings
        private TabPagesForwardGlobalHotKeySettings tabPagesForwardGlobalHotKeySettings;
        private TabPagesBackwardGlobalHotKeySettings tabPagesbackwardGlobalHotKeySettings;
        private SplitScreenGlobalHotKeySettings splitScreenGlobalHotKeySettings;
        private SFTPManagerGlobalHotKeySettings sftpManagerGlobalHotKeySettings;
        private KotarakGlobalHotKeySettings kotarakGlobalHotKeySettings;

        // GlobalHotKeys Manager Main Form settings
        private List<CheckBox> tabPagesForwardModifiersCheckBoxes = null;
        private List<CheckBox> tabPagesbackwardModifiersCheckBoxes = null;
        private List<CheckBox> splitScreenModifiersCheckBoxes = null;
        private List<CheckBox> sftpManagerModifierCheckBoxes = null;
        private List<CheckBox> kotarakModifierCheckBoxes = null;

        // GlobalHotKeys registration
        GlobalHotKeysWorker TabPagesForwardGlobalHotKeyWorker;
        GlobalHotKeysWorker TabPagesBackwardGlobalHotKeyWorker;
        GlobalHotKeysWorker SplitScreenGlobalHotKeyWorker;
        GlobalHotKeysWorker SFTPManagerGlobalHotKeyWorker;
        GlobalHotKeysWorker KotarakGlobalHotKeyWorker;
        

        public GlobalHotKeysManager(GlobalHotKeysWorker _TabPagesForwardGlobalHotKeyWorker, GlobalHotKeysWorker _TabPagesBackwardGlobalHotKeyWorker,
            GlobalHotKeysWorker _SplitScreenGlobalHotKeyWorker, GlobalHotKeysWorker _SFTPManagerGlobalHotKeyWorker, GlobalHotKeysWorker _KotarakGlobalHotKeyWorker)
        {
            // GlobalHotKeys Manager Main Form settings
            tabPagesForwardModifiersCheckBoxes = new List<CheckBox>();
            tabPagesbackwardModifiersCheckBoxes = new List<CheckBox>();
            splitScreenModifiersCheckBoxes = new List<CheckBox>();
            sftpManagerModifierCheckBoxes = new List<CheckBox>();
            kotarakModifierCheckBoxes = new List<CheckBox>();

            // GlobalHotKeysSettings
            tabPagesForwardGlobalHotKeySettings = new TabPagesForwardGlobalHotKeySettings();
            tabPagesbackwardGlobalHotKeySettings = new TabPagesBackwardGlobalHotKeySettings();
            splitScreenGlobalHotKeySettings = new SplitScreenGlobalHotKeySettings();
            sftpManagerGlobalHotKeySettings = new SFTPManagerGlobalHotKeySettings();
            kotarakGlobalHotKeySettings = new KotarakGlobalHotKeySettings();

            // GlobalHotKeys registration
            this.TabPagesForwardGlobalHotKeyWorker = _TabPagesForwardGlobalHotKeyWorker;
            this.TabPagesBackwardGlobalHotKeyWorker = _TabPagesBackwardGlobalHotKeyWorker;
            this.SplitScreenGlobalHotKeyWorker = _SplitScreenGlobalHotKeyWorker;
            this.SFTPManagerGlobalHotKeyWorker = _SFTPManagerGlobalHotKeyWorker;
            this.KotarakGlobalHotKeyWorker = _KotarakGlobalHotKeyWorker;

            // Fill Key comboboxes with strings of Keys type
            object keys = Enum.GetValues(typeof(Keys));
            IEnumerable enumerable = keys as IEnumerable;
            InitializeComponent();

            if (enumerable != null)
            {
                foreach (object element in enumerable)
                {
                    TabPagesForwardCombobox.Items.Add(element);
                    TabPagesBackwardCombobox.Items.Add(element);
                    SplitScreenCombobox.Items.Add(element);
                    SFTPManagerCombobox.Items.Add(element);
                    KotarakCombobox.Items.Add(element);
                }
            }           
        }

        /// <summary>
        /// Convert strings back to Keys type
        /// </summary>
        public static Keys ConvertFromStringToKey(string keystr)
        {
            return (Keys)Enum.Parse(typeof(Keys), keystr);
        }

        /// <summary>
        /// Saves the new GlobalHotKeys configuration and unregister -> register new hotkeys again!
        /// </summary>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!(TabPagesForwardCTRLMOD.Checked || TabPagesForwardALTMOD.Checked || TabPagesForwardSHIFTMOD.Checked ||
                TabPagesForwardWINMOD.Checked) || TabPagesForwardCombobox.SelectedItem == null)
            {
                MessageBox.Show("GlobalHotKey for TabPagesForward must be set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(TabPagesBackwardCTRLMOD.Checked || TabPagesBackwardALTMOD.Checked || TabPagesBackwardSHIFTMOD.Checked ||
                TabPagesBackwardWINMOD.Checked) || TabPagesBackwardCombobox == null)
            {
                MessageBox.Show("GlobalHotKey for TabPagesBackward must be set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(SplitScreenCTRLMOD.Checked || SplitScreenALTMOD.Checked || SplitScreenSHIFTMOD.Checked ||
                SplitScreenWINMOD.Checked) || SplitScreenCombobox == null)
            {
                MessageBox.Show("GlobalHotKey for SplitScreen must be set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(SFTPManagerCTRLMOD.Checked || SFTPManagerALTMOD.Checked || SFTPManagerSHIFTMOD.Checked ||
                SFTPManagerWINMOD.Checked) || SFTPManagerCombobox == null)
            {
                MessageBox.Show("GlobalHotKey for SFTPManager must be set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(KotarakCTRLMOD.Checked || KotarakALTMOD.Checked || KotarakSHIFTMOD.Checked || KotarakWINMOD.Checked) ||
                KotarakCombobox == null)
            {
                MessageBox.Show("GlobalHotKey for Kotarak must be set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save to the config first!
            try
            {
                tabPagesForwardGlobalHotKeySettings.modifier = GetGlobalHotKeysSettingsModifier(tabPagesForwardModifiersCheckBoxes);
                tabPagesForwardGlobalHotKeySettings.key = TabPagesForwardCombobox.Text;

                tabPagesbackwardGlobalHotKeySettings.modifier = GetGlobalHotKeysSettingsModifier(tabPagesbackwardModifiersCheckBoxes);
                tabPagesbackwardGlobalHotKeySettings.key = TabPagesBackwardCombobox.Text;

                splitScreenGlobalHotKeySettings.modifier = GetGlobalHotKeysSettingsModifier(splitScreenModifiersCheckBoxes);
                splitScreenGlobalHotKeySettings.key = SplitScreenCombobox.Text;

                sftpManagerGlobalHotKeySettings.modifier = GetGlobalHotKeysSettingsModifier(sftpManagerModifierCheckBoxes);
                sftpManagerGlobalHotKeySettings.key = SFTPManagerCombobox.Text;

                kotarakGlobalHotKeySettings.modifier = GetGlobalHotKeysSettingsModifier(kotarakModifierCheckBoxes);
                kotarakGlobalHotKeySettings.key = KotarakCombobox.Text;

                tabPagesForwardGlobalHotKeySettings.Save();
                tabPagesbackwardGlobalHotKeySettings.Save();
                splitScreenGlobalHotKeySettings.Save();
                sftpManagerGlobalHotKeySettings.Save();
                kotarakGlobalHotKeySettings.Save();

            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Register new hotkeys!
            try
            {
                Form SessionsForm = Application.OpenForms["Sessions"];

                if (SessionsForm != null && SessionsForm.Handle != IntPtr.Zero)
                {
                    TabPagesForwardGlobalHotKeyWorker.Handle = SessionsForm.Handle;
                    TabPagesForwardGlobalHotKeyWorker.RegisterGlobalHotKey((int)ConvertFromStringToKey(tabPagesForwardGlobalHotKeySettings.key), 
                        tabPagesForwardGlobalHotKeySettings.modifier);

                    TabPagesBackwardGlobalHotKeyWorker.Handle = SessionsForm.Handle;
                    TabPagesBackwardGlobalHotKeyWorker.RegisterGlobalHotKey((int)ConvertFromStringToKey(tabPagesbackwardGlobalHotKeySettings.key), 
                        tabPagesbackwardGlobalHotKeySettings.modifier);

                    SplitScreenGlobalHotKeyWorker.Handle = SessionsForm.Handle;
                    SplitScreenGlobalHotKeyWorker.RegisterGlobalHotKey((int)ConvertFromStringToKey(splitScreenGlobalHotKeySettings.key), 
                        splitScreenGlobalHotKeySettings.modifier);

                    SFTPManagerGlobalHotKeyWorker.Handle = SessionsForm.Handle;
                    SFTPManagerGlobalHotKeyWorker.RegisterGlobalHotKey((int)ConvertFromStringToKey(sftpManagerGlobalHotKeySettings.key), 
                        sftpManagerGlobalHotKeySettings.modifier);

                    KotarakGlobalHotKeyWorker.Handle = SessionsForm.Handle;
                    KotarakGlobalHotKeyWorker.RegisterGlobalHotKey((int)ConvertFromStringToKey(kotarakGlobalHotKeySettings.key), 
                        kotarakGlobalHotKeySettings.modifier);

                    MessageBox.Show("GlobalHotKeys saved to the config and registered!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    MessageBox.Show("GlobalHotKeys saved to the config but not registered because" +
                         " SessionsForm has been closed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TabPagesForwardCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void TabPagesBackwardCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void SFTPManagerCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void KotarakCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void SplitScreenCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        /// <summary>
        /// Set checkboxes and comboboxes based on configuration values when GLobalHotKeys 
        /// Manager main form starts up
        /// </summary>
        private void SetGlobalHotKeysControls (int modifier, string key, List<CheckBox> checkboxes, ComboBox combobox)
        {
            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            int[] modifiers = new int[] { 1, 2, 4, 8 };
            int moddiff = modifier;

            for (int i = modifiers.Length - 1; i >= 0; i--)
            {
                if (moddiff >= modifiers[i])
                {
                    moddiff = moddiff - modifiers[i];
                    if (moddiff != 0)
                    {
                        foreach (CheckBox checkbox in checkboxes)
                        {
                            if ((int)checkbox.Tag == modifiers[i])
                            {
                                checkbox.Checked = true;
                            }
                        }
                    } else
                    {
                        foreach (CheckBox checkbox in checkboxes)
                        {
                            if ((int)checkbox.Tag == modifiers[i])
                            {
                                checkbox.Checked = true;
                            }
                        }
                    }
                }
            }
            combobox.Text = key;
        }

        /// <summary>
        /// Save the modifier sum value to the configuration file based on checked checkboxes
        /// </summary>
        private int GetGlobalHotKeysSettingsModifier (List<CheckBox> checkboxes)
        {
            int modifier_sum = 0;

            foreach (CheckBox checkbox in checkboxes)
            {
                if (checkbox.Checked)
                {
                    modifier_sum += (int)checkbox.Tag;
                }
            }

            return modifier_sum;
        }

        /// <summary>
        /// GlobalHotKeys Manager Form initialization
        /// </summary>
        private void GlobalHotKeyManager_Load(object sender, EventArgs e)
        {
            TabPagesForwardALTMOD.Tag = 1;
            this.tabPagesForwardModifiersCheckBoxes.Add(TabPagesForwardALTMOD);
            TabPagesForwardCTRLMOD.Tag = 2;
            this.tabPagesForwardModifiersCheckBoxes.Add(TabPagesForwardCTRLMOD);
            TabPagesForwardSHIFTMOD.Tag = 4;
            this.tabPagesForwardModifiersCheckBoxes.Add(TabPagesForwardSHIFTMOD);
            TabPagesForwardWINMOD.Tag = 8;
            this.tabPagesForwardModifiersCheckBoxes.Add(TabPagesForwardWINMOD);

            TabPagesBackwardCTRLMOD.Tag = 2;
            this.tabPagesbackwardModifiersCheckBoxes.Add(TabPagesBackwardCTRLMOD);
            TabPagesBackwardALTMOD.Tag = 1;
            this.tabPagesbackwardModifiersCheckBoxes.Add(TabPagesBackwardALTMOD);
            TabPagesBackwardSHIFTMOD.Tag = 4;
            this.tabPagesbackwardModifiersCheckBoxes.Add(TabPagesBackwardSHIFTMOD);
            TabPagesBackwardWINMOD.Tag = 8;
            this.tabPagesbackwardModifiersCheckBoxes.Add(TabPagesBackwardWINMOD);

            SplitScreenCTRLMOD.Tag = 2;
            this.splitScreenModifiersCheckBoxes.Add(SplitScreenCTRLMOD);
            SplitScreenALTMOD.Tag = 1;
            this.splitScreenModifiersCheckBoxes.Add(SplitScreenALTMOD);
            SplitScreenSHIFTMOD.Tag = 4;
            this.splitScreenModifiersCheckBoxes.Add(SplitScreenSHIFTMOD);
            SplitScreenWINMOD.Tag = 8;
            this.splitScreenModifiersCheckBoxes.Add(SplitScreenWINMOD);

            SFTPManagerCTRLMOD.Tag = 2;
            this.sftpManagerModifierCheckBoxes.Add(SFTPManagerCTRLMOD);
            SFTPManagerALTMOD.Tag = 1;
            this.sftpManagerModifierCheckBoxes.Add(SFTPManagerALTMOD);
            SFTPManagerSHIFTMOD.Tag = 4;
            this.sftpManagerModifierCheckBoxes.Add(SFTPManagerSHIFTMOD);
            SFTPManagerWINMOD.Tag = 8;
            this.sftpManagerModifierCheckBoxes.Add(SFTPManagerWINMOD);

            KotarakCTRLMOD.Tag = 2;
            this.kotarakModifierCheckBoxes.Add(KotarakCTRLMOD);
            KotarakALTMOD.Tag = 1;
            this.kotarakModifierCheckBoxes.Add(KotarakALTMOD);
            KotarakSHIFTMOD.Tag = 4;
            this.kotarakModifierCheckBoxes.Add(KotarakSHIFTMOD);
            KotarakWINMOD.Tag = 8;
            this.kotarakModifierCheckBoxes.Add(KotarakWINMOD);

            // Initialize checkboxes and comboboxes with values from configuration
            try
            {
                SetGlobalHotKeysControls(tabPagesForwardGlobalHotKeySettings.modifier, tabPagesForwardGlobalHotKeySettings.key,
                        tabPagesForwardModifiersCheckBoxes, TabPagesForwardCombobox);

                SetGlobalHotKeysControls(tabPagesbackwardGlobalHotKeySettings.modifier, tabPagesbackwardGlobalHotKeySettings.key,
                        tabPagesbackwardModifiersCheckBoxes, TabPagesBackwardCombobox);

                SetGlobalHotKeysControls(splitScreenGlobalHotKeySettings.modifier, splitScreenGlobalHotKeySettings.key,
                        splitScreenModifiersCheckBoxes, SplitScreenCombobox);

                SetGlobalHotKeysControls(sftpManagerGlobalHotKeySettings.modifier, sftpManagerGlobalHotKeySettings.key,
                        sftpManagerModifierCheckBoxes, SFTPManagerCombobox);

                SetGlobalHotKeysControls(kotarakGlobalHotKeySettings.modifier, kotarakGlobalHotKeySettings.key,
                        kotarakModifierCheckBoxes, KotarakCombobox);
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
