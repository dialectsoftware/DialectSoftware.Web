using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using mshtml;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void IHTMLDocument2TestMethod()
        {
            var d = new DialectSoftware.Web.HTMLDOMDocument(new Uri("http://www.github.com")) as IHTMLDocument2;
            Assert.IsNotNull(d.body.innerText);
            Assert.IsTrue(d.body.innerHTML.Length > 0);
        }

        [TestMethod]
        public void IHTMLDocument3TestMethod()
        {
            var d = new DialectSoftware.Web.HTMLDOMDocument(new Uri("http://www.github.com")) as IHTMLDocument3;
            var anchors = d.getElementsByTagName("a");
            IHTMLElement anchor = anchors.item(0) as IHTMLElement;
            Assert.IsNotNull(anchors);
            Assert.IsTrue(anchors.length > 0);
        }
    }
}
