using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter {
    public class CharReader {
        public CharReader() {
            while (true) {
                string s = System.Console.ReadLine();
                if (s == null) {
                    break;
                }
                this.InputLine = s + "\n";  // 終了するには文字のない行に移動してからCtrl+Z
            }
            this.Line = "";
            this.LineLength = 0;
            this.LineIndex = 1;
        }
        public CharReader(string str) {
            this.InputLine = str;
            this.Line = "";
            this.LineLength = 0;
            this.LineIndex = 1;
        }
        private Queue<string> inputLine = new Queue<string>();
        public string InputLine {
            set {
                this.inputLine.Enqueue(value);
            }
        }
        public string Line {
            get; set;
        }
        public int LineLength {
            get; set;
        }
        public int LineIndex {
            get; set;
        }

        public string GetLine() {
            try {
                this.Line = this.inputLine.Dequeue();
                this.LineLength = this.Line.Length;
                this.LineIndex = 0;
            } catch (Exception e) { this.Line = null; }
            return this.Line;
        }

        public char NextChar() { // 次の一文字を返す
            if (this.LineIndex < this.LineLength) { // 文字列にまだ読みだす文字が残っているとき
                char c = this.Line[this.LineIndex];
                this.LineIndex++;
                return c;
            }
            this.LineIndex++;
            if (this.LineIndex == this.LineLength) {
                return '\n';
            }
            if (this.GetLine() != null) { // LineIndex > LineLengthの場合は一行読んでnextChar()する
                return this.NextChar();
            }
            return '\0'; // 読むべき行がなくなったら0(End Of File)を返す
        }

        public void BackChar() {
            this.LineIndex--;
        }

        public static void Main_cr() {
            CharReader reader = new CharReader();
            string s = "";
            while (true) {
                char ch = reader.NextChar();
                if (ch == '\0') {
                    break;
                }
                if (ch == '\n') {
                    s += ch;
                } else {
                    s = s + ch + " ";
                }
            }
            System.Console.WriteLine(s);
            System.Console.ReadLine();
        }
    }
}
