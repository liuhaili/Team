using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lemon
{
    /// <summary>
    /// 对象显示回收基类
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private int _isDisposed;

        /// <summary>
        /// 显示释放对象资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 检查对象是否已被显示释放了
        /// </summary>
        protected void CheckDisposed()
        {
            if (_isDisposed == 1)
            {
                throw new Exception(string.Format("The {0} object has be disposed.", this.GetType().Name));
            }
        }

        /// <summary>
        /// 释放非托管资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    //释放 托管资源 
            //}
            //释放非托管资源 
            if (disposing)
            {
                Interlocked.Exchange(ref _isDisposed, 1);
                GC.SuppressFinalize(this);
            }
        }

        public bool IsDisposed()
        {
            return _isDisposed == 1;
        }
    }
}