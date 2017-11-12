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

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    /// <summary>
    /// Defines methods to support the comparison of objects for equality.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryCompare<TKey, TValue> : IEqualityComparer<Dictionary<TKey, TValue>>
    {
        public bool Equals(Dictionary<TKey, TValue> x, Dictionary<TKey, TValue> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (x.Count != y.Count)
            {
                return false;
            }
            foreach (var kvp in x)
            {
                TValue value;
                if (!y.TryGetValue(kvp.Key, out value))
                {
                    return false;
                }
                try
                {
                    if (!Object.Equals(kvp.Value, value))
                    {
                        return false;
                    }
                } catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            return true;
        }

        public int GetHashCode(Dictionary<TKey, TValue> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            int hash = 0;
            foreach (var kvp in obj)
            {
                hash = hash ^ kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode();
            }
            return hash;
        }

    }
}
