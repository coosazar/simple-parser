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
                exp := trm add
                add := + trm mul | mul
                mul := * trm
                trm := num
                num := [0-9](\.[0-9])?
            */

            doLexer("2 * 3");

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
            var res = p_add(num);
            return res;
        }

        static double p_add(double num){
            var op = peek();

            if(op == SYMB_ADD){
                next();

                var val = p_trm();

                string p;
                if((p = peek()) == null){
                    return num += val;
                }
                else {
                    return num += p_mul(val);
                }
            }

            return p_mul(num);
        }
    
        static double p_mul(double num){
            next();

            return num * Double.Parse(next());
        }

        static double p_trm(){
            return p_num();
        }

        static double p_num() => Double.Parse(next());
        


        const string
            SYMB_ADD = "+",
            SYMB_SUB = "-",
            SYMB_MUL = "*";
                
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
