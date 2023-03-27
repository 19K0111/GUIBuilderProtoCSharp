using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GUIBuilderProtoCSharp {
    internal class ControlsJson {
        public string ControlTypeName { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        public CheckedListBox.ObjectCollection Items { get; set; } // CheckedListBox

        public object Tag { get; set; }

        public bool AutoCheck { get; set; } // CheckBox

        public bool AutoEllipsis { get; set; }

        public bool CheckOnClick { get; set; } // CheckedListBox

        public int ColumnWidth { get; set; } // CheckedListBox

        public bool Enabled { get; set; }

        public int HorizontalExtent { get; set; } // CheckedListBox

        public bool HorizontalScrollbar { get; set; } // CheckedListBox

        public ImeMode ImeMode { get; set; } // CheckedListBox

        public bool IntegralHeight { get; set; } // CheckedListBox

        public bool MultiColumn { get; set; } // CheckedListBox

        public bool ScrollAlwaysVisible { get; set; } // CheckedListBox

        public SelectionMode SelectionMode { get; set; } // CheckedListBox

        public bool Sorted { get; set; } // CheckedListBox

        public int TabIndex { get; set; }

        public bool ThreeState { get; set; } // CheckBox

        public bool TabStop { get; set; }

        public bool Visible { get; set; }

        public AnchorStyles Anchor { get; set; }

        public bool AutoSize { get; set; }

        public AutoSizeMode AutoSizeMode { get; set; }

        public DockStyle Dock { get; set; }

        public Point Location { get; set; }

        public Padding Margin { get; set; }

        public Size MaximumSize { get; set; }

        public Size MinimumSize { get; set; }

        public Padding Padding { get; set; }

        public Size Size { get; set; }

        public Appearance Appearance { get; set; } // CheckBox

        public Color BackColor { get; set; }

        public Image BackgroundImage { get; set; }

        public ImageLayout BackgroundImageLayout { get; set; }

        public BorderStyle BorderStyle { get; set; } // CheckedListBox

        public ContentAlignment CheckAlign { get; set; } // CheckBox

        public bool Checked { get; set; } // CheckBox

        public CheckState CheckState { get; set; } // CheckBox

        public Cursor Cursor { get; set; }

        public FlatButtonAppearance FlatAppearance { get; set; }

        public FlatStyle FlatStyle { get; set; }

        public Font Font { get; set; }

        public Color ForeColor { get; set; }

        public Image Image { get; set; }

        public ContentAlignment ImageAlign { get; set; }

        public string Text { get; set; }

        public ContentAlignment TextAlign { get; set; }

        public TextImageRelation TextImageRelation { get; set; }

        public bool ThreeDCheckBoxes { get; set; } // CheckedListBox

        public bool UseMnemonic { get; set; }

        public bool UseVisualStyleBackColor { get; set; }

        //[JsonPropertyName("type")]
        //public string ControlTypeName {
        //    get; set;
        //}

        //[JsonPropertyName("anchor")]
        //public string Anchor {
        //    get; set;
        //}

        //[JsonPropertyName("autoellipsis")]
        //public bool AutoEllipsis {
        //    get; set;
        //}

        //[JsonPropertyName("back-color")]
        //public int[] BackColor {
        //    get; set;
        //}

        //[JsonPropertyName("bg-image")]
        //public string BackgroundImage {
        //    get; set;
        //}

        //[JsonPropertyName("bg-image-layout")]
        //public string BackgroundImageLayout {
        //    get; set;
        //}

        //[JsonPropertyName("dock")]
        //public string Dock {
        //    get; set;
        //}

        //[JsonPropertyName("enabled")]
        //public bool Enabled {
        //    get; set;
        //}

        //[JsonPropertyName("font")]
        //public string[] Font {
        //    get; set;
        //}

        //[JsonPropertyName("font-height")]
        //public int FontHeight {
        //    get; set;
        //}

        //[JsonPropertyName("fore-color")]
        //public int[] ForeColor {
        //    get; set;
        //}

        //[JsonPropertyName("height")]
        //public int Height {
        //    get; set;
        //}

        //[JsonPropertyName("image")]
        //public string Image {
        //    get; set;
        //}

        //[JsonPropertyName("image-align")]
        //public string ImageAlign {
        //    get; set;
        //}

        //[JsonPropertyName("location")]
        //public int[] Location {
        //    get; set;
        //}

        //[JsonPropertyName("margin")]
        //public int[] Margin {
        //    get; set;
        //}

        //[JsonPropertyName("name")]
        //public string Name {
        //    get; set;
        //}

        //[JsonPropertyName("padding")]
        //public int[] Padding {
        //    get; set;
        //}

        //[JsonPropertyName("size")]
        //public int[] Size {
        //    get; set;
        //}

        //[JsonPropertyName("text")]
        //public string Text {
        //    get; set;
        //}

        //[JsonPropertyName("text-align")]
        //public string TextAlign {
        //    get; set;
        //}

        //[JsonPropertyName("visible")]
        //public bool Visible {
        //    get; set;
        //}

        //[JsonPropertyName("width")]
        //public int Width {
        //    get; set;
        //}
    }
}
