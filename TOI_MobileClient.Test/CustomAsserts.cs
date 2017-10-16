using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TOI_MobileClient.Test
{
    public static class CustomAsserts
    {
        public static void ThrowsAsync<T>(Func<object> testMethod)
            where T : Exception
        {
            try
            {
                testMethod();
                Assert.Fail("No exception was thrown.");
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.GetType());
                Assert.IsInstanceOfType(e.InnerException, typeof(T));
            }
            catch (Exception e)
            {
                Assert.Fail("A wrong exception was thrown.");
            }
        }
    }
}
