using System;

class FinancialForecasting
{
    static double FutureValueRecursive(double presentValue, double rate, int years)
    {
        if (years == 0)
            return presentValue;

        return (1 + rate) * FutureValueRecursive(presentValue, rate, years - 1);
    }

    static double FutureValueMemoized(double presentValue, double rate, int years, double[] memo)
    {
        if (years == 0)
            return presentValue;

        if (memo[years] != 0)
            return memo[years];

        memo[years] = (1 + rate) * FutureValueMemoized(presentValue, rate, years - 1, memo);
        return memo[years];
    }

    static void Main()
    {
        double presentValue = 1000;  
        double rate = 0.05;          
        int years = 10;

        double futureValue = FutureValueRecursive(presentValue, rate, years);
        Console.WriteLine("Future Value (Recursive): " + futureValue);


        double[] memo = new double[years + 1];
        double futureValueMemo = FutureValueMemoized(presentValue, rate, years, memo);
        Console.WriteLine("Future Value (Memoized): " + futureValueMemo);
    }
}

