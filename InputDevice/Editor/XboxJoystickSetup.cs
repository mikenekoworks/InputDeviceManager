using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Input.Device.Setting {

    public class XboxJoystickSetup {

        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player1", false, 21 )]
        public static void MenuRemovePlayer1Config() { RemoveConfig( 0 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player2", false, 22 )]
        public static void MenuRemovePlayer2Config() { RemoveConfig( 1 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player3", false, 23 )]
        public static void MenuRemovePlayer3Config() { RemoveConfig( 2 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player4", false, 24 )]
        public static void MenuRemovePlayer4Config() { RemoveConfig( 3 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player5", false, 25 )]
        public static void MenuRemovePlayer5Config() { RemoveConfig( 4 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player6", false, 26 )]
        public static void MenuRemovePlayer6Config() { RemoveConfig( 5 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player7", false, 27 )]
        public static void MenuRemovePlayer7Config() { RemoveConfig( 6 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/Player8", false, 28 )]
        public static void MenuRemovePlayer8Config() { RemoveConfig( 7 ); }

        [MenuItem( "Joystick Config/Xbox Controllerの設定/削除/一括削除", false, 40 )]
        public static void MenuRemoveConfig() {
            RemoveAllConfig();
        }

        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/2 Players", false, 1 )]
        public static void MenuAdd2PlayersConfig() {
            AddConfig( 0 );
            AddConfig( 1 );
        }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/4 Players", false, 2 )]
        public static void MenuAdd4PlayersConfig() {
            AddConfig( 0 );
            AddConfig( 1 );
            AddConfig( 2 );
            AddConfig( 3 );
        }

        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player1", false, 21 )]
        public static void MenuAddPlayer1Config() { AddConfig( 0 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player2", false, 22 )]
        public static void MenuAddPlayer2Config() { AddConfig( 1 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player3", false, 23 )]
        public static void MenuAddPlayer3Config() { AddConfig( 2 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player4", false, 24 )]
        public static void MenuAddPlayer4Config() { AddConfig( 3 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player5", false, 25 )]
        public static void MenuAddPlayer5Config() { AddConfig( 4 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player6", false, 26 )]
        public static void MenuAddPlayer6Config() { AddConfig( 5 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player7", false, 27 )]
        public static void MenuAddPlayer7Config() { AddConfig( 6 ); }
        [MenuItem( "Joystick Config/Xbox Controllerの設定/追加/Player8", false, 28 )]
        public static void MenuAddPlayer8Config() { AddConfig( 7 ); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="button_name"></param>
        /// <param name="button_assign"></param>
        /// <param name="description"></param>
        private static void AddButton( InputManagerConfigurator config, int joy_num, string button_name, string button_assign, string description ) {
            // ボタンの追加。
            var new_button = new InputManagerConfigurator.VirtualButton();
            new_button.name = button_name;
            new_button.positiveButton = button_assign;
            new_button.descriptiveName = description;
            new_button.joyNum = joy_num;

            config.Add( new_button );
        }

        /// <summary>
        /// コントローラーのコンフィグを追加。
        /// </summary>
        /// <param name="player_index"></param>
        private static void AddConfig( int player_index ) {
            InputManagerConfigurator config = new InputManagerConfigurator();

            int joy_num = player_index + 1;

            var stick_base_name = string.Format( XboxJoystick.XBOXJOYSTICK_PREFIX + "{0} ", player_index );
            {
                // 左スティックの設定
                // 水平軸の作成
                var new_horizontal_axis = new InputManagerConfigurator.VirtualJoystick();
                new_horizontal_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_LS_HORIZONTAL;
                new_horizontal_axis.descriptiveName = "LS Left";
                new_horizontal_axis.descriptiveNegativeName = "LS Right";
                new_horizontal_axis.joyNum = joy_num;
                new_horizontal_axis.axis = 1;

                config.Add( new_horizontal_axis );

                // 垂直軸の設定
                var new_virtual_axis = new InputManagerConfigurator.VirtualJoystick();
                new_virtual_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_LS_VERTICAL;
                new_virtual_axis.descriptiveName = "LS Up";
                new_virtual_axis.descriptiveNegativeName = "LS Down";
                new_virtual_axis.joyNum = joy_num;
                new_virtual_axis.axis = 2;

                config.Add( new_virtual_axis );
            }

            {
                // 右スティックの設定
                // 水平軸の作成
                var new_horizontal_axis = new InputManagerConfigurator.VirtualJoystick();
                new_horizontal_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_RS_HORIZONTAL;
                new_horizontal_axis.descriptiveName = "RS Left";
                new_horizontal_axis.descriptiveNegativeName = "RS Right";
                new_horizontal_axis.joyNum = joy_num;
                new_horizontal_axis.axis = 4;

                config.Add( new_horizontal_axis );

                // 垂直軸の設定
                var new_virtual_axis = new InputManagerConfigurator.VirtualJoystick();
                new_virtual_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_RS_VERTICAL;
                new_virtual_axis.descriptiveName = "RS Up";
                new_virtual_axis.descriptiveNegativeName = "RS Down";
                new_virtual_axis.joyNum = joy_num;
                new_virtual_axis.axis = 5;

                config.Add( new_virtual_axis );

            }

            {
                // 方向パッドの設定
                // 水平軸の作成
                var new_horizontal_axis = new InputManagerConfigurator.VirtualJoystick();
                new_horizontal_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_DPAD_HORIZONTAL;
                new_horizontal_axis.descriptiveName = "DPad Left";
                new_horizontal_axis.descriptiveNegativeName = "DPad Right";
                new_horizontal_axis.joyNum = joy_num;
                new_horizontal_axis.axis = 6;

                config.Add( new_horizontal_axis );

                // 垂直軸の設定
                var new_virtual_axis = new InputManagerConfigurator.VirtualJoystick();
                new_virtual_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_DPAD_VERTICAL;
                new_virtual_axis.descriptiveName = "DPad Up";
                new_virtual_axis.descriptiveNegativeName = "DPad Down";
                new_virtual_axis.joyNum = joy_num;
                new_virtual_axis.axis = 7;
                new_virtual_axis.invert = true;

                config.Add( new_virtual_axis );
            }

            {
                // LT/RTの設定
                // 水平軸の作成
                var new_horizontal_axis = new InputManagerConfigurator.VirtualJoystick();
                new_horizontal_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_LT;
                new_horizontal_axis.descriptiveName = "LT";
                new_horizontal_axis.joyNum = joy_num;
                new_horizontal_axis.axis = 9;

                config.Add( new_horizontal_axis );

                // 垂直軸の設定
                var new_virtual_axis = new InputManagerConfigurator.VirtualJoystick();
                new_virtual_axis.name = stick_base_name + XboxJoystick.SETTING_NAME_RT;
                new_virtual_axis.descriptiveName = "RT";
                new_virtual_axis.joyNum = joy_num;
                new_virtual_axis.axis = 10;

                config.Add( new_virtual_axis );
            }


            {
                // ボタン全部
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_A, string.Format( "joystick {0} button {1}", player_index, 0 ), "A Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_B, string.Format( "joystick {0} button {1}", player_index, 1 ), "B Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_X, string.Format( "joystick {0} button {1}", player_index, 2 ), "X Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_Y, string.Format( "joystick {0} button {1}", player_index, 3 ), "Y Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_L, string.Format( "joystick {0} button {1}", player_index, 4 ), "L Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_R, string.Format( "joystick {0} button {1}", player_index, 5 ), "R Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_BACK, string.Format( "joystick {0} button {1}", player_index, 6 ), "Back Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_START, string.Format( "joystick {0} button {1}", player_index, 7 ), "Start Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_LS, string.Format( "joystick {0} button {1}", player_index, 8 ), "LeftStick Button" );
                AddButton( config, joy_num, stick_base_name + XboxJoystick.SETTING_NAME_RS, string.Format( "joystick {0} button {1}", player_index, 9 ), "RightStick Button" );
            }


        }

        /// <summary>
        /// コントローラーのコンフィグを削除
        /// </summary>
        /// <param name="player_index"></param>
        private static void RemoveConfig( int player_index ) {
            InputManagerConfigurator config = new InputManagerConfigurator();

            var stick_base_name = string.Format( XboxJoystick.XBOXJOYSTICK_PREFIX + "{0} ", player_index );
            config.RemoveAll( stick_base_name );
        }
        /// <summary>
        /// 
        /// </summary>
        private static void RemoveAllConfig() {
            InputManagerConfigurator config = new InputManagerConfigurator();

            var stick_base_name = string.Format( XboxJoystick.XBOXJOYSTICK_PREFIX );
            config.RemoveAll( stick_base_name );
        }
    }

}

