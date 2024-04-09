namespace FabulaUltimaDataImporter.IO
{
    public class UserIOWrapper
    {
        public virtual string? ReadLine()
        {
            return Console.ReadLine();
        }

        public uint? ReadUnsignedInt()
        {
            var result = ReadLine();
            if (uint.TryParse(result, out var value))
            {
                return value;
            }
            return null;
        }

        public virtual void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void WriteLines(params string[] lines)
        {
            foreach (var line in lines)
            {
                WriteLine(line);
            }
        }

        public virtual void Write(string line)
        {
            Console.Write(line);
        }

        public uint GetUnsignedInt(string key, uint min = 1, uint max = int.MaxValue, ISet<int?> allowedValues = null)
        {
            Write($"{key}: ");
            uint? value;
            while (true)
            {
                var result = ReadUnsignedInt();
                bool badValue = false;
                if (result == null)
                {
                    WriteLine($"Invalid {key}");
                    badValue = true;
                }

                if (allowedValues?.Any() == true && !allowedValues.Contains((int?)result))
                {
                    WriteLine($"{key} must be [{string.Join(" ,", allowedValues)}]");
                    badValue = true;
                }

                if (result < min)
                {
                    WriteLine($"{key} must be greater than {min}");
                    badValue = true;
                }

                if (result > max)
                {
                    WriteLine($"{key} must be less than or equal to {max}");
                    badValue = true;
                }

                if (badValue)
                {
                    Write($"Enter valid {key}: ");
                    continue;
                }
                value = result;
                break;
            }
            return value.Value;
        }

        public bool WillUserContinue(string question)
        {
            WriteLine(question);
            while (true)
            {
                WriteLine("y/n?");
                var answer = ReadLine();
                bool badAnswer = false;
                if (string.IsNullOrWhiteSpace(answer))
                {
                    badAnswer = true;
                }

                if (!string.Equals(answer, "y", StringComparison.InvariantCultureIgnoreCase) &&
                    !string.Equals(answer, "n", StringComparison.InvariantCultureIgnoreCase))
                {
                    badAnswer = true;
                }

                if (badAnswer)
                {
                    WriteLine("please provide valid response");
                    continue;
                }
                return string.Equals(answer, "y", StringComparison.InvariantCultureIgnoreCase) ? true : false;
            }
        }

        public string GetValidString(string key, bool allowEmpty = false, Func<string, (bool verified, string error)> additionalVerification = null)
        {
            Write($"{key}: ");

            string value;
            while (true)
            {
                var result = ReadLine();

                bool badValue = false;

                if (result == null)
                {
                    WriteLine($"Invalid {key}");
                    badValue = true;
                }

                if (string.IsNullOrWhiteSpace(result) && !allowEmpty)
                {
                    WriteLine($"{key} must have value");
                    badValue = true;
                }

                if (additionalVerification != null)
                {
                    var additional = additionalVerification.Invoke(result);
                    if (!additional.verified)
                    {
                        WriteLine($"verification error for {key}:  {additional.error}");
                        badValue = true;
                    }
                }

                if (badValue)
                {
                    Write($"Enter valid {key}: ");
                    continue;
                }
                value = result;
                break;
            }

            return value;
        }

        public bool? GetBoolean(string question, string trueString = "yes", string falseString = "no")
        {
            (bool verified, string error) OnlyAllowBooleanStrings(string arg)
            {
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (
                    !string.Equals(arg, trueString, StringComparison.InvariantCultureIgnoreCase) &&
                    !string.Equals(arg, falseString, StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };
            WriteLine($"{question} {trueString} or {falseString} ");
            var response = GetValidString("Answer", additionalVerification: OnlyAllowBooleanStrings);
            return string.Equals(response, trueString, StringComparison.InvariantCultureIgnoreCase) ? true : false;
        }
    }
}
