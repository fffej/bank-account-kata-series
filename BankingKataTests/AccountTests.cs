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
        public void Any_Amount_Of_Money_Produces_A_Receipt()
        {
            var account = new Account();

            Func<int,bool> x = (value) => account.Deposit(new Money(value)) != null;

            Prop.ForAll(x).QuickCheckThrowOnFailure();
        }

        public class Money
        {
            public Money(int value)
            {
            }
        }

        public class Receipt
        {
        }


        public class Account
        {
            public Receipt Deposit(Money money)
            {
                return new Receipt();
            }
        }
    }
}
