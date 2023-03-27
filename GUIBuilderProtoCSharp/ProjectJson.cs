using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace GUIBuilderProtoCSharp {
    public static class GUIBuilderExtensions {
        public static string Project {
            get {
                return ".proj";
            }
        }
        public static string Design {
            get {
                return ".dsn";
            }
        }
        public static string BlockCode {
            get {
                return ".blk";
            }
        }
        public static string CSharpCode {
            get {
                return ".cs";
            }
        }
    }

    internal class ProjectJson {
        internal static JsonSerializerOptions options = new JsonSerializerOptions {
            // https://hirahira.blog/json-serialization/
            // シリアライズオプション
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
            IgnoreReadOnlyProperties = true, // 読み取り専用プロパティを無視する
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault, // 規定値とnullのプロパティを無視する
            ReferenceHandler = ReferenceHandler.IgnoreCycles, // ループ参照を無視する
            IgnoreReadOnlyFields = true,

            // デシリアライズオプション
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            Converters = {new CursorJsonConverter(), new PointJsonConverter(), new IconJsonConverter(), new BindingContextJsonConverter(), new FontJsonConverter(),
                new WindowTargetJsonConverter(), new ColorJsonConverter(), new ImageJsonConverter(), new ItemsJsonConverter()},
        };
        internal static Newtonsoft.Json.JsonSerializerSettings newton_options = new Newtonsoft.Json.JsonSerializerSettings() {
            DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore, // 規定値プロパティを無視する
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, // nullのプロパティを無視する
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, // ループ参照を無視する
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,  // 一覧にないプロパティ無視する
        };
        
        [JsonConstructor]
        public ProjectJson() {
        }

        public ProjectJson(params string[] args) {
            Name = new string[args.Length];
            Designer = new string[args.Length];
            Code = new string[args.Length];
            UseBlockCode = true;
            for (int i = 0; i < args.Length; i++) {
                Name[i] = "Form";
                Designer[i] = Name[i] + ".dsn";
                Code[i] = Name[i] + ".blk";
            }
        }
        [JsonIgnore]
        public string[] Name {
            get; set;
        }

        [JsonPropertyName("designer")]
        public string[] Designer {
            get; set;
        }

        [JsonPropertyName("src")]
        public string[] Code {
            get; set;
        }

        [JsonPropertyName("block-code")]
        public bool UseBlockCode {
            get;set;
        }
    }
}
