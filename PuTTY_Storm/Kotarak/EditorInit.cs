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

using System.Windows.Forms;
using ScintillaNET;
using System.Drawing;

namespace PuTTY_Storm
{
    public class EditorInit
    {
        string BASH =            
@"#!/bin/bash
# Bash script created by PuTTY Storm's Kotarak plugin
#
# Bash tutorial: http://linuxconfig.org/Bash_scripting_Tutorial
#
# Usage: ./script.sh
# ----------------------------------------------------------------------------- 
";

        string COMMAND =
@"# Only per line expressions enclosed in double quotes are supported for now!
# Example 1: ""date; uptime; free -m""
# Example 2: ""cat /proc/cpuinfo | grep processor; cat /proc/cpuinfo | grep ""model name""""
# Example 3: ""cat /proc/cpuinfo | grep -E 'processor|model name'""
";

        public void BashInit(RadioButton radiobutton, Scintilla scintilla)
        {
            if (radiobutton.Checked)
            {
                scintilla.StyleResetDefault();
                scintilla.Styles[Style.Default].Font = "Courier new";
                scintilla.Styles[Style.Default].Size = 10;
                scintilla.StyleClearAll();

                scintilla.Styles[BashLexer.StyleDefault].ForeColor = Color.Black;
                scintilla.Styles[BashLexer.StyleMainKeywords].ForeColor = Color.DarkGoldenrod;
                scintilla.Styles[BashLexer.StyleSecondaryKeywords].ForeColor = Color.Blue;
                scintilla.Styles[BashLexer.StyleIdentifier].ForeColor = Color.Black;
                scintilla.Styles[BashLexer.StyleNumber].ForeColor = Color.Purple;
                scintilla.Styles[BashLexer.StyleString].ForeColor = Color.Green;
                scintilla.Styles[BashLexer.StyleSingleQuotes].ForeColor = Color.Green;
                scintilla.Styles[BashLexer.StyleBackQuotes].ForeColor = Color.Green;
                scintilla.Styles[BashLexer.StyleComment].ForeColor = Color.Gray;

                scintilla.Lexer = Lexer.Container;

                scintilla.Margins[0].Width = 40;
                scintilla.Styles[Style.LineNumber].Font = "Consolas";
                scintilla.Margins[0].Type = MarginType.Number;

                scintilla.Text = BASH;
            }
        }

        public void CommandInit(RadioButton radiobutton, Scintilla scintilla)
        {
            if (radiobutton.Checked)
            {
                scintilla.StyleResetDefault();
                scintilla.Styles[Style.Default].Font = "Courier new";
                scintilla.Styles[Style.Default].Size = 10;
                scintilla.StyleClearAll();

                scintilla.Styles[BashLexer.StyleDefault].ForeColor = Color.Black;
                scintilla.Styles[BashLexer.StyleComment].ForeColor = Color.Gray;

                scintilla.Lexer = Lexer.Container;

                scintilla.Margins[0].Width = 40;
                scintilla.Styles[Style.LineNumber].Font = "Consolas";
                scintilla.Margins[0].Type = MarginType.Number;

                scintilla.Text = COMMAND;
            }
        }

    }
}
