using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace GUIBuilderProtoCSharp {
    internal class ProjectJson {
        internal static JsonSerializerOptions options = new JsonSerializerOptions {
            // https://hirahira.blog/json-serialization/
            // シリアライズオプション
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,

            // デシリアライズオプション
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
        public static string Extension {
            get {
                return ".proj";
            }
        }
        [JsonConstructor]
        public ProjectJson() {
        }

        public ProjectJson(params string[] args) {
            Name = new string[args.Length];
            Designer = new string[args.Length];
            Code = new string[args.Length];
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
    }
}
