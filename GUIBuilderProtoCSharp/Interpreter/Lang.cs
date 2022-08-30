using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter {
    public enum Mnemonic {
        LDC = 0,
        STV = 1,
        LDV = 2,
        PUSH = 3,
        POP = 4,
        AD = 5,
        SB = 33,
        ML = 6,
        DV = 34,
        MD = 35,
        EQ = 7,
        NE = 8,
        LT = 9,
        GT = 10,
        LE = 11,
        GE = 12,
        WNL = 13,
        WRI = 14,
        J = 15,
        FJ = 16,
        TJ = 17,
        CALL = 24,
        EF = 25,
        HLT = 18,
        EXC = 19,
        AND = 20,
        OR = 21,
        XOR = 22,
        OTHER = 23,
    }

    public enum TC {
        LPAR = 0,
        RPAR = 1,
        LBRACE = 2,
        RBRACE = 3,
        INT = 4,
        PUTINT = 5,
        BOOL = 30,
        IF = 6,
        ELSE = 36,
        DO = 7,
        WHILE = 8,
        FUN = 27,
        SEMI = 9,
        DOT = 10,
        COMMA = 11,
        PLUS = 12,
        MINUS = 34,
        MULT = 13,
        DIV = 35,
        MOD = 37,
        EQ = 14,
        NE = 15,
        LT = 16,
        GT = 17,
        LE = 18,
        GE = 19,
        EXC = 20,
        ASSIGN = 21,
        NUM = 22,
        IDENT = 23,
        STR = 24,
        EOF = 25,
        RETURN = 28,
        EVENT = 29,
        OTHERS = 26,

        AND = 253,
        OR = 254,
        XOR = 255,
    }

    public enum Type {
        VAR = 0, FUNC = 1, OTHERS = 2, ARG = 3, EVENT = 4,
    }

    public class Tokenizer {
        public Tokenizer() {
            this.Reader = new CharReader();
            this.Tokens = new List<TC>(); // TCのリスト
            this.Leximes = new List<string>();
            this.Sb = "";
            this.SetupKeyword();
        }
        public Tokenizer(CharReader chr) {
            this.Reader = chr;
            this.Tokens = new List<TC>(); // TCのリスト
            this.Leximes = new List<string>();
            this.Sb = "";
            this.SetupKeyword();
        }
        public CharReader Reader {
            get; set;
        }
        public List<TC> Tokens {
            get; set;
        }
        public List<string> Leximes {
            get; set;
        }
        public string Sb {
            get; set;
        }
        public Dictionary<string, TC> KeyTable {
            get; set;
        }
        public void SetupKeyword() {
            this.KeyTable = new Dictionary<string, TC>();
            string[] keys = { "int", "putint", "if", "else", "do", "while", "return", "fun", "event", "true", "false" };
            TC[] tclasses = { TC.INT, TC.PUTINT, TC.IF, TC.ELSE, TC.DO, TC.WHILE, TC.RETURN, TC.FUN, TC.EVENT, TC.BOOL, TC.BOOL };
            for (int i = 0; i < keys.Length; i++) {
                this.KeyTable.Add(keys[i], tclasses[i]);
            }
        }
        public TC GetKeyword(string s) {
            TC tc;
            try {
                tc = KeyTable[s];
            } catch (KeyNotFoundException e) {
                tc = TC.IDENT;
            }
            return tc;
        }
        public TC ReadSymbol(char ch) {
            char? next = null;
            TC ret = TC.OTHERS;

            if (ch == '(') {
                ret = TC.LPAR;
            } else if (ch == ')') {
                ret = TC.RPAR;
            } else if (ch == '{') {
                ret = TC.LBRACE;
            } else if (ch == '}') {
                ret = TC.RBRACE;
            } else if (ch == '+') {
                ret = TC.PLUS;
            } else if (ch == '-') {
                ret = TC.MINUS;
            } else if (ch == '*') {
                ret = TC.MULT;
            } else if (ch == '/') {
                ret = TC.DIV;
            } else if (ch == '%') {
                ret = TC.MOD;
            } else if (ch == ';') {
                ret = TC.SEMI;
            } else if (ch == '.') {
                ret = TC.DOT;
            } else if (ch == ',') {
                ret = TC.COMMA;
            } else if (ch == '=') {
                next = this.Reader.NextChar();
                if (next != '=') {
                    ret = TC.ASSIGN;
                    this.Reader.BackChar();
                } else {
                    this.Sb += next;
                    ret = TC.EQ;
                }
            } else if (ch == '!') {
                next = this.Reader.NextChar();
                if (next != '=') {
                    ret = TC.EXC;
                    this.Reader.BackChar();
                } else {
                    this.Sb += next;
                    ret = TC.NE;
                }
            } else if (ch == '<') {
                next = this.Reader.NextChar();
                if (next != '=') {
                    ret = TC.LT;
                    this.Reader.BackChar();
                } else {
                    this.Sb += next;
                    ret = TC.LE;
                }
            } else if (ch == '>') {
                next = this.Reader.NextChar();
                if (next != '=') {
                    ret = TC.GT;
                    this.Reader.BackChar();
                } else {
                    this.Sb += next;
                    ret = TC.GE;
                }
            } else if (ch == '&') {
                next = this.Reader.NextChar();
                if (next != '&') {
                    ret = TC.AND;
                    this.Reader.BackChar();
                }
            } else if (ch == '|') {
                next = this.Reader.NextChar();
                if (next != '|') {
                    ret = TC.OR;
                    this.Reader.BackChar();
                }
            } else if (ch == '^') {
                next = this.Reader.NextChar();
                if (next != '^') {
                    ret = TC.XOR;
                    this.Reader.BackChar();
                }
            }
            return ret;
        }
        public TC NextToken() {
            char? ch = null;
            int nextState = 1;
            this.Sb = "";
            TC token = TC.OTHERS;
            this.SkipWhitespace();
            while (true) {
                ch = this.Reader.NextChar();
                if (nextState == 1) {
                    if (char.IsLetter((char)ch)) {
                        this.Sb += ch;
                        nextState = 2;
                        continue;
                    }
                    if (char.IsDigit((char)ch)) {
                        this.Sb += ch;
                        nextState = 3;
                        continue;
                    }
                    if (ch == '/') {
                        // this.Sb += ch;
                        if (this.Reader.NextChar() == '/') {
                            // コメントの処理
                            char nch = '_';
                            while (nch != '\n' && nch != '\0') {
                                nch = this.Reader.NextChar();
                            }
                            this.SkipWhitespace();
                            continue;
                        }
                        this.Reader.BackChar();
                        if (this.Reader.NextChar() == '*') {
                            // コメントの処理
                            char nch = '_';
                            while (nch != '*') {
                                nch = this.Reader.NextChar();
                                if (nch == '*') {
                                    nch = this.Reader.NextChar();
                                    if (nch == '/') {
                                        break;
                                    }
                                }
                            }
                            this.SkipWhitespace();
                            continue;
                        }
                        this.Reader.BackChar();
                    }
                    if (ch == '"') {
                        // 文字列の処理
                        char nch = '_';
                        while (nch != '"') {
                            nch = this.Reader.NextChar();
                            if (nch == '"') {
                                break;
                            } else if (nch == '\0') {
                                break;
                            }
                            this.Sb += nch;
                        }
                        token = TC.STR;
                        break;
                    }
                    if (ch == '\0') {
                        token = TC.EOF;
                        break;
                    }
                    this.Sb += ch;
                    token = this.ReadSymbol((char)ch);
                    if (token == TC.OTHERS) {
                        this.Error(nextState, ch);
                    }
                    break;
                } else if (nextState == 2) {
                    if (char.IsLetterOrDigit((char)ch) || ch == '.') {
                        this.Sb += ch;
                        nextState = 2;
                        continue;
                    } else {
                        token = this.GetKeyword(this.CurrentString());
                        this.Reader.BackChar();
                        break;
                    }
                } else if (nextState == 3) {
                    if (char.IsDigit((char)ch)) {
                        this.Sb += ch;
                        nextState = 3;
                        continue;
                    } else {
                        token = TC.NUM;
                        this.Reader.BackChar();
                        break;
                    }
                }
            }
            this.Tokens.Add(token);
            this.Leximes.Add(this.CurrentString());
            return token;
        }
        public void SkipWhitespace() {
            char ch = this.Reader.NextChar();
            while (char.IsWhiteSpace(ch)) {
                ch = this.Reader.NextChar();
            }
            this.Reader.BackChar();
        }
        public void Error(int state, char? ch) {
            string s = $"in State: {state}\n" +
                        $"\n不正な文字=[" + ch + "]" +
                        "\nTokens = " + this.Tokens +
                        "\nLeximes = " + this.Leximes;
            Console.WriteLine(s);
            throw new Exception(s);
        }

        public string CurrentString() {
            return this.Sb;
        }
    }

    public class TestTokenizer {
        // testCaseフィールドに入力となるトークン列を登録しておく。
        // nextToken()が呼び出されるごとにcurrentIndexがすすむ
        public TestTokenizer(List<TC> tokens, List<string> leximes) {
            this.Tokens = tokens;  // 入力となるトークン列
            this.Leximes = leximes;  // レクシム
            this.CurrentIndex = -1;  // 次に読み込むトークンの位置
        }
        public List<TC> Tokens {
            get; set;
        }
        public List<string> Leximes {
            get; set;
        }
        public int CurrentIndex {
            get; set;
        }
        public TC NextToken() {
            this.CurrentIndex += 1;
            TC nextToken = this.Tokens[this.CurrentIndex];  // 現在のトークンを返し
            return nextToken;  // 配列の範囲のチェックはしていない
        }
        public TC GetNextToken() {
            TC nextToken = this.Tokens[this.CurrentIndex + 1];  // 現在のトークンを返し
            return nextToken;  // 配列の範囲のチェックはしていない
        }
        public string CurrentString() {
            return this.Leximes[this.CurrentIndex];
        }
        public TC CurrentToken() {
            return this.Tokens[this.CurrentIndex];
        }
    }

    public static class Lang {
        internal static TC? next = null;
        internal static TestTokenizer? s = null;
        internal static string result = "";
        internal static string code = "";
        internal static string eventName = "";
        internal static string statements = "";
        internal static string scope = "";
        internal static NameTable table; // 記号表
        internal static Dictionary<string, List<string>> eventTable = new(); // イベントリスト
        internal static List<EventList> eventList = new();
        internal static List<Inst> codeTable = new();
        internal static Queue<Tuple<Mnemonic, int, int>> _tmp;
        // static Dictionary<> local_vars;
        internal static List<Queue<Tuple<Mnemonic, int, int>>> _vars;

        public static void S() { // S -> DeclList StmList
            table = new NameTable();
            _tmp = new();
            _vars = new(); // 関数呼び出しのときのスタック
            // 同じ名前の変数を定義するとエラー -> シンボリテーブルを作る
            // local_vars = {};
            DeclList();
            int numGVs = table.NextAddress; // グローバル変数の数
            // AddCode(Mnemonic.PUSH, 0, numGVs);
            // AddCode(Mnemonic.CALL, 0, 0);
            int callSite = CurrentCodeAddress();
            // AddCode(Mnemonic.POP, 0, numGVs + 1);
            // AddCode(Mnemonic.HLT, 0, 0);
            ProcList();
            // codeTable[callSite].Arg2 = table.Get("main", "main").Address;
        }
        public static void ProcList() {
            ProcHead();
            Body();
            while (true) {
                if (next != TC.FUN && next != TC.EVENT) {
                    break;
                }
                ProcHead();
                Body();
            }
        }
        public static void ProcHead() {
            switch (next) {
                case TC.FUN:
                    Check(TC.FUN);
                    Proceed(TC.INT);
                    Proceed(TC.IDENT);
                    string funcName = s.CurrentString();
                    table.AddFunc(funcName, CurrentCodeAddress() + 1);
                    break;
                case TC.EVENT:
                    /*Check(TC.EVENT);
                    Proceed(TC.IDENT);
                    string control = s.CurrentString();
                    Proceed(TC.DOT);
                    Proceed(TC.IDENT);
                    string eventName = s.CurrentString();
                    table.AddEvent(control, eventName, CurrentCodeAddress() + 1);*/

                    Check(TC.EVENT);
                    Proceed(TC.IDENT);
                    eventName = s.CurrentString();
                    eventTable[eventName] = new List<string> {
                        /* イベントが呼び出されたときに実行される文 */

                    };
                    table.AddEvent(eventName.Split(".")[0], eventName.Split(".")[1], CurrentCodeAddress() + 1);
                    break;
                default:
                    break;
            }
            table.UpLevel();
            Proceed(TC.LPAR);
            ProceedOnly();
            int varAddress = -1;
            while (next != TC.RPAR) {
                if (next == TC.INT) {
                    ProceedOnly();
                    Check(TC.IDENT);
                    table.AddArg(s.CurrentString(), varAddress);
                    varAddress--;
                    ProceedOnly();
                    if (next == TC.COMMA) {
                        ProceedOnly();
                    }
                } else if (next == TC.EOF) {
                    UnexpectedTokenError();
                }
            }
            // table.NextAddress--;
            Check(TC.RPAR);
            ProceedOnly();
        }
        /// <summary>
        /// ProcHead()メソッドの一部の呼び出しを展開したもの
        /// Check(FUN)は現在先読みしているトークン(nextに代入されている)がFUNであることを確認するメソッドである。展開すると下記の1文である
        /// </summary>
        public static void ProcHeadExtract() { // -> FUN INT IDENT LPAR RPAR
            if (next != TC.FUN) {
                UnexpectedTokenError(TC.FUN);
            }
            /* proceed(INT)は、next = s.nextToken()で新しいトークンを読み込んだあと
               それがINTであるかをチェックする。展開すると下記の2文である。 */
            next = s.NextToken();
            if (next != TC.INT) {
                UnexpectedTokenError(TC.INT);
            }
            /* 以下の2文はproceed(IDENT)を展開したもの */
            next = s.NextToken();
            if (next != TC.IDENT) {
                UnexpectedTokenError(TC.IDENT);
            }
            string funcName = s.CurrentString();
            table.AddFunc(funcName, CurrentCodeAddress() + 1);
            table.UpLevel();
            Proceed(TC.LPAR);
            Proceed(TC.RPAR);
        }
        public static void Body() {
            // -> LBRACE RBRACE
            // -> LBRACE STMLIST RBRACE
            // -> LBRACE DECLLIST STMLIST RBRACE
            _tmp.Clear();

            statements = "";
            Check(TC.LBRACE);
            statements += s.CurrentString(); // {}を含むならコメントアウト解除する
            ProceedOnly();
            DeclList();
            int allocSize = table.GetAllocationSize();
            // AddCode(Mnemonic.PUSH, 0, allocSize);
            while (_tmp.Count != 0) {
                var p = _tmp.Dequeue();
                // AddCode(p.Item1, p.Item2, p.Item3);
            }
            int si = s.CurrentIndex;
            StmList();
            statements = statements.Substring(1, statements.Length - 2); // ブロックの{}を取り除く
            eventList.Add(new EventList(eventName, statements, si, s.CurrentIndex - 1));
            table.DebugDownLevel(false);
            Check(TC.RBRACE);
            ProceedOnly();
        }
        public static void Decl() { // DECL -> INT IDENT SEMI
            if (next != TC.INT) {
                return;
            }
            Proceed(TC.IDENT);
            table.AddVar(s.CurrentString(), table.Level);
            // AddVarの第2引数は、現在処理しているレベル(0 or 1)を表す
            TC s_next = s.GetNextToken();
            if (s_next == TC.ASSIGN) {
                StmAssign(immidiate: true);
            } else if (s_next == TC.SEMI) {
                Proceed(TC.SEMI);
                ProceedOnly();
            }
        }
        public static void DeclList() { // DECLLIST -> {DECL}
            while (next == TC.INT) {
                Decl();
            }
        }
        public static void StmList() { // STMLIST -> {STM}
            while (next != TC.RBRACE) {
                Stm();
            }
        }
        public static void Stm() {
            // statements = s.CurrentString();
            if (next == TC.IDENT) {  // 変数名
                TC s_next = s.GetNextToken();
                if (s_next == TC.DOT) {
                    s_next = s.GetNextToken();
                }
                if (s_next == TC.ASSIGN || s_next == TC.SEMI) {  // 代入
                    StmAssign();
                    eventTable[eventName] = new List<string> { statements };
                } else {  // 関数
                    E();
                    while (s.CurrentToken() == TC.NUM) {
                        E();
                        if (next == TC.RPAR) {
                            next = s.NextToken();
                            break;
                        }
                    }
                    if (s.CurrentToken() != TC.SEMI) {
                        UnexpectedTokenError();
                    }
                    next = s.NextToken();
                }
            } else if (next == TC.PUTINT) {
                StmPutInt();
            } else if (next == TC.IF) {
                StmIf();
            } else if (next == TC.DO) {
                StmDo();
            } else if (next == TC.WHILE) {
                StmWhile();
            } else if (next == TC.LBRACE) {
                StmBlock();
            } else if (next == TC.RETURN) {
                StmReturn();
            } else {
                UnexpectedTokenError();
            }
        }
        public static void StmAssign(bool immidiate = false) {
            string var = s.CurrentString();
            Proceed(TC.ASSIGN);
            ProceedOnly();
            E(immidiate);
            Name entry = table.Get(var, scope);
            if (var == null) {
                UndeclaredVariableError(var);
            }
            if (immidiate) { // 
                _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.STV, table.Level - entry.Level, entry.Address));
            } else {
                // AddCode(Mnemonic.STV, table.Level - entry.Level, entry.Address);
            }
            Check(TC.SEMI);
            statements += s.CurrentString();
            ProceedOnly();
            // statements = statements.Substring(0, statements.Length - 1); // {}を含むならコメントアウトする
        }
        public static void StmPutInt() {
            if (s.NextToken() != TC.LPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            E(); // Cond();
            if (next != TC.RPAR) {
                UnexpectedTokenError();
            }
            // AddCode(Mnemonic.WRI, 0, 0);
            if (s.NextToken() != TC.SEMI) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
        }
        public static void StmIf() {
            next = s.NextToken();
            if (next != TC.LPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            Cond();
            if (next == TC.RPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            // AddCode(Mnemonic.FJ, 0, 0);
            int fj = CurrentCodeAddress();
            Stm();
            int here = CurrentCodeAddress() + 1;
            codeTable[fj].Arg2 = here;
            // else文
            if (next == TC.ELSE) {
                // next = s.NextToken();
                next = s.GetNextToken();
                if (next == TC.IF) {
                    // else if
                    next = s.NextToken();
                    StmIf();
                } else {
                    // AddCode(Mnemonic.J, 0, 0);
                    int j = CurrentCodeAddress();
                    next = s.NextToken();
                    Stm();
                    here = CurrentCodeAddress() + 1;
                    codeTable[fj].Arg2++;
                    codeTable[j].Arg2 = here;
                }
            }
        }
        public static void StmDo() {
            next = s.NextToken();
            int here = CurrentCodeAddress() + 1;
            Stm();
            if (next != TC.WHILE) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            if (next != TC.LPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            Cond();
            if (next != TC.RPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            if (next != TC.SEMI) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            // AddCode(Mnemonic.TJ, 0, here);
        }
        public static void StmWhile() {
            next = s.NextToken();
            int here = CurrentCodeAddress() + 1;
            if (next != TC.LPAR) {
                UnexpectedTokenError();
            }
            next = s.NextToken();
            Cond();
            if (next != TC.RPAR) {
                UnexpectedTokenError();
            }
            // AddCode(Mnemonic.FJ, 0, 0);
            int fj = CurrentCodeAddress();
            next = s.NextToken();
            Stm();
            // next = s.NextToken();
            // AddCode(Mnemonic.J, 0, here);
            int j = CurrentCodeAddress();
            codeTable[fj].Arg2 = j + 1;
        }
        public static void StmBlock() {
            next = s.NextToken();
            while (next != TC.RBRACE) {
                Stm();
            }
            next = s.NextToken();
        }
        public static void StmReturn() {
            ProceedOnly();
            E();
            Check(TC.SEMI);
            int numArgs = table.GetNumArgs();
            int allocSize = table.GetAllocationSize();
            // AddCode(Mnemonic.EF, 0, numArgs);
            // AddCode(Mnemonic.POP, 0, allocSize);
            ProceedOnly();
        }
        public static void StmTk(TC cmd) {
        }
        public static void StmColor(TC cmd) {
        }
        public static void StmFill(TC cmd) {
        }
        public static void E(bool immidiate = false) { // 戻り値を決めると値を取り出せる
            // E -> T {'+' T}
            T(immidiate);
            while (true) {
                if (next == TC.PLUS) {
                    next = s.NextToken();
                    T(immidiate);
                    if (immidiate) {
                        _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.AD, 0, 0));
                    } else {
                        // AddCode(Mnemonic.AD, 0, 0);
                    }
                } else if (next == TC.MINUS) {
                    next = s.NextToken();
                    T(immidiate);
                    if (immidiate) {
                        _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.SB, 0, 0));
                    } else {
                        // AddCode(Mnemonic.SB, 0, 0);
                    }
                } else {
                    break;
                }
            }
        }
        public static void T(bool immidiate = false) {
            // T -> F { '*' F}
            F(immidiate);
            while (true) {
                if (next == TC.MULT) {
                    next = s.NextToken();
                    F(immidiate);
                    if (immidiate) {
                        _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.ML, 0, 0));
                    } else {
                        // AddCode(Mnemonic.ML, 0, 0);
                    }
                } else if (next == TC.DIV) {
                    next = s.NextToken();
                    F(immidiate);
                    if (immidiate) {
                        _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.DV, 0, 0));
                    } else {
                        // AddCode(Mnemonic.DV, 0, 0);
                    }
                } else if (next == TC.MOD) {
                    next = s.NextToken();
                    F(immidiate);
                    if (immidiate) {
                        _tmp.Enqueue(new Tuple<Mnemonic, int, int>(Mnemonic.MD, 0, 0));
                    } else {
                        // AddCode(Mnemonic.MD, 0, 0);
                    }
                } else {
                    break;
                }
            }
        }
        public static void F(bool immidiate = false) {
            // F -> ( E ) | NUM | IDENT
            string sign = "";
            if (next == TC.PLUS || next == TC.MINUS) {
                sign = s.CurrentString();
                ProceedOnly();
                // s.CurrentString() = sign + s.CurrentString();
            }
            if (next == TC.LPAR) {
                next = s.NextToken();
                E();
                if (next == TC.RPAR) {
                    next = s.NextToken();
                } else {
                    UnexpectedTokenError();
                }
            } else if (next == TC.NUM) {
                if (immidiate) {
                    _tmp.Enqueue(new(Mnemonic.LDC, 0, int.Parse(sign + s.CurrentString())));
                } else {
                    // AddCode(Mnemonic.LDC, 0, int.Parse(sign + s.CurrentString()));
                }
                next = s.NextToken();
            } else if (next == TC.STR) {
                // 文字列代入
                next = s.NextToken();
            } else if (next == TC.IDENT) {
                FVarRefOrFunCall(immidiate);
                // if (s.GetNextToken() == TC.LPAR) {
                //     FVarRefOrFunCall(true); // IDENTのあとにLPARが来た場合は関数呼び出しの処理
                // } else {
                //     FVarRefOrFunCall(false); // IDENTのあとにLPARが来ない場合は変数参照の処理
                // }
            } else if (next == TC.BOOL) {
                // 論理値代入
                next = s.NextToken();
            } else {
                UnexpectedTokenError();
            }
        }
        public static void FVarRefOrFunCall(bool immidiate = false) {
            string name = s.CurrentString();
            ProceedOnly();
            if (next != TC.LPAR) { // IDENTのあとにLPARが来ない場合は変数参照の処理
                Name entry = table.Get(name, scope);
                if (entry == null) {
                    UndeclaredVariableError(name);
                }
                if (immidiate) {
                    _tmp.Enqueue(new(Mnemonic.LDV, table.Level - entry.Level, entry.Address));
                } else {
                    // AddCode(Mnemonic.LDV, table.Level - entry.Level, entry.Address);
                }
            } else { // IDENTのあとにLPARが来た場合は関数呼び出しの処理
                ProceedOnly();
                while (next != TC.RPAR) {
                    E(true);
                    _vars.Add(_tmp);
                    _tmp.Clear();
                    if (next == TC.COMMA) {
                        ProceedOnly();
                    }
                }
                while (_vars.Count != 0) {
                    var q = _vars.Last();
                    _vars.RemoveAt(_vars.Count - 1);
                    while (q.Count != 0) {
                        var p = q.Dequeue();
                        // AddCode(p.Item1, p.Item2, p.Item3);
                    }
                }
                // AddCode(Mnemonic.CALL, 1, table.Get(name, name).Address);
                ProceedOnly();
            }
        }
        public static void Cond() {
            E();
            Mnemonic op = Mnemonic.OTHER;
            TC[] tcList = { TC.EQ, TC.NE, TC.LT, TC.GT, TC.LE, TC.GE };
            Mnemonic[] opList = { Mnemonic.EQ, Mnemonic.NE, Mnemonic.LT, Mnemonic.GT, Mnemonic.LE, Mnemonic.GE };
        }
        public static void AddCode(Mnemonic op, int arg1, int arg2) {
            codeTable.Add(new Inst(op, arg1, arg2));
        }
        public static int CurrentCodeAddress() {
            return codeTable.Count - 1;
        }
        public static void UnexpectedTokenError() {
            Console.WriteLine(result);
            throw new Exception($"Unexpexted Token: {s.CurrentString()}, Kind: {Enum.GetName(typeof(TC), next)}");
        }
        public static void UnexpectedTokenError(TC expected) {
            Console.WriteLine(result);
            string e = expected != null ? ("Expected" + Enum.GetName(typeof(TC), expected)) : "";
            throw new Exception($"Unexpexted Token: {s.CurrentString()}, Kind: {Enum.GetName(typeof(TC), next)} {e}");
        }
        public static void Proceed(TC expected) {
            ProceedOnly();
            Check(expected);
        }
        public static void ProceedOnly() {
            next = s.NextToken();
            statements += s.CurrentString();
        }
        public static void Check(TC expected) {
            if (next != expected) {
                UnexpectedTokenError();
            }
        }

        static string[] text;
        // テストプログラム
        private static void Test(int i) {
            Console.WriteLine($"text:{i}");
            Console.WriteLine(text[i]);
            Compile(text[i]);
        }
        public static void Compile(string s) {
            Console.Clear();
            statements = "";
            eventList.Clear();
            Tokenizer tokenizer = new Tokenizer(new CharReader(s));
            Scan(tokenizer);
            // 字句解析の結果を表示
            // Print(tokenizer.tokens)
            // Print(tokenizer.leximes)
            // 構文解析と同時にコード生成
            Parse(tokenizer.Tokens, tokenizer.Leximes);
        }

        public static void Scan(Tokenizer tokenizer) {
            TC token = tokenizer.NextToken();
            while (token != TC.EOF) {
                token = tokenizer.NextToken();
            }
        }

        public static void Parse(List<TC> tokens, List<string> leximes) {
            s = new TestTokenizer(tokens, leximes);
            next = s.NextToken();
            codeTable.Clear();
            S();
            table.Print();
            ShowCode(codeTable, showLine);
        }
        public static int Search(string name) {
            Name n = table.Get(name, "");
            if (n != null) {
                UndeclaredVariableError(name);
            }
            return n.Address;
        }
        public static void ShowCode(List<Inst> codeList, bool showLine) {
            int pc = 0;
            foreach (Inst inst in codeList) {
                if (showLine) {
                    Console.WriteLine($"{pc}: {inst}");
                    pc++;
                } else {
                    code += ($"{inst}\n");
                    Console.Write($"{inst}\n");
                }
            }
        }
        public static void UndeclaredVariableError(string undeclared) {
            throw new Exception($"Undeclared {undeclared}");
        }
        const int NUMSET = 9;
        static bool showLine = false;
        public static void SetupSource() {
            text = new string[NUMSET];
            text[0] = "fun int proc(){int a = 4;return a;} fun int main(){putint(proc());return 0;}";
            text[0] = "event Button1.Click(){Button1.Text = \"Hello\";\nButton1.Enabled = false;}";
        }
        public static void Main_Lang() {
            SetupSource();
            Test(0);
            Console.ReadLine();
        }
    }

    // 記号表
    public class Name {
        public Name(string id, Type t, int addr, int level, string scope = "") {
            this.Ident = id;
            this.Address = addr;
            this.Type = t;
            this.Level = level;
            this.Scope = scope;
        }
        public string Ident {
            get; set;
        }
        public int Address {
            get; set;
        }
        public Type Type {
            get; set;
        }
        public int Level {
            get; set;
        }
        public string Scope {
            get; set;
        }
        public override string ToString() {
            return $"id:{this.Ident}, type:{this.Type}, address:{this.Address}, level:{this.Level}";
        }
    }

    class NameTable {
        public NameTable() {
            this.NameTable_ = new List<Name>();
            this.NextAddress = 0;
            this.Index = 0;
            this.Level = 0;
            this.PtrL1 = 0;
            this.NextAddressL0 = 0;
        }
        public List<Name> NameTable_ {
            get; set;
        }
        public int NextAddress {
            get; set;
        }
        public int Index {
            get; set;
        }
        public int Level {
            get; set;
        }
        public int PtrL1 {
            get; set;
        }
        public int NextAddressL0 {
            get; set;
        }
        public override string ToString() {
            string result = "";
            for (int i = 0; i < Index; i++) {
                result = result + this.NameTable_[i].ToString() + "\n";
            }
            return result.Substring(0, result.Length - 1);
        }
        public Name Get(string ident, string scope) {
            /* 表からidentを探し、見つかったらNameインスタンスを返す。見つからない場合はNoneを返す。 */
            foreach (Name entry in this.NameTable_) {
                if (entry.Ident == ident && entry.Scope == scope) {
                    return entry;
                }
            }
            Control control = GUIBuilderProtoCSharp.Form1.f3.Controls.Find(ident.Split(".")[0], true)[0];
            if (control != null && ident != scope) {
                Name entry = new Name(control.Name, Type.VAR, 0, 0, scope);
                return entry;
            }
            return null;
        }
        public void AddFunc(string ident, int codeAddress) {
            // global scope;
            Lang.scope = ident;
            this.CheckName(ident, Lang.scope);
            this.NameTable_.Add(new Name(ident, Type.FUNC, codeAddress, 0, ident));
            this.Index++;
        }
        public void AddEvent(string ident, string ev, int codeAddress) {
            Lang.scope = ident;
            this.CheckName(ident, Lang.scope);
            this.NameTable_.Add(new Name(ident, Type.EVENT, codeAddress, 0, ev));
            this.Index++;
        }
        public int AddVar(string ident, int level) {
            int size = 1;
            int ret = this.NextAddress;
            bool func_flag = false;
            string scope = "";
            for (int i = this.NameTable_.Count - 1; i >= 0; i--) {
                if (this.NameTable_[i].Type == Type.FUNC && !func_flag) {
                    func_flag = true;
                    scope = this.NameTable_[i].Ident;
                    break;
                }
            }
            this.CheckName(ident, scope);
            this.NameTable_.Add(new Name(ident, Type.VAR, this.NextAddress, level, scope));
            this.Index++;
            this.NextAddress += size;
            return ret;
        }
        public void AddArg(string ident, int address) {
            // size = 1;
            bool func_flag = false;
            string scope = "";
            for (int i = this.NameTable_.Count - 1; i >= 0; i--) {
                if (this.NameTable_[i].Type == Type.FUNC && !func_flag) {
                    func_flag = true;
                    scope = this.NameTable_[i].Ident;
                    break;
                }
            }
            this.CheckName(ident, scope);
            this.NameTable_.Add(new Name(ident, Type.ARG, address, this.Level, scope));
            this.Index++;
            // this.NextAddress += size;
        }
        public void CheckName(string ident, string scope) {
            if (this.Get(ident, scope) != null) {
                throw new Exception("その名前はすでに登録されています。addName : " + ident);  // 適切に書き直す必要あり
            }
        }
        public void UpLevel() {
            this.Level++;
            this.PtrL1 = this.Index;
            this.NextAddressL0 = this.NextAddress;
            this.NextAddress = 3; // 戻り値(RV), DL, SLの格納場所を飛ばす
        }
        public void DownLevel() {
            this.Level--;
            this.Index = this.PtrL1;
            this.NextAddress = this.NextAddressL0;
        }
        public void DebugDownLevel(bool debug) {
            if (debug) {
                Console.WriteLine(this.ToStringLevel1());
            }
            this.DownLevel();
        }
        public int GetNumArgs() {
            int result = 0;
            int start = (this.Level == 0 ? 0 : this.PtrL1);
            for (int i = start; i < this.Index; i++) {
                if (this.NameTable_[i].Type != Type.ARG) {
                    break;
                }
                result++;
            }
            return result;
        }
        public int GetAllocationSize() {
            return this.NextAddress;
        }
        public string ToStringLevel1() {
            if (this.Level != 1) {
                Console.WriteLine("illegal call of NameTable>toStringLevel1()");
            }
            string buf = this.NameTable_[this.PtrL1 - 1].Ident + "-> { ";
            for (int i = this.PtrL1; i < this.Index; i++) {
                buf += this.NameTable_[i].ToString() + " ";
            }
            buf += "}";
            return buf;
        }
        //public int AddName() {
        //    /* テーブルに名前（エントリ）を登録する。エラーがあったら例外を投げる。領域のどこに登録したかを返す。
        //    現在はintのみをallocateするので1. 配列等の場合適切なsizeを指定する必要がある。 */
        //    int size = 1;
        //    if (this.Get(ident)!=null) {
        //        throw new Exception("その名前は既に登録されています。addName");
        //    }
        //    int ret = this.NextAddress;
        //    this.NameTable_.Add(new Name(ApplicationIdentity, this.NextAddress));
        //    this.NextAddress = this.NextAddress + size;
        //    return ret;
        //}
        public void Print() {
            foreach (Name x in this.NameTable_) {
                Console.WriteLine(x.ToString());
            }
        }
    }
    public class Inst {
        public Inst(Mnemonic op, int arg1, int arg2) {
            this.op = op;
            this.Arg1 = arg1;
            this.Arg2 = arg2;
        }
        public Mnemonic op {
            get; set;
        }
        public int Arg1 {
            get; set;
        }
        public int Arg2 {
            get; set;
        }
        public override string ToString() {
            return $"{Enum.GetName(typeof(Mnemonic), op)} {this.Arg1} {this.Arg2}";
        }
    }

    public class EventList {
        public EventList() {

        }
        public EventList(string name, string originalCode, int tokenStart, int tokenEnd) {
            this.Name = name;
            this.OriginalCode = originalCode;
            this.TokenStart = tokenStart;
            this.TokenEnd = tokenEnd;
            this.Statements = this.OriginalCode.Split(";").ToList();
            CurrentTokenIndex = TokenStart;
        }
        public string Name {
            get; set;
        }
        public string OriginalCode {
            get; set;
        }
        public int TokenStart {
            get; set;
        }
        public int TokenEnd {
            get; set;
        }
        public List<string> Statements {
            get; set;
        }
        public static int CurrentTokenIndex {
            get; set;
        }

        public static void Do(string name) {
            EventList? el = null;
            foreach (var item in Lang.eventList) {
                if (item.Name == name) {
                    el = item;
                    CurrentTokenIndex = el.TokenStart;
                    break;
                }
            }
            if (el == null) {
                return;
            }
            foreach (var item in el.Statements) {
                if (Lang.s.Tokens[CurrentTokenIndex] == TC.IDENT && Lang.s.Tokens[CurrentTokenIndex + 1] == TC.ASSIGN) {
                    SetProperty(Lang.s.Leximes[CurrentTokenIndex].Split(".")[0], Lang.s.Leximes[CurrentTokenIndex].Split(".")[1]);
                }

                Console.WriteLine(item);
            }
        }
        public static void SetProperty(string controlName, string propName) {
            Control control = GUIBuilderProtoCSharp.Form1.f3.Controls.Find(controlName, true)[0];
            CurrentTokenIndex += 2;
            if (propName == "Text") {
                string assignText = "";
                while (Lang.s.Tokens[CurrentTokenIndex] != TC.SEMI) {
                    if (Lang.s.Tokens[CurrentTokenIndex] == TC.PLUS) {
                        CurrentTokenIndex++;
                    }
                    assignText += Lang.s.Leximes[CurrentTokenIndex];
                    CurrentTokenIndex++;
                }
                control.Text = assignText;
            } else if (propName == "Enabled") {
                control.Enabled = bool.Parse(Lang.s.Leximes[CurrentTokenIndex]);
                CurrentTokenIndex++;
            } else if (propName == "Width" || propName == "Height") {
                var property = typeof(Control).GetProperty(propName);
                property.SetValue(control, int.Parse(Lang.s.Leximes[CurrentTokenIndex]));
                CurrentTokenIndex++;
            }
            CurrentTokenIndex++;
        }
    }
}
