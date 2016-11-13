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
using System.Collections.Generic;
using PuTTY_Storm;
using System.Runtime.InteropServices;
using System.Linq;

namespace Utilities.Windows.Forms
{
    /// <summary>
    /// DraggableTabControl
    /// </summary>
    public class DraggableTabControl : TabControl
    {
        private TabPage predraggedTab;
        List<ProcessInfo> my_ProcessInfo_List;

        public DraggableTabControl(List<ProcessInfo> my_ProcessInfo_List)
        {
            this.AllowDrop = true;
            this.my_ProcessInfo_List = my_ProcessInfo_List;
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
                this.DoDragDrop(predraggedTab, DragDropEffects.Move);

            base.OnMouseMove(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            TabPage draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            TabPage pointedTab = getPointedTab();

            if (draggedTab == predraggedTab && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;

                if (pointedTab != draggedTab)
                    swapTabPages(draggedTab, pointedTab);
            }

            base.OnDragOver(drgevent);
        }

        private TabPage getPointedTab()
        {
            for (int i = 0; i < this.TabPages.Count; i++)
                if (this.GetTabRect(i).Contains(this.PointToClient(Cursor.Position)))
                    return this.TabPages[i];

            return null;
        }

        private void swapTabPages(TabPage src, TabPage dst)
        {
            int srci = this.TabPages.IndexOf(src);
            int dsti = this.TabPages.IndexOf(dst);

            this.TabPages[dsti] = src;
            this.TabPages[srci] = dst;

            if (this.SelectedIndex == srci)
                this.SelectedIndex = dsti;
            else if (this.SelectedIndex == dsti)
                this.SelectedIndex = srci;

            ProcessInfo source = my_ProcessInfo_List[srci];
            ProcessInfo destination = my_ProcessInfo_List[dsti];

            my_ProcessInfo_List[dsti] = source;
            my_ProcessInfo_List[srci] = destination;

            NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(dsti).mainhandle);

            this.Refresh();
        }
    }
}