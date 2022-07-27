using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GUIBuilderProtoCSharp {
    internal class ControlsJson {
        [JsonPropertyName("type")]
        public string Type {
            get; set;
        }

        [JsonPropertyName("anchor")]
        public string Anchor {
            get; set;
        }

        [JsonPropertyName("autoellipsis")]
        public bool AutoEllipsis {
            get; set;
        }

        [JsonPropertyName("back-color")]
        public int[] BackColor {
            get; set;
        }

        [JsonPropertyName("bg-image")]
        public string BackgroundImage {
            get; set;
        }

        [JsonPropertyName("bg-image-layout")]
        public string BackgroundImageLayout {
            get; set;
        }

        [JsonPropertyName("dock")]
        public string Dock {
            get; set;
        }

        [JsonPropertyName("enabled")]
        public bool Enabled {
            get; set;
        }

        [JsonPropertyName("font")]
        public string[] Font {
            get; set;
        }

        [JsonPropertyName("font-height")]
        public int FontHeight {
            get; set;
        }

        [JsonPropertyName("fore-color")]
        public int[] ForeColor {
            get; set;
        }

        [JsonPropertyName("height")]
        public int Height {
            get; set;
        }

        [JsonPropertyName("image")]
        public string Image {
            get; set;
        }

        [JsonPropertyName("image-align")]
        public string ImageAlign {
            get; set;
        }

        [JsonPropertyName("location")]
        public int[] Location {
            get; set;
        }

        [JsonPropertyName("margin")]
        public int[] Margin {
            get; set;
        }

        [JsonPropertyName("name")]
        public string Name {
            get; set;
        }

        [JsonPropertyName("padding")]
        public int[] Padding {
            get; set;
        }

        [JsonPropertyName("size")]
        public int[] Size {
            get; set;
        }

        [JsonPropertyName("text")]
        public string Text {
            get; set;
        }

        [JsonPropertyName("text-align")]
        public string TextAlign {
            get; set;
        }

        [JsonPropertyName("visible")]
        public bool Visible {
            get; set;
        }

        [JsonPropertyName("width")]
        public int Width {
            get; set;
        }
    }
}
