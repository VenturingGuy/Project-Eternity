﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEternity.Core
{
    public abstract class FormulaParser
    {
        public static FormulaParser ActiveParser;

        public string Evaluate(string Expression)
        {
            if (Expression.StartsWith("//C#"))
            {
                return EvaluateWithRoslyn(Expression);
            }

            Expression = Expression.ToLower();
            //Expression = Expression.Replace(" ", "");
            Expression = Expression.Replace("true", "1");
            Expression = Expression.Replace("false", "0");

            Stack<string> StackOperation = new Stack<string>();

            string ValueBuffer = "";
            for (int i = 0; i < Expression.Length; i++)
            {
                char ActiveChar = Expression.Substring(i, 1)[0];
                // pick up any logical operators of 2 characters first and skip to the next character.
                if (i < Expression.Length - 1)
                {
                    string op = Expression.Substring(i, 2);
                    if (op == "<=" || op == ">=" || op == "==" || op == "!=" || op == "&&" || op == "||")
                    {
                        if (!string.IsNullOrWhiteSpace(ValueBuffer))
                        {
                            StackOperation.Push(ValueBuffer);//Add the digit or variable found before the sign.
                        }
                        ValueBuffer = "";//Reset the value.
                        StackOperation.Push(op);//Add the operator.
                        i++;//Skip to the next character.
                        continue;
                    }
                }
                if (ActiveChar.Equals('-') && StackOperation.Count == 0 && ValueBuffer == "")
                {
                    ValueBuffer += ActiveChar;
                }

                #region Process brackets.

                else if (ActiveChar.Equals('('))
                {
                    if (!string.IsNullOrWhiteSpace(ValueBuffer))
                    {
                        StackOperation.Push(ValueBuffer);
                    }
                    ValueBuffer = "";

                    string BracketBuffer = "";
                    ++i; //Fetch Next Character
                    int BracketCount = 0;
                    for (; i < Expression.Length; i++)
                    {
                        ActiveChar = Expression.Substring(i, 1)[0];

                        //An other bracket as been found, count it.
                        if (ActiveChar.Equals('('))
                            BracketCount++;

                        //Found the end of a bracket, decrement the bracket count.
                        if (ActiveChar.Equals(')'))
                        {//End of the first bracket, end the loop.
                            if (BracketCount == 0)
                                break;
                            BracketCount--;
                        }
                        //Add the current character to the bracket buffer.
                        BracketBuffer += ActiveChar;
                    }
                    //Compute the value of the BracketBuffer.
                    StackOperation.Push(Evaluate(BracketBuffer));
                }

                #endregion

                else if (ActiveChar.Equals('+') || ActiveChar.Equals('-') ||
                         ActiveChar.Equals('*') || ActiveChar.Equals('/') ||
                         ActiveChar.Equals('%') ||
                         ActiveChar.Equals('<') || ActiveChar.Equals('>'))
                {
                    if (!string.IsNullOrWhiteSpace(ValueBuffer))
                    {
                        StackOperation.Push(ValueBuffer);
                    }
                    ValueBuffer = "";

                    StackOperation.Push(ActiveChar.ToString());
                }
                else if (ActiveChar == ' ')
                {
                    //Ignore whitespaces
                }
                else if (char.IsLetter(ActiveChar) || (ValueBuffer != "" && char.IsLetter(ValueBuffer[0])))
                {
                    ValueBuffer += ActiveChar;
                }
                //It's a number or a point from a decimal, add the character to the buffer.
                else if (char.IsDigit(ActiveChar) || ActiveChar == '.')
                {
                    ValueBuffer += ActiveChar;
                    //Number have more then 1 point. (Ex: 1.1.1.1)
                    if (ValueBuffer.Split('.').Length > 2)
                        throw new Exception("Invalid decimal.");
                }
                else
                {
                    throw new Exception("Invalid character.");
                }

                //End of string reached.
                if (i == (Expression.Length - 1))
                {
                    if (!string.IsNullOrWhiteSpace(ValueBuffer))
                    {
                        StackOperation.Push(ValueBuffer);
                    }
                }
            }

            #region Process sign operations

            List<string> ListOperation = StackOperation.ToList();

            bool RightIsNumber;
            bool LeftIsNumber;
            string Right;
            string Left;
            double RightValue;
            double LeftValue;

            #region Process divisions

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "/")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        ListOperation[i] = (LeftValue / RightValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        ListOperation[i] = "[" + (LeftX / RightX) + "," + (LeftY / RightY) + "]";
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process multiplications

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "*")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        ListOperation[i] = (LeftValue * RightValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        ListOperation[i] = "[" + (LeftX * RightX) + "," + (LeftY * RightY) + "]";
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process modulos.

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "%")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        ListOperation[i] = (LeftValue % RightValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        ListOperation[i] = "[" + (LeftX % RightX) + "," + (LeftY % RightY) + "]";
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process additions.

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "+")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        ListOperation[i] = (LeftValue + RightValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        ListOperation[i] = "[" + (LeftX + RightX) + "," + (LeftY + RightY) + "]";
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process subtractions.

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "-")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        ListOperation[i] = (LeftValue - RightValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        ListOperation[i] = "[" + (LeftX - RightX) + "," + (LeftY - RightY) + "]";
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #endregion

            #region Proccess comparison operations

            #region Process <, <=, >, >=

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "<" || ListOperation[i] == "<=" || ListOperation[i] == ">"|| ListOperation[i] == ">=")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        if (ListOperation[i] == "<")
                            ListOperation[i] = (LeftValue < RightValue) ? "1" : "0";
                        else if (ListOperation[i] == "<=")
                            ListOperation[i] = (LeftValue <= RightValue) ? "1" : "0";
                        else if (ListOperation[i] == ">")
                            ListOperation[i] = (LeftValue > RightValue) ? "1" : "0";
                        else if (ListOperation[i] == ">=")
                            ListOperation[i] = (LeftValue >= RightValue) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        if (ListOperation[i] == "<")
                            ListOperation[i] = (LeftX + LeftY < RightX + RightY) ? "1" : "0";
                        else if (ListOperation[i] == "<=")
                            ListOperation[i] = (LeftX + LeftY <= RightX + RightY) ? "1" : "0";
                        else if (ListOperation[i] == ">")
                            ListOperation[i] = (LeftX + LeftY > RightX + RightY) ? "1" : "0";
                        else if (ListOperation[i] == ">=")
                            ListOperation[i] = (LeftX + LeftY >= RightX + RightY) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right);
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process ==, !=

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "==" || ListOperation[i] == "!=")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        if (ListOperation[i] == "==")
                            ListOperation[i] = (LeftValue == RightValue) ? "1" : "0";
                        else if (ListOperation[i] == "!=")
                            ListOperation[i] = (LeftValue != RightValue) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right);
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        string[] RightExpression = Right.Split(',', '[', ']');
                        string[] LeftExpression = Left.Split(',', '[', ']');

                        double RightX = Convert.ToDouble(RightExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double RightY = Convert.ToDouble(RightExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        double LeftX = Convert.ToDouble(LeftExpression[0], System.Globalization.CultureInfo.InvariantCulture);
                        double LeftY = Convert.ToDouble(LeftExpression[1], System.Globalization.CultureInfo.InvariantCulture);

                        if (ListOperation[i] == "<")
                            ListOperation[i] = (LeftX == RightX && LeftY == RightY) ? "1" : "0";
                        else if (ListOperation[i] == "<=")
                            ListOperation[i] = (LeftX != RightX || LeftY != RightY) ? "1" : "0";
                        else
                            throw new Exception(Right + ListOperation[i] + Left);
                        ListOperation[i] = "[" + (LeftX / RightX) + "," + (LeftY / RightY) + "]";
                    }
                    else if (!RightIsNumber && !LeftIsNumber)
                    {
                        if (ListOperation[i] == "==")
                            ListOperation[i] = (Left == Right) ? "1" : "0";
                        else if (ListOperation[i] == "!=")
                            ListOperation[i] = (Left != Right) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right);
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #endregion

            #region Proccess logical operations

            #region Process logical AND (&&)

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "&&")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        if ((LeftValue == 0 || LeftValue == 1) && (RightValue == 0 || RightValue == 1))
                            ListOperation[i] = (LeftValue == 1 && RightValue == 1) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right + ", one of the values isn't a boolean.");
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        throw new Exception(Left + ListOperation[i] + Right + ", && operation not supported on 2D points.");
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #region Process logical OR (||)

            for (int i = ListOperation.Count - 2; i >= 0; i--)
            {
                if (ListOperation[i] == "||")
                {
                    Right = ListOperation[i - 1];
                    Left = ListOperation[i + 1];

                    ComputeValues(ref Right, ref Left, out RightValue, out LeftValue, out RightIsNumber, out LeftIsNumber);

                    if (RightIsNumber && LeftIsNumber)
                    {
                        if ((LeftValue == 0 || LeftValue == 1) && (RightValue == 0 || RightValue == 1))
                            ListOperation[i] = (LeftValue == 1 || RightValue == 1) ? "1" : "0";
                        else
                            throw new Exception(Left + ListOperation[i] + Right + ", one of the values isn't a boolean.");
                    }
                    else if (Right[0].Equals('[') && Right.Last().Equals(']') && Left[0].Equals('[') && Left.Last().Equals(']'))
                    {
                        throw new Exception(Left + ListOperation[i] + Right + ", || operation not supported on 2D points.");
                    }
                    else
                        throw new Exception(Left + ListOperation[i] + Right);

                    ListOperation.RemoveAt(i + 1);
                    ListOperation.RemoveAt(i - 1);
                    i -= 2;
                }
            }

            #endregion

            #endregion

            if (ListOperation.Count == 1)
                return ListOperation[0];
            else
                throw new Exception(StackOperation.ToString() + " is invalid");
        }

        private string EvaluateWithRoslyn(string Expression)
        {
            return Roslyn.RoslynWrapper.EvaluateAsync<string>(Expression);
        }

        protected void ComputeValues(ref string Right, ref string Left, out double RightValue, out double LeftValue, out bool RightIsNumber, out bool LeftIsNumber)
        {
            string RightNumber = Right.Replace(" ", "");
            string LeftNumber = Left.Replace(" ", "");
            //Just remove space from numbers. Need to remove space so IsNumber work.
            if (char.IsNumber(RightNumber[0]))
            {
                Right = RightNumber;
            }
            if (char.IsNumber(LeftNumber[0]))
            {
                Left = LeftNumber;
            }

            RightValue = 0;
            LeftValue = 0;

            if (char.IsNumber(Right[0]) || Right[0] == '-')
            {
                RightValue = Convert.ToDouble(Right, System.Globalization.CultureInfo.InvariantCulture);
                RightIsNumber = true;
            }
            else
            {
                Right = ValueFromVariable(Right);

                if (char.IsNumber(Right[0]) || Right[0] == '-')
                {
                    RightValue = Convert.ToDouble(Right, System.Globalization.CultureInfo.InvariantCulture);
                    RightIsNumber = true;
                }
                else
                    RightIsNumber = false;
            }
            if (char.IsNumber(Left[0]) || Left[0] == '-')
            {
                LeftValue = Convert.ToDouble(Left, System.Globalization.CultureInfo.InvariantCulture);
                LeftIsNumber = true;
            }
            else
            {
                Left = ValueFromVariable(Left);

                if (char.IsNumber(Left[0]) || Left[0] == '-')
                {
                    LeftValue = Convert.ToDouble(Left, System.Globalization.CultureInfo.InvariantCulture);
                    LeftIsNumber = true;
                }
                else
                    LeftIsNumber = false;
            }
        }

        protected abstract string ValueFromVariable(string Input);
    }
}
