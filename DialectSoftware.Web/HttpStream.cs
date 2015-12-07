using System;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;


namespace DialectSoftware.Web
{
    public class HttpStream : System.IO.Stream
    {
        /// <summary>
        /// http://www.pinvoke.net/
        /// The URLMON library contains this function, URLDownloadToFile, which is a way
        /// to download files without user prompts.  The ExecWB( _SAVEAS ) function always
        /// prompts the user, even if _DONTPROMPTUSER parameter is specified, for "internet
        /// security reasons".  This function gets around those reasons.
        /// </summary>
        /// <param name="pCaller">Pointer to caller object (AX).</param>
        /// <param name="szURL">String of the URL.</param>
        /// <param name="szFileName">String of the destination filename/path.</param>
        /// <param name="dwReserved">[reserved].</param>
        /// <param name="lpfnCB">A callback function to monitor progress or abort.</param>
        /// <returns>0 for okay.</returns>
        [DllImport("urlmon.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Int32 URLDownloadToFile(
            [MarshalAs(UnmanagedType.IUnknown)] object pCaller,
            [MarshalAs(UnmanagedType.LPWStr)] string szURL,
            [MarshalAs(UnmanagedType.LPWStr)] string szFileName,
            Int32 dwReserved,
            IntPtr lpfnCB);

        private bool ready;
        private System.IO.MemoryStream mem;

        public HttpStream(string Url, bool Async)
            : base()
        {

            ready = false;
            if (Async)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
                t.Start(Url);
            }
            else
            {
                ThreadProc(Url);
            }
        }

        public void ThreadProc(Object Url)
        {
            string filename = System.IO.Path.GetTempFileName();
            URLDownloadToFile(0, Url.ToString(), filename, 0, IntPtr.Zero);
            byte[] data = new byte[]{};
            using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, (Int32)fs.Length);
                fs.Close();
            }
            mem = new System.IO.MemoryStream(data);
            mem.Flush();
            mem.Seek(0, System.IO.SeekOrigin.Begin);
            ready = true;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return mem.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return mem.BeginWrite(buffer, offset, count, callback, state);
        }

        public override bool CanRead
        {
            get { return ready && mem.CanRead; }
        }

        public override bool CanWrite
        {
            get { return ready && mem.CanWrite; }
        }

        public override bool CanSeek
        {
            get { return ready && mem.CanSeek; }
        }

        public override void Flush()
        {
            mem.Flush();
        }

        public override long Length
        {
            get { return mem.Length; }
        }

        public override long Position
        {
            get
            {
                return mem.Position;
            }
            set
            {
                mem.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return mem.Read(buffer, offset, count);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return mem.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            mem.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            mem.Write(buffer, offset, count);
        }

        public override void Close()
        {
            mem.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            mem.Dispose();
            base.Dispose(disposing);
        }
    }
}
