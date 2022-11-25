using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LR_parser
{
    public class LrParser
    {
        private string[,] ruleSystem;
        private string input;
        private string nr;
        private Stack<string> inputStack;
        private Stack<string> computingStack;

        public LrParser(string[,] ruleSystem, string input)
        {
            this.Init(ruleSystem, input);
        }

        public void Init(string[,] ruleSystem, string input)
        {
            this.ruleSystem = ruleSystem;
            this.input = input;

            //initializing the computingStack
            this.computingStack = new();
            this.computingStack.Push("#");
            this.computingStack.Push("E");
            //converting the input string into a stack
            List<string> inputStackList = new();
            string inputString = (string)this.input.Clone();

            while (inputString.Length > 0)
            {
                for (int i = 1; i < ruleSystem.GetLength(0); i++)
                {
                    //if the input string contains an element of the first row of the rule system
                    string checkedSign = ruleSystem[i, 0];
                    if (inputString.StartsWith(checkedSign))       
                    {
                        inputStackList.Add(checkedSign);
                        inputString = inputString.Remove(0, checkedSign.Length);
                        break;
                    }
                }
            }

            //saving the inputStackList into the inputStack
            this.inputStack = new();
            for (int i = inputStackList.Count-1; i >=0; i--)
            {
                this.inputStack.Push(inputStackList[i]);
            }
        }

        private bool TryParse()
        {
            int x = 0 , y = 0;
            string lastInput = inputStack.Peek();
            string lastCompute = computingStack.Peek();
            for (int i = 1; i < ruleSystem.GetLength(0); i++)
            {
                if (ruleSystem[i, 0] == lastInput) x = i;
                break;
            }
            for (int i = 1; i < ruleSystem.GetLength(1); i++)
            {
                if (ruleSystem[0, i] == lastCompute) y = i;
                break;
            }

            string inspectedCell = (string)ruleSystem[x, y].Clone();

            if (inspectedCell == "") return false;
            if (inspectedCell == "accept") return false;

            if (inspectedCell == "pop")
            {
                inputStack.Pop();
                computingStack.Pop();
                return true;
            }

            string[] instructions = inspectedCell.Substring(1, inspectedCell.Length - 2)    //deletes the starting '(' and ending ')'
                                                 .Split(",");                               //separates the instructions
            string computingStackInstruction = instructions[0];
            string nrAdd = instructions[1];

            List<string> computingStackAdd = new();
            while (computingStackInstruction.Length > 0)
            {
                for (int i = 1; i < ruleSystem.GetLength(1); i++)
                {
                    //if the input string contains an element of the first row of the rule system
                    string checkedSign = ruleSystem[0, i];
                    if (computingStackInstruction.StartsWith(checkedSign))
                    {
                        computingStackAdd.Add(checkedSign);
                        computingStackInstruction = computingStackInstruction.Remove(0, checkedSign.Length);
                        break;
                    }
                }
            }
            computingStack.Pop();
            for (int i = computingStackAdd.Count - 1; i >= 0; i--)
            {
                this.computingStack.Push(computingStackAdd[i]);
            }
            this.nr += nrAdd;

            return true;
        }

        public List<string> GetResult()
        {
            List<string> result = new();
            do
            {
                result.Add(this.ToString());
            } while (this.TryParse());

            if(computingStack.Peek() != "#" &&
               inputStack.Peek() != "#")
            {
                result.Add("Wrong input!");
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('(').Append(inputStack.ToString()).Append(',')
                          .Append(computingStack.ToString()).Append(',')
                          .Append(nr).Append(')');
            return sb.ToString();
        }
    }
}
