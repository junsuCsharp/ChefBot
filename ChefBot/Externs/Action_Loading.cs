using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Externs
{
    class Action_Loading
    {
        //LoadProcess
        private Thread thLoadSplash;
        private EventWaitHandle loaded;
        private Forms.FormLoading form;

        delegate void CloseCallback();
        delegate void updateCallback(string msg);

        public Action_Loading()
        {
            thLoadSplash = new Thread(new ThreadStart(RunSplash));
            loaded = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        private void RunSplash()
        {
            form = new Forms.FormLoading();
            form.Load += new EventHandler(OnLoad);
            form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmClosed);
            form.ShowDialog();
        }

        public void Open()
        {
            thLoadSplash.Start();
        }

        public void Close()
        {
            loaded.WaitOne();
            try
            {
                //thLoadSplash.Abort();
                form.Invoke(new CloseCallback(form.Close));
            }
            catch
            {
                thLoadSplash.Abort();
                form.Close();
            }

        }

        public bool IsDispose()
        {
            return form.IsDisposed;
        }

        public void OnLoad(object sender, EventArgs e)
        {
            loaded.Set();
        }

        public void frmClosed(object sender, EventArgs e)
        {
            //try
            //{
            //    if (thLoadSplash != null)
            //    {
            //        if (thLoadSplash.IsAlive)
            //        {
            //            thLoadSplash.Abort();
            //
            //        }
            //    }
            //
            //  
            //}
            //catch 
            //{
            //    Environment.Exit(0);
            //}
        }

        public void Join()
        {
            thLoadSplash.Join();
        }

        public void UpdateProgress(string msg)
        {
            loaded.WaitOne();
            try
            {
                form.Invoke(new updateCallback(form.updateTime), msg);
            }
            catch
            {
                thLoadSplash.Abort();
                form.Close();
            }
        }
    }
}
