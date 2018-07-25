using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Depth_First_Top_Down
{
    class Program
    {
        private static int csvMax = 5;
        private static char StartSymbol = 'S';
        private static char endSymbol = '#';
        private static int intToChar = 48;
        private static int formatLength = 15;

        private static Dictionary<char, List<string>> getFileContent(string name)
        {
            var csvReader = new StreamReader(name);
            int index = 0; //the first line is for description ,not real data, skip it
            var content = new Dictionary<char, List<string>>();
            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();

                index++;
                if (index == 1)
                {
                    continue;
                }

                var values = line.Split(',');
                if (values[0] != "" && values[0] != " ")
                {
                    var List = new List<string>();
                    for (int i = 1; i <= csvMax; ++i)
                    {
                        if (values[i] != "" && values[i] != " ")
                        {
                            List.Add(values[i]);
                        }
                    }
                    content[values[0][0]] = List;
                }
            }

            return content;
        }

        private static Dictionary<char, List<string>> content;

        private static HashSet<char> nonTerminalHashset = new HashSet<char>()
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z'
        };

        private static Stack<char> restInputStack = new Stack<char>();
        private static Stack<char> matchedCharStack = new Stack<char>();
        private static Stack<char> predictionStack = new Stack<char>();
        private static Stack<char> analysisStack = new Stack<char>();

        static void Main(string[] args)
        {
            content = getFileContent("rules.csv");
            if (content.ContainsKey(StartSymbol) == false)
            {
                return;
            }

            restInputStack.Clear();
            restInputStack.Push(endSymbol);
            Console.WriteLine("input your input string:");
            var UserInput = Console.ReadLine();
            if (UserInput.Length == 0)
            {
                return;
            }
            for (int i = UserInput.Length - 1; i > -1; i--)
            {
                restInputStack.Push(UserInput[i]);
            }
            predictionStack.Push(endSymbol);
            predictionStack.Push(StartSymbol);
            analysisStack.Push(endSymbol);


            analysisAgain();
            Console.ReadLine();
        }

        /// <summary>
        /// get the 'ruleIndex'th rule to replace the first symbol(non-terminal) in prediction Stack
        /// </summary>
        /// <param name="non_Terminal">the first symbol(non-terminal) in prediction Stack</param>
        /// <param name="ruleIndex">the 'ruleIndex'th in the csv rules</param>
        private static void replacePredictionByRules(int ruleIndex)
        {
            if (predictionStack.Count == 1)
            {
                return;
            }

            char non_Terminal = predictionStack.Pop();
            string replaceString = content[non_Terminal][ruleIndex];
            Console.WriteLine("replace non-terminal in prediction, rule Index: " + intToUnicodeCharShow(ruleIndex));
            analysisStack.Push(non_Terminal);
            analysisStack.Push(Convert.ToChar(ruleIndex));

            for(int i = replaceString.Length - 1; i > -1; --i)
            {
                predictionStack.Push(replaceString[i]);
            }
            showCurrent();

            analysisAgain();
        }

        private static void analysisAgain()
        {
            if (predictionStack.Count == 1)
            {
                Console.WriteLine("finished, the analysis is: " + getReplaceRules(analysisStack));
                return;
            }

            if (nonTerminalHashset.Contains(predictionStack.Peek())) //the first is still non-terminal, replace again
            {
                replacePredictionByRules(0);
            }
            else
            {
                if (predictionStack.Peek() == restInputStack.Peek())  //same, change all the stack, moveforward
                {
                    moveForwardWhenMatch();
                }
                else  //backtrack
                {
                    backTrack();
                }
            }
        }

        private static void showCurrent()
        {
            Console.WriteLine("{0,15}   {1,-15}\n{2,15}   {3,-15}",
                getReplaceRules(matchedCharStack), getReplaceRules(restInputStack),
                getReplaceRules(analysisStack), getReplaceRules(predictionStack));
        }

        private static void moveForwardWhenMatch()
        {
            Console.WriteLine("terminal matches, moveforward, char: " + intToUnicodeCharShow(restInputStack.Peek()));
            matchedCharStack.Push(restInputStack.Pop());
            analysisStack.Push(predictionStack.Pop());
            showCurrent();
            analysisAgain();
        }

        private static void backTrack()
        {
            int backIndex = analysisStack.Peek();
            if (backIndex > -1 && backIndex < 10) //number indicates that this is the non-terminal(A1). pop A and 1
            {
                backIndex = analysisStack.Pop();
                char non_terminal = analysisStack.Pop();
                List<string> rules = content[non_terminal];
                if (rules.Count > (backIndex + 1)) //change to next rules(n + 1)
                {
                    Console.WriteLine(non_terminal + " --change to next match, index:" + intToUnicodeCharShow(backIndex + 1));
                    string oldRules = rules[backIndex];
                    for (int i = 0; i < oldRules.Length; i++) //pop the old rules
                    {
                        predictionStack.Pop();
                    }

                    string newRules = rules[backIndex + 1];
                    for (int i = newRules.Length - 1; i > -1; i--) //insert the new rules
                    {
                        predictionStack.Push(newRules[i]);
                    }
                    analysisStack.Push(non_terminal);
                    analysisStack.Push(Convert.ToChar(backIndex + 1));
                    showCurrent();
                    analysisAgain();
                }
                else //the non_terminal's rules is the last, backtrack again.
                {
                    Console.WriteLine("backtracking, non_terminal'rule is traversed: " + non_terminal);
                    string oldRules = rules[backIndex];
                    for (int i = 0; i < oldRules.Length; i++) //pop the old rules
                    {
                        predictionStack.Pop();
                    }
                    predictionStack.Push(non_terminal);
                    showCurrent();
                    backTrack();
                }
            }
            else
            {
                Console.WriteLine("backtracking, char: " + intToUnicodeCharShow(analysisStack.Peek()));
                predictionStack.Push(analysisStack.Pop());
                restInputStack.Push(matchedCharStack.Pop());
                showCurrent();
                backTrack();
            }
        }

        private static string getReplaceRules(Stack<char> currentStack)
        {
            Stack<char> tempStack = new Stack<char>(currentStack.Reverse());
            string content = "";
            while (tempStack.Count != 0)
            {
                int pop = tempStack.Pop();
                if (pop < 10 && pop > -1)//  0-9(int) change to 0-9(unicode)
                {
                    content = content + intToUnicodeCharShow(tempStack.Pop()) + intToUnicodeCharShow(pop);
                }
                else
                {
                    content = content + intToUnicodeCharShow(pop);
                }
            }
            return content;
        }

        private static char intToUnicodeCharShow(int pop)
        {
            if (pop < 10 && pop > -1)//  0-9(int) change to 0-9(unicode)
            {
                return Convert.ToChar(pop + 1 + intToChar);
            }
            else
            {
                return Convert.ToChar(pop);
            }
        }
    }
}
