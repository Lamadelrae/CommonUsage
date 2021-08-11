using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace UnitTesting
{
    [TestClass]
    public class ExtensionTests
    {

        [TestMethod]
        public void ShouldConvertToDouble()
        {
            try
            {
                decimal a = 100.11m;
                double b = a.ToDouble();

                float c = 100.00f;
                double d = c.ToDouble();
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }
    }
}
