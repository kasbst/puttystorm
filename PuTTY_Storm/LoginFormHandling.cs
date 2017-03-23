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
using System.Security.Cryptography;

namespace PuTTY_Storm
{
    class LoginFormHandling
    {
        LoginForm login_form;
        int Result = 0;

        public LoginFormHandling(LoginForm _login_form)
        {
            this.login_form = _login_form;
            this.login_form.Text = GlobalVar.VERSION + " - Login";
            this.login_form.MaximizeBox = false;
            this.login_form.MaximumSize = DPIAwareScaling.ScaleSize(450, 300);
            this.login_form.Size = DPIAwareScaling.ScaleSize(450, 300);
            this.login_form.MinimumSize = DPIAwareScaling.ScaleSize(450, 300);
            this.login_form.StartPosition = FormStartPosition.CenterScreen;
            this.login_form.BackColor = Color.SlateGray;
            this.login_form.Load += new EventHandler(Login_Form_Load);
            this.login_form.FormClosing += new FormClosingEventHandler(Login_Form_isClosing);
        }

        MyUserSettings mus = new MyUserSettings();

        /// <summary>
        /// Initialize login form.
        /// </summary>
        private void Login_Form_Load(object sender, EventArgs e)
        {                     
            Button ok_button = new Button();
            ok_button.Text = "OK";
            ok_button.Name = "ok_button";
            ok_button.Click += new EventHandler(OK_Button_Click);
            ok_button.Location = DPIAwareScaling.ScalePoint(130, 150);
            ok_button.Size = DPIAwareScaling.ScaleSize(70, 30);
            ok_button.UseVisualStyleBackColor = true;

            Button cancel_button = new Button();
            cancel_button.Text = "Cancel";
            cancel_button.Name = "cancel_button";
            cancel_button.Click += new EventHandler(Cancel_Button_Click);
            cancel_button.Location = DPIAwareScaling.ScalePoint(230, 150);
            cancel_button.Size = DPIAwareScaling.ScaleSize(70, 30);
            cancel_button.UseVisualStyleBackColor = true;

            TextBox login_textbox = new TextBox();
            login_textbox.Font = new Font("Calibri", 10);
            login_textbox.Location = DPIAwareScaling.ScalePoint(130, 90);
            login_textbox.Size = DPIAwareScaling.ScaleSize(170, 27);
            login_textbox.Name = "login_secret_textbox";
            login_textbox.UseSystemPasswordChar = true;
            login_textbox.AcceptsReturn = true;
            login_textbox.KeyDown += new KeyEventHandler(Login_Secret_Textbox_KeyDown);

            TextBox new_login_textbox = new TextBox();
            new_login_textbox.Font = new Font("Calibri", 10);
            new_login_textbox.Location = DPIAwareScaling.ScalePoint(130, 110);
            new_login_textbox.Size = DPIAwareScaling.ScaleSize(170, 27);
            new_login_textbox.Name = "new_login_secret_textbox";
            new_login_textbox.UseSystemPasswordChar = true;
            new_login_textbox.AcceptsReturn = true;
            new_login_textbox.KeyDown += new KeyEventHandler(New_Login_Secret_Textbox_KeyDown);
            new_login_textbox.Hide();

            TextBox new_login_textbox_confirm = new TextBox();
            new_login_textbox_confirm.Font = new Font("Calibri", 10);
            new_login_textbox_confirm.Location = DPIAwareScaling.ScalePoint(130, 70);
            new_login_textbox_confirm.Size = DPIAwareScaling.ScaleSize(170, 27);
            new_login_textbox_confirm.Name = "new_login_secret_textbox_confirm";
            new_login_textbox_confirm.UseSystemPasswordChar = true;
            new_login_textbox_confirm.Hide();

            Label login_label = new Label();
            login_label.Font = new Font("Calibri", 16, FontStyle.Bold);
            login_label.BackColor = Color.SlateGray;
            login_label.ForeColor = Color.White;
            login_label.Location = DPIAwareScaling.ScalePoint(100, 30);
            login_label.Size = DPIAwareScaling.ScaleSize(90, 35);
            login_label.AutoSize = true;
            login_label.Name = "login_secret_label";

            if (mus.password_secret == null)
            {
                login_label.Text = "Enter your new Safe Combination";
                login_label.Location = DPIAwareScaling.ScalePoint(60, 30);
                login_textbox.Hide();
                new_login_textbox.Show();
                new_login_textbox_confirm.Show();
            }
            else
            {
                login_label.Text = "Enter Safe Combination";
            }

            this.login_form.Controls.Add(new_login_textbox_confirm);
            this.login_form.Controls.Add(new_login_textbox);
            this.login_form.Controls.Add(login_textbox);            
            this.login_form.Controls.Add(ok_button);
            this.login_form.Controls.Add(cancel_button);
            this.login_form.Controls.Add(login_label);            
        }

        bool ok_clicked = false;

        /// <summary>
        /// Handle login form exit via X button / ALT+F4 / OK or Cancel button.
        /// </summary>
        private void Login_Form_isClosing(object sender, FormClosingEventArgs e)
        {
            //if (string.Equals((sender as Button).Name, @"ok_button"))
            if(ok_clicked)
            {
                Result = 0;
            }
            else
            {
                Result = 1;
            }
        }

        /// <summary>
        /// Handle login form closing via OK button.
        /// </summary>
        private void OK_Button_Click(object sender, EventArgs e)
        {
            CryptoHelper crypto = new CryptoHelper();

            if (mus.password_secret == null)
            {
                Control[] new_login_secret_textbox_text = this.login_form.Controls.Find("new_login_secret_textbox", true);
                Control[] new_login_secret_textbox_confirm_text = this.login_form.Controls.Find("new_login_secret_textbox_confirm", true);

                if (new_login_secret_textbox_text[0].Text == "" || new_login_secret_textbox_confirm_text[0].Text == "")
                {
                    MessageBox.Show("Password Secret Cannot Be Null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((new_login_secret_textbox_text[0].Text == new_login_secret_textbox_confirm_text[0].Text))
                {
                    mus.password_secret = crypto.ComputeHash(new_login_secret_textbox_text[0].Text, new SHA256CryptoServiceProvider());
                    GlobalVar.SECRET = new_login_secret_textbox_text[0].Text;
                    mus.Save();
                } else
                {
                    MessageBox.Show("Password Secret Is Not The Same!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    new_login_secret_textbox_text[0].Text = null;
                    new_login_secret_textbox_confirm_text[0].Text = null;
                    return;
                }
            } else
            {
                Control[] login_secret_textbox_text = this.login_form.Controls.Find("login_secret_textbox", true);
                string saved_secret = mus.password_secret;
                string compare_hash = crypto.ComputeHash(login_secret_textbox_text[0].Text, new SHA256CryptoServiceProvider());

                if (compare_hash != saved_secret)
                {
                    MessageBox.Show("Login Failed! Password Secret Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    login_secret_textbox_text[0].Text = null;
                    return;
                }

                GlobalVar.SECRET = login_secret_textbox_text[0].Text;
            }

            Result = 0;
            ok_clicked = true;            
            Console.WriteLine("Login Form closed with OK button!");
            this.login_form.Close();
        }

        /// <summary>
        /// Handle login form closing via keyboard "enter" button when secret is already set.
        /// </summary>
        private void Login_Secret_Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            CryptoHelper crypto = new CryptoHelper();

            if (e.KeyCode == Keys.Enter)
            {
                if (sender is TextBox)
                {
                    TextBox txb = (TextBox)sender;

                    string saved_secret = mus.password_secret;
                    string compare_hash = crypto.ComputeHash(txb.Text, new SHA256CryptoServiceProvider());

                    if (compare_hash != saved_secret)
                    {
                        MessageBox.Show("Login Failed! Password Secret Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txb.Text = null;
                        return;
                    }

                    GlobalVar.SECRET = txb.Text;
                }

                Result = 0;
                ok_clicked = true;
                Console.WriteLine("Login Form closed with OK button!");
                this.login_form.Close();
            }
        }

        /// <summary>
        /// Handle login form closing via keyboard "enter" button during the first application startup.
        /// - when secret is beign set.
        /// </summary>
        private void New_Login_Secret_Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            CryptoHelper crypto = new CryptoHelper();

            if (e.KeyCode == Keys.Enter)
            {
                if (mus.password_secret == null)
                {
                    Control[] new_login_secret_textbox_text = this.login_form.Controls.Find("new_login_secret_textbox", true);
                    Control[] new_login_secret_textbox_confirm_text = this.login_form.Controls.Find("new_login_secret_textbox_confirm", true);

                    if (new_login_secret_textbox_text[0].Text == "" || new_login_secret_textbox_confirm_text[0].Text == "")
                    {
                        MessageBox.Show("Password Secret Cannot Be Null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if ((new_login_secret_textbox_text[0].Text == new_login_secret_textbox_confirm_text[0].Text))
                    {
                        mus.password_secret = crypto.ComputeHash(new_login_secret_textbox_text[0].Text, new SHA256CryptoServiceProvider());
                        GlobalVar.SECRET = new_login_secret_textbox_text[0].Text;
                        mus.Save();
                    }
                    else
                    {
                        MessageBox.Show("Password Secret Is Not The Same!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new_login_secret_textbox_text[0].Text = null;
                        new_login_secret_textbox_confirm_text[0].Text = null;
                        return;
                    }
                }

                Result = 0;
                ok_clicked = true;
                Console.WriteLine("Login Form closed with OK button!");
                this.login_form.Close();
            }
        }

        /// <summary>
        /// Handle login form closing via cancel button.
        /// </summary>
        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.login_form.Close();
        }

        public int Get_Result ()
        {
            return Result;           
        }
    }
}
