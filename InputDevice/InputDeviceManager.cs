using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Input {

    public class InputDeviceManager : MonoBehaviour {

        [SerializeField]
        private List< Input.Device.IInputDevice > devices = new List<Device.IInputDevice>();

        [SerializeField]
        private List< DeviceId > assignDevices;

        private string[] joystickNames;

        /// <summary>
        /// 入力デバイスの作成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assign_id"></param>
        private void Create<T>( int assign_id ) where T : Input.Device.IInputDevice, new() {

            T new_device = new T();
            new_device.Create( assign_id );

            devices.Add( new_device );
        }


        /// <summary>
        /// 入力デバイスの初期化
        /// </summary>
        /// <param name="device_ids"></param>
        public void Initialize( DeviceId[] device_ids ) {

            joystickNames = UnityEngine.Input.GetJoystickNames();
            assignDevices.Clear();

            // リフレクションからジェネリックメソッドの呼び出しをしようかともおもったけど、ひとまずswitchで分岐させておく。
            for ( int i = 0; i < device_ids.Length; ++i ) {
                switch ( device_ids[ i ] ) {
                case DeviceId.Keyboard:
                        assignDevices.Add( DeviceId.Keyboard );
                    break;
                case DeviceId.XboxJoystick:

                    // コントローラーリストを検索して対応する名前を探し出す。
                    // あるかどうかが重要で何番目にみつかったとかは関係ない。
                    // プレイヤーのIDとして割り振ったiの値がジョイスティックのキーアサインに付与される。
                    int find_index = Input.Device.XboxJoystick.FindJoystickName( joystickNames );
                    if ( find_index == -1 ) {
                        Debug.LogWarning( i + "番目に指定されているコントローラーが見つかりません。" );
                        assignDevices.Add( DeviceId.None );
                    } else {
                        // コントローラーの名前リストに名前があるので使えるコントローラーと認識。
                        Create<Input.Device.XboxJoystick>( i );

                        // リストから名前消す
                        joystickNames[ find_index ] = "";
                        assignDevices.Add( DeviceId.XboxJoystick );
                    }
                    break;
                }
            }

        }


        /// <summary>
        /// 入力機器の状態更新
        /// </summary>
        void Update() {

            for ( int i = 0; i < devices.Count; ++i ) {
                devices[ i ].Update();
            }

        }

    }

}

