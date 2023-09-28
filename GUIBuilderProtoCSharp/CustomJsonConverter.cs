using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GUIBuilderProtoCSharp {
    public class CustomJsonConverter {
    }

    public class CursorJsonConverter : JsonConverter<Cursor> {
        public override Cursor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return null;
            // return typeof(Cursors).GetProperty(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Cursor value, JsonSerializerOptions options) {
            writer.WriteStringValue($"{null}");
        }
    }

    public class PointJsonConverter : JsonConverter<Point> {
        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            // 字句解析っぽいことをしてる
            Point p = new Point();
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonTokenType.PropertyName:
                        if (reader.GetString() == "X") {
                            reader.Read();
                            p.X = reader.GetInt32();
                        } else if (reader.GetString() == "Y") {
                            reader.Read();
                            p.Y = reader.GetInt32();
                        }
                        break;
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.EndObject:
                        return p;
                }
            }
            throw new JsonException("JSONオブジェクトの終了タグが見つかりませんでした");
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteNumberValue(value.X);
            writer.WritePropertyName("Y");
            writer.WriteNumberValue(value.Y);
            writer.WriteEndObject();
        }
    }

    public class IconJsonConverter : JsonConverter<Icon> {
        static int count = 0;
        public override Icon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            try {
                return new Icon(reader.GetString());
            } catch (FileNotFoundException) {
                return new Form().Icon;
            }
        }

        public override void Write(Utf8JsonWriter writer, Icon value, JsonSerializerOptions options) {
            string fileName = $"{Form1.workingDirectory}\\Resources\\icon{++count}.ico";
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            value.Save(fs);
            fs.Close();
            writer.WriteStringValue(fileName);
        }
    }

    public class BindingContextJsonConverter : JsonConverter<BindingContext> {
        public override BindingContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, BindingContext value, JsonSerializerOptions options) {
            writer.WriteStringValue("");
        }
    }

    public class FontJsonConverter : JsonConverter<Font> {
        public override Font? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string fontName = "";
            double fontSize = 9;
            FontStyle fontStyle = FontStyle.Regular;
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonTokenType.PropertyName:
                        if (reader.GetString() == "Name") {
                            reader.Read();
                            fontName = reader.GetString();
                        } else if (reader.GetString() == "Size") {
                            reader.Read();
                            fontSize = reader.GetDouble();
                        } else if (reader.GetString() == "Style") {
                            reader.Read();
                            //List<string> styles=new List<string>();
                            string[] styles = System.Text.RegularExpressions.Regex.Replace(reader.GetString(), @"[\s]+", "").Split(',');
                            for (int i = 0; i < styles.Length; i++) {
                                if (styles[i] == "Regular") {
                                    fontStyle = FontStyle.Regular;
                                } else if (styles[i] == "Bold") {
                                    fontStyle ^= FontStyle.Bold;
                                } else if (styles[i] == "Italic") {
                                    fontStyle ^= FontStyle.Italic;
                                } else if (styles[i] == "Underline") {
                                    fontStyle ^= FontStyle.Underline;
                                } else if (styles[i] == "Strikeout") {
                                    fontStyle ^= FontStyle.Strikeout;
                                }
                            }
                        }
                        break;
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.EndObject:
                        return new Font(fontName, (float)fontSize, fontStyle);
                }
            }
            throw new JsonException("JSONオブジェクトの終了タグが見つかりませんでした");
        }

        public override void Write(Utf8JsonWriter writer, Font value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteStringValue($"{value.Name}");
            writer.WritePropertyName("Size");
            writer.WriteNumberValue(value.Size);
            writer.WritePropertyName("Style");
            writer.WriteStringValue($"{value.Style}");
            writer.WriteEndObject();
        }
    }

    public class WindowTargetJsonConverter : JsonConverter<IWindowTarget> {
        public override IWindowTarget? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, IWindowTarget value, JsonSerializerOptions options) {
            writer.WriteStringValue("");
        }
    }

    public class ColorJsonConverter : JsonConverter<Color> {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return ColorTranslator.FromHtml(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) {
            writer.WriteStringValue($"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}");
        }
    }
    public class ImageJsonConverter : JsonConverter<Image> {
        static int count = 0;
        public override Image? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return Image.FromFile(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options) {
            string fileName = $"{Form1.workingDirectory}\\Resources\\image{++count}.png";
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            value.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
            fs.Close();
            writer.WriteStringValue(fileName);
        }
    }

    public class ItemsJsonConverter : JsonConverter<CheckedListBox.ObjectCollection> {
        public override CheckedListBox.ObjectCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            CheckedListBox checkedListBox = new();
            CheckedListBox.ObjectCollection objectCollection = checkedListBox.Items;
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.StartArray:
                        break;
                    case JsonTokenType.EndArray:
                        return objectCollection;
                    case JsonTokenType.String:
                        objectCollection.Add(reader.GetString());
                        System.Diagnostics.Debug.WriteLine("CheckedListBox.ObjectCollection Read");
                        break;
                    case JsonTokenType.EndObject:
                        break;
                }
            }
            throw new JsonException("JSONオブジェクトの終了タグが見つかりませんでした");
        }

        public override void Write(Utf8JsonWriter writer, CheckedListBox.ObjectCollection value, JsonSerializerOptions options) {
            writer.WriteStringValue("abcde"/*value.ToString()*/);
            System.Diagnostics.Debug.WriteLine("CheckedListBox.ObjectCollection Write");
        }
    }

    public class ComboBoxItemsJsonConverter : JsonConverter<ComboBox.ObjectCollection> {
        public override ComboBox.ObjectCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            ComboBox ComboBox = new();
            ComboBox.ObjectCollection objectCollection = ComboBox.Items;
            while (reader.Read()) {
                switch (reader.TokenType) {
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.StartArray:
                        break;
                    case JsonTokenType.EndArray:
                        return objectCollection;
                    case JsonTokenType.String:
                        objectCollection.Add(reader.GetString());
                        System.Diagnostics.Debug.WriteLine("ComboBox.ObjectCollection Read");
                        break;
                    case JsonTokenType.EndObject:
                        break;
                }
            }
            throw new JsonException("JSONオブジェクトの終了タグが見つかりませんでした");
        }

        public override void Write(Utf8JsonWriter writer, ComboBox.ObjectCollection value, JsonSerializerOptions options) {
            writer.WriteStringValue("abcde"/*value.ToString()*/);
            System.Diagnostics.Debug.WriteLine("ComboBox.ObjectCollection Write");
        }
    }
}
