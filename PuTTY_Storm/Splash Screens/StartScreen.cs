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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuTTY_Storm.Splash_Screens
{
    public partial class StartScreen : Form
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        //Delegate for cross thread call to close StartScreen
        private delegate void CloseStartScreenDelegate();

        private static StartScreen startScreen;
        private static Task task;

        /// <summary>
        /// Call this method on place where StartScreen should be displayed!
        /// </summary>
        static public void ShowStartScreen()
        {
            try
            {
                if (startScreen != null)
                    return;
                task = Task.Factory.StartNew(() =>
                 {
                     Console.WriteLine("## New StartScreen Task started!");
                     StartScreen.ShowStartScreenForm();
                 });
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static private void ShowStartScreenForm()
        {
            startScreen = new StartScreen();
            Application.Run(startScreen);
        }

        /// <summary>
        /// Call this method on place where StartScreen should be stopped!
        /// </summary>
        static public void CloseStartScreen()
        {
            try
            {
                startScreen.Invoke(new CloseStartScreenDelegate(StartScreen.CloseStartScreenFormInternal));
                while (true)
                {
                    Console.WriteLine("## StartScreen Task is still running");
                    if (task.IsCompleted)
                    {
                        Console.WriteLine("## StartScreen Closed and Task completed!");
                        break;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        static private void CloseStartScreenFormInternal()
        {
            startScreen.Close();
        }

        private void StartScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
