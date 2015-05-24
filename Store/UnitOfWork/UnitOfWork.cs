using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Models;

namespace Store.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private Entities _context;
        public Entities Repository
        {
            get
            {

                if (this._context== null)
                {
                    this._context = new Entities();
                }
                return _context;
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}