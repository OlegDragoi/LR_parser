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
                        inputString.Remove(0, checkedSign.Length);
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


    }
}
