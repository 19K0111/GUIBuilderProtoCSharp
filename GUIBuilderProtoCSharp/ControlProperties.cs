using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilderProtoCSharp {
    internal class ControlProperties {
        static string[] commonProperties = new string[] {
            "Anchor",
            "AutoEllipsis",
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
            "TextAlign",
            "Visible",
            "Width",
        };

        Dictionary<string, string[]> properties = new Dictionary<string, string[]>();

        internal ControlProperties() {
            properties.Add(nameof(UserButton), commonProperties);
        }
    }
}
