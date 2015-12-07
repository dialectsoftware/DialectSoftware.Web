using mshtml;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DialectSoftware.InteropServices;

namespace DialectSoftware.Web
{
    //[ComVisible(true), ComImport(), Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
    public class HTMLDOMDocument : HTMLDocumentClass
    {
        private class UrlRequest
        {
            public String url;
            public HTMLDocument doc;
        }

        public static HTMLDocument CreateDocumentFromURL(string url, int millisecondsTimeOut)
        {
            UrlRequest request = new UrlRequest();
            request.url = url;

            System.Threading.ParameterizedThreadStart s = new System.Threading.ParameterizedThreadStart(ThreadProc);
            System.Threading.Thread T = new System.Threading.Thread(s);
            
            if (System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA)
            {
                T.SetApartmentState(System.Threading.ApartmentState.MTA);
            }

            T.Start(request);

            if (millisecondsTimeOut > 0)
            {
                T.Join(millisecondsTimeOut);
            }
            else
            {
                T.Join();
            }

            return request.doc;
       }

        #region Requires MTAThread
       //protected static void ThreadProc2(object request)
       // {
       //     HTMLDocument doc = new HTMLDocument();
       //     comsupport.IPersistStreamInit objIPS = (comsupport.IPersistStreamInit)doc;
       //     objIPS.InitNew();
       //     IHTMLDocument2 document = doc.createDocumentFromUrl(((UrlRequest)request).url , "");
       //     while (document.readyState != "complete")
       //     {
       //         System.Threading.Thread.Sleep(1000);
       //     }
       //     ((UrlRequest)request).doc = (HTMLDocument)document;
       // }
       #endregion

        #region Requires MTAThread
        protected static void ThreadProc(object request)
        {
            HTMLDocument doc = new HTMLDocument();
            doc.designMode = "on";
            COM.IPersistMoniker persistMoniker = (COM.IPersistMoniker)doc;
            IMoniker moniker = null;
            int iResult = WIN32.CreateURLMoniker(null, ((UrlRequest)request).url, out moniker);
            IBindCtx bindContext = null;
            iResult = WIN32.CreateBindCtx(0, out bindContext);
            iResult = persistMoniker.Load(0, moniker, bindContext, COM.CONSTANTS.STGM_READ);
            while (doc.readyState != "complete")
            {
                System.Threading.Thread.Sleep(1000);
            }
            persistMoniker = null;
            bindContext = null;
            moniker = null;
            ((UrlRequest)request).doc = doc;
        }
        #endregion

        public HTMLDOMDocument()
        { 
        
        }

        public  HTMLDOMDocument(Uri url)
        {
            Load(url);
        }

        public void Load(Uri url)
        {
            HttpStream s = null;
            System.IO.StreamReader sr = null;
            try
            {
                s = new HttpStream(url.ToString(), false);
                sr = new System.IO.StreamReader(s);
                LoadHTML(sr.ReadToEnd());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                    s.Dispose();
                }

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }

        }

        public void Load(String path)
        {
            System.IO.StreamReader sr = null;
            try
            {
                sr = new System.IO.StreamReader(path);
                String HTML = sr.ReadToEnd();
                LoadHTML(HTML);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
            
        }

        public void LoadHTML(String HTML)
        {
            ((IHTMLDocument2)this).designMode = "On";
            ((IHTMLDocument2)this).write(new object[] { HTML });
        }

    }
}
