using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FsCheck;
using FsCheck.Xunit;

namespace BankingKataTests
{
    public class AccountTests
    {
        [Fact]
        public void Any_Amount_Of_Money_Produces_A_Transaction()
        {
            var account = new Account();

            Func<int,bool> x = (value) => account.Deposit(new Money(value)) != null;

            Prop.ForAll(x).QuickCheckThrowOnFailure();
        }

        [Fact]
        public void The_Transaction_Log_Balances()
        {
            var account = new Account();

            Func<int,bool> x = (value) =>
            {
                var money = new Money(Math.Abs(value));
                account.Deposit(money);
                account.Withdrawl(money);

                return account.Balance().Equals(Money.Zero);
            };

            Prop.ForAll(x).QuickCheckThrowOnFailure();
        }

        public class Money
        {
            public static Money Zero = new Money(0);

            private readonly int m_Value;

            public Money(int value)
            {
                m_Value = value;
            }

            public static Money Add(Money a, Money b)
            {
                return new Money(a.m_Value + b.m_Value);
            }

            public static Money Subtract(Money a, Money b)
            {
                return new Money(a.m_Value - b.m_Value);
            }

            protected bool Equals(Money other)
            {
                return m_Value == other.m_Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Money) obj);
            }

            public override int GetHashCode()
            {
                return m_Value;
            }

            public static bool operator ==(Money left, Money right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Money left, Money right)
            {
                return !Equals(left, right);
            }
        }

        public abstract class Transaction
        {
            public abstract Money Value { get; }

            public class Deposit : Transaction
            {
                private readonly Money m_Money;

                public Deposit(Money money)
                {
                    m_Money = money;
                }

                public override Money Value
                {
                    get { return m_Money; }
                }
            }

            public class Withdrawl : Transaction
            {
                private readonly Money m_Money;

                public Withdrawl(Money money)
                {
                    m_Money = money;
                }

                public override Money Value
                {
                    get { return (Money.Subtract(Money.Zero, m_Money)); }
                }
            }
        }


        public class Account
        {
            private readonly List<Transaction> m_TransactionLog = new List<Transaction>();

            public Transaction Deposit(Money money)
            {
                var deposit = new Transaction.Deposit(money);
                m_TransactionLog.Add(deposit);

                return deposit;
            }

            public Transaction Withdrawl(Money money)
            {
                var withdrawl = new Transaction.Withdrawl(money);
                m_TransactionLog.Add(withdrawl);

                return withdrawl;
            }

            public Money Balance()
            {
                return m_TransactionLog.Select(x => x.Value).Aggregate(Money.Add);
            }
        }
    }
}
