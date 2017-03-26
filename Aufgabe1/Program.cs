//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class reads, saves and performs calculations with vectors and scalars, which are defined by the user.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class reads, saves and performs calculations with vectors and scalars, which are defined by the user.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Maximum amount of variables, which can be defined by the user.
        /// </summary>
        private const int MaxInputs = 10;

        /// <summary>
        /// Error code, which indicates that the user has entered an unsupported operator.
        /// </summary>
        private const char UnsupportedOperator = 'u';

        /// <summary>
        /// Error code, which indicates that the user input was invalid.
        /// </summary>
        private const char InvalidFormatting = 'i';

        /// <summary>
        /// Array of valid operators, which are supported by this program.
        /// </summary>
        private static char[] validOperators = new char[] { '+', '-', '*', 'x' };

        /// <summary>
        /// This method represents the entry point of the program.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        public static void Main(string[] args)
        {
            string[] variableNames = new string[10];
            string[] values = new string[10];
            int variablesAmount = 0;

            Console.WriteLine("You can use this program to perform calculations on vectors and scalars.\nThe maximum amount of input values is {0}!", MaxInputs);
            Console.WriteLine("\nConsider following formats for defining a - vector: A=[1,2,3,4,...]");
            Console.WriteLine("                                          - scalar: B=4");
            Console.WriteLine("\nYou can also use the functions fibo(n) and prim(n),\nwhich deliver the n-th Fibonacci or Prime number. n must be larger than 0!");
            Console.WriteLine("\nTo finish the input, just press Enter.\n");

            variablesAmount = ReadVectorsAndScalars(variableNames, values);

            if (variablesAmount >= 1)
            {
                Console.WriteLine("\nYou can now enter any arithmetical operation.");
                Console.WriteLine("Following operations are supported: - Addition    (+)        Subtraction   (-)");
                Console.WriteLine("                                    - Dot product (*)        Cross product (x)");
                Console.WriteLine("\nConsider that you have to use this format: A [operator] B");
                Console.WriteLine("\nTo exit this programm just press Enter.\n");

                ReadArithmeticalOperations(variableNames, values);
            }
            else
            {
                Console.WriteLine("\nYou have to define at least one variable as a vector or a scalar!");
            }
        }

        /// <summary>
        /// This method prompts the user to type in a limited amount of definitions of vectors and scalars.
        /// </summary>
        /// <param name="variableNames">Array of variable names, which will be altered.</param>
        /// <param name="values">Array of correct definitions, which will be altered.</param>
        /// <returns>The amount of definitions the user entered.</returns>
        private static int ReadVectorsAndScalars(string[] variableNames, string[] values)
        {
            int variablesAmount = 0;
            string input = string.Empty;

            do
            {
                if (input.Length > 0)
                {
                    if (IsValidVector(input) || IsValidScalar(input))
                    {
                        // Only save when the variable name does not exist yet
                        if (Array.IndexOf(variableNames, GetVariableName(input)) == -1)
                        {
                            variableNames[variablesAmount] = GetVariableName(input);

                            // Calculate the values and save them together with the variable name, so that big Fibonacci or prime numbers do not have to be computed every time
                            if (IsValidVector(input))
                            {
                                values[variablesAmount] = GetVariableName(input) + "=" + GetFormattedArray(GetVectorValues(input));
                            }
                            else
                            {
                                int temp = 0;
                                GetScalarValue(input, ref temp);

                                values[variablesAmount] = GetVariableName(input) + "=" + temp.ToString();
                            }

                            variablesAmount++;
                        }
                        else
                        {
                            Console.WriteLine("You have already defined some vector or scalar with this variable name!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input! Please make sure it complies with the above-mentioned format!\n");
                    }
                }

                if (variablesAmount < MaxInputs)
                {
                    Console.Write("{0,2}: ", variablesAmount + 1);
                }

                // If the user enters nothing or only whitespaces, the prompt closes
            }
            while (variablesAmount < MaxInputs && !string.IsNullOrWhiteSpace((input = Console.ReadLine()).Trim()));

            return variablesAmount;
        }

        /// <summary>
        /// This method prompts the user to type in an unlimited amount of calculations with defined variables.
        /// </summary>
        /// <param name="variableNames">Array of variable names.</param>
        /// <param name="values">Array of correct definitions.</param>
        private static void ReadArithmeticalOperations(string[] variableNames, string[] values)
        {
            string input = string.Empty;
            char charOperator;
            string variable1;
            string variable2;

            do
            {
                if (input.Length > 0)
                {
                    charOperator = GetOperator(input);

                    // Depending on the error code, the user gets some appropriate message
                    if (charOperator == UnsupportedOperator)
                    {
                        Console.WriteLine("This operator is not supported!\n");
                    }
                    else if (charOperator == InvalidFormatting)
                    {
                        Console.WriteLine("Invalid input! Please make sure it complies with the above-mentioned format!\n");
                    }
                    else
                    {
                        // To work with the variables, they have to exist
                        if (Array.IndexOf(variableNames, GetOperand(input, 0)) == -1)
                        {
                            Console.WriteLine("You have not defined the first operand!\n");
                        }
                        else if (Array.IndexOf(variableNames, GetOperand(input, 1)) == -1)
                        {
                            Console.WriteLine("You have not defined the second operand!\n");
                        }
                        else
                        {
                            variable1 = values[Array.IndexOf(variableNames, GetOperand(input, 0))];
                            variable2 = values[Array.IndexOf(variableNames, GetOperand(input, 1))];

                            Console.WriteLine(PerformCalculation(variable1, variable2, charOperator) + "\n");
                        }
                    }
                }

                Console.Write("> ");

                // If the user enters nothing or only whitespaces, the prompt closes
            }
            while (!string.IsNullOrWhiteSpace((input = Console.ReadLine()).Trim()));
        }

        /// <summary>
        /// Performs different arithmetical operations on vectors and / or scalars.
        /// </summary>
        /// <param name="variable1">Represents the first variable, which is a definition of a vector or a scalar.</param>
        /// <param name="variable2">Represents the second variable, which is a definition of a vector or a scalar.</param>
        /// <param name="charOperator">Char indicating the type of arithmetical operation.</param>
        /// <returns>A string containing either the result or an error message.</returns>
        private static string PerformCalculation(string variable1, string variable2, char charOperator)
        {
            string result = "result: ";

            switch (charOperator)
            {
                case '+':
                    // Check if two vectors or two scalars are added...
                    if (IsValidVector(variable1) && IsValidVector(variable2))
                    {
                        // Check if their dimension is the same
                        if (GetVectorValues(variable1).Length == GetVectorValues(variable2).Length)
                        {
                            result += GetFormattedArray(AddVectors(GetVectorValues(variable1), GetVectorValues(variable2)));
                        }
                        else
                        {
                            result = "To add two vectors, their dimension must be the same!";
                        }
                    }
                    else if (IsValidScalar(variable1) && IsValidScalar(variable2))
                    {
                        int value1 = 0;
                        int value2 = 0;

                        GetScalarValue(variable1, ref value1);
                        GetScalarValue(variable2, ref value2);

                        result += (value1 + value2).ToString();
                    }
                    else
                    {
                        result = "You cannot add a scalar and a vector!";
                    }

                    break;
                case '-':
                    // Check if two vectors or two scalars are subtracted...
                    if (IsValidVector(variable1) && IsValidVector(variable2))
                    {
                        // Check if their dimension is the same
                        if (GetVectorValues(variable1).Length == GetVectorValues(variable2).Length)
                        {
                            result += GetFormattedArray(SubtractVectors(GetVectorValues(variable1), GetVectorValues(variable2)));
                        }
                        else
                        {
                            result = "To subtract two vectors, their dimension must be the same!";
                        }
                    }
                    else if (IsValidScalar(variable1) && IsValidScalar(variable2))
                    {
                        int value1 = 0;
                        int value2 = 0;

                        GetScalarValue(variable1, ref value1);
                        GetScalarValue(variable2, ref value2);

                        result += (value1 - value2).ToString();
                    }
                    else
                    {
                        result = "You cannot subtract a scalar and a vector!";
                    }

                    break;
                case '*':
                    // Check if the two variables are vectors
                    if (IsValidVector(variable1) && IsValidVector(variable2))
                    {
                        // Check if their dimension is the same
                        if (GetVectorValues(variable1).Length == GetVectorValues(variable2).Length)
                        {
                            result += GetDotProduct(GetVectorValues(variable1), GetVectorValues(variable2)).ToString();
                        }
                        else
                        {
                            result = "To get the dot product of two vectors, their dimension must be the same!";
                        }
                    }
                    else
                    {
                        result = "You need two vectors to generate the dot product!";
                    }

                    break;
                case 'x':
                    // Check if the two variables are vectors
                    if (IsValidVector(variable1) && IsValidVector(variable2))
                    {
                        // Check if their dimension is three
                        if (GetVectorValues(variable1).Length == 3 && GetVectorValues(variable2).Length == 3)
                        {
                            result += GetFormattedArray(GetCrossProduct(GetVectorValues(variable1), GetVectorValues(variable2)));
                        }
                        else
                        {
                            result = "To get the cross product of two vectors, their dimension must be 3!";
                        }
                    }
                    else
                    {
                        result = "You need two vectors to generate the dot product!";
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Extracts the operator from a calculation.
        /// </summary>
        /// <param name="s">String, which represents the calculation.</param>
        /// <returns>A char containing the operator or a error code in the case of a corrupt input.</returns>
        private static char GetOperator(string s)
        {
            string calculation = s.Trim();
            string[] splittedByOperator = calculation.Split(' ');

            if (splittedByOperator.Length == 3)
            {
                // Altogether, there have to be exactly two whitespaces between first operand, operator and second operand
                if (splittedByOperator[1].Length == 1 && (calculation.Length - (splittedByOperator[0].Length + splittedByOperator[1].Length + splittedByOperator[2].Length)) == 2)
                {
                    if (validOperators.Contains(splittedByOperator[1][0]))
                    {
                        return splittedByOperator[1][0];
                    }
                    else
                    {
                        return UnsupportedOperator;
                    }
                }
                else
                {
                    return InvalidFormatting;
                }
            }
            else
            {
                return InvalidFormatting;
            }
        }

        /// <summary>
        /// Extracts some operand from an user input.
        /// </summary>
        /// <param name="s">String, which represents the user input.</param>
        /// <param name="index">Numeric value to decide, which operand will be returned.</param>
        /// <returns>A string containing the operand at the given index.</returns>
        private static string GetOperand(string s, int index)
        {
            if (GetOperator(s) != ' ')
            {
                return s.Split(GetOperator(s))[index].Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Calculates the sum of two vectors.
        /// </summary>
        /// <param name="vec1">Integer array, which describes the first vector.</param>
        /// <param name="vec2">Integer array, which describes the second vector.</param>
        /// <returns>A new vector, which is the sum of the two given vectors.</returns>
        private static int[] AddVectors(int[] vec1, int[] vec2)
        {
            if (vec1.Length == vec2.Length)
            {
                int[] newVec = new int[vec1.Length];

                for (int i = 0; i < newVec.Length; i++)
                {
                    newVec[i] = vec1[i] + vec2[i];
                }

                return newVec;
            }
            else
            {
                return new int[] { };
            }
        }

        /// <summary>
        /// Calculates the difference of two vectors.
        /// </summary>
        /// <param name="vec1">Integer array, which describes the first vector.</param>
        /// <param name="vec2">Integer array, which describes the second vector.</param>
        /// <returns>A new vector, which is the difference of the two given vectors.</returns>
        private static int[] SubtractVectors(int[] vec1, int[] vec2)
        {
            return AddVectors(vec1, MultiplyVector(vec2, -1));
        }

        /// <summary>
        /// Multiplies a given vector with some value.
        /// </summary>
        /// <param name="vec">Integer array, which describes the vector.</param>
        /// <param name="value">Numeric value.</param>
        /// <returns>A new integer array, which is the resulting vector of the multiplication.</returns>
        private static int[] MultiplyVector(int[] vec, int value)
        {
            int[] newVec = new int[vec.Length];

            for (int i = 0; i < newVec.Length; i++)
            {
                newVec[i] = vec[i] * value;
            }

            return newVec;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vec1">Integer array, which describes the first vector.</param>
        /// <param name="vec2">Integer array, which describes the second vector.</param>
        /// <returns>An integer, which is the dot product.</returns>
        private static int GetDotProduct(int[] vec1, int[] vec2)
        {
            if (vec1.Length == vec2.Length)
            {
                int newValue = 0;

                for (int i = 0; i < vec1.Length; i++)
                {
                    newValue += vec1[i] * vec2[i];
                }

                return newValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vec1">Integer array, which describes the first vector.</param>
        /// <param name="vec2">Integer array, which describes the second vector.</param>
        /// <returns>A new integer array, which is the cross product.</returns>
        private static int[] GetCrossProduct(int[] vec1, int[] vec2)
        {
            if (vec1.Length == 3 && vec2.Length == 3)
            {
                int[] newValues = new int[vec1.Length];

                newValues[0] = (vec1[1] * vec2[2]) - (vec1[2] * vec2[1]);
                newValues[1] = (vec1[2] * vec2[0]) - (vec1[0] * vec2[2]);
                newValues[2] = (vec1[0] * vec2[1]) - (vec1[1] * vec2[0]);

                return newValues;
            }
            else
            {
                return new int[] { };
            }
        }

        /// <summary>
        /// Decides whether the given string is a valid scalar or not.
        /// </summary>
        /// <param name="s">String, which represents the user input.</param>
        /// <returns>A boolean, whose value depends on whether the given string is valid or not.</returns>
        private static bool IsValidScalar(string s)
        {
            int temp = 0;

            return GetVariableName(s).Length > 0 && GetScalarValue(s, ref temp);
        }

        /// <summary>
        /// Decides whether the given string is a valid vector or not.
        /// </summary>
        /// <param name="s">String, which represents the user input.</param>
        /// <returns>A boolean, whose value depends on whether the given string is valid or not.</returns>
        private static bool IsValidVector(string s)
        {
            return GetVariableName(s).Length > 0 && GetVectorValues(s).Length > 0;
        }

        /// <summary>
        /// Extracts the variable name of an user input.
        /// </summary>
        /// <param name="s">String, which represents the user input.</param>
        /// <returns>A string, which contains the variable name.</returns>
        private static string GetVariableName(string s)
        {
            string[] splittedByEqualsSign = s.Trim().Split('=');

            // The variable name must be at least one character long
            if (splittedByEqualsSign.Length == 2 && splittedByEqualsSign[0].Trim().Length > 0)
            {
                string tempVN = splittedByEqualsSign[0].Trim();

                // Variable names cannot start with a digit
                try
                {
                    Convert.ToInt32(tempVN[0].ToString());

                    return string.Empty;
                }
                catch (FormatException)
                {
                    return tempVN;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts the user input into an array of numeric values, also called a vector.
        /// </summary>
        /// <param name="s">User input, which will be converted.</param>
        /// <returns>A new vector, which is only empty if the user input was corrupt.</returns>
        private static int[] GetVectorValues(string s)
        {
            string[] splittedByEqualsSign = s.Trim().Split('=');

            if (splittedByEqualsSign.Length == 2)
            {
                splittedByEqualsSign[1] = splittedByEqualsSign[1].Trim();

                if (splittedByEqualsSign[1][0] == '[' && splittedByEqualsSign[1][splittedByEqualsSign[1].Length - 1] == ']')
                {
                    splittedByEqualsSign[1] = splittedByEqualsSign[1].Substring(1, splittedByEqualsSign[1].Length - 2);

                    // If not all values can be converted successfully, an empty array will be returned
                    string[] splittedByComma = splittedByEqualsSign[1].Split(',');

                    int[] tempValues = new int[splittedByComma.Length];

                    for (int i = 0; i < tempValues.Length; i++)
                    {
                        if (!GetValue(splittedByComma[i], ref tempValues[i]))
                        {
                            return new int[] { };
                        }
                    }

                    return tempValues;
                }
                else
                {
                    return new int[] { };
                }
            }
            else
            {
                return new int[] { };
            }
        }

        /// <summary>
        /// Tries to get the numeric value from an user input.
        /// </summary>
        /// <param name="s">User input, which will be converted.</param>
        /// <param name="value">Reference of an integer, which will be altered after successful conversion.</param>
        /// <returns>A boolean, which indicates whether the conversion was successfully or not.</returns>
        private static bool GetScalarValue(string s, ref int value)
        {
            string[] splittedByEqualsSign = s.Split('=');

            if (splittedByEqualsSign.Length == 2)
            {
                return GetValue(splittedByEqualsSign[1], ref value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to convert a given string to an integer.
        /// </summary>
        /// <param name="s">String, which will be converted.</param>
        /// <param name="value">Reference of an integer, which will be altered after successful conversion.</param>
        /// <returns>A boolean, which indicates whether the conversion was successfully or not.</returns>
        private static bool GetValue(string s, ref int value)
        {
            s = s.Trim();

            try
            {
                if ((s.StartsWith("fibo(") || s.StartsWith("prim(")) && s.EndsWith(")"))
                {
                    int index = Convert.ToInt32(s.Substring(5, s.Length - 6));

                    // Only a positive index can be passed to the methods fibo and prime
                    if (index >= 1)
                    {
                        if (s[0] == 'f')
                        {
                            value = Fibo(Convert.ToInt32(s.Substring(5, s.Length - 6)));
                        }
                        else
                        {
                            value = Prim(Convert.ToInt32(s.Substring(5, s.Length - 6)));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    value = Convert.ToInt32(s);
                }

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        /// <summary>
        /// Delivers a formatted string of a vector.
        /// </summary>
        /// <param name="array">Array of numbers, which represent the vector.</param>
        /// <returns>A formatted string of the vector.</returns>
        private static string GetFormattedArray(int[] array)
        {
            string ret = "[";

            for (int i = 0; i < array.Length - 1; i++)
            {
                ret += array[i] + ",";
            }

            return ret + array[array.Length - 1] + "]";
        }

        /// <summary>
        /// Calculates the nth prime number.
        /// </summary>
        /// <param name="index">Represents the index of a number of all prime numbers.</param>
        /// <returns>The prime number at a certain index.</returns>
        private static int Prim(int index)
        {
            int number = 1;

            while (index > 0)
            {
                number++;

                if (IsPrime(number))
                {
                    index--;
                }
            }

            return number;
        }

        /// <summary>
        /// Decides if some number is a prime.
        /// </summary>
        /// <param name="number">Represents the reviewed number.</param>
        /// <returns>A boolean, whose value depends on whether the given number is a prime or not.</returns>
        private static bool IsPrime(int number)
        {
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates the nth number of the Fibonacci sequence.
        /// </summary>
        /// <param name="index">Represents the index of a number of the Fibonacci sequence.</param>
        /// <returns>A number of the Fibonacci sequence.</returns>
        private static int Fibo(int index)
        {
            int left = 0;
            int right = 1;
            int temp;

            for (int i = 0; i < index; i++)
            {
                temp = right;
                right = left + right;
                left = temp;
            }

            return left;
        }
    }
}