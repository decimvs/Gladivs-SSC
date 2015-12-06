/********************************************************************************************
*	Gladivs Simple Screen Capture															*
*-------------------------------------------------------------------------------------------*
*	(c) Copyright  2015 Guillermo Espert Carrasquer											*
*===========================================================================================*
*																							*
*			Licensed under the Apache License, Version 2.0 (the "License");					*
*			you may not use this file except in compliance with the License.				*
*			You may obtain a copy of the License at											*
*																							*
*			http://www.apache.org/licenses/LICENSE-2.0										*
*																							*
*			Unless required by applicable law or agreed to in writing, software				*
*			distributed under the License is distributed on an "AS IS" BASIS,				*
*			WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.		*
*			See the License for the specific language governing permissions and				*
*			limitations under the License.													*
*																							*
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace gsscScreenshotManager
{
    public partial class RegionCapture : Form
    {
        private Image _screenshot = null;

        public Image Screenshot
        {
            get { return _screenshot; }
            set { _screenshot = value;  }
        }

        public RegionCapture()
        {
            InitializeComponent();
        }
        

        public enum RegionToolType
        {
            Pointer,
            Rectangle,
            Path,
            NumberOfRegionTools
        };
        

        private DrawRectangle _drawRectangle; 

        private RegionToolType _activeTool; 
        private Tool[] _tools; 

        public RegionToolType ActiveTool
        {
            get { return _activeTool; }
            set { _activeTool = value; }
        }


        public DrawRectangle DrawRectangle
        {
            get { return _drawRectangle; }
            set { _drawRectangle = value; }
        }


        private void RegionForm_Paint(object sender, PaintEventArgs e)
        {
            if (_drawRectangle != null)
            {
                _drawRectangle.Draw(e.Graphics);
            }
        }

        private void RegionForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _tools[(int)_activeTool].OnMouseUp(this, e);
            }
        }


        private void RegionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
            {
                _tools[(int)_activeTool].OnMouseMove(this, e);
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void RegionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _tools[(int)_activeTool].OnMouseDown(this, e);
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.Close();
                this.Dispose();
            }
        }


        private void RegionForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Rectangle rect = ((DrawRectangle)DrawRectangle).Rectangle;
                if (rect.Contains(new Point(e.X, e.Y)))
                {
                    _tools[(int)_activeTool].OnMouseDoubleClick(this, e);
                    this.Close();
                    this.Dispose();
                }
            }
        }


        public void Initialize()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            
            _tools = new Tool[(int)RegionToolType.NumberOfRegionTools];
            _tools[(int)RegionToolType.Pointer] = new ToolPointer();
            _tools[(int)RegionToolType.Rectangle] = new ToolRectangle();
            _tools[(int)RegionToolType.Path] = new ToolPath();
        }
    }
}