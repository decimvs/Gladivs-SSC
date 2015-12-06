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
using System.Windows.Forms;
using System.Drawing;

namespace gsscScreenshotManager
{
    class ToolRectangle : Tool
    {
        private Cursor _cursor;


        protected Cursor Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }



        public ToolRectangle()
        {
            Cursor = Cursors.Default;
        }

        public override void OnMouseUp(RegionCapture regionForm, MouseEventArgs e)
        {
            regionForm.DrawRectangle.Normalize();
           
            regionForm.ActiveTool = RegionCapture.RegionToolType.Pointer;

            regionForm.Capture = false;
            regionForm.Refresh();
        }

        public override void OnMouseDown(RegionCapture regionForm, MouseEventArgs e)
        {
            ChangeObject(regionForm, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        public override void OnMouseMove(RegionCapture regionForm, MouseEventArgs e)
        {
            regionForm.Cursor = Cursor;

            if (e.Button == MouseButtons.Left)
            {
                Point point = new Point(e.X, e.Y);
                regionForm.DrawRectangle.MoveHandleTo(point, 5);
                regionForm.Refresh();
            }
        }

        protected void ChangeObject(RegionCapture regionForm, DrawRectangle o)
        {
            regionForm.DrawRectangle = o;

            regionForm.Capture = true;
            regionForm.Refresh();
        }
    }
}
