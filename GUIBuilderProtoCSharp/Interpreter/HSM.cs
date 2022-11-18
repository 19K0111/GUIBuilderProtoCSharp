using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter {
    public enum OP {
        LDC = 0,
        STV = 1,
        LDV = 2,

        PUSH = 16,
        POP = 17,

        AD = 32,  // +
        SB = 33,  // -
        ML = 34,  // *
        DV = 35,  // /
        MD = 36,  // %
        EQ = 37,  // ==
        NE = 38,  // !=
        LT = 39,  // <
        GT = 40,  // >
        LE = 41,  // <=
        GE = 42,  // >=

        WNL = 64,
        WRI = 65,

        J = 128,
        FJ = 129,
        TJ = 130,
        CALL = 192,
        EF = 194,

        HLT = 255,
    }
    public class HSM {
        public HSM() {
            this.InstructionCount = 0;
            this.InstructionCountMax = 65535;
            this.ShowState = false;
        }
        public int InstructionCount {
            get; set;
        }
        public int InstructionCountMax {
            get; private set;
        }
        public bool ShowState {
            get; set;
        }

        public void Execute(List<int> code) {
            int pc = 0;
            int[] s = new int[256];
            int sp = -1;
            int b = 0;
            while (this.InstructionCount < this.InstructionCountMax) {
                this.InstructionCount += 1;
                if (this.ShowState) {
                    this.PrintState(code, pc, s, sp, b);
                }
                if (code[pc] == (int)OP.LDC) {
                    sp += 1;
                    s[sp] = code[pc + 2];
                } else if (code[pc] == (int)OP.LDV) {
                    sp += 1;
                    s[sp] = s[this.BaseOffset(s, b, code[pc + 1], code[pc + 2])];
                } else if (code[pc] == (int)OP.STV) {
                    s[this.BaseOffset(s, b, code[pc + 1], code[pc + 2])] = s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.PUSH) {
                    sp = sp + code[pc + 2];
                } else if (code[pc] == (int)OP.POP) {
                    sp = sp - code[pc + 2];
                } else if (code[pc] == (int)OP.AD) {
                    s[sp - 1] = s[sp - 1] + s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.SB) {
                    s[sp - 1] = s[sp - 1] - s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.ML) {
                    s[sp - 1] = s[sp - 1] * s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.DV) {
                    s[sp - 1] = s[sp - 1] / s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.MD) {
                    s[sp - 1] = s[sp - 1] % s[sp];
                    sp -= 1;
                } else if (code[pc] == (int)OP.WRI) {
                    Console.WriteLine(s[sp]);
                    sp -= 1;
                    // } else if (code[pc] ==(int)OP.UP) {
                    //     t.up();
                    //     // sp -= 1
                    // } else if (code[pc] ==(int)OP.DOWN) {
                    //     t.down();
                    //     // sp -= 1
                    // } else if (code[pc] ==(int)OP.FWD) {
                    //     t.forward(int(s[sp]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.BACK) {
                    //     t.backward(int(s[sp]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.LEFT) {
                    //     t.left(int(s[sp]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.RIGHT) {
                    //     t.right(int(s[sp]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.PEN) {
                    //     if ((int(s[sp]) == 0)) {
                    //         t.down();
                    //     } else if (int(s[sp]) == 1){
                    //         t.up();
                    //     }else {
                    //         throw (new Exception($"illegal argument: pen({s[sp]})"));
                    //         sp -= 1;
                    //     }
                    // } else if (code[pc] ==(int)OP.PCLR) {
                    //     // print(f"pencolor({s[sp]}, {s[sp-1]}, {s[sp-2]})")
                    //     t.pencolor(s[sp], s[sp - 1], s[sp - 2]);
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.FCLR) {
                    //     t.fillcolor(s[sp], s[sp - 1], s[sp - 2]);
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.BFIL) {
                    //     t.begin_fill();
                    //     // sp -= 1
                    // } else if (code[pc] ==(int)OP.EFIL) {
                    //     t.end_fill();
                    //     // sp -= 1
                    // } else if (code[pc] ==(int)OP.CIR) {
                    //     t.circle(int(s[sp]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.POS) {
                    //     t.goto(int(s[sp]), int(s[sp - 1]));
                    //     sp -= 1;
                    // } else if (code[pc] ==(int)OP.VIS) {
                    //     if ((code[pc + 2] == 0)) {
                    //         t.hideturtle();
                    //     } else if (code[pc + 2] == 1){
                    //         t.showturtle();
                    //     }else {
                    //         throw(new Exception(
                    //             $"illegal argument:VIS 0 {code[pc+2]}");
                    //     }
                    //     // sp -= 1
                } else if (code[pc] == (int)OP.NE) {
                    sp -= 1;
                    if (s[sp] != s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                    }
                } else if (code[pc] == (int)OP.LT) {
                    sp -= 1;
                    if (s[sp] < s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                    }
                } else if (code[pc] == (int)OP.EQ) {
                    sp -= 1;
                    if (s[sp] == s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                    }
                } else if (code[pc] == (int)OP.GT) {
                    sp -= 1;
                    if (s[sp] > s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                    }
                } else if (code[pc] == (int)OP.LE) {
                    sp -= 1;
                    if (s[sp] <= s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                    }
                } else if (code[pc] == (int)OP.GE) {
                    sp -= 1;
                    if (s[sp] >= s[sp + 1]) {
                        s[sp] = 1;
                    } else {
                        s[sp] = 0;
                        // logInstructionCountal operators here
                    }
                } else if (code[pc] == (int)OP.J) {
                    pc = code[pc + 2] * 3 - 3;
                } else if (code[pc] == (int)OP.FJ) {
                    if (s[sp] == 0) {
                        pc = code[pc + 2] * 3 - 3;
                        sp -= 1;
                    }
                } else if (code[pc] == (int)OP.TJ) {
                    if (s[sp] == 1) {
                        pc = code[pc + 2] * 3 - 3;
                        sp -= 1;
                    }
                } else if (code[pc] == (int)OP.CALL) {
                    s[sp + 1] = b;
                    s[sp + 2] = (int)(pc / 3);
                    s[sp + 3] = this.BaseOffset(s, b, code[pc + 1], 0);
                    b = sp + 1;
                    pc = code[pc + 2] * 3 - 3;
                } else if (code[pc] == (int)OP.EF) {
                    int q = code[pc + 2];
                    int nextB = s[b];
                    pc = s[b + 1] * 3;
                    s[b - q] = s[sp];
                    sp = b - q;
                    b = nextB;
                } else if (code[pc] == (int)OP.HLT) {
                    break;
                } else {
                    throw (new Exception("Illegal Instruction): " + code[pc]));
                }
                pc += 3;
            }
        }
        public int BaseOffset(int[] s, int b, int level, int offset) {
            int ret = b;
            for (int i = 0; i < level; i++) {
                ret = s[b + 2];
                // if (level>0) {
                //     ret= s[b + 2];
                // }
            }
            return ret + offset;
        }
        public void PrintState(List<int> code, int pc, int[] s, int sp, int b) {
            int[] _code = new int[3];
            int[] _s = new int[sp + 1];
            Array.Copy(code.ToArray(), pc, _code, 0, 3);
            Array.Copy(s, 0, _s, 0, sp + 1);
            string __s = $"";
            for (int i = 0; i < _s.Length; i++) {
                if (i == _s.Length - 1) {
                    __s += $"{_s[i]}";
                } else {
                    __s += $"{_s[i]}, ";
                }
            }
            Console.WriteLine($"pc={(int)(pc / 3)}, code=[{_code[0]}, {_code[1]}, {_code[2]}], stack=[{__s}], b={b} (vm's pc={pc}), ");
            this.PrintFrame(s, sp, b);
        }
        public void PrintFrame(int[] s, int sp, int b) {
            Queue<int> bases = new Queue<int>() { };
            bases.Enqueue(b);
            while (true) {
                if (b == 0 && bases.ElementAt(0) == 0) {
                    break;
                }
                b = s[b];
                Queue<int> _bases = new Queue<int>();
                _bases.Enqueue(b);
                foreach (int item in bases) {
                    _bases.Enqueue(item);
                }
                bases = _bases;
            }
            bases.Enqueue(int.MinValue);
            string sb = "";

            sb += "   frame view = [";
            if (bases.ElementAt(0) == 0) {
                sb += "| ";
                bases.Dequeue();
            }
            if (sp == -1) {
                sb += "]";
                Console.WriteLine(sb);
                return;
            }
            sb += s[0].ToString();
            for (int i = 1; i < sp + 1; i++) {
                if (bases.ElementAt(0) == i) {
                    sb += " | ";
                    bases.Dequeue();
                } else {
                    sb += ", ";
                }
                sb += s[i].ToString();
            }
            sb += "]";
            Console.WriteLine(sb);
        }
        public void FromString(string inst) {
            // TODO
            HSMAssembler asm = new HSMAssembler(inst);
            HSM vm = new HSM();
            List<int> code = asm.Assemble();
            Console.WriteLine(code);
            vm.Execute(code);
        }
    }

    public class HSMAssembler {
        public HSMAssembler(string str = "") {
            this.Code = str;
            this.Table = new Dictionary<string, int>();
            this.SetTable();
            this.MakeOpCodeMap();
        }
        public string Code {
            get; set;
        }
        public Dictionary<string, int> Table {
            get; set;
        }
        public Dictionary<string, int> OpCodeMap {
            get; set;
        }
        public void SetTable() {
            foreach (OP op in Enum.GetValues(typeof(OP))) {
                this.Table[op.ToString()] = (int)op;
            }
        }
        public List<int> Assemble() {
            List<int> code = new List<int>();
            string[] lines = this.Code.Split('\n');
            foreach (string line in lines) {
                if (line == "") { continue; }
                string[] inst = line.Split(' ');
                if (inst.Length != 3) {
                    throw new Exception("illegal instruction: " + inst);
                }
                this.DecodeAndLoad(inst, code);
            }
            return code;
        }
        public void DecodeAndLoad(string[] inst, List<int> code) {
            string op = inst[0];

            int operand1 = int.Parse(inst[1]);
            int operand2 = int.Parse(inst[2]);
            int opcode;
            try {
                opcode = this.OpCodeMap[op];
            } catch (KeyNotFoundException) {
                throw (new Exception("illegal instruction (mnemonic) : " + op));
            }
            code.Add(opcode);
            code.Add(operand1);
            code.Add(operand2);
        }

        public void MakeOpCodeMap() {
            this.OpCodeMap = new() {{Mnemonic.LDC.ToString(), (int)OP.LDC},{ Mnemonic.WNL.ToString(), (int)OP.WNL},{ Mnemonic.WRI.ToString(), (int)OP.WRI},
                { Mnemonic.HLT.ToString(), (int)OP.HLT},{ Mnemonic.AD.ToString(), (int)OP.AD},{ Mnemonic.SB.ToString(), (int)OP.SB},{ Mnemonic.ML.ToString(), (int)OP.ML},
                { Mnemonic.DV.ToString(), (int)OP.DV},{ Mnemonic.MD.ToString(), (int)OP.MD},{ Mnemonic.PUSH.ToString(), (int)OP.PUSH},{ Mnemonic.POP.ToString(), (int)OP.POP},
                { Mnemonic.NE.ToString(), (int)OP.NE},{ Mnemonic.EQ.ToString(), (int)OP.EQ},{ Mnemonic.LT.ToString(), (int)OP.LT},{ Mnemonic.GT.ToString(), (int)OP.GT},
                { Mnemonic.LE.ToString(), (int)OP.LE},{ Mnemonic.GE.ToString(), (int)OP.GE},{ Mnemonic.J.ToString(), (int)OP.J},{ Mnemonic.FJ.ToString(), (int)OP.FJ},
                { Mnemonic.TJ.ToString(), (int)OP.TJ},{ Mnemonic.LDV.ToString(), (int)OP.LDV},{ Mnemonic.STV.ToString(), (int)OP.STV},{Mnemonic.CALL.ToString(), (int)OP.CALL},
                { Mnemonic.EF.ToString(), (int)OP.EF }
            };
        }

        public void ExeCode(string hsm) {
            List<int> code = new HSMAssembler(hsm).Assemble();
            HSM vm = new HSM();
            vm.ShowState = true;
            vm.Execute(code);
        }
        public static void Main_asm() {
            const int NUMTEST = 9;
            string[] q = new string[NUMTEST];
            string[] c = new string[NUMTEST];
            q[0] = "fun int proc(int x){return x;} fun int main(){putint(proc(256));return 0;}";
            c[0] = "PUSH 0 0\nCALL 0 7\nPOP 0 1\nHLT 0 0\nPUSH 0 3\nLDV 0 -1\nEF 0 1\nPUSH 0 3\nLDC 0 256\nCALL 1 4\nWRI 0 0\nLDC 0 0\nEF 0 0\n";
            int i = 0;
            Console.WriteLine($"---- \ntest({i})={q[i]}");
            Console.WriteLine(c[i]);
            List<int> code = new HSMAssembler(c[i]).Assemble();
            HSM vm = new HSM();
            vm.ShowState = true;
            vm.Execute(code);
        }
    }
}
