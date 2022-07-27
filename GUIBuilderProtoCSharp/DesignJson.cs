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
            Name = pj.Name[0];
            Text = Name;
            Size = new int[] { 300, 300 };
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("size")]
        public int[] Size { get; set; }

        [JsonPropertyName("controls")]
        public List<ControlsJson> Controls { get; set; }
    }
}
