using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace GUIBuilderProtoCSharp {
    // JSON形式にシリアライズ　https://takap-tech.com/entry/2020/08/21/001300
    internal class DesignJson {
        public static string Extension {
            get {
                return ".dsn";
            }
        }
        [JsonConstructor]
        public DesignJson() {
        }

        public DesignJson(ProjectJson pj) {
            for (int i = 0; i < this.GetType().GetProperties().Length; i++) { 
                string propName = this.GetType().GetProperties()[i].Name;
                try {
                    this.GetType().GetProperty(propName).SetValue(this, typeof(Form3).GetProperty(propName).GetValue(Form1.f3));
                } catch (NullReferenceException) { }
                catch(ArgumentException) { }
            }
            //Name = pj.Name[0];
            //Text = Name;
            //Size = new Size(300,300);
            // Size = new int[] { 300, 300 };
        }

        public bool ControlBox { get; set; }

        public Icon Icon { get; set; }

        public bool MaximizeBox { get; set; }

        public bool MinimizeBox { get; set; }

        public double Opacity { get; set; }

        public bool ShowIcon { get; set; }

        public bool ShowInTaskbar { get; set; }

        public bool TopMost { get; set; }

        public object Tag { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        // public AnchorStyles Anchor { get; set; }

        public bool AutoScroll { get; set; }

        public Size AutoScrollMargin { get; set; }

        public Size AutoScrollMinSize { get; set; }

        public bool AutoSize { get; set; }

        public AutoSizeMode AutoSizeMode { get; set; }

        public Size MaximumSize { get; set; }

        public Size MinimumSize { get; set; }

        public Padding Padding { get; set; }

        public Size Size { get; set; }

        public Color BackColor { get; set; }

        public Image BackgroundImage { get; set; }

        public ImageLayout BackgroundImageLayout { get; set; }

        public Cursor Cursor { get; set; }

        public Font Font { get; set; }

        public Color ForeColor { get; set; }

        public RightToLeft RightToLeft { get; set; }

        public bool RightToLeftLayout { get; set; }

        // [JsonPropertyName("name")]
        public string Name { get; set; }

        //v[JsonPropertyName("text")]
        public string Text { get; set; }

        // public Control.ControlCollection Controls { get; set; }

        // [JsonPropertyName("size")]
        // public int[] Size { get; set; }

        // [JsonPropertyName("controls")]
        public List<ControlsJson> Controls { get; set; }
    }
}
