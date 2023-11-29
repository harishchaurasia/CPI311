using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class fraction
    {

        private int numerator;
        private int denominator;



        // properties

        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; //simplify();
                                     }
        }
        public int Denominator
        {
            get { return denominator; }
            set { denominator = value; //simplify();
                                       }
        }


        // Methods
        public fraction(int n = 0, int d = 1)
        {

            numerator = n;

            if (d == 0) d = 1;
            denominator = d;
            simplify();
        }
        public override string ToString()
        {
            return numerator + "/" + denominator;
        }
        private void simplify()
        {
            if (denominator < 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            int gcd = GCD(Math.Max(numerator, denominator), Math.Min(numerator, denominator));
            numerator /= gcd;
            denominator /= gcd;
        }
        private int GCD(int n, int d)
        {
            if (n == 0)
            {
                return d;
            }
            if (d == 0)
            {
                return n;
            }
            else
            {
                return GCD(d, n % d);
            }
        }

        public static fraction Multiply(fraction a, fraction b)
        {
            return new fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static fraction Divide(fraction a, fraction b)
        {
            return new fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static fraction Add(fraction a, fraction b)
        {
        int commonDenominator = a.denominator * b.denominator;
        int sumNumerator =  a.numerator * b.denominator + b.numerator * a.denominator;
        return new fraction(sumNumerator, commonDenominator);  

        }

        public static fraction Subtract(fraction a, fraction b)
        {
        int commonDenominator = a.denominator * b.denominator;
        int diffNumerator = a.numerator * b.denominator - b.numerator * a.denominator;
        return new fraction(diffNumerator, commonDenominator);
        }

        public static fraction operator *(fraction a, fraction b)
        {
            return new fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static fraction operator /(fraction a, fraction b)
        {
            return new fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static fraction operator +(fraction a, fraction b)
        {
            int commonDenominator = a.denominator * b.denominator;
            int sumNumerator =  a.numerator * b.denominator + b.numerator * a.denominator;
            return new fraction(sumNumerator, commonDenominator);  
        }

        public static fraction operator -(fraction a, fraction b)
        {
            int commonDenominator = a.denominator * b.denominator;
            int diffNumerator = a.numerator * b.denominator - b.numerator * a.denominator;
            return new fraction(diffNumerator, commonDenominator);
        }
    }
}
