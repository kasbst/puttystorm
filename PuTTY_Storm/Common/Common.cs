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
using System.Collections.Generic;
using System.Linq;

namespace PuTTY_Storm
{
    public static class Common
    {
        /// <summary>
        /// Method for List of Dictionaries (hashes) comparison for deep equality.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool _eqDeep(List<Dictionary<string, object>> first, List<Dictionary<string, object>> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if(!(first.Count == second.Count))
            {
                return false;
            }

            IEqualityComparer<Dictionary<string, object>> comparer = new DictionaryCompare<string, object>();
            for (int i = 0; i < first.Count; i++)
            {
                if(!comparer.Equals(first.ElementAt(i), second.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Debug method for printing content of List of Dictionaries (hashes) to the console.
        /// </summary>
        /// <param name="items"></param>
        public static void ListOfDictionariesDumper(List<Dictionary<string, object>> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("-----------------------------------------------");
                foreach (var pair in item)
                {
                    Console.WriteLine(pair.Key + " : " + pair.Value);
                }
            }
        }

    }
}
