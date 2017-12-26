using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input.Device {

    [System.Serializable]
    public class XboxJoystick : IInputDevice {

        public enum Button {
            Up,       // DPAD 
            Down,
            Left,
            Right,

            A,          // Buttons
            B,
            X,
            Y,

            L,
            R,
            LT,
            RT,

            Back,
            Start,

            LS,         // Stick Button
            RS,
        }


        [System.Flags]
        public enum ButtonBit {
            Up = 1 << Button.Up,
            Down = 1 << Button.Down,
            Left = 1 << Button.Left,
            Right = 1 << Button.Right,

            A = 1 << Button.A,
            B = 1 << Button.B,
            X = 1 << Button.X,
            Y = 1 << Button.Y,

            L = 1 << Button.L,
            R = 1 << Button.R,
            LT = 1 << Button.LT,
            RT = 1 << Button.RT,

            Back = 1 << Button.Back,
            Start = 1 << Button.Start,

            LS = 1 << Button.LS,
            RS = 1 << Button.RS,
        
        }

        private enum StickNameIndex {
            LeftStickHorizontal,
            LeftStickVertical,
            RightStickHorizontal,
            RightStickVertical,
            DPadHorizontal,
            DPadVertical,
            LeftTrigger,
            RightTrigger
        }

        private enum ButtonNameIndex {
            A,
            B,
            X,
            Y,
            L,
            R,
            Back,
            Start,
            LS,
            RS
        }

        public readonly static string XBOXJOYSTICK_PREFIX = "XboxJoystick";
        // ボタンの設定名称
        public readonly static string SETTING_NAME_LS_HORIZONTAL = "LS H";
        public readonly static string SETTING_NAME_LS_VERTICAL = "LS V";
        public readonly static string SETTING_NAME_RS_HORIZONTAL = "RS H";
        public readonly static string SETTING_NAME_RS_VERTICAL = "RS V";
        public readonly static string SETTING_NAME_DPAD_HORIZONTAL = "DPAD H";
        public readonly static string SETTING_NAME_DPAD_VERTICAL = "DPAD V";
        public readonly static string SETTING_NAME_A = "A";
        public readonly static string SETTING_NAME_B = "B";
        public readonly static string SETTING_NAME_X = "X";
        public readonly static string SETTING_NAME_Y = "Y";
        public readonly static string SETTING_NAME_L = "L";
        public readonly static string SETTING_NAME_R = "R";
        public readonly static string SETTING_NAME_LT = "Left Trigger";
        public readonly static string SETTING_NAME_RT = "Rirght Trigger";
        public readonly static string SETTING_NAME_BACK = "Back";
        public readonly static string SETTING_NAME_START = "Start";
        public readonly static string SETTING_NAME_LS = "LS Thumb";
        public readonly static string SETTING_NAME_RS = "RS Thumb";

        private uint oldInput;
        // トリガー
        public uint Trigger {
            get;
            private set;
        }
        // プレス
        public uint Press {
            get;
            private set;
        }
        // リリース
        public uint Release {
            get;
            private set;
        }

        private Vector2 leftStickRaw;
        private Vector2 rightStickRaw;

        public Vector2 LeftStick { get { return leftStickRaw; } }
        public Vector2 RightStick { get { return rightStickRaw; } }

        public float LeftTrigger {
            get;
            private set;
        }
        public float RightTrigger {
            get;
            private set;
        }

        private int assignId;
        private List<string> buttonNames = new List<string>();
        private List<string> stickNames = new List<string>();

        /// <summary>
        /// スティックの入力値を取得（フィルタ値なし）
        /// </summary>
        /// <param name="stick_index"></param>
        /// <returns></returns>
        private float GetAxisRaw( StickNameIndex stick_index ) {
            return UnityEngine.Input.GetAxisRaw( stickNames[ ( int )stick_index ] );
        }
        /// <summary>
        /// スティックの入力値を取得（フィルタ値あり）
        /// </summary>
        /// <param name="stick_index"></param>
        /// <returns></returns>
        private float GetAxis( StickNameIndex stick_index ) {
            return UnityEngine.Input.GetAxis( stickNames[ ( int )stick_index ] );
        }

        /// <summary>
        /// ボタンの押し下げ状態を取得
        /// </summary>
        /// <param name="stick_index"></param>
        /// <returns></returns>
        private bool GetPress( ButtonNameIndex button_index ) {
            return UnityEngine.Input.GetButton( buttonNames[ ( int )button_index ] );
        }

        /// <summary>
        /// ジョイスティック名から使用するジョイスティック名を拾い上げる
        /// </summary>
        /// <param name="joystrick_names"></param>
        /// <returns></returns>
        public static int FindJoystickName( string[] joystrick_names ) {

            for ( int i = 0; i < joystrick_names.Length; i++ ) {

                if ( joystrick_names[ i ].ToLower().IndexOf( "xbox" ) >= 0 ) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// ジョイスティックデバイスの作成
        /// </summary>
        /// <param name="assign_id"></param>
        /// <returns></returns>
        public void Create( int assign_id ) {
            assignId = assign_id;

            string joystick_string = XBOXJOYSTICK_PREFIX + "{0} {1}";

            buttonNames.Clear();
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_A ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_B ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_X ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_Y ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_L ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_R ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_BACK ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_START ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_LS ) );
            buttonNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_RS ) );

            stickNames.Clear();
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_LS_HORIZONTAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_LS_VERTICAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_RS_HORIZONTAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_RS_VERTICAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_DPAD_HORIZONTAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_DPAD_VERTICAL ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_LT ) );
            stickNames.Add( string.Format( joystick_string, assignId, SETTING_NAME_RT ) );

        }

        /// <summary>
        /// スティック状態の更新
        /// </summary>
        public void Update() {

            uint new_input = 0;

            // 十字パッド
            new_input |= ( GetAxis( StickNameIndex.DPadHorizontal ) < 0 ) ? ( uint )ButtonBit.Left : 0;
            new_input |= ( GetAxis( StickNameIndex.DPadHorizontal ) > 0 ) ? ( uint )ButtonBit.Right : 0;
            new_input |= ( GetAxis( StickNameIndex.DPadVertical ) < 0 ) ? ( uint )ButtonBit.Up : 0;
            new_input |= ( GetAxis( StickNameIndex.DPadVertical ) > 0 ) ? ( uint )ButtonBit.Down : 0;

            // ボタン
            new_input |= GetPress( ButtonNameIndex.A ) ? ( uint )ButtonBit.A : 0;
            new_input |= GetPress( ButtonNameIndex.B ) ? ( uint )ButtonBit.B : 0;
            new_input |= GetPress( ButtonNameIndex.X ) ? ( uint )ButtonBit.X : 0;
            new_input |= GetPress( ButtonNameIndex.Y ) ? ( uint )ButtonBit.Y : 0;
            new_input |= GetPress( ButtonNameIndex.Back ) ? ( uint )ButtonBit.Back : 0;
            new_input |= GetPress( ButtonNameIndex.Start ) ? ( uint )ButtonBit.Start : 0;
            new_input |= GetPress( ButtonNameIndex.LS ) ? ( uint )ButtonBit.LS : 0;
            new_input |= GetPress( ButtonNameIndex.RS ) ? ( uint )ButtonBit.RS : 0;

            //L1/R1
            new_input |= GetPress( ButtonNameIndex.L ) ? ( uint )ButtonBit.L : 0;
            new_input |= GetPress( ButtonNameIndex.R ) ? ( uint )ButtonBit.R : 0;

            //LT/RT
            new_input |= ( GetAxisRaw( StickNameIndex.LeftTrigger ) > 0 ) ? ( uint )ButtonBit.LT : 0;
            new_input |= ( GetAxisRaw( StickNameIndex.RightTrigger ) > 0 ) ? ( uint )ButtonBit.RT : 0;

            uint changed_input = oldInput ^ new_input;

            uint new_trigger = changed_input & new_input;
            uint new_release = changed_input & ~new_input;

            Trigger = new_trigger;
            Release = new_release;
            Press = new_input;

            oldInput = new_input;


            leftStickRaw.x = GetAxisRaw( StickNameIndex.LeftStickHorizontal );
            leftStickRaw.y = GetAxisRaw( StickNameIndex.LeftStickVertical );
            rightStickRaw.x = GetAxisRaw( StickNameIndex.RightStickHorizontal );
            rightStickRaw.y = GetAxisRaw( StickNameIndex.RightStickVertical );
            LeftTrigger = GetAxisRaw( StickNameIndex.LeftTrigger );
            RightTrigger = GetAxisRaw( StickNameIndex.RightTrigger );


            //if ( new_input != 0 ) {
            //    Debug.Log( GetAxisRaw( StickNameIndex.LeftTrigger ) + " : " + GetAxisRaw( StickNameIndex.RightTrigger ) );
            //}

        }


    }

}
