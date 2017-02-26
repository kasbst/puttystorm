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
 * 
 * BIG THANK YOU to jacobslusser for his great work and inspiration!!!
 */

using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuTTY_Storm
{
    public class BashLexer
    {
        public const int StyleDefault = 0;
        public const int StyleMainKeywords = 1;
        public const int StyleIdentifier = 2;
        public const int StyleNumber = 3;
        public const int StyleString = 4;
        public const int StyleComment = 5;
        public const int StyleSingleQuotes = 6;
        public const int StyleBackQuotes = 7;
        public const int StyleSecondaryKeywords = 8;

        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_COMMENT = 4;
        private const int STATE_SINGLE_QUOTES = 5;
        private const int STATE_BACK_QUOTES = 6;

        private HashSet<string> mainKeywords;
        private HashSet<string> secondaryKeywords;

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            var length = 0;
            var state = STATE_UNKNOWN;

            // Start styling
            scintilla.StartStyling(startPos);

            char lastChar = ' ';

            while (startPos < endPos)
            {
                var c = (char)scintilla.GetCharAt(startPos);

                REPROCESS:
                switch (state)
                {
                    case STATE_UNKNOWN:
                        if (c == '"')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleString);
                            state = STATE_STRING;
                        }
                        else if (c == '\'')
                        {
                            scintilla.SetStyling(1, StyleString);
                            state = STATE_SINGLE_QUOTES;
                        }

                        else if (c == '`')
                        {
                            scintilla.SetStyling(1, StyleString);
                            state = STATE_BACK_QUOTES;
                        }

                        else if (Char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (Char.IsLetter(c))
                        {
                            state = STATE_IDENTIFIER;
                            goto REPROCESS;
                        }
                        else if (lastChar != '$' && c == '#')
                        {
                            Console.WriteLine("LAST CHARACTER: " + lastChar);
                            // start of "comment"
                            state = STATE_COMMENT;
                            goto REPROCESS;
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, StyleDefault);
                            lastChar = c;
                        }
                        break;

                    case STATE_STRING:
                        if (c == '"')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleString);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_SINGLE_QUOTES:
                        if (c == '\'')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleSingleQuotes);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_BACK_QUOTES:
                        if (c == '`')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleBackQuotes);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_NUMBER:
                        if (Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, StyleNumber);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;

                    case STATE_IDENTIFIER:
                        if (Char.IsLetterOrDigit(c))
                        {
                            length++;
                        }
                        else
                        {
                            var style = StyleIdentifier;
                            var identifier = scintilla.GetTextRange(startPos - length, length);
                            if (mainKeywords.Contains(identifier))
                                style = StyleMainKeywords;

                            if (secondaryKeywords.Contains(identifier))
                                style = StyleSecondaryKeywords;

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;

                     case STATE_COMMENT:
                        if (c.Equals('\n'))
                        {
                            scintilla.SetStyling(length, StyleComment);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        else
                        {
                            length++;
                        }
                        break;
                }

                startPos++;
            }
        }

        public BashLexer(string mainKeywords, string secondaryKeywords)
        {
            // Put keywords in a HashSet
            var main = Regex.Split(mainKeywords ?? string.Empty, @"\s+").Where(l => !string.IsNullOrEmpty(l));
            var secondary = Regex.Split(secondaryKeywords ?? string.Empty, @"\s+").Where(l => !string.IsNullOrEmpty(l));
            this.mainKeywords = new HashSet<string>(main);
            this.secondaryKeywords = new HashSet<string>(secondary);
        }
    }
}
