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

//Esta clase escolta les tecles que es pressionen i envia un event que en ser tractat ens permet saber quina tecla s'ha pressionat i actuar en consequència.

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace gladivsSSC
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
    public class KeyboardListener
    {
        public static bool KeyShift;
        public static int KeyUp = 257;
        public static int KeyDown = 256;

        private static ListeningWindow _listeningWindow;
        public static event EventHandler EventHandler;

        private static void KeyHandler(ushort key, uint message)
        {
            if (EventHandler != null)
            {
                Delegate[] delegates = EventHandler.GetInvocationList();

                foreach (Delegate del in delegates)
                {
                    EventHandler eventHandler = (EventHandler)del;
                    eventHandler(null, new KeyboardEventArgs(key, message));
                }
            }
        }

        public class KeyboardEventArgs : KeyEventArgs
        {
            public uint MMessage;
            public ushort MKey;
            public bool MShift;

            public KeyboardEventArgs(ushort key, uint message) : base((Keys)key)
            {
                MMessage = message;
                MKey = key;
            }
        }

        static KeyboardListener()
        {
            ListeningWindow.KeyDelegate keyDelegate = new ListeningWindow.KeyDelegate(KeyHandler);
            _listeningWindow = new ListeningWindow(keyDelegate);
        }

        private class ListeningWindow : NativeWindow
        {
            [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern unsafe bool RegisterRawInputDevices(Rawinputdev* rawInputDevices, uint numDevices, uint size);

            [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.I4)]
            internal static extern unsafe int GetRawInputData(void* hRawInput, uint uiCommand, byte* pData, uint* pcbSize, uint cbSizeHeader);

            public delegate void KeyDelegate(ushort key, uint message);

            private const int
                WsClipchildren = 0x02000000,
                WmInput = 0x00FF,
                RidevInputsink = 0x00000100,
                RidInput = 0x10000003,
                RimTypekeyboard = 1;

            private uint _mPreviousMessage = 0;
            private ushort _mPreviousControlKey = 0;
            private KeyDelegate _mKeyHandler = null;

            internal unsafe struct Rawinputdev
            {
                public ushort UsUsagePage;
                public ushort UsUsage;
                public uint DwFlags;
                public void* HwndTarget;
            };

            internal unsafe struct Rawinputheader
            {
                public uint DwType;
                public uint DwSize;
                public void* HDevice;
                public void* WParam;
            };

            internal unsafe struct Rawinputhkeyboard
            {
                public Rawinputheader Header;
                public ushort MakeCode;
                public ushort Flags;
                public ushort Reserved;
                public ushort VKey;
                public uint Message;
                public uint ExtraInformation;
            };

            public ListeningWindow(KeyDelegate keyHandlerFunction)
            {
                _mKeyHandler = keyHandlerFunction;

                CreateParams createParams = new CreateParams();

                createParams.Caption = "Hidden window";
                createParams.ClassName = null;
                createParams.X = 0x7FFFFFFF;
                createParams.Y = 0x7FFFFFFF;
                createParams.Height = 0;
                createParams.Width = 0;
                createParams.Style = WsClipchildren;

                this.CreateHandle(createParams);

                unsafe
                {
                    Rawinputdev rawInputDevice = new Rawinputdev();
                    rawInputDevice.UsUsagePage = 0x01;
                    rawInputDevice.UsUsage = 0x06;
                    rawInputDevice.DwFlags = RidevInputsink;
                    rawInputDevice.HwndTarget = this.Handle.ToPointer();

                    RegisterRawInputDevices(&rawInputDevice, 1, (uint)sizeof(Rawinputdev));
                }
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WmInput:
                        {
                            unsafe
                            {
                                uint dwSize, receivedBytes;
                                uint sizeofRawinputheader = (uint)(sizeof(Rawinputheader));

                                int res = GetRawInputData(m.LParam.ToPointer(), RidInput, null, &dwSize, sizeofRawinputheader);

                                if (res == 0)
                                {
                                    byte* lpb = stackalloc byte[(int)dwSize];

                                    receivedBytes = (uint)GetRawInputData((Rawinputhkeyboard*)(m.LParam.ToPointer()), RidInput, lpb, &dwSize, sizeofRawinputheader);

                                    if (receivedBytes == dwSize)
                                    {
                                        Rawinputhkeyboard* keybData = (Rawinputhkeyboard*)lpb;

                                        if (keybData->Header.DwType == RimTypekeyboard)
                                        {
                                            if ((_mPreviousControlKey != keybData->VKey) || (_mPreviousMessage != keybData->Message))
                                            {
                                                _mPreviousControlKey = keybData->VKey;
                                                _mPreviousMessage = keybData->Message;

                                                _mKeyHandler(keybData->VKey, keybData->Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }

                base.WndProc(ref m);
            }
        }
    }
}
