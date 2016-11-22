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
        static int TakeIndexTC1;
        List<ProcessInfo> my_ProcessInfo_List_TC_1;
        List<ProcessInfo> my_ProcessInfo_List_TC_2;
        SplitContainer SessionsSplitContainer;

        private const int WM_NCHITTEST = 0x84;
        private const int HTTRANSPARENT = -1;
        private const int HTCLIENT = 1;

        public DraggableTabControl(List<ProcessInfo> _my_ProcessInfo_List_TC_1, List<ProcessInfo> _my_ProcessInfo_List_TC_2, SplitContainer _SessionsSplitContainer)
        {
            this.AllowDrop = true;
            this.my_ProcessInfo_List_TC_1 = _my_ProcessInfo_List_TC_1;
            this.my_ProcessInfo_List_TC_2 = _my_ProcessInfo_List_TC_2;
            this.SessionsSplitContainer = _SessionsSplitContainer;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
            {
                if (m.Result.ToInt32() == HTTRANSPARENT)
                    m.Result = new IntPtr(HTCLIENT);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab();

            for (int i = 0; i < this.TabPages.Count; i++)
            {
                if (this.GetTabRect(i).Contains(this.PointToClient(Cursor.Position)))
                {
                    TakeIndexTC1 = this.TabPages.IndexOf(predraggedTab);
                }
            }
                       
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
                this.DoDragDrop(predraggedTab, DragDropEffects.Move);

            base.OnMouseMove(e);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            TabPage draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));

            if (draggedTab.Parent != this)
            {

                if (this.Name == "TABCONTROL_2")
                {                   
                    ProcessInfo swap_process = my_ProcessInfo_List_TC_1[TakeIndexTC1];
                    my_ProcessInfo_List_TC_1.RemoveAt(TakeIndexTC1);
                    my_ProcessInfo_List_TC_2.Add(swap_process);
                    ResizeTabPage(swap_process, SessionsSplitContainer.Panel2);
                    NativeMethods.SetForegroundWindow(swap_process.mainhandle);
                }

                if (this.Name == "TABCONTROL_1")
                {
                    ProcessInfo swap_process = my_ProcessInfo_List_TC_2[TakeIndexTC1];
                    my_ProcessInfo_List_TC_2.RemoveAt(TakeIndexTC1);
                    my_ProcessInfo_List_TC_1.Add(swap_process);
                    ResizeTabPage(swap_process, SessionsSplitContainer.Panel1);
                    NativeMethods.SetForegroundWindow(swap_process.mainhandle);
                }
                draggedTab.Parent = this;
                this.SelectedTab = draggedTab;
            } else
            {
                TabPage pointedTab = getPointedTab();
                if (pointedTab != draggedTab)
                    swapTabPages(draggedTab, pointedTab);
            }
            predraggedTab = null;

            base.OnDragDrop(drgevent);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            
            TabPage draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            TabPage pointedTab = getPointedTab();
            
            if (draggedTab == predraggedTab && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;
            }
            
            else if (draggedTab != null && draggedTab.Parent != this)
            {
                drgevent.Effect = DragDropEffects.Move;
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
                
            if (this.Name == "TABCONTROL_1")
            {
                ProcessInfo source = my_ProcessInfo_List_TC_1[srci];
                ProcessInfo destination = my_ProcessInfo_List_TC_1[dsti];

                my_ProcessInfo_List_TC_1[dsti] = source;
                my_ProcessInfo_List_TC_1[srci] = destination;

                NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(dsti).mainhandle);
            } else
            {
                ProcessInfo source = my_ProcessInfo_List_TC_2[srci];
                ProcessInfo destination = my_ProcessInfo_List_TC_2[dsti];

                my_ProcessInfo_List_TC_2[dsti] = source;
                my_ProcessInfo_List_TC_2[srci] = destination;

                NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_2.ElementAt(dsti).mainhandle);
            }
            this.Refresh();
        }

        private void ResizeTabPage(ProcessInfo my_ProcessInfo, SplitterPanel panel)
        {
            if (my_ProcessInfo.mainhandle != IntPtr.Zero)
            {
                NativeMethods.MoveWindow(my_ProcessInfo.mainhandle, -8, -30, panel.Width + 5, panel.Height + 5, true);
            }           
        }


    }
}