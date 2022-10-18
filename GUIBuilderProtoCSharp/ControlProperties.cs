using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilderProtoCSharp {
    public abstract class PropertiesFactory {
        protected static string[] propertiesList;
        protected static string[] descriptionList;

        public int Count {
            get {
                return propertiesList.Length;
            }
        }

        Dictionary<string, string[]> properties = new Dictionary<string, string[]>();
        Dictionary<string, string> propAndDesc = new Dictionary<string, string>();

        public void CheckCorresponds() {
            if (propertiesList.Length != descriptionList.Length) {
                throw new MissingFieldException($"プロパティと説明の個数が一致しません。\nプロパティ：{propertiesList.Length}, 説明：{descriptionList.Length}");
            }
            properties.Add(nameof(UserControl), propertiesList);
            for (int i = 0; i < propertiesList.Length; i++) {
                propAndDesc.Add(propertiesList[i], descriptionList[i]);
            }
        }

        public string GetDescription(string propName) {
            return propAndDesc[propName];
        }

        public string GetProperty(int index) {
            return propertiesList[index];
        }
    }

    public class ControlProperties : PropertiesFactory {

        public ControlProperties() {
            propertiesList = new string[] {
                "Anchor",
                "BackColor",
                "BackgroundImage",
                "BackgroundImageLayout",
                "Dock",
                "Enabled",
                "Font",
                // "FontHeight",
                "ForeColor",
                "Height",
                "Image",
                "ImageAlign",
                "Location",
                "Margin",
                "Name",
                "Padding",
                "Size",
                "Text",
                "Visible",
                "Width",
            };

            descriptionList = new string[] {
                "コントロールがバインドされるコンテナーの端を取得または設定し、親のサイズ変更時に、コントロールのサイズがどのように変化するかを決定します。",
                "コントロールの背景色を取得または設定します。",
                "コントロールに表示される背景イメージを取得または設定します。",
                "ImageLayout 列挙型で定義される背景画像のレイアウトを取得または設定します。",
                "コントロールの境界のうち、親コントロールにドッキングする境界を取得または設定します。また、コントロールのサイズが親コントロール内でどのように変化するかを決定します。",
                "コントロールがユーザーとの対話に応答できるかどうかを示す値を取得または設定します。",
                "コントロールによって表示されるテキストのフォントを取得または設定します。",
                // "コントロールのフォントの高さを取得または設定します。",
                "コントロールの前景色を取得または設定します。",
                "コントロールの高さを取得または設定します。",
                "コントロールに表示されるイメージを取得または設定します。",
                "コントロール上のイメージの配置を取得または設定します。",
                "コンテナーの左上隅に対する相対座標として、コントロールの左上隅の座標を取得または設定します。",
                "コントロール間の空白を取得または設定します。",
                "コントロールの名前を取得または設定します。",
                "コントロールの埋め込みを取得または設定します。",
                "コントロールの高さと幅を取得または設定します。",
                "このコントロールに関連付けられているテキストを取得または設定します。",
                "コントロールとそのすべての子コントロールが表示されているかどうかを示す値を取得または設定します。",
                "コントロールの幅を取得または設定します。",
            };

            CheckCorresponds();
        }
    }

    public class ButtonProperties : PropertiesFactory {
        public ButtonProperties() {
            propertiesList = new string[] {
                "AutoEllipsis",
                "TextAlign",
            };

            descriptionList = new string[] {
                "ボタン コントロールのテキストが、指定されたコントロールの長さを超えることを示す省略記号文字 (...) を、コントロールの右端に表示するかどうかを示す値を取得または設定します。",
                "ボタン コントロールのテキストの配置を取得または設定します。",
            };

            CheckCorresponds();
        }
    }
}
