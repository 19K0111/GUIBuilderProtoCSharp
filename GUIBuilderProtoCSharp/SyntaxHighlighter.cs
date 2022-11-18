using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace GUIBuilderProtoCSharp {
    internal abstract class SyntaxHighlighter : IDisposable {
        // 参考：https://qiita.com/Apeworks/items/f1ea7a41af8abcde7de5

        private RichTextBox generate = new RichTextBox();

        /// <summary>
        /// リソース解放済みフラグを取得します。
        /// </summary>
        public bool IsDisposed {
            get; private set;
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!this.IsDisposed) {
                if (disposing) {
                    this.generate.Dispose();
                }
                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// リッチテキスト形式のテキストを取得します。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="wideFont">全角文字のフォント</param>
        /// <returns></returns>
        public string GetRtf(string text, Font font, Font wideFont = null) {
          this.generate.Clear();
            this.generate.Text = text;
            if (!string.IsNullOrEmpty(text)) {
                // 各パターンに一致する文字色を設定
                foreach (var syntax in this.EnumerateSyntaxes()) {
                    foreach (Match m in Regex.Matches(text, syntax.Pattern, RegexOptions.IgnoreCase)) {
                        this.generate.Select(m.Index, m.Length);
                        this.generate.SelectionColor = syntax.Color;
                    }
                }

                // フォントを設定
                this.generate.SelectAll();
                this.generate.SelectionFont = font;
                if (wideFont != null) {
                    // 全角 (半角以外) のフォントを設定
                    foreach (Match m in Regex.Matches(text, @"[^\x01-\x7E]", RegexOptions.IgnoreCase)) {
                        this.generate.Select(m.Index, m.Length);
                        this.generate.SelectionFont = wideFont ?? font;
                    }
                }
            }
            return this.generate.Rtf;
        }

        /// <summary>
        /// <see cref="Syntax"/> を列挙します。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Syntax> EnumerateSyntaxes();

        /// <summary>
        /// ハイライトのパターンと色を表します。
        /// </summary>
        protected struct Syntax {
            public Syntax(string pattern, Color color) {
                this.Pattern = pattern;
                this.Color = color;
            }
            public string Pattern {
                get;
            }
            public Color Color {
                get;
            }
        }
    }

    internal class Highlighter : SyntaxHighlighter {
        /// <summary>予約語の色</summary>
        public Color WordColor { get; set; } = Color.Blue;

        /// <summary>記号の色</summary>
        public Color SignColor { get; set; } = Color.Black;

        /// <summary>数字の色</summary>
        public Color DigitColor { get; set; } = Color.FromArgb(9, 134, 68);

        /// <summary>文字列の色</summary>
        public Color StringColor { get; set; } = Color.FromArgb(0xa3, 0x15, 0x15);

        /// <summary>コメントの色</summary>
        public Color CommentColor { get; set; } = Color.FromArgb(0x00, 0x80, 0x00);

        /// <summary>予約語</summary>
        public string[] Words {
            get; set;
        }

        /// <summary>
        /// 予約語を列挙します。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> EnumerateWords() => this.Words ?? this.EnumerateKeywords();

        /// <summary>
        /// C#の予約語
        /// </summary>
        private IEnumerable<string> EnumerateKeywords() {
            yield return "abstract";
            yield return "as";
            yield return "base";
            yield return "bool";
            yield return "break";
            yield return "byte";
            yield return "case";
            yield return "catch";
            yield return "char";
            yield return "checked";
            yield return "class";
            yield return "const";
            yield return "continue";
            yield return "decimal";
            yield return "default";
            yield return "delegate";
            yield return "do";
            yield return "double";
            yield return "else";
            yield return "enum";
            yield return "event";
            yield return "explicit";
            yield return "extern";
            yield return "false";
            yield return "finally";
            yield return "fixed";
            yield return "float";
            yield return "for";
            yield return "foreach";
            yield return "goto";
            yield return "if";
            yield return "implicit";
            yield return "in";
            yield return "int";
            yield return "interface";
            yield return "internal";
            yield return "is";
            yield return "lock";
            yield return "long";
            yield return "namespace";
            yield return "new";
            yield return "null";
            yield return "object";
            yield return "operator";
            yield return "out";
            yield return "override";
            yield return "params";
            yield return "private";
            yield return "protected";
            yield return "public";
            yield return "readonly";
            yield return "ref";
            yield return "return";
            yield return "sbyte";
            yield return "sealed";
            yield return "short";
            yield return "sizeof";
            yield return "stackalloc";
            yield return "static";
            yield return "string";
            yield return "struct";
            yield return "switch";
            yield return "this";
            yield return "throw";
            yield return "true";
            yield return "try";
            yield return "typeof";
            yield return "uint";
            yield return "ulong";
            yield return "unchecked";
            yield return "unsafe";
            yield return "ushort";
            yield return "using";
            yield return "virtual";
            yield return "void";
            yield return "volatile";
            yield return "while";
        }


        protected override IEnumerable<Syntax> EnumerateSyntaxes() {// 予約語
            var words = this.EnumerateWords();
            foreach (var pattern in words.Select(item => this.ToPattern(item, true))) {
                yield return new Syntax(pattern, this.WordColor);
            }

            // 記号
            var signs = new[] { ",", ";", ":", "=", "+", "-", "*", "/", "%", "&", "|", "^", "~", "(", ")", "!" };
            foreach (var pattern in signs.Select(item => this.ToPattern(item, false))) {
                yield return new Syntax(pattern, this.SignColor);
            }

            // 数字
            // yield return new Syntax(@"(?!\W).\d+",this.DigitColor);

            // 文字列
            yield return new Syntax(@"""(\.|[^""])*""", this.StringColor);

            // char
            yield return new Syntax(@"'(\.|[^'])*'", this.StringColor);

            // コメント
            yield return new Syntax(@"//.*[$\r\n]*", this.CommentColor);
            yield return new Syntax(@"/\*[\s\S]*?\*/|//.*", this.CommentColor);
        }

        /// <summary>
        /// 単語を正規表現パターンに変換します。
        /// </summary>
        /// <param name="word"></param>
        /// <param name="whole">完全一致フラグ</param>
        /// <returns></returns>
        private string ToPattern(string word, bool whole) {
            var pattern = Regex.Escape(word).Replace(" ", @"\s+");
            if (whole) pattern = @"\b" + pattern + @"\b";
            return pattern;
        }
    }
}
