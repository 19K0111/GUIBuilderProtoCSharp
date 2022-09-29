using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilderProtoCSharp {
    public class ControlProperties {
        public static readonly string[] commonProperties = new string[] {
            "Anchor",
            // "AutoEllipsis",
            "BackColor",
            "BackgroundImage",
            "BackgroundImageLayout",
            "Dock",
            "Enabled",
            "Font",
            "FontHeight",
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
            // "TextAlign",
            "Visible",
            "Width",
        };

        public static readonly string[] commonPropertiesDescription = new string[] {
            "コントロールがバインドされるコンテナーの端を取得または設定し、親のサイズ変更時に、コントロールのサイズがどのように変化するかを決定します。",
            // "コントロールのテキストが、指定されたコントロールの長さを超えることを示す省略記号文字 (...) を、コントロールの右端に表示するかどうかを示す値を取得または設定します。",
            "コントロールの背景色を取得または設定します。",
            "コントロールに表示される背景イメージを取得または設定します。",
            "ImageLayout 列挙型で定義される背景画像のレイアウトを取得または設定します。",
            "コントロールの境界のうち、親コントロールにドッキングする境界を取得または設定します。また、コントロールのサイズが親コントロール内でどのように変化するかを決定します。",
            "コントロールがユーザーとの対話に応答できるかどうかを示す値を取得または設定します。",
            "コントロールによって表示されるテキストのフォントを取得または設定します。",
            "コントロールのフォントの高さを取得または設定します。",
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
            // "コントロールのテキストの配置を取得または設定します。",
            "コントロールとそのすべての子コントロールが表示されているかどうかを示す値を取得または設定します。",
            "コントロールの幅を取得または設定します。",
        };

        public int Count {
            get {
                return commonProperties.Length;
            }
        }

        Dictionary<string, string[]> properties = new Dictionary<string, string[]>();
        Dictionary<string, string> propAndDesc = new Dictionary<string, string>();

        public ControlProperties() {
            if (commonProperties.Length!=commonPropertiesDescription.Length) {
                throw new MissingFieldException($"プロパティと説明の個数が一致しません。\nプロパティ：{commonProperties.Length}, 説明：{commonPropertiesDescription.Length}");
            }
            properties.Add(nameof(UserControl), commonProperties);
            for (int i = 0; i < commonProperties.Length; i++) {
                propAndDesc.Add(commonProperties[i], commonPropertiesDescription[i]);
            }
        }

        public string GetDescription(string propName) {
            return propAndDesc[propName];
        }
    }
}
