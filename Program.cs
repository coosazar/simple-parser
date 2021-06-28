using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace simple_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            /*
                exp    := trm addsub
                addsub := (+|-) trm addsub | muldiv
                muldiv := (*|/) trm muldiv | lambda
                trm    := num
                num    := [0-9](\.[0-9])?
            */

            doLexer("3 + 2 * 3 + 1 / 2 - 0.5");

            Console.WriteLine(String.Join(", ", _lex));

            var res = doParser();
            
            Console.WriteLine(res);
        }

        static int _pos = 0;
        static  string next(){
            if(_pos == _lex.Count) return null;
            return _lex[_pos++];
        }

        static string peek(){
            if(_pos == _lex.Count) return null;
            return _lex[_pos];
        }

        static double doParser() => p_exp();

        static double p_exp(){
            var num = p_trm();
            var res = p_addsub(num);
            return res;
        }

        static double p_addsub(double num){
            double ret = num;

            string op;
            while((op = peek()) == SYMB_ADD || op == SYMB_SUB){
                next();

                var val = p_trm();

                string p;
                if((p = peek()) != null){
                    val = p_muldiv(val);
                }

                switch(op) {
                    case SYMB_ADD: ret += val; break;
                    case SYMB_SUB: ret -= val; break;
                    default: throw new Exception();
                };
            }
            
            return p_muldiv(ret);
        }
    
        static double p_muldiv(double num){
            var ret = num;

            string op;
            while((op = peek()) == SYMB_MUL || op == SYMB_DIV){
                next();

                var val = p_trm();

                // string p;
                // if((p = peek()) != null){
                //     val += p_muldiv(num);
                // }

                switch(op) {
                    case SYMB_MUL: ret *= val; break;
                    case SYMB_DIV: ret /= val; break;
                    default: throw new Exception();
                };
            }
            
            return ret;
        }

        static double p_trm(){
            return p_num();
        }

        static double p_num() => Double.Parse(next());
        

        const string
            SYMB_ADD = "+",
            SYMB_SUB = "-",
            SYMB_MUL = "*",
            SYMB_DIV = "/";
                
        static string[] _symb = new [] { SYMB_ADD, SYMB_SUB, SYMB_MUL };

        static List<string> _lex = null;
        static void doLexer(string exp){
            _lex = new List<string>();

            int pos = 0;
            while(true){
                while(pos < exp.Length && exp[pos] == ' ') pos++;
                var st = pos;

                while(pos < exp.Length && (exp[pos] != ' ' || _symb.Contains(exp[pos]+""))) pos++;

                _lex.Add(exp.Substring(st, pos - st));

                if(pos == exp.Length) break;
            }
        }
    }
}
