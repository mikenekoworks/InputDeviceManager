using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Input.Device.Setting {

    /// <summary>
    /// InputManagerを設定するためのクラス
    /// </summary>
    public class InputManagerConfigurator {

        /// <summary>
        /// 軸のタイプ
        /// </summary>
        public enum AxisType {
            KeyOrMouseButton = 0,
            MouseMovement = 1,
            JoystickAxis = 2

        };

        /// <summary>
        /// 入力の情報
        /// </summary>
        public class VirtualAxisBase {
            public string name = "";
            public string descriptiveName = "";
            public string descriptiveNegativeName = "";
            public string negativeButton = "";
            public string positiveButton = "";
            public string altNegativeButton = "";
            public string altPositiveButton = "";

            public float gravity = 0;
            public float dead = 0;
            public float sensitivity = 0;

            public bool snap = false;
            public bool invert = false;

            public AxisType type = AxisType.KeyOrMouseButton;

            // 1から始まる。
            public int axis = 1;
            // 0なら全てのゲームパッドから取得される。1以降なら対応したゲームパッド。
            public int joyNum = 0;

        }

        public class VirtualButton : VirtualAxisBase {

            public VirtualButton() {
                gravity = 1000;
                dead = 0.001f;
                sensitivity = 1000;
                type = AxisType.KeyOrMouseButton;
            }

        }

        public class VirtualJoystick : VirtualAxisBase {

            public VirtualJoystick() {
                dead = 0.2f;
                sensitivity = 1;
                type = AxisType.JoystickAxis;
            }

        }

        public class VirtualKey : VirtualAxisBase {

            public VirtualKey() {
                gravity = 3;
                sensitivity = 3;
                dead = 0.001f;
                type = AxisType.KeyOrMouseButton;
            }
        }

        SerializedObject serializedObject;
        SerializedProperty axesProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputManagerConfigurator() {
            // InputManager.assetをシリアライズされたオブジェクトとして読み込む
            serializedObject = new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/InputManager.asset" )[ 0 ] );
            axesProperty = serializedObject.FindProperty( "m_Axes" );
        }

        private List< string > GetPropertyNames( SerializedProperty prop ) {
            List< string > props = new List< string >();

            SerializedProperty child = prop.Copy();
            child.Next( true );

            do {
                props.Add( child.name );
            } while ( child.Next( false ) );


            return props;

        }

        private void UpdatePropertyValue( SerializedProperty parent, string name, float value ) {
            var prop = parent.FindPropertyRelative( name );
            if ( prop == null ) {
                Debug.LogWarning( name + " が見つかりません。" );
                return;
            }
            if ( prop.propertyType != SerializedPropertyType.Float ) {
                Debug.LogWarning( "プロパティの型が " + prop.propertyType + " と一致しません" );
                return;
            }
            prop.floatValue = value;
        }

        private void UpdatePropertyValue( SerializedProperty parent, string name, int value ) {
            var prop = parent.FindPropertyRelative( name );
            if ( prop == null ) {
                Debug.LogWarning( name + " が見つかりません。" );
                return;
            }
            if ( prop.propertyType == SerializedPropertyType.Integer ) {
                prop.intValue = ( int )value;
                return;
            }

            if ( prop.propertyType == SerializedPropertyType.Enum ) {
                prop.enumValueIndex = ( int )value;
                return;
            }

            Debug.LogWarning( "プロパティの型が " + prop.propertyType + " と一致しません" );
        }

        private void UpdatePropertyValue( SerializedProperty parent, string name, string value ) {
            var prop = parent.FindPropertyRelative( name );
            if ( prop == null ) {
                Debug.LogWarning( name + " が見つかりません。" );
                return;
            }
            if ( prop.propertyType != SerializedPropertyType.String ) {
                Debug.LogWarning( "プロパティの型が " + prop.propertyType + " と一致しません" );
                return;
            }
            prop.stringValue = value;

        }

        private void UpdatePropertyValue( SerializedProperty parent, string name, bool value ) {
            var prop = parent.FindPropertyRelative( name );
            if ( prop == null ) {
                Debug.LogWarning( name + " が見つかりません。" );
                return;
            }
            if ( prop.propertyType != SerializedPropertyType.Boolean ) {
                Debug.LogWarning( "プロパティの型が " + prop.propertyType + " と一致しません" );
                return;
            }
            prop.boolValue = value;
        }

        /// <summary>
        /// 新しい軸を追加する。
        /// </summary>
        /// <param name="new_axis"></param>
        public void Add( VirtualAxisBase new_axis ) {

            if ( new_axis.axis < 1 ) {
                Debug.LogError( "Axisは1以上に設定してください。" );
            }
            SerializedProperty axesProperty = serializedObject.FindProperty( "m_Axes" );

            // 設定の上書きをする為に、既存のものを検索する。
            SerializedProperty find_property = null;
            for ( int i = 0; i < axesProperty.arraySize; ++i ) {
                SerializedProperty prop = axesProperty.GetArrayElementAtIndex( i );

                // あったなら同じ名前かをチェック
                if ( prop.displayName == new_axis.name ) {
                    find_property = axesProperty.GetArrayElementAtIndex( i );
                    break;
                }
            }

            if ( find_property == null ) {
                axesProperty.arraySize++;
                serializedObject.ApplyModifiedProperties();

                find_property = axesProperty.GetArrayElementAtIndex( axesProperty.arraySize - 1 );
            }

            UpdatePropertyValue( find_property, "m_Name", new_axis.name );
            UpdatePropertyValue( find_property, "descriptiveName", new_axis.descriptiveName );
            UpdatePropertyValue( find_property, "descriptiveNegativeName", new_axis.descriptiveNegativeName );
            UpdatePropertyValue( find_property, "negativeButton", new_axis.negativeButton );
            UpdatePropertyValue( find_property, "positiveButton", new_axis.positiveButton );
            UpdatePropertyValue( find_property, "altNegativeButton", new_axis.altNegativeButton );
            UpdatePropertyValue( find_property, "altPositiveButton", new_axis.altPositiveButton );
            UpdatePropertyValue( find_property, "gravity", new_axis.gravity );
            UpdatePropertyValue( find_property, "dead", new_axis.dead );
            UpdatePropertyValue( find_property, "sensitivity", new_axis.sensitivity );
            UpdatePropertyValue( find_property, "snap", new_axis.snap );
            UpdatePropertyValue( find_property, "invert", new_axis.invert );
            UpdatePropertyValue( find_property, "type", (int)new_axis.type );
            UpdatePropertyValue( find_property, "axis", new_axis.axis - 1 );
            UpdatePropertyValue( find_property, "joyNum", new_axis.joyNum );

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 渡した文字列と先頭一致した設定を削除します。
        /// </summary>
        /// <param name="target_name"></param>
        public void RemoveAll( string target_name ) {

            SerializedProperty axesProperty = serializedObject.FindProperty( "m_Axes" );

            // 設定を削除するのでターゲットを探す
//            for ( int i = 0; i < axesProperty.arraySize; ++i ) {
            for ( int i = axesProperty.arraySize - 1; i >= 0;  --i ) {
                SerializedProperty prop = axesProperty.GetArrayElementAtIndex( i );

                if ( prop.displayName.Length < target_name.Length ) {
                    // 対象文字列の方が短いようだったらマッチしないので次へ
                    continue;
                }

                // 文字列を切り出して一致するようだと削除
                if ( prop.displayName.Substring( 0, target_name.Length ) == target_name ) {
                    axesProperty.DeleteArrayElementAtIndex( i );
                }

            }
            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// 設定を全てクリアします。
        /// </summary>
        public void Clear() {
            axesProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem( "Joystick Config/全ての設定を消去します" )]
        public static void ClearSettings() {
            InputManagerConfigurator config = new InputManagerConfigurator();
            config.Clear();
        }

    }

}