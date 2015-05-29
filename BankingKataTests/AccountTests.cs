using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankingKataTests
{
    public class AccountTests
    {
        [Fact]
        public void Accounts_Can_Make_Deposits()
        {
            var account = new Account();
            var receipt = account.Deposit(Money.Pound);

            Assert.NotNull(receipt);
        }

        public enum Money
        {
            Pound
        }

        public class Account
        {
            public object Deposit(object money)
            {
                return new object();
            }
        }
    }
}
